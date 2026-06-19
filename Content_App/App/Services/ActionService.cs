using Content_App.App.DTOs.Item;
using Content_App.Infrastructure.Data;
using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class ActionService
    {
        private readonly LogDbContext _context;
        private readonly DateConverter _dateConvert;

        public ActionService(LogDbContext context, DateConverter dateConvert)
        {
            this._context = context;
            this._dateComvert = dateConvert;
        }

        public async Task NewItemActionAsync(ItemDto dto, Guid userId, Guid picId, Guid itemId)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(user == null)
            {
                throw new KeyNotFoundException();
            }
            
            _context.LogActions.Add(new LogAction
            {
                EmpCode = user.EmpCode,
                PicId = picId,
                ItemId = itemId,
                Qty = dto.Quantity,
                Kind = "create",
                Reason = $"{user.Fullname}: Create New Item",
                DateAction = _dateConvert.D_TimeStampNow_Unspecified()
            });
            await _context.SaveChangesAsync();
        }

        public async Task ReceiveItemAsync(ItemActivityDto dto, Guid userId, Guid picId)
        {
            var item = await _context.Items.FindAsync(dto.ItemId);
            var user = await _context.Employees.FindAsync(userId);

            if(item == null || user == null)
            {
                throw new KeyNotFoundException();
            }

            item.Quantity = item.Quantity + dto.Qty;

            _context.Items.Update(item);
            
            _context.LogActions.Add(new LogAction
            {
                EmpCode = user.EmpCode,
                PicId = picId,
                ItemId = item.Id,
                Qty = dto.Qty,
                Kind = "receive",
                Reason = $"{user.Fullname}: add item quantity",
            });

            await _context.SaveChangeAsync();
        }

        public async Task<CreateRequestDto> DeliveryItemAsync(ItemActivityDto dto, Guid userId, Guid picId)
        {
            var item = await _context.Items.FindAsync(dto.ItemId);
            var user = await _context.Employees.FindAsync(userId);

            if(item == null || user == null)
            {
                throw new KeyNotFoundException();
            }

            item.Quantity = item.Quantity - dto.Qty;
            _context.Items.Update(item);

            LogAction action = new LogAction
            {
                Empcode = dto.Empcode!,
                PicId = picId,
                ItemId = dto.ItemId,
                Qty = dto.Qty,
                Kind = "delivery",
                Reason = $"{dto.EmpName}: {dto.Reason}",
                Item = item
            };

            _context.LogAction.Add(action);

            await _context.SaveChangesAsync();

            //create new request
            CreateRequestDto req = new CreateRequestDto
            {
                itemId = dto.ItemId,
                qty = dto.Qty,
                Reason = dto.Reason,
                Ucode = dto.EmpCode,
                Uname = dto.EmpName
            };

            return req;
        }

        public async Task UpdateItemActionAsync(Item item, Guid userId)
        {
            var user = await _context.Accounts.FindAsync(userId);
            if(user == null)
            {
                throw new NullReferenceException();
            }
            var action = new LogAction
            {
                EmpCode = user.UserCode,
                PicId = userId,
                ItemId = item.Id,
                Qty = item.Quantity,
                Kind = "update",
                Reason = "update item",
                DateAction = DateTime.UtcNow.AddHours(7)
            };

            _context.Items.Update(item);
            _context.LogActions.Add(action);

            await _context.SaveChangesAsync();
        }

        public async Task<List<InventDto>> GetInventAsync(Guid uId, string roleName, DateTime fromD, DateTime toD)
        {
            List<Currency> moneyInit = await _context.Currencies.ToListAsync();

            var data = await GetDataReport(uId, roleName, fromD, toD);

            List<InventDto> res = new List<InventDto>{ };

            foreach(var inv in data)
            {
                InventDto dto = new InventDto{ };
                if(inv.currency!.ToLower() != "usd)
                {
                    dto.price = Math.Round(ExchangeValue(inv.currency.ToLower(), Convert.ToDecimal(inv.cost), moneyInit), 3);
                } else
                {
                    dto.price = Math.Round((decimal)inv.cost!, 3);
                }

                dto.itemNo = inv.itemCode;
                dto.itemName = $"{inv.enName}|{inv.vnName}";
                dto.unit = inv.unit!.ToUpper();
                dto.stock = inv.remain;
                dto.input = inv.totalIn;
                dto.output = inv.totalOut;
                dto.actualStock = 0;
                dto.totalAmount = dto.price * dto.stock;

                res.Add(dto);
            }

            return res;
        }

        public async Task<List<HistoryDto>> GetHistoryAsync(Guid userId, string roleName)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(user == null)
            {
                throw new NullReferenceException();
            }

            var history = await _context.LogActions
                .Join(_context.Accounts,
                     action => action.PicId,
                     account => account.Id,
                     (action, account) => new {action, account})
                .Join(_context.Items,
                     tmp => tmp.action.ItemId,
                     item => item.Id,
                     (tmp, item) => new {tmp, item})
                .Join(_context.Employees,
                     temp => temp.tmp.account.UserCode,
                     emp => emp.EmpCode,
                     (temp, emp) => new {temp, emp})
                .OrderByDescending(x => x.temp.tmp.action.DateAction)
                .Select(final => new
                {
                    id = final.temp.tmp.action.Id,
                    action_userCode = final.emp.EmpCode,
                    pic_Code = final.emp.EmpCode,
                    pic_Name = final.emp.FullName,
                    itemName_En = final.temp.item.EnName!,
                    itemName_Vn = final.temp.item.VnName!,
                    qty = final.temp.tmp.action.Qty,
                    kind = final.temp.tmp.action.Kind,
                    reason = final.temp.tmp.action.Reason!,
                    date_action = final.temp.tmp.action.DateAction,
                    deptId = final.emp.DeptId
                })
                .ToListAsync();

            List<HistoryDto> res = new List<HistoryDto> { };

            //Filter follow role
            if(roleName != "gm" && roleName != "dev")
            {
                foreach(var h in history)
                {
                    if(h.deptId == user.DeptId && (h.kind.ToLower() == "receive" || h.kind.ToLower() == "delivery"))
                    {
                        res.Add(new HistoryDto
                        {
                            empCode = h.action_userCode,
                            itemName = $"{h.itemName_En}"{h.itemName_Vn}",
                            pic = $"{h.pic_Code}-{h.pic_Name}",
                            qty = h.qty,
                            kind = h.kind,
                            reason = h.reason,
                            dateAction = _dateConvert.D_Convert((DateTime)h.date_action!)
                        });
                    }
                }

                return res;
            }

            foreach(var h in history)
            {
                var tmp = _dateConvert.D_Convert((DateTime)h.date_action!);
                res.Add(new HistoryDto
                {
                    empCode = h.action_userCode,
                    itemName = $"{h.itemName_En}"{h.itemName_Vn}",
                    pic = $"{h.pic_Code}-{h.pic_Name}",
                    qty = h.qty,
                    kind = h.kind,
                    reason = h.reason,
                    dateAction = _dateConvert.D_Convert((DateTime)h.date_action!)
                });
            }

            return res;
        }

        private async Task<IEnumerable<dynamic>> GetDataReport(Guid userId, string roleName, DateTime fromD, DateTime toD)
        {
            var user = await _context.Employees.FindAsync(userId);

            var data = await _context.Items
                .Where(x => x.DeletedAt == null)
                .Select(item => new
                {
                    id = item.Id,
                    itemCode = item.ItemCode,
                    enName = item.EnName,
                    vnName = item.VnName,
                    unit = item.Unit,
                    cost = item.Cost,
                    maker = item.Maker,
                    position_in = item.PositionIn,
                    currency = item.Currency,
                    remain = item.Quantity,
                    deptId = item.DeptId,
                    totalIn = item.LogActions
                        .Where(x => (x.Kind.ToLower() == "receive" || x.Kind.ToLower() == "create") && x.DateAction >= fromD && x.DateAction <= toD.AddDays(1))
                        .Sum(x => (int?)x.Qty ?? 0),
                    totalOut = item.LogActions
                        .Where(x => x.Kind.ToLower() == "delivery"  && x.DateAction >= fromD && x.DateAction <= toD.AddDays(1))
                        .Sum(x => (int?)x.Qty ?? 0),
                })
                .ToListAsync();
            if(data.Count() == 0) return new List<object>{ };

            //Filter data follow role
            //GM, dev ==> get all
            //Mgr, admin, super ==> follow dept
            bool isLimitData = roleName == "admin" || roleName == "super" || roleName == "manager";
            if(isLimitData)
            {
                for(var i = 0; i < data.Count(); i++)
                {
                    if(data[i].deptId != user!.DeptId)
                    {
                        data.Remove(data[i]);
                        i--;
                    }
                }
            }

            return data;
        }

        private decimal ExchangeValue(string? exchangeName, decimal exhcangevalue, List<Currency> dto)
        {
            foreach(var c in dto)
            {
                if(c.CurrentName!.ToLower() == exchangeName)
                {
                    return (decimal)(exchangevalue / c.ExchangeRate!);
                }
            }
            return exchangevalue;
        }
    }
}

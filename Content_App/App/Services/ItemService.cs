using Content_App.App.DTOs.Item;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Shared.Constants;
using Content_App.Shated.DatePipe;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

namespace Content_App.App.Services
{
    public class ItemService
    {
        private readonly LogDbContext _context;
        private readonly ActionService _actionService;
        private readonly DateConverter _dateConvert;
        
        public ItemService(LogDbContext context, ActionService actionService, DateConverter dateConvert)
        {
            this._context = context;
            this._actionService = actionService;
            this._dateConvert = dateConvert;
        }


        public async Task<List<Item>> GetItemAsync(string roleName, Guid userId)
        {
            var data = await _context.Items
                .Join(_context.Departments,
                     item => item.DeptId,
                     dept => dept.Id,
                     (item, dept) => new {item, dept})
                .Where(res => res.item.DeletedAt == null)
                .OrderBy(res => res.item.EnName)
                .Select(res => new
                {
                    id = res.item.Id,
                    itemCode = res.item.ItemCode,
                    enName = res.item.EnName,
                    vnName = res.item.VnName,
                    deptName = res.dept.DeptName.ToLower() == "ts" ? "Tien Son" : (res.dept.DeptName.ToLower() == "tl" ? "Thang Long" : "Que Vo"),
                    maker = res.item.Maker,
                    supplier = res.item.Supplier,
                    positionIn = res.item.PositionIn,
                    qty = res.item.Quantity,
                    unit = res.item.Unit,
                    currency = res.item.Currency,
                    cost = res.item.Cost,
                    image = res.item.Image,
                    deptId = res.dept.Id
                })
                .ToListAsync();

            if(data.Count() == 0)
            {
                throw new NullReferenceException();
            }

            List<ItemDto> items = new List<ItemDto>{ };

            foreach(var item in data)
            {
                items.Add(new ItemDto
                {
                    Id = item.id,
                    DeptId = item.deptId,
                    ItemCode = item.itemCode,
                    EnName = item.enName!,
                    VnName = item.vnName!,
                    DeptName = item.deptName,
                    Maker = item.maker!,
                    Supplier = item.supplier!,
                    PositionIn = item.positionIn!,
                    Quantity = item.qty,
                    Unit = item.unit!,
                    Currency = item.currency!,
                    Cost = item.cost,
                    Image = item.image
                });
            }

            if(roleName == "dev" || roleName == "gm")
            {
                return items;
            }

            var user = await _context.Employees.FindAsync(userId);

            if(user == null)
            {
                throw new NullReferenceException();
            }

            for(var i = 0; i < items.Count(); i++)
            {
                if(items[i].DeptId != user.DeptId)
                {
                    items.Remove(item[i]);
                    i--;
                }
            }
            
            return items;
        }

        public async Task CreateItemAsync(ItemDto dto, Guid userId)
        {
            var user = await _context.Employees.FindAsync(userId);
            var newId = await GenItemId();

            if(user == null)
            {
                throw new KeyNotFoundException();
            }
            
            await _context.Items.AddAsync(new Item
            {
                Id = newId,
                ItemCode = dto.ItemCode,
                EnName = dto.EnName,
                VnName = dto.VnName,
                Maker = dto.Maker,
                Supplier = dto.Supplier,
                PositionIn = dto.PositionIn,
                Quantity = dto.Quantity,
                DeptId = user.DeptId,
                AreaId = user.AreaId,
                Unit = dto.Unit.ToUpper(),
                Cost = dto.Cost,
                Currency = dto.Currency.ToUpper(),
                Image = "",
                DeletedAt = null
            });

            await _context.SaveChangesAsync();

            return newId;
        }

        public async Task CreateItemFromFileAsync(IFormFile file, Guid userId, Guid picId)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(file == null || user == null)
            {
                throw new ArgumentNullException();
            }

            using(var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using(var pkg = new ExcelPackage(stream))
                {
                    var ws = pkg.Workbook.Worksheets.First();

                    //get data
                    for(var row = 2; row <= ws.Dimension.End.Row, row++)
                    {
                        if(!string.IsNullOrWhiteSpace(ws.Cells[row, 2].Text))
                        {
                            Guid newId = await GentItemId();
                            var qty = ws.Cells[row, 8].Value;

                            var item = new Item
                            {
                                Id = newId,
                                ItemCode = ws.Cells[row, 2].Text,
                                EnName = ws.Cells[row, 3].Text,
                                Vnname = ws.Cells[row, 4].Text,
                                Maker = ws.Cells[row, 5].Text,
                                Supplier = ws.Cells[row, 6].Text,
                                PositionIn = ws.Cells[row, 7].Text,
                                Quantity = int.TryParse(ws.Cells[row, 8].Text, out var Qty) ? Qty : 0,
                                DeptId = user.DeptId,
                                AreaId = user.AreaId,
                                Unit = ws.Cells[row, 10].Text,
                                Cost = decimal.TryParse(ws.Cells[row, 11].Text, out var cost_) ? cost_ : 0,
                                Currency = ws.Cells[row, 12].Text == "" ? "" : ws.Cells[row, 12].Text.ToUpper()
                            };

                            //check one of these item existed?
                            if(await CheckItemExist(item))
                            {
                                throw new InvalidDataException();
                            }

                            await _context.Items.AddAsync(item);

                            _context.LogActions.Add(new LogAction
                            {
                                EmpCode = user.EmpCode,
                                PicId = picId,
                                ItemId = newId,
                                Qty = item.Quantity,
                                Kind = "create",
                                Reason = $"{user.Fullname}: Create New Item"
                            });

                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task UpdateItemAsync(Item item)
        {
            var itemRev = await _context.Items.FindAsync(item.Id);
            if(itemRev != null)
            {
                itemRev.ItemCode = item.ItemCode;
                itemRev.EnName = item.EnName;
                itemRev.VnName = item.VnName;
                itemRev.Maker = item.Maker;
                itemRev.Supplier = item.Supplier;
                itemRev.PositionIn = item.PositionIn;
                itemRev.Quantity = item.Quantity;
                itemRev.Unit = item.Unit;
                itemRev.Cost = item.Cost;
                itemRev.Currency = item.Currency;

                _context.Items.Update(itemRev);

                await _context.SaveChangesAsync();
            }
        }

        public async Task ItemUpdateImageAsync(IFormFile file, Guid itemId)
        {
            var fileType = file.FileName.SubString(file.FileName.Length - 3);
            var path = Path.Combine(VariableConstant.pathImage, $"{itemId}.{fileType}");
            var item = await _context.Items.FindAsync(itemId);

            if(item == null)
            {
                throw new KeyNotFoundException();
            }
            item.Image = $"{itemId}.{fileType}";
            _context.Items.Update(item);

            using var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid itemId, Guid picId)
        {
            var item = await _context.Items.FindAsync(itemId);
            var pic = await _context.Accounts.FindAsync(picId);
            
            if(item != null && pic != null)
            {
                _context.LogActions.Add(new LogAction
                {
                    EmpCode = pic.UserCode,
                    PicId = picId,
                    ItemId = itemId,
                    Qty = item.Quantity,
                    Kind = "delete",
                    Reason = "delete item"
                });

                item.DeletedAt = _dateConvert.D_TimeStampNow_Unspecified();
                _context.Items.Update(item);
                
                await _context.SaveChangesAsync();
            }
        }

        private async Task<Boolean> CheckItemExist(Item item)
        {
            var tmp = await _context.Items.Where(i => i.ItemCode == item.ItemCode && 
                    i.EnName!.ToLower == item.EnName!.ToLower() &&
                    i.VnName!.ToLower() == item.VnName!.ToLower()).ToListAsync();
            if(tmp.Count() == 0)
            {
                return false;
            }

            return true;
        }

        private async Task<Guid> GenItemId()
        {
            var newId = Guid.NewGuid();

            while(true)
            {
                var item = await _context.Items.FindAsync(newId);
                if(item == null) break;

                newId = Guid.NewGuid();
            }
            return newId;
        }
    }
}

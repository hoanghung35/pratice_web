using Content_App.App.DTOs.Item;
using Content_App.App.DTOs.User;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Content_App.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;

namespace Content_App.App.Services
{
    public class FileService
    {
        private readonly LogDbContext _context;

        public FileService(LogDbContext context)
        {
            this._context = context;
            ExcelPackage.License.SetNonCommercialOrganization("me");
        }

        public async Task UpLoadFileItemAsync(IFormFile file, Guid userId)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(file == null || file.Length == 0)
            {
                throw new NullReferenceException();
            }

            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                using (var package = new  ExcelPackage(stream))
                {
                    var ws = package.Workbook.Worksheets.First();

                    //getData
                    for(int row = 2; row <= ws.Dimension.End.Row; row++)
                    {
                        if (!string.IsNullOrWhiteSpace(ws.Cells[row, 2].Text))
                        {
                            var item = new Item
                            {
                                ItemCode = ws.Cells[row, 2].Text,
                                EnName = ws.Cells[row, 3].Text,
                                VnName = ws.Cells[row, 4].Text,
                                Maker = ws.Cells[row, 5].Text,
                                Supplier = ws.Cells[row, 6].Text,
                                PositionIn = ws.Cells[row, 7].Text,
                                Quantity = ws.Cells[row, 8].Text == "" ? 0 : int.Parse(ws.Cells[row, 8].Text),
                                Unit = ws.Cells[row, 9].Text,
                                Cost = ws.Cells[row, 10].Text == ""? 0: decimal.Parse(ws.Cells[row, 10].Text),
                                Currency = ws.Cells[row, 11].Text,
                            };

                            _context.Items.Add(item);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
        }

        public async Task<byte[]> ExportItemAsync(UserDto dto)
        {
            var items = await _context.Items.ToListAsync();
            switch (dto.RoleName)
            {
                case "admin":
                    {
                        items.Clear();
                        items = await _context.Items.Where(i => i.DeptId.ToString() == dto.DepId && i.AreaId.ToString() == dto.AreaId).ToListAsync();
                        break;
                    }
                case "super":
                    {
                        items.Clear();
                        items = await _context.Items.Where(i => i.Dept.ToString() == dto.DepId).ToListAsync();
                        break;
                    }
            }

            //Init file
            using (var package = new ExcelPackage())
            {
                var index = 1;
                var ws = package.Workbook.Worksheets.First();

                //header
                ws.Cells["A1"].Value = "No.";
                ws.Cells["B1"].Value = "Item Code";
                ws.Cells["C1"].Value = "En Name";
                ws.Cells["D1"].Value = "Vn Name";
                ws.Cells["E1"].Value = "Maker";
                ws.Cells["F1"].Value = "Supplier";
                ws.Cells["G1"].Value = "Position In";
                ws.Cells["H1"].Value = "Quantity";
                ws.Cells["I1"].Value = "Unit";
                ws.Cells["J1"].Value = "Cost";
                ws.Cells["K1"].Value = "Currency";
                
                for(int i = 0; i <= items.Count(); i++)
                {
                    ws.Cells[i + 2, 1].Value = index;
                    ws.Cells[i + 2, 2].Value = items[i].ItemCode;
                    ws.Cells[i + 2, 3].Value = items[i].EnName;
                    ws.Cells[i + 2, 4].Value = items[i].VnName;
                    ws.Cells[i + 2, 5].Value = items[i].Maker;
                    ws.Cells[i + 2, 6].Value = items[i].Supplier;
                    ws.Cells[i + 2, 7].Value = items[i].PositionIn;
                    ws.Cells[i + 2, 8].Value = items[i].Quantity;
                    ws.Cells[i + 2, 9].Value = items[i].Unit;
                    ws.Cells[i + 2, 10].Value = items[i].Cost.ToString().IsNullOrEmpty() ? 0 : items[i].Cost;
                    ws.Cells[i + 2, 11].Value = items[i].Currency;
                    index++;
                }
                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                return await package.GetAsByteArrayAsync();
            }
        }

        public async Task<byte[]> GenerateReportAsync(UserDto dto)
        {
            var items = await _context.Items.Where(i => i.DeptId.ToString() == dto.DepId).ToListAsync();
            var currency = await _context.Currencies.ToListAsync();
            var actions = await _context.LogActions.ToListAsync();

            using (var package = new ExcelPackage(VariableConstant.PathReport))
            {
                var ws = package.Workbook.Worksheets.First();

                for(int i = 0; i <= items.Count(); i++)
                {
                    int index = i + 14;
                    var item = items[i];
                    IODto tmpIO = IOItem(actions, item.Id);
                    decimal cost = Exchange(item, currency);

                    //format excel
                    ws.Cells[$"B{index}:C{index}"].Merge = true;

                    //fill data
                    ws.Cells["B" + index].Value = item.ItemCode;
                    ws.Cells["D" + index].Value = item.EnName;
                    ws.Cells["E" + index].Value = "";
                    ws.Cells["F" + index].Value = item.Unit;
                    ws.Cells["G" + index].Value = cost;
                    ws.Cells["H" + index].Value = item.Quantity;
                    ws.Cells["I" + index].Value = tmpIO.Input;
                    ws.Cells["J" + index].Value = tmpIO.Output;
                    ws.Cells["K" + index].Value = tmpIO.Input - tmpIO.Output + item.Quantity;
                    ws.Cells["L" + index].Value = item.Quantity * cost;
                }

                return await package.GetAsByteArrayAsync();
            }
        }

        private decimal Exchange(Item item, List<Currency> currencies)
        {
            decimal res = 0;
            foreach(var c in currencies)
            {
                if(item.Currency.ToUpper() == c.CurrenName.ToUpper())
                {
                    res = (decimal)(item.Quantity / c.ExchangeRate);
                }
            }
            return res;
        }

        private IODto IOItem(List<LogAction> actions, Guid itemId)
        {
            int input = 0, output = 0;
            foreach(var action in actions)
            {
                if(action.ItemId == itemId)
                {
                    input = action.Kind == "receive" ? input + action.Qty : input;
                    output = action.Kind == "delivery" ?output + action.Qty : output;
                }
            }

            return new IODto
            {
                Input = input,
                Output = output
            };

        }
    }
}

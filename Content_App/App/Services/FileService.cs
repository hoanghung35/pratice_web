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

        public async Task<byte[]> ExportFileInventAsync(List<InventDto> dto)
        {
            if(dto == null)
            {
                throw new ArgumentNullException();
            }

            using (var pkg = new ExcelPackage())
            {
                var ws = pkg.Workbook.Worksheets.Add("Sheet1");

                ws.Cells.Style.Font.Name = "Meiryo";
                ws.Cells.Style.Font.Size = 11;

                //header
                ws.Cells["O1"].Value = "LOG_GEN-Att 06";
                ws.Cells["O1"].Style.Font.Size = 12;

                //Merge cell
                ws.Cells["A2:O2"].Merge = true;
                ws.Cells[4, 1, 6, 2].Merge = true;
                ws.Cells[4, 5, 6, 6].Merge = true;
                ws.Cells["H4:I4"].Merge = true;
                ws.Cells["H5:I5"].Merge = true;
                ws.Cells["H6:I6"].Merge = true;
                ws.Cells[4, 10, 6, 11].Merge = true;
                ws.Cells["M4:O4"].Merge = true;
                ws.Cells["M5:O5"].Merge = true;
                ws.Cells["M6:O6"].Merge = true;
                ws.Cells["B8:D8"].Merge = true;
                ws.Cells["F8:L8"].Merge = true;
                ws.Cells["B11:C11"].Merge = true;
                ws.Cells["F9:G9"].Merge = true;
                ws.Cells["H9:L9"].Merge = true;
                ws.Cells["F10:G10"].Merge = true;
                ws.Cells["I10:J10"].Merge = true;
                ws.Cells["K10:L10"].Merge = true;
                ws.Cells["F11:G11"].Merge = true;
                ws.Cells["I11:J11"].Merge = true;
                ws.Cells["K11:L11"].Merge = true;
                ws.Cells["B13:C13"].Merge = true;

                //File tex header, format, font
                ws.Cells["A2"].Value = "MONTH GEN INVENTORY CHECKING (Office Supplies & Material)";
                ws.Cells["A2"].Style.Font.Size = 18;
                ws.Cells["A2"].Style.Font.Bold = true;
                ws.Cells["A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                ws.Cells["A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                ws.Cells["F11:K11"].Style.WrapText = true;
                ws.Cells["F11:K11"].Style.Font.Italic = true;

                //border
                ws.Cells["D4"].style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells["D5"].style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells["D6"].style.Border.BorderAround(ExcelBorderStyle.Thin);
                ws.Cells["D7"].style.Border.BorderAround(ExcelBorderStyle.Thin);

                //width, height
                ws.Column(1).Width = 7;
                ws.Column(2).Width = 6;
                ws.Column(3).Width = 15;
                ws.Column(4).Width = 32;
                ws.Column(5).Width = 12;

                ws.Row(1).Height = 19;
                ws.Row(2).Height = 26;
                ws.Row(3).Height = 27;
                ws.Row(4).Height = 23;
                ws.Row(5).Height = 31;

                //table
                ws.Cells["A13"].Value = "No";
                ws.Cells["B13"].Value = "Item No";
                ws.Cells["D13"].Value = "Item Name";
                ws.Cells["E13"].Value = "Unit";
                ws.Cells["F13"].Value = "Price";

                //Fill data
                var index = 1;
                int normalPart = 0;
                int expPart = 0;

                foreach(InventDto x in dto)
                {
                    ws.Cells["A" + (index + 13)].Value = index;
                    ws.Cells["A" + (index + 13)].Style.Font.Name = "Arial";

                    ws.Cells["B" + (index + 13)].Value = x.itemNo;

                    ws.Cells["B" + (index + 13)].Value = x.itemName;

                    ws.Cells["D" + (index + 13)].Value = x.unit;

                    ws.Cells["E" + (index + 13)].Value = x.price;
                    index++;
                }

                return await pkg.GetAsByteAsync();
            }
        }

        public async Task<byte[]> ExportFileItemAsync(Guid userId, string roleName)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(user == null)
            {
                throw new KeyNotFoundException();
            }

            //get all item for 3 fac
            var data = await _context.Items
                .Join(_context.Department,
                     item => item.DeptId,
                     dept => dept.Id,
                     (item, dept) => new {ite, dept})
                .Select(res => new 
                {
                    itemCode = res.item.ItemCode,
                    enName = res.item.EnName,
                    vnName = res.item.VnName,
                    qty = res.item.Quantity,
                    maker = res.item.Maker,
                    supplier = res.item.Supplier,
                    positionIn = res.item.PositionIn,
                    deptName = res.dept.DeptName,
                    unit = res.item.Unit,
                    cost = res.item.Cost,
                    currency = res.item.Currency,
                    deptId = res.item.DeptId
                })
                .ToListAsync();

            //data with special role

            if(roleName != "dev" && roleName != "gm")
            {
                for(var i = 0; i < data.Count(); i++)
                {
                    if(data[i].deptId != user.DeptId)
                    {
                        data.Remove(data[i]);
                        i--;
                    }
                }
            }

            //Init file
            using (var package = new ExcelPackage())
            {
                var index = 2;
                var ws = package.Workbook.Worksheets.Add("Sheet1");

                //header
                ws.Cells["A1"].Value = "No.";
                ws.Cells["B1"].Value = "Item Code";
                ws.Cells["C1"].Value = "En Name";
                ws.Cells["D1"].Value = "Vn Name";
                ws.Cells["E1"].Value = "Maker";
                ws.Cells["F1"].Value = "Supplier";
                ws.Cells["G1"].Value = "Position_In";
                ws.Cells["H1"].Value = "Quantity";
                ws.Cells["K1"].Value = "Dept";
                ws.Cells["I1"].Value = "Unit";
                ws.Cells["J1"].Value = "Cost";
                ws.Cells["L1"].Value = "Currency";
                
                for(int i = 0; i <= items.Count(); i++)
                {
                    ws.Cells[i + 2, 1].Value = index - 1;
                    ws.Cells[i + 2, 2].Value = items[i].ItemCode;
                    ws.Cells[i + 2, 3].Value = items[i].EnName;
                    ws.Cells[i + 2, 4].Value = items[i].VnName;
                    ws.Cells[i + 2, 5].Value = items[i].Maker;
                    ws.Cells[i + 2, 6].Value = items[i].Supplier;
                    ws.Cells[i + 2, 7].Value = items[i].PositionIn;
                    ws.Cells[i + 2, 8].Value = items[i].Quantity;
                    ws.Cells[i + 2, 9].Value = item[i].deptName.ToUpper() == "TS" ? "Tien Son" : (item[i].deptName.ToUpper() == "TL" ? "Thang Long" : "Que Vo");
                    ws.Cells[i + 2, 10].Value = items[i].Unit;
                    ws.Cells[i + 2, 11].Value = items[i].Cost.ToString().IsNullOrEmpty() ? 0 : items[i].Cost;
                    ws.Cells[i + 2, 12].Value = items[i].Currency;
                    index++;
                }
                return await package.GetAsByteArrayAsync();
            }
        }

        public async Task<byte[]> ExportFileHistoryAsync(List<HistoryDto> dto)
        {
            if(dto == null)
            {
                throw new ArgumentNullException();
            }
            using(var pkg = new ExcelPackage())
            {
                var ws = pkg.Workbook.Worksheets.Add("Sheet1");
                long index = 1;

                //header init
                ws.Cells["A1"].Value = "No";
                ws.Cells["B1"].Value = "Employee Code";
                ws.Cells["C1"].Value = "En Name";
                ws.Cells["D1"].Value = "Vn Name";
                ws.Cells["E1"].Value = "Quantity";
                ws.Cells["F1"].Value = "Kind";
                ws.Cells["G1"].Value = "Reason";
                ws.Cells["H1"].Value = "DateTime";
                ws.Cells["I1"].Value = "Pic Code";
                ws.Cells["J1"].Value = "Pic Name";

                foreach(var x in dto)
                {
                    var temp = x.itemName.Split("|");
                    var pic = x.pic.Split("-");

                    ws.Cells["A" + (index + 1)].Value = index;
                    ws.Cells["B" + (index + 1)].Value = x.empCode;
                    ws.Cells["C" + (index + 1)].Value = temp[0] == null ? "" : temp[0];
                    ws.Cells["D" + (index + 1)].Value = temp[1] == null ? "" : temp[1];
                    ws.Cells["E" + (index + 1)].Value = x.qty;
                    ws.Cells["F" + (index + 1)].Value = x.kind;
                    ws.Cells["G" + (index + 1)].Value = x.reason;
                    ws.Cells["H" + (index + 1)].Value = x.dateAction;
                    ws.Cells["I" + (index + 1)].Value = pic[0] == null ? "" : pic[0];
                    ws.Cells["J" + (index + 1)].Value = pic[1] == null ? "" : pic[1];
                    index++;
                }
                return await pkg.GetAsByteArrayAsync();
            }
        }

        public void RecordUserAction(string recordPath, string? ipClient, string? userName)
        {
            if(File.Exists(recordPath))
            {
                string str = $"{ipClient} --- {userName} --- {DateTime.UtcNow.AddHours(7)}";
            }

            using(StreamWriter = new StreamWriter(recordPath, true))
            {
                writer.WriteLine(str);
            }
        }
    }
}

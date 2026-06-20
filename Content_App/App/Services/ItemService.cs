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

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<List<Item>> GetItemByLocationAsync(Guid deptId, Guid areaId)
        {
            return await _context.Items.Where(item => item.DeptId == deptId && item.AreaId == areaId).ToListAsync();
        }

        public async Task CreateAsync(ItemDto dto)
        {
            _context.Items.Add(new Item
            {
                ItemCode = dto.ItemCode,
                EnName = dto.EnName,
                VnName = dto.VnName,
                Maker = dto.Maker,
                Supplier = dto.Supplier,
                PositionIn = dto.PositionIn,
                Quantity = dto.Quantity,
                DeptId = Guid.NewGuid(),
                AreaId = Guid.NewGuid(),
                Unit = dto.Unit,
                Cost = dto.Cost,
                Image = dto.Image
            });

            await _context.SaveChangesAsync();
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
                item.Cost = item.Cost;
                itemRev.Image = item.Image;

                _context.Items.Update(itemRev);

                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string itemId)
        {
            var item = await _context.Items.FindAsync(new Guid(itemId));
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}

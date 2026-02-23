using Content_App.App.DTOs.Item;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Xml;

namespace Content_App.App.Services
{
    public class ItemService
    {
        private readonly LogDbContext _context;
        
        public ItemService(LogDbContext context)
        {
            this._context = context;
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

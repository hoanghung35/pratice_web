using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class ItemService
    {
        private readonly AppDbContext _db;

        public ItemService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return _db.Items.ToList();
        }
        public async Task<List<Item>> GetByAreaAsync(Guid areaId)
        {
            return await _db.Items.Where(i => i.AreaId == areaId).ToListAsync();
        }

        public async Task CreateAsync(Item dto)
        {
            _db.Items.Add(dto);
            await _db.SaveChangesAsync();
        }
    }
}

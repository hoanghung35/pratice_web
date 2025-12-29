using Content_App.Infrastructure.Data;

namespace Content_App.App.Services
{
    public class ItemService
    {
        private readonly AppDbContext _db;

        public ItemService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ItemDto>> GetByAreaAsync(Guid areaId)
        {
            return await _db.Items
                .Where(i => i.AreaId == areaId)
                .Select(i => new ItemDto
                {
                    ItemCode = i.ItemCode,
                    NameVi = i.NameVi,
                    Quantity = i.Quantity
                })
                .ToListAsync();
        }
    }
}

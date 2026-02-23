using Content_App.App.DTOs.Item;
using Content_App.Infrastructure.Data;
using Content_App.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class ActionService
    {
        private readonly LogDbContext _context;

        public ActionService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task NewItemActionAsync(ItemDto dto)
        {
            _context.LogActions.Add(new LogAction
            {
                EmpCode = "",
                PicId = Guid.NewGuid(),
                ItemId = Guid.NewGuid(),
                Qty = dto.Quantity,
                Kind = "create",
                Reason = "create new item",
                DateAction = DateTime.UtcNow.AddHours(7)
            });
            await _context.SaveChangesAsync();
        }

        public async Task ReceiveItemAsync(ItemActivityDto dto)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == dto.ItemId);

            if(item != null)
            {
                item.Quantity = item.Quantity + dto.Qty;

                _context.Items.Update(item);
                _context.LogActions.Add(new LogAction
                {
                    EmpCode = dto.EmpCode,
                    PicId = dto.PicId,
                    ItemId = dto.ItemId,
                    Qty = dto.Qty,
                    Kind = "receive",
                    Reason = $"reveive {dto.Qty}-{item.Unit}",
                    DateAction = DateTime.UtcNow.AddHours(7)
                });
            }
        }

        public async Task UpdateItemActionAsync(Item item, Guid userId)
        {
            var user = await _context.Accounts.FindAsync(userId);
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
    }
}

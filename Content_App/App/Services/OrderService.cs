using Content_App.App.DTOs.Order;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;

namespace Content_App.App.Services
{
    public class OrderService
    {
        private readonly LogDbContext _context;
        
        public OrderService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task<Guid> CreateOrder(CreateOrderDto dto, Guid userId)
        {
            var order = new OrderItem
            {
                ItemId = dto.ItemId,
                PicId = userId,
                Qty = dto.Qty,
                PlanOrder = dto.PlanOrder,
                Reason = dto.Reason
            };

            var approve = new Approve
            {
                Order = order,
                ItemId = dto.ItemId,
                RequestorId = userId,
                Qty = dto.Qty
            };

            _context.OrderItems.Add(order);
            _context.Approves.Add(approve);

            await _context.SaveChangesAsync();
            return order.Id;
        }
    }
}

using Content_App.App.DTOs.Order;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Content_App.App.Services
{
    public class OrderService
    {
        private readonly LogDbContext _context;
        
        public OrderService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task <List<OrderDto>> GetOrderAsync()
        {
            var data = await _context.OrderItems
                .Join(_context.Items,
                order => order.ItemId,
                item => item.Id,
                (order, item) => new {order, item})
                .Select(res => new
                {
                    id = res.order.Id,
                    enname = res.item.EnName,
                    vnname = res.item.VnName,
                    maker = res.item.Maker,
                    qty = res.item.Quantity,
                    position = res.item.PositionIn,
                    dateOrder = res.order.DateOrder
                })
                .ToListAsync();

            List<OrderDto> res = new List<OrderDto>();

            return res;
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

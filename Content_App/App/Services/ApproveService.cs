using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class ApproveService
    {
        private readonly LogDbContext _context;

        public ApproveService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task ApproveOrder(Guid approveId, Guid adminId, bool flag)
        {
            var approve = await _context.Approves.Include(p => p.Order).FirstAsync(p => p.Id == approveId);
            if(flag)
            {
                approve.Status = "approval";
                var item = await _context.Items.FindAsync(approve.ItemId);

                if(item != null)
                {
                    item.Quantity += approve.Qty;
                }
            }
            else
            {
                approve.Status = "rejected";
            }
        }
    }
}

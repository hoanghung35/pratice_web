using Content_App.App.DTOs.Order;
using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design.Internal;

namespace Content_App.App.Services
{
    public class RequestService
    {
        private readonly LogDbContext _context;
        
        public RequestService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task <List<RequestDto>> GetRequestAsync(Guid picId)
        {
            var data = await _context.Requests
                .Join(_context.Items,
                request => request.ItemId,
                item => item.Id,
                (request, item) => new {request, item})
                
                .Join(_context.Accounts,
                tmp => tmp.request.PicId,
                account => account.Id,
                (tmp, account) => new {tmp, account})

                .Join(_context.Employees,
                tmp2 => tmp2.account.UserCode,
                emp => emp.EmpCode,
                (tmp2, emp) => new {tmp2, emp})

                .Join(_context.Approves,
                tmp3 => tmp3.tmp2.tmp.request.Id,
                approve => approve.RequestId,
                (tmp3, approve) => new {tmp3, approve})

                .Where(res => res.tm3.tmp2.tmp.request.PicId == picId)

                .OrderByDescending(res => res.tmp3.tmp2.tmp.request.DateRequest)
                
                .Select(res => new
                {
                    id = res.tmp3.tmp2.tmp.request.Id,
                    picName = $"{res.tmp3.emp.EmpCode}-{res.tmp3.emp.Fullname}",
                    itemName = $"{res.tmp3.tmp2.tmp.item.EnName}|{res.tmp3.tmp2.tmp/.item.VnName}",
                    qty = res.tmp3.tmp2.tmp.request.Qty,
                    kind = res.tmp3.tmp2.tmp.request.Kind,
                    requestorCode = res.tmp3.tmp2.tmp.request.UCode,
                    requestorName = res.tmp3.tmp2.tmp.request.UName,
                    reason = res.tmp3.tmp2.tmp.request.reason,
                    dateEntry = res.approve.DateEntry,
                    planRequest = res.tmp3.tmp2.tmp.request.PlanRequest == null ? "" : res.tmp3.tmp2.tmp.request.PlanRequest.ToString(),
                    dateRequest = res.tmp3.tmp2.tmp.request.DateRequest.ToString()),
                    status = res.approve.Status
                })
                .ToListAsync();

            List<RequestDto> requests = new List<RequestDto>();

            if(data.Count() == 0)
            {
                return requests;
            }

            string snew;

            foreach(var request in data)
            {
                int totalDay = 0;
                if(request.dateEntry != null)
                {
                    DateTime now = DateTime.UtcNow.AddHours(7);
                    TimeSpan tmp = (TimeSpan)(now - request.dateEntry!);
                    totalDay = tmp.Days;
                }

                bool isShowApprove = (request.status == "rejected" || request.status == "approved") && totalDay <= VariableConstant.totalDayKeep;

                if(request.status != "rejected" && request.status != "approved" || isShowApproval)
                {
                    snew = "";
                    switch(request.status)
                    {
                        case "pending":
                            {
                                snew = "Pending";
                                break;
                            }
                        case "rejected":
                            {
                                snew = "Rejected";
                                break;
                            }
                        case "approved":
                            {
                                snew = "Approved";
                                break;
                            }
                        case "past":
                            {
                                snew = "Wait Mgr approve";
                                break;
                            }
                    }

                    requests.Add(new RequestDto
                    {
                        id = request.id,
                        picName = request.picName,
                        itemName = request.itemName,
                        qty = request.qty,
                        kind = request.kind!,
                        requestorName = request.requestorName!,
                        requestorCode = request.requestorCode!,
                        reason = request.reason,
                        dateRequest = request.dateRequest!,
                        planDate = request.PlanRequest!,
                        status = snew
                    });
                }
            }

            return requests;
        } 
        public async Task<Guid> CreateRequestAsync(CreateRequestDto dto, Guid picId)
        {
            var requestId = Guid.NewGuid();

            while(true)
            {
                var req = await _context.Requests.FindAsync(requestId);

                if(req == null) break;

                requestId = Guid.NewGuid();
            }
            var request = new Request
            {
                Id = requestId
                ItemId = dto.ItemId,
                PicId = picId,
                Qty = dto.Qty,
                Reason = dto.Reason,
                Kind = "delivery",
                UCode = dto.Ucode,
                UName = dto.Uname
            };

            _context.Requests.Add(request);

            await _context.SaveChangesAsync();
            return requestId;
        }
    }
}

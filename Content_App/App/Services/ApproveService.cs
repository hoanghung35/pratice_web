using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class ApproveService
    {
        private readonly LogDbContext _context;
        private readonly DateConverter _dateConvert;

        public ApproveService(LogDbContext context, DateConverter dateConvert)
        {
            this._context = context;
            this._dateConvert = dateConvert;
        }

        public async Task<List<ApproveDto>> GetApproveAsync(Guid userId, string roleName)
        {
            var user = await _context.Employees.FindAsync(userId);

            if(user == null)
            {
                throw new KeyNotFoundException();
            }

            var data = await _context.Approves
                .Join(_context.Requests,
                approve => approve.RequestId,
                request => request.Id,
                (approve, request) => new { approve, request})

                .Join(_context.Items,
                tmp => tmp.request.ItemId,
                item => item.Id,
                (tmp, item) => new {tmp, item})

                .Join(_context.Accounts,
                tmp2 => tmp2.tmp.request.PicId,
                account => account.Id,
                (tmp2, account) => new {tmp2, account})

                .Join(_context.Employees,
                ftmp => ftmp.account.UserCode,
                emp => emp.EmpCode,
                (ftmp, emp) => new {ftmp, emp})

                .Join(_context.Departments,
                fntmp => fntmp.emp.DeptId,
                department => department.Id,
                (fntmp, department) => new {fntmp, department})

                .Where(x => x.fntmp.ftmp.tmp2.tmp.approve.Status != "rejected" &&
                            x.fntmp.ftmp.tmp2.tmp.request.Status != "approved")
                .OrderByDescending(x => x.fntmp.ftmp.tmp2.tmp.request.DateRequest)

                .Select(res => new
                {
                    approveId = res.fntmp.ftmp.tmp2.tmp.approve.Id,
                    requestorCode = res.fntmp.ftmp.tmp2.tmp.request.UCode,
                    requestorName = res.fntmp.ftmp.tmp2.tmp.request.UName,
                    picName = $"{res.fntmp.ftmp.account.UserCode} - {res.fntmp.emp.FullName}",
                    itemName = $"{res.fntmp.ftmp.tmp2.item.EnName}|{res.fntmp.ftmp.tmp2.item.VnName}",
                    qty = res.fntmp.ftmp.tmp2.tmp.request.Qty,
                    dept = res.department.DeptName,
                    kind = res.fntmp.ftmp.tmp2.tmp.request.Kind,
                    purpose = res.fntmp.ftmp.tmp2.tmp.request.Reason,
                    planRequest = res.fntmp.ftmp.tmp2.tmp.request.PlanRequest,
                    dateRequest = res.fntmp.ftmp.tmp2.tmp.request.DateRequest,
                    status = res.fntmp.ftmp.tmp2.tmp.approve.Status,
                    deptId = res.department.Id
                })
                .ToListAsync();

            if(data == null)
            {
                throw new ArgumentOutOfRangeException();
            }

            List<ApproveDto> approves = new List<ApproveDto>{ };

            foreach(var approve in data)
            {
                approves.Add(new ApproveDto
                {
                    id = approve.approveId,
                    picName = approve.picName,
                    dept = appprove.dept,
                    itemName = approve.itemName,
                    qty = approve.qty,
                    kind = approve.kind ?? "",
                    requestorCode = approve.requestorCode ?? "",
                    requestorName = approve.requestorName ?? "",
                    purpose = approve.purpose!,
                    dateRequest = approve.dateRequest.ToString() == "" ? "" : _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                    planRequest = _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                    status = approve.status
                });
            }

            //role G</dev ==> all
            if(roleName == "gm" || roleName == "dev")
            {
                return approves;
            }

            //filter with: manager/super ==> each factory
            approves.Clear();

            if(roleName == "manager")
            {
                foreach(var approve in data)
                {
                    if(approve.deptId == user.DeptId && approve.status == "past")
                    {
                        approves.Add(new ApproveDto
                        {
                            id = approve.approveId,
                            picName = approve.picName,
                            dept = appprove.dept,
                            itemName = approve.itemName,
                            qty = approve.qty,
                            kind = approve.kind ?? "",
                            requestorCode = approve.requestorCode ?? "",
                            requestorName = approve.requestorName ?? "",
                            purpose = approve.purpose!,
                            dateRequest = approve.dateRequest.ToString() == "" ? "" : _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                            planRequest = _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                            status = approve.status
                        });
                    }
                }
                return approves;
            }

            //super
            approves.Clear();
            foreach(var approve in data)
            {
                if(approve.deptId == user.DeptId && approve.status == "past")
                {
                    approves.Add(new ApproveDto
                    {
                        id = approve.approveId,
                        picName = approve.picName,
                        dept = appprove.dept,
                        itemName = approve.itemName,
                        qty = approve.qty,
                        kind = approve.kind ?? "",
                        requestorCode = approve.requestorCode ?? "",
                        requestorName = approve.requestorName ?? "",
                        purpose = approve.purpose!,
                        dateRequest = approve.dateRequest.ToString() == "" ? "" : _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                        planRequest = _dateConvert.D_Convert((DateTime)approve.dateRequest!),
                        status = approve.status
                    });
                }
            }
            return approves;
        }

        public async Task CreateApproveAsync(ApproveDto? dto, Guid reqId)
        {
            if(dto == null)
            {
                _context.Approves.Add(new Approve
                {
                    RequestId = reqId,
                    Status = "pending"
                });
            }
            await _context.SaveChangesAsync();
        }

        public async Task ApproveRequestAsync(Guid approveId, stirng roleName)
        {
            var approve = await _context.Approves.FindAsync(approveId);
            bool isFullAccess = roleName == "dev" || roleName == "gm" || roleName == "manager";

            if(approve == null)
            {
                throw new KeyNotFoundException();
            }

            var req = await _context.Requests.FindAsync(approve.RequestId);

            if(isFullAccess)
            {
                approve.Status == "approved";
            }
            else
            {
                approve.Status = "past";
            }

            approve.DateEntry = _dateConvert.D_TimeStampNow_Unspecified();
            req!.PlanRequest = approve.DateEntry;

            _context.Approves.Update(approve);

            await _context.SaveChangeAsync();
        }
    }
}

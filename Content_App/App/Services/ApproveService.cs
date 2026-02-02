using Content_App.App.DTOs;

namespace Content_App.App.Services
{
    public class ApproveService
    {
        public async Task<ReturnDto> GetPendingAsync()
        {
            return new ReturnDto { id = "" };
        }

        public async Task ApproveAsync(Guid id)
        {

        }

        public async Task RejectAsync (Guid id, string s)
        {

        }
    }
}

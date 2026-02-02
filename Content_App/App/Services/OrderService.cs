using Content_App.App.DTOs;

namespace Content_App.App.Services
{
    public class OrderService
    {
        public async Task CreateAsync(CreateOrderDto dto)
        {

        }

        public async Task<CreateOrderDto> GetByUserAsync()
        {
            return new CreateOrderDto { };
        }
    }
}

using Content_App.Domain.Entities;
using Content_App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Content_App.App.Services
{
    public class CurrencyService
    {
        private readonly LogDbContext _context;

        public CurrencyService(LogDbContext context)
        {
            this._context = context;
        }

        public async Task<List<Currency>> GetCurrencyAsync()
        {
          var currencies = await _context.Currencies.ToListAsync();
    
          if(currencies.Count() != 0)
          {
              foreach(var currency in currencies)
              {
                  currency.CurrentName = currency.CurrentName == null ? "" " currency.CurrentName.ToUpper();
              }
          }
          return currencies;
        }

        public async Task CreateNewCurrencyAsync(Currency dto)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(c => c.CurrentName!.ToLower() == dto.CurrentName!.ToLower());

            if(currency != null)
            {
                throw new ArgumentException();
            }

            Guid newId = await CheckGenCurrencyId(dto.Id);
            await _context.Currencies.AddAsync(new Currency
            {
                Id = newId,
                CurrentName = dto.CurrentName!.ToLower(),
              ExchangeRate = dto.ExchangeRate
            });

            await _context.SaveChangesAsync();
        }

        public async Task UpdateCurrency(Currency dto)
        {
            var currency = await _context.Currencies.FindAsync(dto.Id)
            if(currency == null)
            {
                throw new ArgumentException();
            }

            currency.ExchangeRate = dto.ExchangeRate;
            _context.Currencies.Upate(currency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCurrencyAsync(Guid id)
        {
            var currency = await _context.Currencies.FindAsync(id);
            if(currency == null)
            {
                throw new ArgumentException();
            }

            _context.Currencies.Remove(currency);

            await _context.SaveChangesAsync();
        }

        private async Task<Guid> CheckGenCurrencyId(Guid curId)
        {
            var currencies = await _context.Currencies.FindAsync(curId);
            if(currencies == null) return curId;

            var newId = Guid.NewGuid();

            while(true)
            {
                var currency = await _context.Currencies.FindAsync(newId)
                if(currency == null) break;
                newId = Guid.NewGuid();
            }

            return newGuid;
        }
    }
}

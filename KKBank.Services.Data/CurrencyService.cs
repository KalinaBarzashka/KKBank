using KKBank.Data;
using System.Collections.Generic;
using System.Linq;

namespace KKBank.Services.Data
{
    public class CurrencyService : ICurrencyService
    {
        private readonly ApplicationDbContext dbContext;

        public CurrencyService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<KeyValuePair<string, string>> GetAllActiveCurrencyAsKeyValuePairs()
        {
            return this.dbContext.Currency.Where(x => x.IsDeleted_17118069 == false).Select(x => new
            {
                x.Id, x.CurrencyAbbreviation
            })
            .ToList()
            .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), x.CurrencyAbbreviation)).ToList();
        }

        public string GetCurrencyAbbrivByCurrencyId(int currencyId)
        {
            return dbContext.Currency.Where(x => x.Id == currencyId).Select(x => x.CurrencyAbbreviation).FirstOrDefault();
        }

        public IEnumerable<int> GetAllActiveCurrencyIds()
        {
            return this.dbContext.Currency
                .Where(x => x.IsDeleted_17118069 == false)
                .Select(x => x.Id)
                .ToList();
        }
    }
}

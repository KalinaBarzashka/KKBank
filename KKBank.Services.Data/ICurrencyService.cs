using System.Collections.Generic;

namespace KKBank.Services.Data
{
    public interface ICurrencyService
    {
        IEnumerable<KeyValuePair<string, string>> GetAllActiveCurrencyAsKeyValuePairs();

        public string GetCurrencyAbbrivByCurrencyId(int currencyId);

        public IEnumerable<int> GetAllActiveCurrencyIds();
    }
}

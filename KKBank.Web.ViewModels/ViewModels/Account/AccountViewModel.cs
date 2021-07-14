namespace KKBank.Web.ViewModels.ViewModels.Account
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string AccountTypeName { get; set; }

        public decimal Available { get; set; }

        public decimal BlockedАmount { get; set; }

        public string IBAN { get; set; }

        public decimal Balance
        {
            get
            {
                return this.Available + this.BlockedАmount;
            }
        }

        public string CurrencyAbbreviation { get; set; }
    }
}

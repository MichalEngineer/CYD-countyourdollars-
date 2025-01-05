using CYD.Services;

namespace CYD.Pages
{
    public partial class AboutPage : ContentPage
    {
        private CoinGeckoService _coinGeckoService;

        public AboutPage()
        {
            InitializeComponent();
            _coinGeckoService = new CoinGeckoService();
            BindingContext = this;
            LoadDataAsync();
        }

        public List<CryptoCurrency> Cryptocurrencies { get; set; } = new();

        private async void LoadDataAsync()
        {
            Cryptocurrencies = await _coinGeckoService.GetTopCryptocurrenciesAsync();
            OnPropertyChanged(nameof(Cryptocurrencies));
        }
    }
}

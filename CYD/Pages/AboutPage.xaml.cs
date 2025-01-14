using CYD.Services; // <--- U�ycie serwisu do pobierania danych o kryptowalutach

namespace CYD.Pages
{
    public partial class AboutPage : ContentPage
    {
        private CoinGeckoService _coinGeckoService; // <--- Inicjalizacja zmiennej przechowuj�cej us�ug� CoinGeckoService, odpowiedzialn� za pobieranie danych o kryptowalutach

        // Konstruktor strony AboutPage
        public AboutPage()
        {
            InitializeComponent(); // <--- Inicjalizacja komponent�w strony (np. UI)

            _coinGeckoService = new CoinGeckoService(); // <--- Tworzenie instancji CoinGeckoService, kt�ra b�dzie u�ywana do pobierania danych

            BindingContext = this; // <--- Ustawienie kontekstu powi�zania (BindingContext) na bie��cy obiekt (AboutPage), umo�liwia bindowanie danych do UI
            LoadDataAsync(); // <--- Wywo�anie metody asynchronicznej w celu za�adowania danych kryptowalut
        }

        // W�a�ciwo�� przechowuj�ca list� kryptowalut
        public List<CryptoCurrency> Cryptocurrencies { get; set; } = new(); // <--- Lista kryptowalut, kt�ra b�dzie powi�zana z UI

        // Asynchroniczna metoda do �adowania danych kryptowalut
        private async void LoadDataAsync()
        {
            Cryptocurrencies = await _coinGeckoService.GetTopCryptocurrenciesAsync(); // <--- Pobiera dane o kryptowalutach z serwisu CoinGecko
            OnPropertyChanged(nameof(Cryptocurrencies)); // <--- Powiadamia o zmianie w�a�ciwo�ci Cryptocurrencies, co od�wie�a UI
        }
    }
}

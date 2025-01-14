using CYD.Services; // <--- U¿ycie serwisu do pobierania danych o kryptowalutach

namespace CYD.Pages
{
    public partial class AboutPage : ContentPage
    {
        private CoinGeckoService _coinGeckoService; // <--- Inicjalizacja zmiennej przechowuj¹cej us³ugê CoinGeckoService, odpowiedzialn¹ za pobieranie danych o kryptowalutach

        // Konstruktor strony AboutPage
        public AboutPage()
        {
            InitializeComponent(); // <--- Inicjalizacja komponentów strony (np. UI)

            _coinGeckoService = new CoinGeckoService(); // <--- Tworzenie instancji CoinGeckoService, która bêdzie u¿ywana do pobierania danych

            BindingContext = this; // <--- Ustawienie kontekstu powi¹zania (BindingContext) na bie¿¹cy obiekt (AboutPage), umo¿liwia bindowanie danych do UI
            LoadDataAsync(); // <--- Wywo³anie metody asynchronicznej w celu za³adowania danych kryptowalut
        }

        // W³aœciwoœæ przechowuj¹ca listê kryptowalut
        public List<CryptoCurrency> Cryptocurrencies { get; set; } = new(); // <--- Lista kryptowalut, która bêdzie powi¹zana z UI

        // Asynchroniczna metoda do ³adowania danych kryptowalut
        private async void LoadDataAsync()
        {
            Cryptocurrencies = await _coinGeckoService.GetTopCryptocurrenciesAsync(); // <--- Pobiera dane o kryptowalutach z serwisu CoinGecko
            OnPropertyChanged(nameof(Cryptocurrencies)); // <--- Powiadamia o zmianie w³aœciwoœci Cryptocurrencies, co odœwie¿a UI
        }
    }
}

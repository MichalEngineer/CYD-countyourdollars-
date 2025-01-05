using CYD.Services;

namespace CYD
{
    public partial class App : Application
    {
        private NavigationPage _navigationPage;
        private readonly CurrencyLayerService _currencyLayerService;

        public static Dictionary<string, decimal> CurrencyRates { get; private set; }

        public App()
        {
            InitializeComponent();

            _currencyLayerService = new CurrencyLayerService();

            // Ładowanie danych przed wyświetleniem LoginPage
            LoadInitialDataAsync();

            _navigationPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Colors.Transparent,
                BarTextColor = Colors.Transparent
            };
            SubscribeToLoginSuccessful();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(_navigationPage);
            return window;
        }

        private void SubscribeToLoginSuccessful()
        {
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                // Zmiana strony na AppShell po udanym logowaniu
                _navigationPage.PushAsync(new AppShell());
            };
        }

        private async void LoadInitialDataAsync()
        {
            try
            {
                // Pobieranie danych z CurrencyLayer API
                CurrencyRates = await _currencyLayerService.GetTopCurrenciesAsync("USD");

                if (CurrencyRates != null)
                {
                    Console.WriteLine("Currency data successfully loaded.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency data: {ex.Message}");
            }
        }
    }
}

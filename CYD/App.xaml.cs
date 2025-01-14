using CYD.Services;
using Plugin.Fingerprint;

namespace CYD
{
    /**
     * @class App
     * @brief Główna klasa aplikacji zarządzająca nawigacją i danymi.
     */
    public partial class App : Application
    {
        private NavigationPage _navigationPage; ///< Główna strona nawigacyjna aplikacji.
        private readonly CurrencyLayerService _currencyLayerService; ///< Serwis do pobierania danych o walutach.

        public static Dictionary<string, decimal> CurrencyRates { get; private set; } ///< Aktualne kursy walut.

        /**
         * @brief Konstruktor aplikacji.
         * 
         * Inicjalizuje komponenty, ładuje dane walutowe i ustawia stronę logowania.
         */
        public App()
        {
            InitializeComponent(); //<--- Inicjalizacja komponentów UI.
            _currencyLayerService = new CurrencyLayerService(); //<--- Inicjalizacja serwisu walutowego.
            LoadInitialDataAsync(); //<--- Asynchroniczne ładowanie danych walutowych.

            var loginPage = new LoginPage(CrossFingerprint.Current); //<--- Tworzenie strony logowania z uwierzytelnianiem odcisków palców.

            _navigationPage = new NavigationPage(loginPage)
            {
                BarBackgroundColor = Colors.Transparent, //<--- Przezroczysty kolor paska nawigacji.
                BarTextColor = Colors.Transparent //<--- Przezroczysty kolor tekstu paska nawigacji.
            };

            SubscribeToLoginSuccessful(); //<--- Subskrypcja zdarzenia pomyślnego logowania.
            MainPage = _navigationPage; //<--- Ustawienie głównej strony aplikacji.
        }

        /**
         * @brief Tworzy okno aplikacji.
         * @param activationState Stan aktywacji aplikacji.
         * @return Zwraca główne okno aplikacji.
         */
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_navigationPage); //<--- Tworzenie nowego okna aplikacji.
        }

        /**
         * @brief Subskrybuje zdarzenie pomyślnego logowania.
         */
        private void SubscribeToLoginSuccessful()
        {
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                _navigationPage.PushAsync(new AppShell()); //<--- Nawigacja do głównej powłoki aplikacji.
            };
        }

        /**
         * @brief Ładuje początkowe dane walutowe asynchronicznie.
         */
        private async void LoadInitialDataAsync()
        {
            try
            {
                CurrencyRates = await _currencyLayerService.GetTopCurrenciesAsync("USD"); //<--- Pobieranie kursów walut z API.

                if (CurrencyRates != null)
                {
                    Console.WriteLine("Currency data successfully loaded."); //<--- Informacja o załadowaniu danych.
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency data: {ex.Message}"); //<--- Obsługa błędu podczas ładowania danych.
            }
        }
    }
}

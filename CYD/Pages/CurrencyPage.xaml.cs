using Microsoft.Maui.Controls; // <--- U¿ycie przestrzeni nazw do kontrolki w aplikacji Maui
using System; // <--- U¿ycie przestrzeni nazw do ogólnych operacji systemowych
using System.Collections.Generic; // <--- U¿ycie przestrzeni nazw do operacji na kolekcjach
using System.Linq; // <--- U¿ycie przestrzeni nazw do operacji na kolekcjach
using System.Threading.Tasks; // <--- U¿ycie przestrzeni nazw do operacji asynchronicznych

namespace CYD.Pages
{
    /** @brief Strona wyœwietlaj¹ca kursy walut */
    public partial class CurrencyPage : ContentPage
    {
        public List<CurrencyRate> CurrencyRates { get; set; } // <--- Lista obiektów CurrencyRate, zawieraj¹ca kursy walut

        /** @brief Konstruktor strony walut */
        public CurrencyPage()
        {
            InitializeComponent(); // <--- Inicjalizacja komponentów strony
            BindingContext = this; // <--- Ustawienie bie¿¹cej strony jako kontekst powi¹zania

            // Asynchronicznie za³aduj dane walut
            LoadCurrencyRatesAsync(); // <--- Wywo³anie asynchronicznej metody wczytania kursów walut
        }

        /** @brief Metoda asynchroniczna ³aduj¹ca dane o kursach walut */
        private async void LoadCurrencyRatesAsync()
        {
            try
            {
                // U¿ywamy CurrencyLayerService do pobrania walut
                var service = new Services.CurrencyLayerService(); // <--- Inicjalizacja serwisu do pobierania kursów walut
                var rates = await service.GetTopCurrenciesAsync("USD", 5); // <--- Pobierz tylko 5 najlepszych walut wzglêdem USD

                if (rates != null && rates.Any()) // <--- Sprawdzamy, czy dane zosta³y poprawnie pobrane
                {
                    LoadCurrencyRates(rates); // <--- Za³aduj dane walut do widoku
                }
                else
                {
                    await DisplayAlert("Error", "No currency rates available.", "OK"); // <--- Wyœwietlamy komunikat o braku dostêpnych danych
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency rates: {ex.Message}"); // <--- Zapisujemy b³¹d w konsoli
                await DisplayAlert("Error", "Failed to load currency rates. Please try again later.", "OK"); // <--- Wyœwietlamy komunikat o b³êdzie
            }
        }

        /** @brief Metoda do za³adowania kursów walut do listy */
        private void LoadCurrencyRates(Dictionary<string, decimal> rates)
        {
            // Przekszta³camy dane w listê do powi¹zania
            CurrencyRates = rates.Select(rate => new CurrencyRate
            {
                Key = rate.Key, // <--- Przypisanie klucza (nazwa waluty)
                Value = rate.Value // <--- Przypisanie wartoœci (kurs waluty)
            }).ToList();

            // Powiadomienie o zmianie danych
            OnPropertyChanged(nameof(CurrencyRates)); // <--- Powiadomienie widoku o zmianie danych
        }
    }

    /** @brief Klasa reprezentuj¹ca pojedynczy kurs waluty */
    public class CurrencyRate
    {
        public string Key { get; set; } // <--- Klucz waluty (np. "USD")
        public decimal Value { get; set; } // <--- Wartoœæ kursu waluty

        /** @brief Formatuje wartoœæ kursu waluty do wyœwietlenia */
        public string DisplayValue => $"{Key}: {Value:F2}"; // <--- Formatowanie do wyœwietlenia jako "USD: 1.23"
    }
}

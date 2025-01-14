using Microsoft.Maui.Controls; // <--- U�ycie przestrzeni nazw do kontrolki w aplikacji Maui
using System; // <--- U�ycie przestrzeni nazw do og�lnych operacji systemowych
using System.Collections.Generic; // <--- U�ycie przestrzeni nazw do operacji na kolekcjach
using System.Linq; // <--- U�ycie przestrzeni nazw do operacji na kolekcjach
using System.Threading.Tasks; // <--- U�ycie przestrzeni nazw do operacji asynchronicznych

namespace CYD.Pages
{
    /** @brief Strona wy�wietlaj�ca kursy walut */
    public partial class CurrencyPage : ContentPage
    {
        public List<CurrencyRate> CurrencyRates { get; set; } // <--- Lista obiekt�w CurrencyRate, zawieraj�ca kursy walut

        /** @brief Konstruktor strony walut */
        public CurrencyPage()
        {
            InitializeComponent(); // <--- Inicjalizacja komponent�w strony
            BindingContext = this; // <--- Ustawienie bie��cej strony jako kontekst powi�zania

            // Asynchronicznie za�aduj dane walut
            LoadCurrencyRatesAsync(); // <--- Wywo�anie asynchronicznej metody wczytania kurs�w walut
        }

        /** @brief Metoda asynchroniczna �aduj�ca dane o kursach walut */
        private async void LoadCurrencyRatesAsync()
        {
            try
            {
                // U�ywamy CurrencyLayerService do pobrania walut
                var service = new Services.CurrencyLayerService(); // <--- Inicjalizacja serwisu do pobierania kurs�w walut
                var rates = await service.GetTopCurrenciesAsync("USD", 5); // <--- Pobierz tylko 5 najlepszych walut wzgl�dem USD

                if (rates != null && rates.Any()) // <--- Sprawdzamy, czy dane zosta�y poprawnie pobrane
                {
                    LoadCurrencyRates(rates); // <--- Za�aduj dane walut do widoku
                }
                else
                {
                    await DisplayAlert("Error", "No currency rates available.", "OK"); // <--- Wy�wietlamy komunikat o braku dost�pnych danych
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency rates: {ex.Message}"); // <--- Zapisujemy b��d w konsoli
                await DisplayAlert("Error", "Failed to load currency rates. Please try again later.", "OK"); // <--- Wy�wietlamy komunikat o b��dzie
            }
        }

        /** @brief Metoda do za�adowania kurs�w walut do listy */
        private void LoadCurrencyRates(Dictionary<string, decimal> rates)
        {
            // Przekszta�camy dane w list� do powi�zania
            CurrencyRates = rates.Select(rate => new CurrencyRate
            {
                Key = rate.Key, // <--- Przypisanie klucza (nazwa waluty)
                Value = rate.Value // <--- Przypisanie warto�ci (kurs waluty)
            }).ToList();

            // Powiadomienie o zmianie danych
            OnPropertyChanged(nameof(CurrencyRates)); // <--- Powiadomienie widoku o zmianie danych
        }
    }

    /** @brief Klasa reprezentuj�ca pojedynczy kurs waluty */
    public class CurrencyRate
    {
        public string Key { get; set; } // <--- Klucz waluty (np. "USD")
        public decimal Value { get; set; } // <--- Warto�� kursu waluty

        /** @brief Formatuje warto�� kursu waluty do wy�wietlenia */
        public string DisplayValue => $"{Key}: {Value:F2}"; // <--- Formatowanie do wy�wietlenia jako "USD: 1.23"
    }
}

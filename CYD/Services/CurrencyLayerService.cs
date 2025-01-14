using System; // <--- Użycie przestrzeni nazw System, potrzebne dla podstawowych klas, takich jak Uri czy Exception
using System.Collections.Generic; // <--- Użycie przestrzeni nazw do pracy z kolekcjami, takimi jak słowniki i listy
using System.Linq; // <--- Użycie przestrzeni nazw do LINQ, umożliwia operacje na kolekcjach
using System.Net.Http; // <--- Użycie przestrzeni nazw do komunikacji HTTP (pobieranie danych z API)
using System.Net.Http.Json; // <--- Użycie rozszerzenia HttpClient do pracy z danymi w formacie JSON
using System.Text.Json.Serialization; // <--- Użycie przestrzeni nazw do serializacji JSON
using System.Threading.Tasks; // <--- Użycie przestrzeni nazw do operacji asynchronicznych

namespace CYD.Services
{
    public class CurrencyLayerService
    {
        private readonly HttpClient _httpClient; // <--- Obiekt HttpClient, który będzie wykorzystywany do wysyłania zapytań HTTP
        private const string ApiKey = "42c7f538c6126b3d6b10018c"; // <--- Klucz API, który będzie używany do uwierzytelniania zapytań do API
        private const string BaseUrl = "https://v6.exchangerate-api.com/v6/"; // <--- Podstawowy URL API do pobierania kursów walut

        // Konstruktor inicjalizujący HttpClient z podstawowym adresem URL
        public CurrencyLayerService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl) // <--- Ustawienie podstawowego URL dla HttpClient
            };
        }

        // Asynchroniczna metoda pobierająca top X walut na podstawie wybranej waluty bazowej
        public async Task<Dictionary<string, decimal>> GetTopCurrenciesAsync(string baseCurrency = "USD", int count = 5)
        {
            try
            {
                var endpoint = $"{ApiKey}/latest/{baseCurrency}"; // <--- Tworzymy pełny endpoint z kluczem API i walutą bazową
                var response = await _httpClient.GetFromJsonAsync<CurrencyRatesResponse>(endpoint); // <--- Wysyłamy zapytanie do API i deserializujemy odpowiedź do obiektu CurrencyRatesResponse

                if (response?.ConversionRates != null) // <--- Sprawdzamy, czy odpowiedź zawiera dane o kursach walut
                {
                    Console.WriteLine("API Request Successful."); // <--- Informujemy, że zapytanie API powiodło się
                    // Ograniczamy do top X walut
                    return response.ConversionRates // <--- Zwracamy konwersje walut
                        .Take(count) // <--- Pobieramy tylko pierwsze 'count' walut
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value); // <--- Zamieniamy wynik na słownik
                }

                Console.WriteLine("API response is null or invalid."); // <--- Informujemy o problemach z odpowiedzią API
                return new Dictionary<string, decimal>(); // <--- Zwracamy pusty słownik w przypadku braku wyników
            }
            catch (Exception ex) // <--- Łapanie wyjątków, które mogą wystąpić podczas zapytania HTTP
            {
                Console.WriteLine($"Error fetching currency rates: {ex.Message}"); // <--- Wyświetlanie błędu w przypadku niepowodzenia zapytania
                return new Dictionary<string, decimal>(); // <--- Zwracamy pusty słownik w przypadku błędu
            }
        }

        // Klasa pomocnicza do deserializacji odpowiedzi API
        public class CurrencyRatesResponse
        {
            [JsonPropertyName("conversion_rates")] // <--- Atrybut pozwalający zmapować pole w odpowiedzi JSON na właściwość ConversionRates
            public Dictionary<string, decimal> ConversionRates { get; set; } // <--- Słownik przechowujący kursy walut (klucz: waluta, wartość: kurs)
        }
    }
}

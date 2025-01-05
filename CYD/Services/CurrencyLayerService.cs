using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CYD.Services
{
    public class CurrencyLayerService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "42c7f538c6126b3d6b10018c"; // Twój klucz API
        private const string BaseUrl = "https://v6.exchangerate-api.com/v6/";

        public CurrencyLayerService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<Dictionary<string, decimal>> GetTopCurrenciesAsync(string baseCurrency = "USD", int count = 5)
        {
            try
            {
                var endpoint = $"{ApiKey}/latest/{baseCurrency}";
                var response = await _httpClient.GetFromJsonAsync<CurrencyRatesResponse>(endpoint);

                if (response?.ConversionRates != null)
                {
                    Console.WriteLine("API Request Successful.");
                    // Ograniczamy do top X walut
                    return response.ConversionRates
                        .Take(count)
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
                }

                Console.WriteLine("API response is null or invalid.");
                return new Dictionary<string, decimal>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching currency rates: {ex.Message}");
                return new Dictionary<string, decimal>();
            }
        }

        public class CurrencyRatesResponse
        {
            [JsonPropertyName("conversion_rates")]
            public Dictionary<string, decimal> ConversionRates { get; set; }
        }
    }
}

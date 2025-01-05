using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace CYD.Services
{
    public class CoinGeckoService
    {
        private readonly HttpClient _httpClient;

        public CoinGeckoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.coingecko.com/api/v3/")
            };
        }

        public async Task<List<CryptoCurrency>> GetTopCryptocurrenciesAsync(int count = 10)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<CryptoCurrency>>($"coins/markets?vs_currency=usd&order=market_cap_desc&per_page={count}&page=1&sparkline=false");
                return result ?? new List<CryptoCurrency>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from CoinGecko: {ex.Message}");
                return new List<CryptoCurrency>();
            }
        }
    }

    public class CryptoCurrency
    {
        public string Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public decimal Current_price { get; set; }
        public string Image { get; set; }
    }
}

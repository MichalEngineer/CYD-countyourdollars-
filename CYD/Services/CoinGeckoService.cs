using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace CYD.Services
{
    /**
     * Service for interacting with the CoinGecko API to fetch cryptocurrency data.
     */
    public class CoinGeckoService
    {
        private readonly HttpClient _httpClient; //<--- HTTP client for making requests to CoinGecko API.

        /**
         * Initializes a new instance of the CoinGeckoService class.
         */
        public CoinGeckoService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.coingecko.com/api/v3/") //<--- Base address for the CoinGecko API.
            };
        }

        /**
         * Fetches the top cryptocurrencies sorted by market cap.
         * @param count The number of cryptocurrencies to fetch. Default is 10.
         * @return A list of top cryptocurrencies.
         */
        public async Task<List<CryptoCurrency>> GetTopCryptocurrenciesAsync(int count = 10)
        {
            try
            {
                // Fetching cryptocurrency data from CoinGecko API.
                var result = await _httpClient.GetFromJsonAsync<List<CryptoCurrency>>($"coins/markets?vs_currency=usd&order=market_cap_desc&per_page={count}&page=1&sparkline=false");
                return result ?? new List<CryptoCurrency>(); //<--- Return fetched data or an empty list if result is null.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching data from CoinGecko: {ex.Message}"); //<--- Log the error message.
                return new List<CryptoCurrency>(); //<--- Return an empty list in case of error.
            }
        }
    }

    /**
     * Represents a cryptocurrency with basic information.
     */
    public class CryptoCurrency
    {
        public string Id { get; set; } //<--- The unique identifier for the cryptocurrency.
        public string Symbol { get; set; } //<--- The ticker symbol for the cryptocurrency.
        public string Name { get; set; } //<--- The name of the cryptocurrency.
        public decimal Current_price { get; set; } //<--- The current price of the cryptocurrency.
        public string Image { get; set; } //<--- The URL of the cryptocurrency's image.
    }
}

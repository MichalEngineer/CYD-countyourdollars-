using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CYD.Pages
{
    public partial class CurrencyPage : ContentPage
    {
        public List<CurrencyRate> CurrencyRates { get; set; }

        public CurrencyPage()
        {
            InitializeComponent();
            BindingContext = this;

            // Asynchronicznie za³aduj dane walut
            LoadCurrencyRatesAsync();
        }

        private async void LoadCurrencyRatesAsync()
        {
            try
            {
                // U¿ywamy CurrencyLayerService do pobrania walut
                var service = new Services.CurrencyLayerService();
                var rates = await service.GetTopCurrenciesAsync("USD", 5); // Pobierz tylko 5 walut

                if (rates != null && rates.Any())
                {
                    LoadCurrencyRates(rates);
                }
                else
                {
                    await DisplayAlert("Error", "No currency rates available.", "OK");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading currency rates: {ex.Message}");
                await DisplayAlert("Error", "Failed to load currency rates. Please try again later.", "OK");
            }
        }

        private void LoadCurrencyRates(Dictionary<string, decimal> rates)
        {
            // Przekszta³camy dane w listê do powi¹zania
            CurrencyRates = rates.Select(rate => new CurrencyRate
            {
                Key = rate.Key,
                Value = rate.Value
            }).ToList();

            // Powiadomienie o zmianie danych
            OnPropertyChanged(nameof(CurrencyRates));
        }
    }

    public class CurrencyRate
    {
        public string Key { get; set; }
        public decimal Value { get; set; }

        // Dodatkowa w³aœciwoœæ formatowania do wyœwietlania na stronie
        public string DisplayValue => $"{Key}: {Value:F2}";
    }
}

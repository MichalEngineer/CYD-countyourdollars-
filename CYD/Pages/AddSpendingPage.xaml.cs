using System;
using CYD.Services;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace CYD.Pages
{
    public partial class AddSpendingPage : ContentPage
    {
        private readonly FirebaseDatabaseService _firebaseService;
        private readonly FirebaseAuthService _authService;

        public AddSpendingPage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseDatabaseService();
            _authService = new FirebaseAuthService();
        }

        // Zapisuje wydatek do Firebase
        private async void OnSaveSpendingClicked(object sender, EventArgs e)
        {
            // Pobieramy e-mail u¿ytkownika
            var userEmail = await _authService.GetCurrentUserEmailAsync();
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in")
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            // Pobieramy wybran¹ kategoriê
            var selectedCategory = CategoryPicker.SelectedItem as string;
            if (string.IsNullOrEmpty(selectedCategory))
            {
                await DisplayAlert("Error", "Please select a category.", "OK");
                return;
            }

            // Sprawdzamy, czy kwota jest prawid³owa
            if (decimal.TryParse(AmountEntry.Text, out var amount))
            {
                // Zapisujemy wydatek do Firebase z przypisanym e-mailem u¿ytkownika
                await _firebaseService.SaveSpendingAsync(selectedCategory, amount, userEmail);
                await DisplayAlert("Success", "Spending saved successfully!", "OK");

                // Czyœcimy pola po zapisaniu
                CategoryPicker.SelectedIndex = -1; // Resetujemy wybór kategorii
                AmountEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK");
            }
        }
    }
}

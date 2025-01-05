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

            // Sprawdzamy, czy kategoria i kwota s¹ prawid³owe
            if (!string.IsNullOrEmpty(CategoryEntry.Text) && decimal.TryParse(AmountEntry.Text, out var amount))
            {
                // Zapisujemy wydatek do Firebase z przypisanym e-mailem u¿ytkownika
                await _firebaseService.SaveSpendingAsync(CategoryEntry.Text, amount, userEmail);
                await DisplayAlert("Success", "Spending saved successfully!", "OK");

                // Czyœcimy pola po zapisaniu
                CategoryEntry.Text = string.Empty;
                AmountEntry.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Please enter valid data.", "OK");
            }
        }
    }
}

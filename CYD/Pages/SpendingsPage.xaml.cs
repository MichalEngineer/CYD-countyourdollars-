using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CYD.Services;
using Microsoft.Maui.Controls;

namespace CYD.Pages
{
    public partial class SpendingsPage : ContentPage
    {
        private readonly FirebaseDatabaseService _firebaseService;
        private readonly FirebaseAuthService _authService;

        public SpendingsPage()
        {
            InitializeComponent();
            _firebaseService = new FirebaseDatabaseService();
            _authService = new FirebaseAuthService();
            LoadSpendings();
        }

        // £adowanie wydatków u¿ytkownika
        private async Task LoadSpendings()
        {
            // Pokazanie wskaŸnika ³adowania
            LoadingIndicator.IsRunning = true;
            LoadingIndicator.IsVisible = true;

            // Sprawdzamy, czy u¿ytkownik jest zalogowany
            var isLoggedIn = await _authService.IsUserLoggedInAsync();
            if (!isLoggedIn)
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            // Pobieramy UID u¿ytkownika
            var userEmail = await _authService.GetCurrentUserEmailAsync();
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in")
            {
                await DisplayAlert("Error", "Failed to retrieve user email.", "OK");
                return;
            }

            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-");
            var spendings = await _firebaseService.GetSpendingsAsync(sanitizedEmail);

            // Wyœwietlenie danych w ListView
            SpendingsListView.ItemsSource = spendings;
            SpendingsListView.IsVisible = true;

            // Ukrycie wskaŸnika ³adowania
            LoadingIndicator.IsRunning = false;
            LoadingIndicator.IsVisible = false;
        }

        // Obs³uga klikniêcia przycisku "Refresh"
        private async void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            // £adowanie danych po klikniêciu
            await LoadSpendings();
        }
    }
}

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

        // �adowanie wydatk�w u�ytkownika
        private async void LoadSpendings()
        {
            // Pobieramy e-mail u�ytkownika
            var userEmail = await _authService.GetCurrentUserEmailAsync();
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in")
            {
                await DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            try
            {
                // Pobieramy wydatki u�ytkownika
                var spendings = await _firebaseService.GetSpendingsAsync(userEmail);

                // Ustawiamy dane jako �r�d�o ListView
                SpendingsListView.ItemsSource = spendings;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load spendings: {ex.Message}", "OK");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CYD.Services;
using Microsoft.Maui.Controls;

namespace CYD.Pages
{
    /**
     * @class SpendingsPage
     * @brief Strona zarz�dzaj�ca wy�wietlaniem wydatk�w u�ytkownika.
     */
    public partial class SpendingsPage : ContentPage
    {
        private readonly FirebaseDatabaseService _firebaseService; ///< Serwis bazy danych Firebase.
        private readonly FirebaseAuthService _authService; ///< Serwis uwierzytelniania Firebase.

        /**
         * @brief Konstruktor inicjalizuj�cy komponenty oraz serwisy.
         */
        public SpendingsPage()
        {
            InitializeComponent(); //<--- Inicjalizacja komponent�w UI.
            _firebaseService = new FirebaseDatabaseService(); //<--- Inicjalizacja serwisu bazy danych.
            _authService = new FirebaseAuthService(); //<--- Inicjalizacja serwisu uwierzytelniania.
            LoadSpendings(); //<--- �adowanie wydatk�w.
        }

        /**
         * @brief Asynchroniczne �adowanie wydatk�w u�ytkownika.
         */
        private async Task LoadSpendings()
        {
            LoadingIndicator.IsRunning = true; //<--- Pokazanie wska�nika �adowania.
            LoadingIndicator.IsVisible = true;

            var isLoggedIn = await _authService.IsUserLoggedInAsync(); //<--- Sprawdzenie, czy u�ytkownik jest zalogowany.
            if (!isLoggedIn)
            {
                await DisplayAlert("Error", "User not logged in.", "OK"); //<--- Komunikat o b��dzie.
                return;
            }

            var userEmail = await _authService.GetCurrentUserEmailAsync(); //<--- Pobranie adresu e-mail u�ytkownika.
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in")
            {
                await DisplayAlert("Error", "Failed to retrieve user email.", "OK"); //<--- Komunikat o b��dzie.
                return;
            }

            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-"); //<--- Sanitacja adresu e-mail.
            var spendings = await _firebaseService.GetSpendingsAsync(sanitizedEmail); //<--- Pobranie wydatk�w.

            SpendingsListView.ItemsSource = spendings; //<--- Przypisanie danych do ListView.
            SpendingsListView.IsVisible = true;

            LoadingIndicator.IsRunning = false; //<--- Ukrycie wska�nika �adowania.
            LoadingIndicator.IsVisible = false;
        }

        /**
         * @brief Obs�uguje klikni�cie przycisku "Refresh".
         * @param sender Obiekt wywo�uj�cy zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnRefreshButtonClicked(object sender, EventArgs e)
        {
            await LoadSpendings(); //<--- Ponowne za�adowanie wydatk�w.
        }
    }
}

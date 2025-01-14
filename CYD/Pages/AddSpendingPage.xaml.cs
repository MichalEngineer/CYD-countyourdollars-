using System; // <--- U¿ycie przestrzeni nazw System
using CYD.Services; // <--- U¿ycie przestrzeni nazw us³ug aplikacji (np. do komunikacji z Firebase)
using Microsoft.Maui.Controls; // <--- U¿ycie przestrzeni nazw do kontrolki w aplikacji Maui
using System.Threading.Tasks; // <--- U¿ycie przestrzeni nazw do pracy z asynchronicznymi operacjami

namespace CYD.Pages
{
    /** @brief Strona dodawania wydatków */
    public partial class AddSpendingPage : ContentPage
    {
        private readonly FirebaseDatabaseService _firebaseService; // <--- Serwis do komunikacji z Firebase
        private readonly FirebaseAuthService _authService; // <--- Serwis do autoryzacji u¿ytkownika

        /** @brief Konstruktor strony dodawania wydatków */
        public AddSpendingPage()
        {
            InitializeComponent(); // <--- Inicjalizuje komponenty strony
            _firebaseService = new FirebaseDatabaseService(); // <--- Inicjalizacja serwisu FirebaseDatabaseService
            _authService = new FirebaseAuthService(); // <--- Inicjalizacja serwisu FirebaseAuthService
        }

        /** @brief Zapisuje wydatek do Firebase po klikniêciu przycisku */
        private async void OnSaveSpendingClicked(object sender, EventArgs e)
        {
            // Pobieramy e-mail u¿ytkownika
            var userEmail = await _authService.GetCurrentUserEmailAsync(); // <--- Pobieramy e-mail aktualnie zalogowanego u¿ytkownika
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in") // <--- Sprawdzamy, czy u¿ytkownik jest zalogowany
            {
                await DisplayAlert("Error", "User not logged in.", "OK"); // <--- Wyœwietlamy komunikat o b³êdzie, jeœli u¿ytkownik nie jest zalogowany
                return;
            }

            // Pobieramy wybran¹ kategoriê wydatku
            var selectedCategory = CategoryPicker.SelectedItem as string; // <--- Pobieramy kategoriê wybran¹ z picker-a
            if (string.IsNullOrEmpty(selectedCategory)) // <--- Sprawdzamy, czy u¿ytkownik wybra³ kategoriê
            {
                await DisplayAlert("Error", "Please select a category.", "OK"); // <--- Wyœwietlamy komunikat o b³êdzie, jeœli kategoria nie zosta³a wybrana
                return;
            }

            // Sprawdzamy, czy kwota jest prawid³owa
            if (decimal.TryParse(AmountEntry.Text, out var amount)) // <--- Próbujemy zamieniæ tekst z AmountEntry na liczbê
            {
                // Zapisujemy wydatek do Firebase z przypisanym e-mailem u¿ytkownika
                await _firebaseService.SaveSpendingAsync(selectedCategory, amount, userEmail); // <--- Zapisujemy dane wydatku w Firebase
                await DisplayAlert("Success", "Spending saved successfully!", "OK"); // <--- Wyœwietlamy komunikat o powodzeniu zapisu

                // Czyœcimy pola po zapisaniu
                CategoryPicker.SelectedIndex = -1; // <--- Resetujemy wybór kategorii
                AmountEntry.Text = string.Empty; // <--- Czyœcimy pole kwoty
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK"); // <--- Wyœwietlamy komunikat o b³êdzie, jeœli kwota jest nieprawid³owa
            }
        }
    }
}

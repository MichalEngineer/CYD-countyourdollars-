using System; // <--- U�ycie przestrzeni nazw System
using CYD.Services; // <--- U�ycie przestrzeni nazw us�ug aplikacji (np. do komunikacji z Firebase)
using Microsoft.Maui.Controls; // <--- U�ycie przestrzeni nazw do kontrolki w aplikacji Maui
using System.Threading.Tasks; // <--- U�ycie przestrzeni nazw do pracy z asynchronicznymi operacjami

namespace CYD.Pages
{
    /** @brief Strona dodawania wydatk�w */
    public partial class AddSpendingPage : ContentPage
    {
        private readonly FirebaseDatabaseService _firebaseService; // <--- Serwis do komunikacji z Firebase
        private readonly FirebaseAuthService _authService; // <--- Serwis do autoryzacji u�ytkownika

        /** @brief Konstruktor strony dodawania wydatk�w */
        public AddSpendingPage()
        {
            InitializeComponent(); // <--- Inicjalizuje komponenty strony
            _firebaseService = new FirebaseDatabaseService(); // <--- Inicjalizacja serwisu FirebaseDatabaseService
            _authService = new FirebaseAuthService(); // <--- Inicjalizacja serwisu FirebaseAuthService
        }

        /** @brief Zapisuje wydatek do Firebase po klikni�ciu przycisku */
        private async void OnSaveSpendingClicked(object sender, EventArgs e)
        {
            // Pobieramy e-mail u�ytkownika
            var userEmail = await _authService.GetCurrentUserEmailAsync(); // <--- Pobieramy e-mail aktualnie zalogowanego u�ytkownika
            if (string.IsNullOrEmpty(userEmail) || userEmail == "User not logged in") // <--- Sprawdzamy, czy u�ytkownik jest zalogowany
            {
                await DisplayAlert("Error", "User not logged in.", "OK"); // <--- Wy�wietlamy komunikat o b��dzie, je�li u�ytkownik nie jest zalogowany
                return;
            }

            // Pobieramy wybran� kategori� wydatku
            var selectedCategory = CategoryPicker.SelectedItem as string; // <--- Pobieramy kategori� wybran� z picker-a
            if (string.IsNullOrEmpty(selectedCategory)) // <--- Sprawdzamy, czy u�ytkownik wybra� kategori�
            {
                await DisplayAlert("Error", "Please select a category.", "OK"); // <--- Wy�wietlamy komunikat o b��dzie, je�li kategoria nie zosta�a wybrana
                return;
            }

            // Sprawdzamy, czy kwota jest prawid�owa
            if (decimal.TryParse(AmountEntry.Text, out var amount)) // <--- Pr�bujemy zamieni� tekst z AmountEntry na liczb�
            {
                // Zapisujemy wydatek do Firebase z przypisanym e-mailem u�ytkownika
                await _firebaseService.SaveSpendingAsync(selectedCategory, amount, userEmail); // <--- Zapisujemy dane wydatku w Firebase
                await DisplayAlert("Success", "Spending saved successfully!", "OK"); // <--- Wy�wietlamy komunikat o powodzeniu zapisu

                // Czy�cimy pola po zapisaniu
                CategoryPicker.SelectedIndex = -1; // <--- Resetujemy wyb�r kategorii
                AmountEntry.Text = string.Empty; // <--- Czy�cimy pole kwoty
            }
            else
            {
                await DisplayAlert("Error", "Please enter a valid amount.", "OK"); // <--- Wy�wietlamy komunikat o b��dzie, je�li kwota jest nieprawid�owa
            }
        }
    }
}

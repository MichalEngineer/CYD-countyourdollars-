using Microsoft.Maui.Controls;

namespace CYD.Pages;

/**
 * @class SignUpPage
 * @brief Strona rejestracji użytkownika z obsługą Firebase.
 */
public partial class SignUpPage : ContentPage
{
    private FirebaseAuthService _authService; ///< Serwis uwierzytelniania Firebase.

    /**
     * @brief Konstruktor inicjalizujący komponenty UI i serwis autoryzacji.
     */
    public SignUpPage()
    {
        InitializeComponent(); //<--- Inicjalizacja komponentów UI.
        _authService = new FirebaseAuthService(); //<--- Inicjalizacja serwisu Firebase.
    }

    /**
     * @brief Obsługuje kliknięcie przycisku rejestracji.
     * @param sender Obiekt wywołujący zdarzenie.
     * @param e Argumenty zdarzenia.
     */
    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text; //<--- Pobranie imienia użytkownika.
        string lastName = LastNameEntry.Text; //<--- Pobranie nazwiska użytkownika.
        string email = EmailEntry.Text; //<--- Pobranie adresu e-mail.
        string password = PasswordEntry.Text; //<--- Pobranie hasła.
        string confirmPassword = ConfirmPasswordEntry.Text; //<--- Pobranie potwierdzenia hasła.

        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
            string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(confirmPassword))
        {
            await DisplayAlert("Error", "Please fill in all fields.", "OK"); //<--- Sprawdzenie pustych pól.
            return;
        }

        if (password != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK"); //<--- Sprawdzenie zgodności haseł.
            return;
        }

        if (password.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 signs!", "OK"); //<--- Sprawdzenie długości hasła.
            return;
        }

        string result = await _authService.RegisterWithEmailAndPasswordAsync(email, password); //<--- Rejestracja użytkownika.

        if (!string.IsNullOrEmpty(result) && result.StartsWith("Error"))
        {
            await DisplayAlert("Error", "Unable to register account", "OK"); //<--- Obsługa błędu rejestracji.
            return;
        }
        else
        {
            await DisplayAlert("Registration Success", "Account created successfully!", "OK"); //<--- Sukces rejestracji.
            await Navigation.PopAsync(); //<--- Powrót do poprzedniej strony.

            EmailEntry.Text = string.Empty; //<--- Wyczyszczenie pola e-mail.
            PasswordEntry.Text = string.Empty; //<--- Wyczyszczenie pola hasła.
            ConfirmPasswordEntry.Text = string.Empty; //<--- Wyczyszczenie pola potwierdzenia hasła.
        }
    }

    /**
     * @brief Obsługuje kliknięcie linku do logowania.
     * @param sender Obiekt wywołujący zdarzenie.
     * @param e Argumenty zdarzenia.
     */
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); //<--- Powrót do poprzedniej strony.
    }
}

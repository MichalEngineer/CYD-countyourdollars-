using Microsoft.Maui.Controls; // <--- Użycie przestrzeni nazw dla kontrolek w aplikacji Maui

namespace CYD.Pages;

public partial class ForgotPassword : ContentPage
{
    /** @brief Konstruktor strony "ForgotPassword" */
    public ForgotPassword()
    {
        InitializeComponent(); // <--- Inicjalizacja komponentów strony
    }

    /** @brief Obsługuje kliknięcie przycisku "Send Reset Link" */
    private async void OnSendResetLinkClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text; // <--- Pobranie adresu e-mail z kontrolki

        // Weryfikacja poprawności e-maila
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "OK"); // <--- Wyświetlanie komunikatu o błędzie, gdy e-mail jest niepoprawny
            return;
        }

        // Logika do wysłania e-maila z instrukcjami resetowania hasła
        await DisplayAlert("Reset Link Sent", "A reset link has been sent to your email address.", "OK"); // <--- Informowanie użytkownika o wysłaniu linku do resetowania hasła
    }

    /** @brief Obsługuje kliknięcie przycisku "Login" w przypadku, gdy użytkownik pamięta hasło */
    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); // <--- Powrót do poprzedniej strony
    }
}

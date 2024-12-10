using Microsoft.Maui.Controls;

namespace CYD.Pages
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        // Obs�uguje klikni�cie przycisku "Send Reset Link"
        private async void OnSendResetLinkClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;

            // Weryfikacja poprawno�ci e-maila
            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }

            // Logika do wys�ania e-maila z instrukcjami resetowania has�a
            await DisplayAlert("Reset Link Sent", "A reset link has been sent to your email address.", "OK");

            // Mo�esz doda� tutaj logik� do faktycznego wysy�ania e-maila z linkiem do resetowania has�a
        }

        // Obs�uguje klikni�cie przycisku "Login" w przypadku, gdy u�ytkownik pami�ta� has�o
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // Mo�esz wr�ci� do strony logowania, np.:
            await Navigation.PopAsync(); // Powr�t do poprzedniej strony
        }
    }
}

using Microsoft.Maui.Controls;

namespace CYD.Pages
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        // Obs�uguje klikni�cie przycisku "Sign Up"
        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            // Pobieranie tekstu z p�l Entry
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;
            string confirmPassword = ConfirmPasswordEntry.Text;

            // Prosta walidacja
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(confirmPassword))
            {
                await DisplayAlert("Error", "Please fill in all fields.", "OK");
                return;
            }

            if (password != confirmPassword)
            {
                await DisplayAlert("Error", "Passwords do not match.", "OK");
                return;
            }

            // Logika rejestracji (np. zapis do bazy danych lub API)
            await DisplayAlert("Success", "You have successfully signed up!", "OK");

            // Przej�cie do strony logowania
            await Navigation.PopAsync();
        }

        // Obs�uguje klikni�cie linku "Login"
        private async void OnLoginTapped(object sender, EventArgs e)
        {
            // Powr�t do strony logowania
            await Navigation.PopAsync();
        }
    }
}

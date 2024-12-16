using Microsoft.Maui.Controls;

namespace CYD.Pages;

public partial class SignUpPage : ContentPage
{
    private FirebaseAuthService _authService;
    public SignUpPage()
	{
		InitializeComponent();
        _authService = new FirebaseAuthService();
    }

    private async void OnSignUpClicked(object sender, EventArgs e)
    {
        string firstName = FirstNameEntry.Text;
        string lastName = LastNameEntry.Text;
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;
        string confirmPassword = ConfirmPasswordEntry.Text;

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

        if (password.Length < 6)
        {
            await DisplayAlert("Error","Password must be at least 6 signs!","OK");
            return;
        }

        string result = await _authService.RegisterWithEmailAndPasswordAsync(email, password);

        if (!string.IsNullOrEmpty(result) && result.StartsWith("Error"))
        {
            await DisplayAlert("Error", "Unable to register account", "OK");
            return;
        }
        else
        {
            await DisplayAlert("Registration Success", "Account created successfully!", "OK");
            await Navigation.PopAsync();

            EmailEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
            ConfirmPasswordEntry.Text = string.Empty;
        }

    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}

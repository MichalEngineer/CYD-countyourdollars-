namespace CYD.Pages;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using System;
using System.Threading.Tasks;

public partial class LoginPage : ContentPage
{
    private readonly FirebaseAuthService _authService;
    public static event EventHandler LoginSuccessful;
    public LoginPage()
    {
        InitializeComponent();
        _authService = new FirebaseAuthService();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Erroe", "Email and password cannot be empty.", "OK");
            return;
        }

        try
        {
            // Pr�ba zalogowania u�ytkownika
            var result = await _authService.SignInWithEmailAndPasswordAsync(email, password);

            if (!string.IsNullOrEmpty(result) && result.StartsWith("Error"))
            {
                // Obs�uga konkretnych b��d�w autoryzacji
                if (result.Contains("INVALID_LOGIN_CREDENTIALS"))
                {
                    await DisplayAlert("Error", "Wrong email or password.", "OK");
                }
                else if (result.Contains("INVALID_EMAIL"))
                {
                    await DisplayAlert("Error", "Wrong email.", "OK");
                }
                else
                {
                    await DisplayAlert("Error", "Unknown mistake. Try again.", "OK");
                }
            }
            else
            {

                // Logowanie powiod�o si�
                await DisplayAlert("Success", "Login succesful!", "OK");
                LoginSuccessful?.Invoke(this, EventArgs.Empty);// Przeniesienie do strony g��wnej
            }
        }
        catch (Exception ex)
        {
            // Obs�uga og�lnych b��d�w
            await DisplayAlert("Error", $"Mistake: {ex.Message}", "OK");
        }
    }
    
    private async void OnSignUpTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignUpPage());
    }

    private async void OnForgotPasswordTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ForgotPassword());
    }

}
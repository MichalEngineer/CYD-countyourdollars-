namespace CYD.Pages;

using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public partial class LoginPage : ContentPage
{
    private readonly IFingerprint fingerprint;
    private readonly FirebaseAuthService _authService;
    public static event EventHandler LoginSuccessful;
    public LoginPage(IFingerprint fingerprint)
    {
        InitializeComponent();
        _authService = new FirebaseAuthService();
        this.fingerprint = fingerprint;
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;
        string password = PasswordEntry.Text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            await DisplayAlert("Error", "Email and password cannot be empty.", "OK");
            return;
        }

        try
        {
            // Próba zalogowania u¿ytkownika
            var result = await _authService.SignInWithEmailAndPasswordAsync(email, password);

            if (!string.IsNullOrEmpty(result) && result.StartsWith("Error"))
            {
                // Obs³uga konkretnych b³êdów autoryzacji
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

                // Logowanie powiod³o siê
                await DisplayAlert("Success", "Login succesful!", "OK");
                LoginSuccessful?.Invoke(this, EventArgs.Empty);// Przeniesienie do strony g³ównej
            }
        }
        catch (Exception ex)
        {
            await SecureStorage.SetAsync("user_email", email);
            // Obs³uga ogólnych b³êdów
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

    private async void Button_Clicked_2(object sender, EventArgs e)
    {
        var request = new AuthenticationRequestConfiguration("Logowanie", "Za pomoc¹ odcisku palca");
        var result = await fingerprint.AuthenticateAsync(request);
        if(result.Authenticated)
        {
            await DisplayAlert("Success", "Login succesful!", "OK");
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await DisplayAlert("Error", "Login failed.", "OK");
        }
    }

}
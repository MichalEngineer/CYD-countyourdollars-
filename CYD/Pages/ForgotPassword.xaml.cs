using Microsoft.Maui.Controls;

namespace CYD.Pages;

public partial class ForgotPassword : ContentPage
{
	public ForgotPassword()
	{
		InitializeComponent();
	}

    // Obs³uguje klikniêcie przycisku "Send Reset Link"
    private async void OnSendResetLinkClicked(object sender, EventArgs e)
    {
        string email = EmailEntry.Text;

        // Weryfikacja poprawnoœci e-maila
        if (string.IsNullOrEmpty(email) || !email.Contains("@"))
        {
            await DisplayAlert("Error", "Please enter a valid email address.", "OK");
            return;
        }

        // Logika do wys³ania e-maila z instrukcjami resetowania has³a
        await DisplayAlert("Reset Link Sent", "A reset link has been sent to your email address.", "OK");


    }

    // Obs³uguje klikniêcie przycisku "Login" w przypadku, gdy u¿ytkownik pamiêta³ has³o
    private async void OnLoginTapped(object sender, EventArgs e)
    {

        await Navigation.PopAsync(); // Powrót do poprzedniej strony
    }
}

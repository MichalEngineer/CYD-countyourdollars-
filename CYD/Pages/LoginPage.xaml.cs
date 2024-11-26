namespace CYD.Pages;

public partial class LoginPage : ContentPage
{
    public static event EventHandler? LoginSuccessful;

    public LoginPage()
    {
        InitializeComponent();
    }

    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;

        if (username == "admin" && password == "password")
        {
            // Wywo³aj zdarzenie logowania
            LoginSuccessful?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            await DisplayAlert("Error", "Invalid username or password", "OK");
        }
    }
}
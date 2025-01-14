namespace CYD.Pages
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Storage;
    using Plugin.Fingerprint.Abstractions;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /**
     * Reprezentuje stronê logowania w aplikacji. Strona ta obs³uguje uwierzytelnianie u¿ytkownika zarówno przez 
     * tradycyjne logowanie (e-mail/has³o), jak i uwierzytelnianie za pomoc¹ odcisku palca. Komunikuje siê z us³ug¹
     * uwierzytelniania Firebase, aby zalogowaæ u¿ytkownika i obs³ugiwaæ b³êdy zwi¹zane z uwierzytelnianiem.
     */
    public partial class LoginPage : ContentPage
    {
        private readonly IFingerprint fingerprint;
        private readonly FirebaseAuthService _authService;

        /**
         * Zdarzenie wyzwalane, gdy logowanie siê powiedzie.
         */
        public static event EventHandler LoginSuccessful;

        /**
         * Inicjalizuje now¹ instancjê klasy LoginPage.
         * @param fingerprint Instancja us³ugi uwierzytelniania odciskiem palca.
         */
        public LoginPage(IFingerprint fingerprint)
        {
            InitializeComponent();
            _authService = new FirebaseAuthService();
            this.fingerprint = fingerprint;
        }

        /**
         * Obs³uguje klikniêcie przycisku logowania. Próbujê uwierzytelniæ u¿ytkownika za pomoc¹ podanego e-maila i has³a.
         * Jeœli logowanie jest udane, u¿ytkownik zostaje zalogowany, a odpowiednie zdarzenie jest wyzwalane.
         * Jeœli logowanie siê nie uda, wyœwietlany jest odpowiedni komunikat o b³êdzie.
         * 
         * @param sender Obiekt, który wyzwoli³ to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            // Sprawdzamy, czy e-mail lub has³o s¹ puste
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) // <--- Sprawdza, czy e-mail lub has³o s¹ puste
            {
                await DisplayAlert("B³¹d", "E-mail i has³o nie mog¹ byæ puste.", "OK");
                return;
            }

            try
            {
                // Próbujemy zalogowaæ u¿ytkownika za pomoc¹ e-maila i has³a
                var result = await _authService.SignInWithEmailAndPasswordAsync(email, password);

                if (!string.IsNullOrEmpty(result) && result.StartsWith("Error")) // <--- Sprawdza, czy wynik zawiera b³¹d
                {
                    // Obs³ugujemy konkretne b³êdy uwierzytelniania
                    if (result.Contains("INVALID_LOGIN_CREDENTIALS")) // <--- Sprawdza, czy b³¹d wynika z nieprawid³owych danych logowania
                    {
                        await DisplayAlert("B³¹d", "Nieprawid³owy e-mail lub has³o.", "OK");
                    }
                    else if (result.Contains("INVALID_EMAIL")) // <--- Sprawdza, czy b³¹d wynika z nieprawid³owego e-maila
                    {
                        await DisplayAlert("B³¹d", "Nieprawid³owy e-mail.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("B³¹d", "Nieznany b³¹d. Spróbuj ponownie.", "OK");
                    }
                }
                else
                {
                    // Logowanie siê powiod³o
                    await DisplayAlert("Sukces", "Logowanie udane!", "OK");
                    LoginSuccessful?.Invoke(this, EventArgs.Empty); // <--- Wyzwala zdarzenie sukcesu logowania
                }
            }
            catch (Exception ex)
            {
                // Przechowujemy e-mail u¿ytkownika w bezpiecznej pamiêci i obs³ugujemy b³êdy ogólne
                await SecureStorage.SetAsync("user_email", email);
                await DisplayAlert("B³¹d", $"B³¹d: {ex.Message}", "OK");
            }
        }

        /**
         * Obs³uguje klikniêcie linku "Zarejestruj siê". Nawigujemy do strony rejestracji.
         * 
         * @param sender Obiekt, który wyzwoli³ to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        /**
         * Obs³uguje klikniêcie linku "Zapomnia³eœ has³a?". Nawigujemy do strony resetowania has³a.
         * 
         * @param sender Obiekt, który wyzwoli³ to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }

        /**
         * Obs³uguje klikniêcie przycisku uwierzytelniania za pomoc¹ odcisku palca. Próbuje uwierzytelniæ u¿ytkownika za pomoc¹ 
         * czytnika odcisków palców. Jeœli uwierzytelnienie jest udane, u¿ytkownik jest zalogowany, a odpowiednie zdarzenie jest wyzwalane.
         * 
         * @param sender Obiekt, który wyzwoli³ to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            var request = new AuthenticationRequestConfiguration("Logowanie", "Za pomoc¹ odcisku palca");
            var result = await fingerprint.AuthenticateAsync(request);

            if (result.Authenticated) // <--- Sprawdza, czy uwierzytelnienie odciskiem palca jest udane
            {
                await DisplayAlert("Sukces", "Logowanie udane!", "OK");
                LoginSuccessful?.Invoke(this, EventArgs.Empty); // <--- Wyzwala zdarzenie sukcesu logowania
            }
            else
            {
                await DisplayAlert("B³¹d", "Logowanie nieudane.", "OK");
            }
        }
    }
}

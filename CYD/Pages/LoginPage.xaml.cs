namespace CYD.Pages
{
    using Microsoft.Maui.Controls;
    using Microsoft.Maui.Storage;
    using Plugin.Fingerprint.Abstractions;
    using System;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    /**
     * Reprezentuje stron� logowania w aplikacji. Strona ta obs�uguje uwierzytelnianie u�ytkownika zar�wno przez 
     * tradycyjne logowanie (e-mail/has�o), jak i uwierzytelnianie za pomoc� odcisku palca. Komunikuje si� z us�ug�
     * uwierzytelniania Firebase, aby zalogowa� u�ytkownika i obs�ugiwa� b��dy zwi�zane z uwierzytelnianiem.
     */
    public partial class LoginPage : ContentPage
    {
        private readonly IFingerprint fingerprint;
        private readonly FirebaseAuthService _authService;

        /**
         * Zdarzenie wyzwalane, gdy logowanie si� powiedzie.
         */
        public static event EventHandler LoginSuccessful;

        /**
         * Inicjalizuje now� instancj� klasy LoginPage.
         * @param fingerprint Instancja us�ugi uwierzytelniania odciskiem palca.
         */
        public LoginPage(IFingerprint fingerprint)
        {
            InitializeComponent();
            _authService = new FirebaseAuthService();
            this.fingerprint = fingerprint;
        }

        /**
         * Obs�uguje klikni�cie przycisku logowania. Pr�buj� uwierzytelni� u�ytkownika za pomoc� podanego e-maila i has�a.
         * Je�li logowanie jest udane, u�ytkownik zostaje zalogowany, a odpowiednie zdarzenie jest wyzwalane.
         * Je�li logowanie si� nie uda, wy�wietlany jest odpowiedni komunikat o b��dzie.
         * 
         * @param sender Obiekt, kt�ry wyzwoli� to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            string email = EmailEntry.Text;
            string password = PasswordEntry.Text;

            // Sprawdzamy, czy e-mail lub has�o s� puste
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) // <--- Sprawdza, czy e-mail lub has�o s� puste
            {
                await DisplayAlert("B��d", "E-mail i has�o nie mog� by� puste.", "OK");
                return;
            }

            try
            {
                // Pr�bujemy zalogowa� u�ytkownika za pomoc� e-maila i has�a
                var result = await _authService.SignInWithEmailAndPasswordAsync(email, password);

                if (!string.IsNullOrEmpty(result) && result.StartsWith("Error")) // <--- Sprawdza, czy wynik zawiera b��d
                {
                    // Obs�ugujemy konkretne b��dy uwierzytelniania
                    if (result.Contains("INVALID_LOGIN_CREDENTIALS")) // <--- Sprawdza, czy b��d wynika z nieprawid�owych danych logowania
                    {
                        await DisplayAlert("B��d", "Nieprawid�owy e-mail lub has�o.", "OK");
                    }
                    else if (result.Contains("INVALID_EMAIL")) // <--- Sprawdza, czy b��d wynika z nieprawid�owego e-maila
                    {
                        await DisplayAlert("B��d", "Nieprawid�owy e-mail.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("B��d", "Nieznany b��d. Spr�buj ponownie.", "OK");
                    }
                }
                else
                {
                    // Logowanie si� powiod�o
                    await DisplayAlert("Sukces", "Logowanie udane!", "OK");
                    LoginSuccessful?.Invoke(this, EventArgs.Empty); // <--- Wyzwala zdarzenie sukcesu logowania
                }
            }
            catch (Exception ex)
            {
                // Przechowujemy e-mail u�ytkownika w bezpiecznej pami�ci i obs�ugujemy b��dy og�lne
                await SecureStorage.SetAsync("user_email", email);
                await DisplayAlert("B��d", $"B��d: {ex.Message}", "OK");
            }
        }

        /**
         * Obs�uguje klikni�cie linku "Zarejestruj si�". Nawigujemy do strony rejestracji.
         * 
         * @param sender Obiekt, kt�ry wyzwoli� to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnSignUpTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        /**
         * Obs�uguje klikni�cie linku "Zapomnia�e� has�a?". Nawigujemy do strony resetowania has�a.
         * 
         * @param sender Obiekt, kt�ry wyzwoli� to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnForgotPasswordTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ForgotPassword());
        }

        /**
         * Obs�uguje klikni�cie przycisku uwierzytelniania za pomoc� odcisku palca. Pr�buje uwierzytelni� u�ytkownika za pomoc� 
         * czytnika odcisk�w palc�w. Je�li uwierzytelnienie jest udane, u�ytkownik jest zalogowany, a odpowiednie zdarzenie jest wyzwalane.
         * 
         * @param sender Obiekt, kt�ry wyzwoli� to zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            var request = new AuthenticationRequestConfiguration("Logowanie", "Za pomoc� odcisku palca");
            var result = await fingerprint.AuthenticateAsync(request);

            if (result.Authenticated) // <--- Sprawdza, czy uwierzytelnienie odciskiem palca jest udane
            {
                await DisplayAlert("Sukces", "Logowanie udane!", "OK");
                LoginSuccessful?.Invoke(this, EventArgs.Empty); // <--- Wyzwala zdarzenie sukcesu logowania
            }
            else
            {
                await DisplayAlert("B��d", "Logowanie nieudane.", "OK");
            }
        }
    }
}

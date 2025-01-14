using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Font = Microsoft.Maui.Font;
using Firebase.Auth;
using Plugin.Fingerprint;

namespace CYD
{
    /**
     * @class AppShell
     * @brief Główna klasa powłoki aplikacji.
     * 
     * Zarządza interfejsem użytkownika oraz operacjami związanymi z motywem,
     * komunikatami i wylogowaniem użytkownika.
     */
    public partial class AppShell : Shell
    {
        private FirebaseAuthService _authService; ///< Serwis uwierzytelniania Firebase.

        /**
         * @brief Konstruktor klasy AppShell.
         * 
         * Inicjalizuje komponenty, serwis uwierzytelniania oraz ustawia motyw aplikacji.
         */
        public AppShell()
        {
            InitializeComponent(); //<--- Inicjalizacja komponentów interfejsu użytkownika.
            _authService = new FirebaseAuthService(); //<--- Inicjalizacja serwisu Firebase.
            var currentTheme = Application.Current!.UserAppTheme; //<--- Pobranie aktualnego motywu aplikacji.
            ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1; //<--- Ustawienie wybranego motywu w kontrolce.
        }

        /**
         * @brief Wyświetla komunikat Snackbar.
         * @param message Treść komunikatu do wyświetlenia.
         * 
         * Tworzy i wyświetla snackbar z określonymi opcjami wizualnymi.
         */
        public static async Task DisplaySnackbarAsync(string message)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(); //<--- Źródło tokena anulowania dla Snackbar.

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Color.FromArgb("#FF3300"), //<--- Ustawienie koloru tła.
                TextColor = Colors.White, //<--- Ustawienie koloru tekstu.
                ActionButtonTextColor = Colors.Yellow, //<--- Ustawienie koloru przycisku akcji.
                CornerRadius = new CornerRadius(0), //<--- Ustawienie promienia zaokrąglenia narożników.
                Font = Font.SystemFontOfSize(18), //<--- Ustawienie rozmiaru czcionki dla tekstu.
                ActionButtonFont = Font.SystemFontOfSize(14) //<--- Ustawienie rozmiaru czcionki dla przycisku.
            };

            var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions); //<--- Utworzenie obiektu Snackbar z określonymi opcjami.
            await snackbar.Show(cancellationTokenSource.Token); //<--- Wyświetlenie Snackbar z obsługą anulowania.
        }

        /**
         * @brief Wyświetla komunikat Toast.
         * @param message Treść komunikatu do wyświetlenia.
         * 
         * Wyświetla komunikat typu Toast, z pominięciem na platformie Windows.
         */
        public static async Task DisplayToastAsync(string message)
        {
            if (OperatingSystem.IsWindows()) //<--- Sprawdzenie, czy system to Windows.
                return; //<--- Anulowanie dla systemu Windows.

            var toast = Toast.Make(message, textSize: 18); //<--- Utworzenie obiektu Toast.

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5)); //<--- Źródło tokena anulowania na 5 sekund.
            await toast.Show(cts.Token); //<--- Wyświetlenie komunikatu Toast.
        }

        /**
         * @brief Obsługuje zmianę motywu aplikacji.
         * @param sender Obiekt wywołujący zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
        {
            Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark; //<--- Zmiana motywu aplikacji.
        }

        /**
         * @brief Obsługuje kliknięcie przycisku wylogowania.
         * @param sender Obiekt wywołujący zdarzenie.
         * @param e Argumenty zdarzenia.
         */
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            await _authService.LogoutAsync(); //<--- Wylogowanie użytkownika przez Firebase.

            Application.Current.MainPage = new NavigationPage(new LoginPage(CrossFingerprint.Current)); //<--- Nawigacja do strony logowania.
            SecureStorage.Remove("user_email"); //<--- Usunięcie zapisanych danych użytkownika.
            SecureStorage.Remove("user_uid"); //<--- Usunięcie zapisanego identyfikatora użytkownika.
        }
    }
}

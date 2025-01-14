using Firebase.Auth; // <--- Użycie pakietu Firebase.Auth, który zawiera funkcje do autoryzacji
using System; // <--- Użycie przestrzeni nazw System, zapewniającej podstawowe funkcje systemowe
using System.Threading.Tasks; // <--- Użycie przestrzeni nazw Tasków, pozwalających na operacje asynchroniczne
using Microsoft.Maui.Storage; // <--- Użycie przestrzeni nazw do pracy z przechowywaniem danych aplikacji

public class FirebaseAuthService
{
    private readonly FirebaseAuthProvider _authProvider; // <--- Provider do autentykacji z Firebase
    private readonly string _firebaseApiKey = "AIzaSyCsPbeDGIi0slQWhIcPr2hI0NShLn7dE7M"; // <--- Klucz API dla Firebase
    private FirebaseAuthLink _authLink; // <--- Przechowuje dane o zalogowanym użytkowniku

    // Konstruktor inicjalizujący usługę Firebase
    public FirebaseAuthService()
    {
        _authProvider = new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey)); // <--- Inicjalizacja FirebaseAuthProvider z kluczem API
    }

    // ------------------------------
    // Rejestracja nowego użytkownika
    // ------------------------------
    public async Task<string> RegisterWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            // Tworzy nowego użytkownika za pomocą podanego e-maila i hasła
            _authLink = await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password); // <--- Rejestracja użytkownika w Firebase

            // Zapisuje email do SecureStorage
            await SecureStorage.SetAsync("user_email", email); // <--- Zapisuje e-mail użytkownika w SecureStorage

            // Pobiera token Firebase
            string token = _authLink.FirebaseToken; // <--- Pobiera token autentykacji Firebase
            return token; // Zwraca token, jeśli rejestracja się powiedzie
        }
        catch (FirebaseAuthException ex)
        {
            // Obsługuje specyficzne błędy Firebase
            return $"Error: {ex.Reason}"; // <--- Zwraca błąd, jeśli wystąpił problem z rejestracją
        }
        catch (Exception ex)
        {
            // Obsługuje ogólne błędy
            return $"Error: {ex.Message}"; // <--- Zwraca ogólny błąd
        }
    }

    // --------------------------
    // Logowanie istniejącego użytkownika
    // --------------------------
    public async Task<string> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            // Loguje użytkownika za pomocą podanego e-maila i hasła
            _authLink = await _authProvider.SignInWithEmailAndPasswordAsync(email, password); // <--- Logowanie użytkownika w Firebase

            // Zapisuje email do SecureStorage
            await SecureStorage.SetAsync("user_email", email); // <--- Zapisuje e-mail użytkownika w SecureStorage

            await SecureStorage.SetAsync("user_uid", _authLink.User.LocalId); // <--- Zapisuje UID użytkownika w SecureStorage
            // Pobiera token Firebase
            string token = _authLink.FirebaseToken; // <--- Pobiera token autentykacji Firebase
            return token; // Zwraca token, jeśli logowanie się powiedzie
        }
        catch (Exception ex)
        {
            // Zwraca błąd, jeśli logowanie nie powiedzie się
            return $"Error: {ex.Message}"; // <--- Zwraca błąd, jeśli wystąpił problem z logowaniem
        }
    }

    // --------------------------
    // Pobiera adres e-mail aktualnie zalogowanego użytkownika
    // --------------------------
    public string GetCurrentUserEmail()
    {
        return _authLink?.User.Email; // <--- Zwraca e-mail użytkownika lub null, jeśli użytkownik nie jest zalogowany
    }

    // Asynchronicznie pobiera e-mail aktualnie zalogowanego użytkownika
    public async Task<string> GetCurrentUserEmailAsync()
    {
        var email = await SecureStorage.GetAsync("user_email"); // <--- Pobiera zapisany e-mail z SecureStorage
        return email ?? "Nie jesteś zalogowany"; // <--- Zwraca e-mail lub komunikat, że użytkownik nie jest zalogowany
    }

    // Asynchronicznie pobiera UID (identyfikator użytkownika) aktualnie zalogowanego użytkownika
    public async Task<string> GetCurrentUserIdAsync()
    {
        // Zwróci UID użytkownika (LocalId)
        var email = await SecureStorage.GetAsync("user_email"); // <--- Pobiera e-mail z SecureStorage
        if (string.IsNullOrEmpty(email)) // <--- Sprawdza, czy e-mail jest pusty (czy użytkownik nie jest zalogowany)
        {
            return string.Empty; // <--- Jeśli użytkownik nie jest zalogowany, zwraca pusty ciąg
        }

        // Logowanie się na podstawie zapisanej lokalnie wiadomości e-mail
        var user = await _authProvider.SignInWithEmailAndPasswordAsync(email, "userPassword"); // <--- Loguje się na podstawie zapisanego e-maila (tu domyślne hasło "userPassword")
        return user.User.LocalId; // <--- Zwraca UID (LocalId) użytkownika
    }

    // --------------------------
    // Wylogowanie użytkownika
    // --------------------------
    public async Task<bool> IsUserLoggedInAsync()
    {
        var email = await SecureStorage.GetAsync("user_email"); // <--- Pobiera zapisany e-mail użytkownika z SecureStorage
        return !string.IsNullOrEmpty(email); // <--- Sprawdza, czy e-mail jest zapisany (czy użytkownik jest zalogowany)
    }

    // Wylogowanie użytkownika
    public async Task LogoutAsync()
    {
        // Usuwa e-mail użytkownika z SecureStorage podczas wylogowania
        SecureStorage.Remove("user_email"); // <--- Usuwa e-mail z SecureStorage, aby wylogować użytkownika
    }
}

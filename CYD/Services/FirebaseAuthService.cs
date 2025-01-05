using Firebase.Auth;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;

public class FirebaseAuthService
{
    private readonly FirebaseAuthProvider _authProvider;
    private readonly string _firebaseApiKey = "AIzaSyCsPbeDGIi0slQWhIcPr2hI0NShLn7dE7M";
    private FirebaseAuthLink _authLink;  // Przechowuje dane zalogowanego u¿ytkownika

    // Konstruktor inicjalizuj¹cy us³ugê Firebase
    public FirebaseAuthService()
    {
        _authProvider = new FirebaseAuthProvider(new FirebaseConfig(_firebaseApiKey));
    }

    // ------------------------------
    // Rejestracja nowego u¿ytkownika
    // ------------------------------
    public async Task<string> RegisterWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            // Tworzy nowego u¿ytkownika
            _authLink = await _authProvider.CreateUserWithEmailAndPasswordAsync(email, password);

            // Zapisuje email do SecureStorage
            await SecureStorage.SetAsync("user_email", email);

            // Pobiera token Firebase
            string token = _authLink.FirebaseToken;
            return token; // Zwraca token, jeœli rejestracja siê powiedzie
        }
        catch (FirebaseAuthException ex)
        {
            // Obs³uguje specyficzne b³êdy Firebase
            return $"Error: {ex.Reason}";
        }
        catch (Exception ex)
        {
            // Obs³uguje ogólne b³êdy
            return $"Error: {ex.Message}";
        }
    }

    // --------------------------
    // Logowanie istniej¹cego u¿ytkownika
    // --------------------------
    public async Task<string> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        try
        {
            // Loguje u¿ytkownika
            _authLink = await _authProvider.SignInWithEmailAndPasswordAsync(email, password);

            // Zapisuje email do SecureStorage
            await SecureStorage.SetAsync("user_email", email);

            await SecureStorage.SetAsync("user_uid", _authLink.User.LocalId);
            // Pobiera token Firebase
            string token = _authLink.FirebaseToken;
            return token;
        }
        catch (Exception ex)
        {
            // Zwraca b³¹d, jeœli logowanie nie powiedzie siê
            return $"Error: {ex.Message}";
        }
    }

    // --------------------------
    // Pobiera adres e-mail aktualnie zalogowanego u¿ytkownika
    // --------------------------
    public string GetCurrentUserEmail()
    {
        return _authLink?.User.Email;  // Zwraca e-mail lub null, jeœli u¿ytkownik nie jest zalogowany
    }

    public async Task<string> GetCurrentUserEmailAsync()
    {
        var email = await SecureStorage.GetAsync("user_email");
        return email ?? "Nie jesteœ zalogowany";  // Zwraca komunikat, jeœli email jest pusty;  // Pobiera e-mail z SecureStorage
    }
    public async Task<string> GetCurrentUserIdAsync()
    {
        // Zwróci UID użytkownika (LocalId)
        var email = await SecureStorage.GetAsync("user_email");
        if (string.IsNullOrEmpty(email))
        {
            return string.Empty;
        }

        // Logowanie się na podstawie zapisanej lokalnie wiadomości e-mail
        var user = await _authProvider.SignInWithEmailAndPasswordAsync(email, "userPassword"); // Dodaj hasło lub przekaż je dynamicznie
        return user.User.LocalId;  // Zwraca LocalId (UID) użytkownika
    }

    // --------------------------
    // Wylogowanie u¿ytkownika
    // --------------------------
    public async Task<bool> IsUserLoggedInAsync()
    {
        var email = await SecureStorage.GetAsync("user_email");
        return !string.IsNullOrEmpty(email);
    }
    public async Task LogoutAsync()
    {
        // Usuwa e-mail u¿ytkownika z SecureStorage podczas wylogowania
        SecureStorage.Remove("user_email");
    }
}
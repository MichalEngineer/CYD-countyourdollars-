using Firebase.Database; // <--- Użycie Firebase.Database do komunikacji z bazą danych Firebase
using Firebase.Database.Query; // <--- Użycie zapytań do bazy danych Firebase
using System.Collections.Generic; // <--- Użycie przestrzeni nazw do pracy z kolekcjami, takimi jak lista
using System.Threading.Tasks; // <--- Użycie przestrzeni nazw do operacji asynchronicznych

namespace CYD.Services
{
    public class FirebaseDatabaseService
    {
        private readonly FirebaseClient _firebaseClient; // <--- Inicjalizacja klienta Firebase, który będzie służył do komunikacji z bazą danych

        // Konstruktor inicjalizujący klienta Firebase
        public FirebaseDatabaseService()
        {
            _firebaseClient = new FirebaseClient("https://countyourdollars-default-rtdb.europe-west1.firebasedatabase.app/"); // <--- Tworzenie klienta Firebase z podanym URL bazy danych
        }

        // Zapisanie wydatku
        // Zaktualizowana metoda SaveSpendingAsync
        public async Task SaveSpendingAsync(string category, decimal amount, string userEmail)
        {
            // Zamieniamy specjalne znaki w e-mailu na bezpieczne dla Firebase
            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-"); // <--- Bezpieczne przekształcenie e-maila użytkownika na format odpowiedni dla Firebase

            // Tworzymy obiekt wydatku
            var spending = new Spending
            {
                Category = category, // <--- Przypisanie kategorii wydatku
                Amount = amount // <--- Przypisanie kwoty wydatku
            };

            // Tworzymy ścieżkę do zapisania wydatku w Firebase
            var refPath = $"users/{sanitizedEmail}/spendings"; // <--- Tworzenie ścieżki do zapisania danych wydatku w Firebase
            await _firebaseClient.Child(refPath).PostAsync(spending); // <--- Zapisanie obiektu wydatku w Firebase w odpowiedniej ścieżce
        }

        // Pobranie wydatków użytkownika na podstawie e-maila
        public async Task<List<Spending>> GetSpendingsAsync(string userEmail)
        {
            // Zamieniamy e-mail użytkownika na format bezpieczny dla Firebase
            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-"); // <--- Bezpieczne przekształcenie e-maila użytkownika

            // Ścieżka do wydatków użytkownika
            var refPath = $"users/{sanitizedEmail}/spendings"; // <--- Ścieżka w bazie danych do wydatków użytkownika

            // Pobieramy dane z Firebase
            var spendings = await _firebaseClient
                .Child(refPath) // <--- Dostęp do węzła w bazie danych, gdzie przechowywane są wydatki
                .OnceAsync<Spending>(); // <--- Pobieranie wszystkich wydatków w postaci obiektów Spending

            // Konwertujemy wyniki na listę wydatków
            var spendingList = new List<Spending>(); // <--- Tworzymy pustą listę do przechowywania pobranych wydatków
            foreach (var item in spendings)
            {
                spendingList.Add(item.Object); // <--- Dodajemy każdy wydatek do listy
            }

            return spendingList; // <--- Zwracamy listę wydatków
        }

    }

    // Klasa reprezentująca wydatek
    public class Spending
    {
        public string Category { get; set; } // <--- Kategoria wydatku (np. "zakupy", "jedzenie")
        public decimal Amount { get; set; } // <--- Kwota wydatku
    }
}

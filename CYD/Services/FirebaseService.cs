using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CYD.Services
{
    public class FirebaseDatabaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseDatabaseService()
        {
            _firebaseClient = new FirebaseClient("https://countyourdollars-default-rtdb.europe-west1.firebasedatabase.app/"); // Twój URL bazy danych
        }

        // Zapisanie wydatku
        // Zaktualizowana metoda SaveSpendingAsync
        public async Task SaveSpendingAsync(string category, decimal amount, string userEmail)
        {
            // Zamieniamy specjalne znaki w e-mailu na bezpieczne
            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-");

            // Tworzymy obiekt wydatku
            var spending = new Spending
            {
                Category = category,
                Amount = amount
            };

            // Tworzymy ścieżkę do zapisania wydatku w Firebase
            var refPath = $"users/{sanitizedEmail}/spendings"; // Ścieżka z użyciem przekształconego e-maila
            await _firebaseClient.Child(refPath).PostAsync(spending); // Zapisujemy wydatek
        }


        // Pobranie wydatków użytkownika na podstawie e-maila
        public async Task<List<Spending>> GetSpendingsAsync(string userEmail)
        {
            // Zamieniamy e-mail użytkownika na format bezpieczny dla Firebase
            var sanitizedEmail = userEmail.Replace("@", "-at-").Replace(".", "-dot-");

            // Ścieżka do wydatków użytkownika
            var refPath = $"users/{sanitizedEmail}/spendings";

            // Pobieramy dane z Firebase
            var spendings = await _firebaseClient
                .Child(refPath)
                .OnceAsync<Spending>();

            // Konwertujemy wyniki na listę wydatków
            var spendingList = new List<Spending>();
            foreach (var item in spendings)
            {
                spendingList.Add(item.Object);
            }

            return spendingList;
        }

    }

    public class Spending
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }
}

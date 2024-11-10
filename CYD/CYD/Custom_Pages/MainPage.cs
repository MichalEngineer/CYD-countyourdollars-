using System;
using System.Diagnostics;
using Xamarin.Forms;

namespace CYD.Custom_Pages
{
    public class MainPage : ContentPage
    {
        Entry userNameEntry;
        Entry passwordEntry;
        Button loginButton;
        Image loginIcon;
        StackLayout stackLayout;

        public MainPage()
        {
            // Tworzymy obrazek jako ikona logowania
            loginIcon = new Image
            {
                Source = "login_icon.png", // Ścieżka do Twojej ikony
                WidthRequest = 200, // Określenie szerokości ikony
                HeightRequest = 100, // Określenie wysokości ikony (np. długie)
                HorizontalOptions = LayoutOptions.Center, // Wyrównanie poziome
                VerticalOptions = LayoutOptions.CenterAndExpand // Wyrównanie pionowe na środku
            };

            // Tworzymy inne elementy (np. pola tekstowe i przycisk)
            userNameEntry = new Entry
            {
                Placeholder = "Login",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            passwordEntry = new Entry
            {
                Placeholder = "Hasło",
                IsPassword = true,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            loginButton = new Button
            {
                Text = "Zaloguj",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Event handler dla przycisku logowania
            loginButton.Clicked += LoginButton_Clicked;

            // Tworzymy StackLayout
            this.Padding = new Thickness(20);
            stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.CenterAndExpand, // Wyśrodkowanie elementów w pionie
                HorizontalOptions = LayoutOptions.CenterAndExpand, // Wyśrodkowanie elementów w poziomie
                Spacing = 15, // Ustawienie odstępu między elementami
                Children = {
                    loginIcon, // Dodajemy obrazek na początku, aby był na górze
                    userNameEntry,
                    passwordEntry,
                    loginButton
                }
            };

            // Ustawiamy StackLayout jako zawartość strony
            this.Content = stackLayout;
        }

        // Event handler dla przycisku logowania
        void LoginButton_Clicked(object sender, EventArgs e)
        {
            // Zapisujemy dane użytkownika
            Debug.WriteLine(string.Format("Login: {0} - Hasło: {1}",
                userNameEntry.Text, passwordEntry.Text));
        }
    }
}

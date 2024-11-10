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
        StackLayout stackLayout;

        public MainPage()
        {
            // Initialize the entries and button
            userNameEntry = new Entry
            {
                Placeholder = "login"
            };

            passwordEntry = new Entry
            {
                Placeholder = "hasło",
                IsPassword = true
            };

            loginButton = new Button
            {
                Text = "Zaloguj"
            };

            // Event handler for login button
            loginButton.Clicked += LoginButton_Clicked;

            // Padding and layout configuration
            this.Padding = new Thickness(20);
            stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Spacing = 10,
                Children = {
                    userNameEntry,
                    passwordEntry,
                    loginButton
                }
            };

            // Set the page content
            this.Content = stackLayout;
        }

        // Button click event handler
        void LoginButton_Clicked(object sender, EventArgs e)
        {
            // Log the entered username and password
            Debug.WriteLine(string.Format("Login: {0} - Hasło: {1}",
                userNameEntry.Text, passwordEntry.Text));
        }
    }
}


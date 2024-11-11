using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CYD.Custom_Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Logowanie : ContentPage
    {
        public Logowanie()
        {

            InitializeComponent();  // To powinno być wywołane, aby załadować komponenty XAML
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (txtLogin.Text == "admin" && txtPassword.Text == "123")
            {
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await DisplayAlert("Alert", "Login/Password incorrect", "OK");
            }
        }

        private async void Tap(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Content;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;

namespace CYD
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LoginActivity : AppCompatActivity
    {
        EditText loginInput, passwordInput;
        Button loginButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_login);

            loginInput = FindViewById<EditText>(Resource.Id.loginInput);
            passwordInput = FindViewById<EditText>(Resource.Id.passwordInput);
            loginButton = FindViewById<Button>(Resource.Id.loginButton);

            loginButton.Click += (sender, e) =>
            {
                string login = loginInput.Text;
                string password = passwordInput.Text;

                if (login == "admin" && password == "haslo")
                {
                    
                }
                else
                {
                    Toast.MakeText(this, "Nieprawidłowy login lub hasło", ToastLength.Short).Show();
                }
            };
        }
    }
}

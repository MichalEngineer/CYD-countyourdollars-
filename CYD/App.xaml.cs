namespace CYD
{
    public partial class App : Application
    {
        private NavigationPage _navigationPage;
        public App()
        {
            InitializeComponent();

            _navigationPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Colors.Transparent,
                BarTextColor = Colors.Transparent
            };
            SubscribeToLoginSuccessful();
        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(_navigationPage);
            return window;
        }

        private void SubscribeToLoginSuccessful()
        {
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                // Zmiana strony na AppShell po udanym logowaniu
                _navigationPage.PushAsync(new AppShell());
            };
        }
    }
}

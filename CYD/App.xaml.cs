namespace CYD
{
    public partial class App : Application
    {
        private NavigationPage _navigationPage;

        public App()
        {
            InitializeComponent();

            // Inicjalizacja NavigationPage w konstruktorze
            _navigationPage = new NavigationPage(new LoginPage())
            {
                BarBackgroundColor = Colors.Transparent,
                BarTextColor = Colors.Transparent
            };
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Tworzymy główne okno
            var window = new Window(_navigationPage);

            // Subskrypcja na zdarzenie udanego logowania
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                // Zmiana strony na AppShell po udanym logowaniu
                _navigationPage.PushAsync(new AppShell());
            };

            return window;
        }
    }
}

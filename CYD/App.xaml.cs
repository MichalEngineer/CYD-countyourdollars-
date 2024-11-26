namespace CYD
{
    public partial class App : Application
    {
        public App()
        {
            
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var window = new Window(new LoginPage());

            // Subskrybuj zdarzenie logowania
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                // Po zalogowaniu zmieniamy Page
                window.Page = new AppShell();
            };

            return window;
        }
    }
}
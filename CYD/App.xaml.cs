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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            
            var window = new Window(_navigationPage);

            
            LoginPage.LoginSuccessful += (sender, args) =>
            {
                window.Page = new AppShell(); 
            };

            return window;
        }
    }
}

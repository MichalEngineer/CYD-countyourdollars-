using System;
using CYD.Services;
using CYD.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CYD.Custom_Pages;

namespace CYD
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Logowanie());
        }
        
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

using CYD.Services;
using CYD.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CYD.Custom_Pages;

namespace CYD
{
    public partial class App : Application
    {

        public App()
        {
            MainPage = new MainPage();
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

using System;
using System.Collections.Generic;
using CYD.Custom_Pages;
using CYD.ViewModels;
using CYD.Views;
using Xamarin.Forms;

namespace CYD.Custom_Pages
{
    public partial class HomePage : Xamarin.Forms.Shell
    {
        public HomePage()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

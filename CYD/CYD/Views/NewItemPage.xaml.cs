using System;
using System.Collections.Generic;
using System.ComponentModel;
using CYD.Models;
using CYD.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CYD.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}
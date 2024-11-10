using CYD.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace CYD.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
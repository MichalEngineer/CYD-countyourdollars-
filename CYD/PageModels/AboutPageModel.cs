using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CYD.PageModels
{
    public class AboutPageModel : ObservableObject
    {
        // Mo�esz doda� w�a�ciwo�ci, kt�re b�d� zwi�zane z Twoj� stron� About
        public string AboutText => "This app is designed to help manage your tasks and projects.";
    }
}

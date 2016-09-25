using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BindablePicker.Views.Pages
{
    class MenuPage : ContentPage
    {
        public MenuPage()
        {
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                Children = {
                    new Button {
                        Text = "XAML",
                        Command = new Command(OpenXamlExample),
                    },
                    new Button {
                        Text = "Code Behind",
                        Command = new Command(OpenCodeBehindExample),
                    }
                }
            };
        }


        private void OpenCodeBehindExample(object obj)
        {
            Navigation.PushAsync(new SamplePickerCodeBehind());
            //var nav = new NavigationPage(new SamplePickerCodeBehind());
        }

        private void OpenXamlExample(object obj)
        {
            Navigation.PushAsync(new SamplePickerXAML());
        }
    }
}

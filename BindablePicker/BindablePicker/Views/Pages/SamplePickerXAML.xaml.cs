using BindablePicker.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BindablePicker.Views.Pages
{
	public partial class SamplePickerXAML : ContentPage
	{
		public SamplePickerXAML ()
		{            
            InitializeComponent ();
            BindingContext = new SamplePickerVM();
        }
	}
}

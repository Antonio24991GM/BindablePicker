using BindablePicker.ViewModels;
using Xamarin.Forms;

namespace BindablePicker.Views
{
	public class SamplePickerCodeBehind : ContentPage
	{
		public SamplePickerCodeBehind ()
		{
            var samplePickerVM = new SamplePickerVM();
            BindingContext = samplePickerVM;
            var picker = new UserControls.BindablePicker();
            picker.SetBinding(UserControls.BindablePicker.FormatProperty, nameof(samplePickerVM.Format));
            picker.SetBinding(UserControls.BindablePicker.ElementsProperty, nameof(samplePickerVM.Elements));
            picker.SetBinding(UserControls.BindablePicker.ItemsSourceProperty, nameof(samplePickerVM.Users));
            picker.SetBinding(UserControls.BindablePicker.SelectedItemProperty, nameof(samplePickerVM.UserSelected));

            var buttonFormat = new Button();
            buttonFormat.SetBinding(Button.TextProperty, nameof(samplePickerVM.ButtontFormatText));
            buttonFormat.SetBinding(Button.CommandProperty, nameof(samplePickerVM.ChangeFormatCommand));

            Content = new StackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
				Children = {
                    picker,
                    buttonFormat
                }
			};
		}
	}
}

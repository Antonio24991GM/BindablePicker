using BindablePicker.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace BindablePicker.Views.UserControls
{
    public class BindablePicker : Picker
    {
        #region Fields

        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ItemsSource != null)
            {
                if (SelectedIndex != -1)
                {
                    var enumerator = ItemsSource.GetEnumerator();
                    enumerator.Reset();
                    int i = 0;
                    while (enumerator.MoveNext())
                    {
                        if (SelectedIndex == i)
                        {
                            SelectedItem = enumerator.Current;
                            break;
                        }
                        i++;
                    }
                }
            }
        }

        //Bindable property for the items source
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(BindablePicker), null, BindingMode.TwoWay, propertyChanged: OnItemsSourcePropertyChanged);

        //Bindable property for the selected item
        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(BindablePicker), null, BindingMode.TwoWay, propertyChanged: OnSelectedItemPropertyChanged);

        //Bindable property for the format in cell
        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format), typeof(string), typeof(BindablePicker), null, BindingMode.TwoWay, propertyChanged: OnFormatChange);

        //Bindable property for the properties to add in cell.
        public static readonly BindableProperty ElementsProperty =
            BindableProperty.Create(nameof(Elements), typeof(string[]), typeof(BindablePicker), null, BindingMode.TwoWay, propertyChanged: OnElementsChange);

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        /// <value>
        /// The items source.
        /// </value>
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)GetValue(ItemsSourceProperty);
            }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <value>
        /// The selected item.
        /// </value>
        public string[] Elements
        {
            get { return (string[])GetValue(ElementsProperty); }
            set { SetValue(ElementsProperty, value); }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when properties change
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnElementsChange(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource == null)
            {
                return;
            }
            int index = picker.ItemsSource.IndexOf(picker.SelectedItem);
            picker.Items?.Clear();
            foreach (var item in picker.ItemsSource)
            {
                var values = GetProperties(picker, item);
                picker.Items.Add((string.Format(picker.Format, values) ?? string.Empty));
            }
            picker.SelectedIndex = index;
        }

        /// <summary>
        /// Called when format change
        /// </summary>
        /// <param name="bindable"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        private static void OnFormatChange(BindableObject bindable, object oldValue, object format)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource == null)
            {
                return;
            }
            int index = picker.ItemsSource.IndexOf(picker.SelectedItem);
            picker.Items?.Clear();
            foreach (var item in picker.ItemsSource)
            {
                var values = GetProperties(picker, item);
                picker.Items.Add((string.Format(picker.Format, values) ?? string.Empty));
            }
            picker.SelectedIndex = index;
        }

        /// <summary>
        /// Called when [items source property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object value, object newValue)
        {

            var picker = (BindablePicker)bindable;
            var notifyCollection = newValue as INotifyCollectionChanged;

            if (notifyCollection != null)
            {
                notifyCollection.CollectionChanged += (sender, args) =>
                {
                    if (args.NewItems != null)
                    {
                        foreach (var newItem in args.NewItems)
                        {
                            if (newItem.GetType() == typeof(string))
                            {
                                try
                                {
                                    picker.Items.Add((newItem.ToString() ?? string.Empty));
                                }
                                catch (Exception) { }                                
                                continue;
                            }
                            var values = GetProperties(picker, newItem);
                            if(values.Length > 0)
                            {
                                picker.Items.Add((string.Format(picker.Format, values) ?? string.Empty));
                            }                            
                        }
                    }
                    if (args.OldItems != null)
                    {
                        foreach (var oldItem in args.OldItems)
                        {
                            var values = GetProperties(picker, oldItem);
                            if (oldItem.GetType() == typeof(string))
                            {
                                try
                                {
                                    picker.Items.Add((oldItem.ToString() ?? string.Empty));
                                }
                                catch (Exception) { }                                
                                continue;
                            }
                            if (values.Length > 0)
                            {
                                picker.Items.Remove((string.Format(picker.Format, values) ?? string.Empty));
                            }                            
                        }
                    }
                };
            }

            if (newValue == null || string.IsNullOrEmpty(picker.Format) || picker.Elements == null)
                return;

            picker.Items.Clear();

            foreach (var item in (IEnumerable)newValue)
            {
                var values = GetProperties(picker, item);
                picker.Items.Add((string.Format(picker.Format, values) ?? string.Empty));
            }
        }

        private static string[] GetProperties(BindablePicker picker, object Value)
        {
            List<string> properties = new List<string>();
            Type objectType = Value.GetType();
            foreach (var prop in objectType.GetProperties())
            {
                if (picker.Elements.Contains(prop.Name))
                {
                    var index = picker.Elements.IndexOf(prop.Name);
                    if(properties.Count < index)
                    {
                        index = properties.Count;
                    }
                    try
                    {
                        properties.Insert(index, Convert.ToString(prop.GetValue(Value, null)));
                    }
                    catch(Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "BindablePicker doesn't support property: {0} type: {1}, Error: {2}",
                            prop.Name,
                            prop.PropertyType.ToString(), 
                            ex.Message);
                    }                    
                }
            }
            return properties.ToArray();
        }

        /// <summary>
        /// Called when [selected item property changed].
        /// </summary>
        /// <param name="bindable">The bindable.</param>
        /// <param name="value">The value.</param>
        /// <param name="newValue">The new value.</param>
        private static void OnSelectedItemPropertyChanged(BindableObject bindable, object value, object newValue)
        {
            var picker = (BindablePicker)bindable;
            if (picker.ItemsSource != null)
                picker.SelectedIndex = picker.ItemsSource.IndexOf(picker.SelectedItem);
        }
        #endregion
    }
}

using BindablePicker.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace BindablePicker.ViewModels
{
    public class SamplePickerVM : BaseVM
    {

        public ObservableCollection<User> _users = new ObservableCollection<User>();
        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }
        

        public User _userSelected;
        public User UserSelected
        {
            get
            {
                return _userSelected;
            }
            set
            {
                _userSelected = value;
                OnPropertyChanged();
            }
        }

        private string _format = "{1}, {0} {2} format 1";
        public string Format
        {
            get
            {
                return _format;
            }
            set
            {
                _format = value;
                OnPropertyChanged();
            }
        }

        private string[] _elements = new string[] { nameof(User.LastName), nameof(User.FisrtName), nameof(User.Age) };
        public string[] Elements
        {
            get
            {
                return _elements;
            }
            set
            {
                _elements = value;
                OnPropertyChanged();
            }
        }

        private string _buttonFormatText = "Change to Format 2";
        public string ButtontFormatText
        {
            get
            {
                return _buttonFormatText;
            }
            set
            {
                _buttonFormatText = value;
                OnPropertyChanged();
            }
        }

        private string _buttontElementsText = "Change Elements";
        public string ButtontElementsText
        {
            get
            {
                return _buttontElementsText;
            }
            set
            {
                _buttontElementsText = value;
                OnPropertyChanged();
            }
        }

        public Command ChangeFormatCommand
        {
            get
            {
                return new Command(ChangeFormat);
            }
        }    

        public SamplePickerVM()
        {
            var firstUser = new User()
                {
                    FisrtName = "Sam",
                    LastName = "Smith",
                    Age = 28
                };
            Users.Add(firstUser);
            Users.Add(
                new User()
                {
                    FisrtName = "Ruth",
                    LastName = "Smith",
                    Age = 22
                }
            );
            UserSelected = firstUser;
        }

        private void ChangeFormat(object obj)
        {
            if (Format == "{1}, {0} {2} format 1")
            {
                ButtontFormatText = "Change to Format 1";
                Format = "{0}, {1} {2} format 2";
            }
            else
            {
                ButtontFormatText = "Change to Format 2";
                Format = "{1}, {0} {2} format 1";
            }
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Example
{
    public class Person : INotifyPropertyChanged
    {
        public Person()
        {
            _firstName = String.Empty;
        }
        
        private string _firstName;

        [Required]
        [RegularExpression("\\D+", ErrorMessage = "Cannot contains numbers")]
        [Display(Name="First Name")]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                InvokePropertyChanged("FirstName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void InvokePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

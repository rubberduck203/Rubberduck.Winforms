using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Example
{
    public class Person : INotifyPropertyChanged, IValidatableObject
    {
        private string _firstName;
        private string _lastName;
        private int _age;

        public Person()
        {
            _firstName = String.Empty;
            _lastName = String.Empty;
        }

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

        [Required]
        [RegularExpression("\\D+", ErrorMessage = "Cannot contains numbers")]
        [Display(Name = "Last Name")]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                InvokePropertyChanged("LastName");
            }
        }

        [Range(18, 150)]
        public int Age
        {
            get { return _age; }
            set
            {
                _age = value;
                InvokePropertyChanged("Age");
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == "John" && LastName == "Doe")
            {
                yield return new ValidationResult("Must provide a real name.", new [] {"FirstName", "LastName"});
            }
        }
    }
}

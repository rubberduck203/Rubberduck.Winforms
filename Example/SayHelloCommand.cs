using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Example
{
    public class SayHelloCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var person = parameter as Person;
            string message = "Hello " + ((person == null) ? "Ducky" : person.FirstName);

            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}

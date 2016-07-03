using System;
using System.Windows.Input;
using System.ComponentModel;

namespace Example
{
    public class SayHelloCommand : ICommand
    {
        private readonly Person _person;

        public SayHelloCommand(Person person)
        {
            _person = person;
            _person.PropertyChanged += OnModelPropertyChanged;
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // When the model changes, tell the world that we may need to re-evaluate CanExecute.
            CanExecuteChanged?.Invoke(this, e);
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return (_person != null && _person.FirstName.Length > 3);
        }

        public void Execute(object parameter)
        {
            string message = (parameter as string ?? "Hello") + " " + _person.FirstName;

            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}

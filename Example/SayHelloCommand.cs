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

        private int count;

        public bool CanExecute(object parameter)
        {
            count++;
            return count % 2 == 0;
        }

        public void Execute(object parameter)
        {
            var message = (parameter as string ?? "Hello Ducky!");
            System.Windows.Forms.MessageBox.Show(message);
        }
    }
}

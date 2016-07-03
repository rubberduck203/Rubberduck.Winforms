using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;

namespace Rubberduck.Winforms
{
    [DefaultBindingProperty("Command")]
    public class CommandButton : Button
    {
        private ICommand _command;

        [Bindable(true)]
        [Category("CommandBinding")]
        public ICommand Command
        {
            get { return _command; }
            set
            {
                if (_command != null)
                {
                    _command.CanExecuteChanged -= OnCanExecuteChanged;
                }

                _command = value;

                if (_command != null)
                {
                    _command.CanExecuteChanged += OnCanExecuteChanged;
                }
            }
        }

        [Bindable(true)]
        [Category("CommandBinding")]
        public Object CommandParameter { get; set; }

        protected void OnCanExecuteChanged(object sender, EventArgs e)
        {
            this.Enabled = Command.CanExecute(CommandParameter);
        }
    }
}

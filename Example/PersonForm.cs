using System;
using Rubberduck.Winforms.DataAnnotations;

namespace Example
{
    public partial class PersonForm : ModelBoundForm
    {
        public event EventHandler Submit;

        public PersonForm()
            :base(model: new Person())
        {
            InitializeComponent();

            // Databind the Person.FirstName property to the FirstNameInput textbox
            FirstNameInput.DataBindings.Add(new TextBinding(this.Model, "FirstName"));
            
            // Register the error label for the input control
            Register(ErrorLabel.For(FirstNameInput));
            //Register(ErrorLabel.For(FirstNameInput, ErrorLabel.MessageAlignment.Bottom));
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                var handler = Submit;
                if (handler != null)
                {
                    handler(sender, e);
                }
            }
        }
    }
}

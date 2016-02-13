using System;
using System.ComponentModel;
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
        }

        private void FirstNameInput_Validating(object sender, CancelEventArgs e)
        {
            // To force the user to get this right before moving on
            e.Cancel = !ValidateTextBox(FirstNameInput);

            // Or to just let the user get back to it when they get back to it
            //ValidateTextBox(FirstNameInput);
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

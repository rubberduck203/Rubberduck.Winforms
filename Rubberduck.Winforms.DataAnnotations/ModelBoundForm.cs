using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public partial class ModelBoundForm : Form
    {
        public ModelBoundForm()
        {
            InitializeComponent();
        }

        public ModelBoundForm(Object model)
        {
            InitializeComponent();

            this.Model = model;
        }

        private readonly Dictionary<string, ErrorLabel> _errorLabels = new Dictionary<string, ErrorLabel>();

        /// <summary>
        /// The view model. It must implement INotifyPropertyChanged for validation to work correctly.
        /// </summary>
        public Object Model { get; set; }

        /// <summary>
        /// Validates the <paramref name="textBox"/>, displaying an <see cref="ErrorLabel"/> if the input is invalid.
        /// </summary>
        /// <param name="textBox">The control to be validated.</param>
        /// <returns>True if valid. False if invalid.</returns>
        protected bool ValidateTextBox(TextBox textBox)
        {
            return ValidateControl(textBox, "Text");
        }

        /// <summary>
        /// Validates the <paramref name="control"/>, displaying an <see cref="ErrorLabel"/> if the input is invalid.
        /// </summary>
        /// <param name="control">The control to be validated.</param>
        /// <param name="controlProperty">The name of the control property that is databound to the <see cref="Model"/>.</param>
        /// <returns>True if valid. False if invalid.</returns>
        protected bool ValidateControl(Control control, string controlProperty)
        {
            ControlBindingsCollection bindings = control.DataBindings;

            if (bindings.Count <= 0)
            {
                throw new InvalidOperationException("There are no bindings for " + control.Name + ".");
            }

            Binding binding = bindings[controlProperty];
            if (binding == null)
            {
                throw new ArgumentException("No binding was set for " + control.Name + "." + controlProperty, "controlProperty");
            }

            string boundField = binding.BindingMemberInfo.BindingField;
            var context = new ValidationContext(this.Model, null, null) { MemberName = boundField };

            object propertyValue = this.Model.GetType().InvokeMember(boundField, System.Reflection.BindingFlags.GetProperty, null, this.Model, null);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, context, validationResults);

            var validation = validationResults.FirstOrDefault();

            if (validation == null)
            {
                // Input is valid, if any error labels exist, remove them.
                RemoveErrorLabelFor(control, boundField);
                return true;
            }

            AddErrorLabelFor(control, boundField, validation.ErrorMessage);
            return false;
        }

        private void AddErrorLabelFor(Control control, string boundField, string errorMessage)
        {
            // We have to see if this failed validation last time so that we don't try to add it a second time.

            ErrorLabel errorLabel;
            if (_errorLabels.TryGetValue(boundField, out errorLabel))
            {
                errorLabel.Text = errorMessage;
            }
            else
            {
                //todo: Give the user the ability to specify whether it goes to the right or below of the control being validated
                errorLabel = ErrorLabel.For(control, errorMessage); 

                _errorLabels.Add(boundField, errorLabel);
                control.Parent.Controls.Add(errorLabel);
            }
        }

        private void RemoveErrorLabelFor(Control control, string boundField)
        {
            ErrorLabel errorLabel;
            if (_errorLabels.TryGetValue(boundField, out errorLabel))
            {
                _errorLabels.Remove(boundField);
                control.Parent.Controls.Remove(errorLabel);
            }
        }
    }
}

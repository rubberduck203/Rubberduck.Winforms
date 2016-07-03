using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Rubberduck.Winforms
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

        private void OnLoaded(object sender, EventArgs e)
        {
            // have to wait for child forms to load before their controls are available.
            foreach (var cmdButton in this.Controls.OfType<CommandButton>())
            {
                cmdButton.Click += OnCommandButtonClick;
            }
        }

        public ErrorLabel ValidationSummary { get; }

        private readonly Dictionary<string, ErrorLabel> _errorLabels = new Dictionary<string, ErrorLabel>();

        /// <summary>
        /// The view model. It must implement INotifyPropertyChanged for validation to work correctly.
        /// </summary>
        public object Model { get; set; }

        /// <summary>
        /// Registers the <see cref="ErrorLabel"/> with the <see cref="ModelBoundForm"/> so that it may be displayed.
        /// </summary>
        /// <param name="errorLabel"></param>
        protected void Register(ErrorLabel errorLabel)
        {
            _errorLabels.Add(errorLabel.Control.Name, errorLabel);
            Controls.Add(errorLabel);

            //todo: I don't care for this much. Add support for other types of input
            errorLabel.Control.Validating += (sender, args) => ValidateControl(errorLabel.Control, "Text");
        }

        /// <summary>
        /// Registers the <see cref="Label"/> with the <see cref="ModelBoundForm"/> so that it may be displayed.
        /// </summary>
        /// <param name="label"></param>
        protected void Register(Label label)
        {
            Controls.Add(label);

            //todo: again, add support for other types of input
            var boundField = GetBoundField(label.Control, "Text");

            var attribute = this.Model.GetType()
                .GetProperty(boundField)
                .GetCustomAttributes(typeof(DisplayAttribute), false)
                .FirstOrDefault();

            label.Text = (attribute == null) ? boundField : ((DisplayAttribute)attribute).Name;
        }

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
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            ErrorLabel errorLabel;
            if (!_errorLabels.TryGetValue(control.Name, out errorLabel))
            {
                throw new InvalidOperationException("Unable to retrieve ErrorLabel for control " + control.Name);
            }

            string boundField = GetBoundField(control, controlProperty);
            var context = new ValidationContext(this.Model, null, null) { MemberName = boundField };

            object propertyValue = this.Model.GetType().InvokeMember(boundField, BindingFlags.GetProperty, null, this.Model, null);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(propertyValue, context, validationResults);

            var validation = validationResults.FirstOrDefault();

            if (validation == null)
            {
                //Input is valid
                errorLabel.Text = String.Empty;
                return true;
            }

            errorLabel.Text = validation.ErrorMessage;
            return false;
        }

        public override bool ValidateChildren()
        {
            if (base.ValidateChildren())
            {
                //Individual properties are valid, now validate the object
                return ValidateModel();
            }

            return false;
        }

        private bool ValidateModel()
        {
            var context = new ValidationContext(this.Model, null, null);

            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this.Model, context, validationResults);

            if (!validationResults.Any())
            {
                return true;
            }

            //todo: find a way to unnest the O(n^3) operation.
            foreach (var result in validationResults)
            {
                foreach (var errorLabel in _errorLabels.Values)
                {
                    foreach (var memberName in result.MemberNames)
                    {
                        var boundField = GetBoundField(errorLabel.Control, "Text");
                        if (boundField == memberName)
                        {
                            errorLabel.Text = result.ErrorMessage;
                        }
                    }
                }
            }

            return false;
        }

        private static string GetBoundField(Control control, string controlProperty)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            if (controlProperty == null)
            {
                throw new ArgumentNullException("controlProperty");
            }

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

            return binding.BindingMemberInfo.BindingField;
        }

        private void OnCommandButtonClick(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                return;
            }

            var cmdButton = sender as CommandButton;
            var command = cmdButton.Command;
            var param = cmdButton.CommandParameter;

            if (command != null && command.CanExecute(param))
            {
                command.Execute(param);
            }
        }
    }
}

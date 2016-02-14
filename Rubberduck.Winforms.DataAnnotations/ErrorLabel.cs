using System.Drawing;
using System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public class ErrorLabel : Label
    {
        /// <summary>
        /// Specifies where the <see cref="ErrorLabel"/> will be rendered in relation to the control that's being validated.
        /// </summary>
        public enum MessageAlignment
        {
            Right,
            Bottom
        }

        /// <summary>
        /// Default padding around the ErrorLabel in Pixels.
        /// </summary>
        public new const int DefaultPadding = 10;

        public static ErrorLabel For(Control control)
        {
            return ErrorLabel.For(control, MessageAlignment.Right);
        }

        public static ErrorLabel For(Control control, MessageAlignment alignment)
        {
            return ErrorLabel.For(control, alignment, DefaultPadding);
        }

        public static ErrorLabel For(Control control, MessageAlignment alignment, int padding)
        {
            var errorLabel = new ErrorLabel(control)
            {
                ForeColor = Color.Red,
                AutoSize = true,
                Visible = true
            };

            switch (alignment)
            {
                case MessageAlignment.Right:
                    errorLabel.Location = new Point(control.Width + control.Location.X + padding, control.Location.Y);
                    break;
                case MessageAlignment.Bottom:
                    errorLabel.Location = new Point(control.Location.X, control.Location.Y + control.Height + padding);
                    break;
            }

            errorLabel.BringToFront();

            return errorLabel;
        }

        // Don't allow anyone to directly create an Error label, force them through the factory methods
        // so that we don't have to deal with calling virtual members in the ctor
        private ErrorLabel(Control control)
        {
            _control = control;
        }

        private readonly Control _control;

        /// <summary>
        /// The control that this ErrorLabel is bound to and will show validation messages for.
        /// </summary>
        public Control Control { get { return _control; } }
    }
}
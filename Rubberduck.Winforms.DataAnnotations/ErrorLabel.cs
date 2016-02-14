using System.Diagnostics;
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

        public static ErrorLabel For(Control control, string errorMessage)
        {
            return ErrorLabel.For(control, errorMessage, MessageAlignment.Right);
        }

        public static ErrorLabel For(Control control, string errorMessage, MessageAlignment alignment)
        {
            return ErrorLabel.For(control, errorMessage, alignment, DefaultPadding);
        }

        public static ErrorLabel For(Control control, string errorMessage, MessageAlignment alignment, int padding)
        {
            var errorLabel = new ErrorLabel
            {
                Text = errorMessage,
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
        private ErrorLabel() { }
    }
}
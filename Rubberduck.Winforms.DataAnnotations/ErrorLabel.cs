using System.Drawing;
using Forms = System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public class ErrorLabel : Label
    {
        /// <summary>
        /// Default padding around the ErrorLabel in Pixels.
        /// </summary>
        public new const int DefaultPadding = 10;

        public static new ErrorLabel For(Forms.Control control)
        {
            return ErrorLabel.For(control, Alignment.Right);
        }

        public static ErrorLabel For(Forms.Control control, Alignment alignment)
        {
            return ErrorLabel.For(control, alignment, DefaultPadding);
        }

        public static ErrorLabel For(Forms.Control control, Alignment alignment, int padding)
        {
            var errorLabel = new ErrorLabel(control)
            {
                ForeColor = Color.Red,
                AutoSize = true,
                Visible = true
            };

            switch (alignment)
            {
                case Alignment.Right:
                    errorLabel.Location = new Point(control.Width + control.Location.X + padding, control.Location.Y);
                    break;
                case Alignment.Bottom:
                    // Adding to Y moved the label DOWN the screen.
                    errorLabel.Location = new Point(control.Location.X, control.Location.Y + control.Height + padding);
                    break;
            }

            errorLabel.BringToFront();

            return errorLabel;
        }

        // Don't allow anyone to directly create an Error label, force them through the factory methods
        // so that we don't have to deal with calling virtual members in the ctor
        private ErrorLabel(Forms.Control control)
            :base(control)
        { }
    }

    /// <summary>
    /// Specifies where the <see cref="ErrorLabel"/> will be rendered in relation to the control that's being validated.
    /// </summary>
    public enum Alignment
    {
        Top,
        Bottom,
        Right,
        Left
    }
}
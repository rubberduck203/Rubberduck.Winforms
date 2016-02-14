using System.Drawing;
using Forms = System.Windows.Forms;

namespace Rubberduck.Winforms
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

        public static new ErrorLabel For(Forms.Control control, Alignment alignment)
        {
            return ErrorLabel.For(control, alignment, DefaultPadding);
        }

        public static new ErrorLabel For(Forms.Control control, Alignment alignment, int padding)
        {
            var errorLabel = new ErrorLabel(control)
            {
                ForeColor = Color.Red,
                AutoSize = true,
                Visible = true
            };

            SetAlignment(errorLabel, control, alignment, padding);

            errorLabel.BringToFront();

            return errorLabel;
        }

        // Don't allow anyone to directly create an Error label, force them through the factory methods
        // so that we don't have to deal with calling virtual members in the ctor
        private ErrorLabel(Forms.Control control)
            :base(control)
        { }
    }
}
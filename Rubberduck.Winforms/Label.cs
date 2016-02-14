using System;
using System.Drawing;
using Forms = System.Windows.Forms;

namespace Rubberduck.Winforms
{
    public class Label : Forms.Label
    {
        /// <summary>
        /// Default padding around the Label in Pixels.
        /// </summary>
        public new const int DefaultPadding = 0;

        public static Label For(Forms.Control control)
        {
           return Label.For(control, Alignment.Top, DefaultPadding);
        }

        public static Label For(Forms.Control control, Alignment alignment)
        {
            return Label.For(control, alignment, DefaultPadding);
        }

        public static Label For(Forms.Control control, Alignment alignment, int padding)
        {
            var label = new Label(control)
            {
                AutoSize = true,
                Visible = true
            };

            label.BringToFront();

            SetAlignment(label, control, alignment, padding);

            return label;
        }

        protected static void SetAlignment(Label label, Forms.Control control, Alignment alignment, int padding)
        {
            switch (alignment)
            {
                case Alignment.Top:
                    // Subtracting actually moves the label UP the screen
                    label.Location = new Point(control.Location.X, control.Location.Y - control.Height - padding);
                    break;
                case Alignment.Bottom:
                    // Adding to Y moved the label DOWN the screen.
                    label.Location = new Point(control.Location.X, control.Location.Y + control.Height + padding);
                    break;
                case Alignment.Right:
                    label.Location = new Point(control.Width + control.Location.X + padding, control.Location.Y);
                    break;
                case Alignment.Left:
                    // todo: I'm not real sure how to go about making sure the label doesn't overlap with the control
                    throw new NotSupportedException("We've not added support for Left alignment yet.");
            }
        }

        private readonly Forms.Control _control;

        /// <summary>
        /// The control that this Label is bound to.
        /// </summary>
        public Forms.Control Control { get { return _control; } }

        // Don't allow anyone to directly create a label.
        // Force them through the factory methods
        // so that we don't have to deal with calling virtual members in the ctor.
        protected Label(Forms.Control control)
        {
            _control = control;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Forms = System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public class Label : Forms.Label
    {
        private new const int DefaultPadding = 0;

        public static Label For(Forms.Control control)
        {
           return Label.For(control, DefaultPadding);
        }

        public static Label For(Forms.Control control, int padding)
        {
            var label = new Label(control)
            {
                AutoSize = true,
                Visible = true
            };

            label.BringToFront();

            // Subtracting actually moves the label UP the screen
            label.Location = new Point(control.Location.X, control.Location.Y - control.Height - padding);

            return label;
        }

        private readonly Forms.Control _control;

        /// <summary>
        /// The control that this Label is bound to.
        /// </summary>
        public Forms.Control Control { get { return _control; } }

        protected Label(Forms.Control control)
        {
            _control = control;
        }

    }
}

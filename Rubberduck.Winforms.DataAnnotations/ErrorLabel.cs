using System.Drawing;
using System.Windows.Forms;

namespace Rubberduck.Winforms.DataAnnotations
{
    public class ErrorLabel : Label
    {
        public ErrorLabel(Control control, string errorMessage)
        {
            this.ForeColor = Color.Red;

            //todo: take a page from ErrorProvider and allow user to specify padding and alignment
            this.Location = new Point(control.Width + control.Location.X + 10, control.Location.Y);

            this.AutoSize = true;

            this.BringToFront();
            this.Visible = true;

            this.Text = errorMessage;
        }

        //note:leave the error provider here as a reminder of how it's possible to implement ErrorLabel with a cleaner interface and look

        //var errorProvider = new ErrorProvider();

        //errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
        //errorProvider.SetIconAlignment(control, ErrorIconAlignment.MiddleRight);
        //errorProvider.SetIconPadding(control, 10);

        //errorProvider.SetError(control, validation.ErrorMessage);
    }
}
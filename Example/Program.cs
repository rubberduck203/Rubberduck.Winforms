using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //todo: move to MVP pattern

            Application.Run(new PersonForm());
        }
    }
}

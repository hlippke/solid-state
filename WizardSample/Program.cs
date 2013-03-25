using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace WizardSample
{
    static class Program
    {
        public static MainForm MainForm;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Create a public reference to the main form that can be used by the pages.
            MainForm = new MainForm();

            Application.Run(MainForm);
        }
    }
}

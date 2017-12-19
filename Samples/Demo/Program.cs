using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Samples.Demo
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
            Application.DoEvents();

            Application.Run(new DemoForm());
        }
    }
}
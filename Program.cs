using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dispetcher2.Class;

namespace Dispetcher2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var d = new DispetcherContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(d.ResolveStartForm());
        }


    }

    
}

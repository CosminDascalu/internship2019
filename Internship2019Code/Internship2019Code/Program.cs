using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Internship2019Code
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

            // the results can be seen here
            ResultsForm resultsForm = new ResultsForm();
            List<Atm> atmsRoute = resultsForm.getRoute();
            for(int i = 0; i < atmsRoute.Count; i++)
            {
                Console.WriteLine(atmsRoute.ElementAt(i).getName());
            }

            Application.Run(resultsForm);
        }
    }
}

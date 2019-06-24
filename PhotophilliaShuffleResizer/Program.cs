using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using WhetStone.Looping;

namespace PhotophilliaShuffleResizer
{
    //todo crop rectange
    //todo build job with destinations
    //todo AOI?
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string jobfile = args.SingleOrDefault();
            var jobselect = new MainForm(jobfile);
            Application.Run(jobselect);
        }
    }
}

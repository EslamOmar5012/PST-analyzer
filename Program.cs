using System;
using System.Windows.Forms;
using PstAnalyzer.Forms;

namespace PstAnalyzer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainAnalyzerForm());
        }
    }
}

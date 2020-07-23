using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace donutTestSimpleDotNetApp
{
    class Program
    {
        static void Main(string[] args)
        {
            int nProcessID = Process.GetCurrentProcess().Id;
            string message = nProcessID.ToString();
            MessageBox.Show(message,"Box from PID:");
        }
    }
}
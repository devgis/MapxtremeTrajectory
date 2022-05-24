using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Devgis.Trajectory
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args == null || args.Length <= 0)
            {
                PublicDim.DataPath=Path.Combine(Application.StartupPath,"Data");
            }
            else
            {
                PublicDim.DataPath = args[0];
            }
            if (!Directory.Exists(PublicDim.DataPath))
            {
                PublicDim.ShowErrorMessage("路径不存在！");
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainMapForm());
            }
        }
    }
}
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
            DateTime dtStart = new DateTime(2014, 4, 27);
            DateTime dtNown = DateTime.Now;
            TimeSpan tsTime = dtNown - dtStart;
            if (tsTime.Days > 60)
            {
                System.Diagnostics.Process.Start("http://flysoft.taobao.com/");
                throw new Exception("产品已经过期!");
            }

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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;

namespace Devgis.Trajectory
{
    public partial class WaitForm : Form
    {
        public delegate void ShowProgressDelegete();

        private static WaitForm frm = null;
        private static Thread th;

        private WaitForm()
        {
            InitializeComponent();
            this.lblMsg.Text = _title;
            this.lbl_Msg.Text = _msg;
        }
        
        private static string title = "数据处理中，请耐心等待……";
        private static string _title  = title;
        private string _msg = string.Empty;

        public static void ShowWaite()
        {
            th = new Thread(new ThreadStart(WaitForm.ShowProgress));
            th.Start();
            while (th.ThreadState != ThreadState.Running)
            {  
            }
        }

        public static void ShowWaite(string title)
        {
            _title = title;
            th = new Thread(new ThreadStart(WaitForm.ShowProgress));
            th.Start();
            while (th.ThreadState != ThreadState.Running)
            {
                MessageBox.Show(Enum.GetName(typeof(ThreadState), th.ThreadState));
            }
        }

        public static void InvokCloseWait(Form frmMain)
        {
            frmMain.Invoke(new ShowProgressDelegete(WaitForm.CloseProgress));
        }

        public static void ChangeMsg(Form frmMain, string msg)
        {
            if (!(frm == null || frm.IsDisposed))
            {
                frm._msg = msg;
                frmMain.Invoke(new ShowProgressDelegete(WaitForm.ChangeLbl));
            }
            
        }

        private static void ChangeLbl()
        {
            while (frm == null)
            {
                th.Join();
            }
            if (!(frm == null || frm.IsDisposed))
            {
                frm.lbl_Msg.Text = frm._msg;
            }
        }

        private static void ShowProgress()
        {
            if (frm == null || frm.IsDisposed)
            {
                frm = new WaitForm();
                frm.ShowDialog();
            }
            else
            {
                frm.ShowDialog();
            }
        }

        private static void CloseProgress()
        {
            if (th.ThreadState == ThreadState.Running)
            {
                th.Abort();
                th.Join();
                Initialize();
            }
        }

        private static void Initialize()
        {
            frm = null;
            th = null;
            _title = title;
        }

        private void lbl_Msg_TextChanged(object sender, EventArgs e)
        {
            pan_Msg.Visible = !String.IsNullOrEmpty(lbl_Msg.Text);
        }
    }
}

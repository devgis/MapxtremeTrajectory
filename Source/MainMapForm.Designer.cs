namespace Devgis.Trajectory
{
    partial class MainMapForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mapControl1 = new MapInfo.Windows.Controls.MapControl();
            this.mapToolBar1 = new MapInfo.Windows.Controls.MapToolBar();
            this.mapToolBarButtonLayerControl = new MapInfo.Windows.Controls.MapToolBarButton();
            this.toolBarButtonSeparator = new System.Windows.Forms.ToolBarButton();
            this.mapToolBarButtonSelect = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonZoomIn = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonZoomOut = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonPan = new MapInfo.Windows.Controls.MapToolBarButton();
            this.toolBarButtonSeparator2 = new System.Windows.Forms.ToolBarButton();
            this.mapToolBarButtonSelectRect = new MapInfo.Windows.Controls.MapToolBarButton();
            this.mapToolBarButtonSelectRadius = new MapInfo.Windows.Controls.MapToolBarButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.timerTrack = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapControl1
            // 
            this.mapControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapControl1.IgnoreLostFocusEvent = false;
            this.mapControl1.Location = new System.Drawing.Point(0, 0);
            this.mapControl1.Name = "mapControl1";
            this.mapControl1.Size = new System.Drawing.Size(1311, 685);
            this.mapControl1.TabIndex = 0;
            this.mapControl1.Text = "mapControl1";
            this.mapControl1.Tools.LeftButtonTool = null;
            this.mapControl1.Tools.MiddleButtonTool = null;
            this.mapControl1.Tools.RightButtonTool = null;
            // 
            // mapToolBar1
            // 
            this.mapToolBar1.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.mapToolBarButtonLayerControl,
            this.toolBarButtonSeparator,
            this.mapToolBarButtonSelect,
            this.mapToolBarButtonZoomIn,
            this.mapToolBarButtonZoomOut,
            this.mapToolBarButtonPan,
            this.toolBarButtonSeparator2,
            this.mapToolBarButtonSelectRect,
            this.mapToolBarButtonSelectRadius});
            this.mapToolBar1.DropDownArrows = true;
            this.mapToolBar1.Location = new System.Drawing.Point(0, 0);
            this.mapToolBar1.MapControl = this.mapControl1;
            this.mapToolBar1.Name = "mapToolBar1";
            this.mapToolBar1.ShowToolTips = true;
            this.mapToolBar1.Size = new System.Drawing.Size(1315, 28);
            this.mapToolBar1.TabIndex = 1;
            // 
            // mapToolBarButtonLayerControl
            // 
            this.mapToolBarButtonLayerControl.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.LayerControl;
            this.mapToolBarButtonLayerControl.Name = "mapToolBarButtonLayerControl";
            this.mapToolBarButtonLayerControl.ToolTipText = "图层控制";
            // 
            // toolBarButtonSeparator
            // 
            this.toolBarButtonSeparator.Name = "toolBarButtonSeparator";
            this.toolBarButtonSeparator.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // mapToolBarButtonSelect
            // 
            this.mapToolBarButtonSelect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Select;
            this.mapToolBarButtonSelect.Name = "mapToolBarButtonSelect";
            this.mapToolBarButtonSelect.ToolTipText = "选择";
            // 
            // mapToolBarButtonZoomIn
            // 
            this.mapToolBarButtonZoomIn.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomIn;
            this.mapToolBarButtonZoomIn.Name = "mapToolBarButtonZoomIn";
            this.mapToolBarButtonZoomIn.ToolTipText = "放大";
            // 
            // mapToolBarButtonZoomOut
            // 
            this.mapToolBarButtonZoomOut.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.ZoomOut;
            this.mapToolBarButtonZoomOut.Name = "mapToolBarButtonZoomOut";
            this.mapToolBarButtonZoomOut.ToolTipText = "缩小";
            // 
            // mapToolBarButtonPan
            // 
            this.mapToolBarButtonPan.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.Pan;
            this.mapToolBarButtonPan.Name = "mapToolBarButtonPan";
            this.mapToolBarButtonPan.ToolTipText = "平移";
            // 
            // toolBarButtonSeparator2
            // 
            this.toolBarButtonSeparator2.Name = "toolBarButtonSeparator2";
            this.toolBarButtonSeparator2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // mapToolBarButtonSelectRect
            // 
            this.mapToolBarButtonSelectRect.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.SelectRectangle;
            this.mapToolBarButtonSelectRect.Name = "mapToolBarButtonSelectRect";
            this.mapToolBarButtonSelectRect.ToolTipText = "矩形选择";
            this.mapToolBarButtonSelectRect.Visible = false;
            // 
            // mapToolBarButtonSelectRadius
            // 
            this.mapToolBarButtonSelectRadius.ButtonType = MapInfo.Windows.Controls.MapToolButtonType.SelectRadius;
            this.mapToolBarButtonSelectRadius.Name = "mapToolBarButtonSelectRadius";
            this.mapToolBarButtonSelectRadius.ToolTipText = "半径选择";
            this.mapToolBarButtonSelectRadius.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.tsslTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 719);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1315, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(131, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.ToolTipText = "地图缩放";
            // 
            // tsslTime
            // 
            this.tsslTime.Name = "tsslTime";
            this.tsslTime.Size = new System.Drawing.Size(0, 17);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.mapControl1);
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1315, 689);
            this.panel1.TabIndex = 3;
            // 
            // timerTrack
            // 
            this.timerTrack.Interval = 200;
            // 
            // MainMapForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1315, 741);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mapToolBar1);
            this.Controls.Add(this.panel1);
            this.Name = "MainMapForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "历史轨迹回放演示(如需购买源码请联系QQ:80163278)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MapForm1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MapInfo.Windows.Controls.MapControl mapControl1;
        private MapInfo.Windows.Controls.MapToolBar mapToolBar1;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonLayerControl;
        private System.Windows.Forms.ToolBarButton toolBarButtonSeparator;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelect;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomIn;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonZoomOut;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonPan;
        private System.Windows.Forms.ToolBarButton toolBarButtonSeparator2;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelectRect;
        private MapInfo.Windows.Controls.MapToolBarButton mapToolBarButtonSelectRadius;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Timer timerTrack;
        private System.Windows.Forms.ToolStripStatusLabel tsslTime;
    }
}


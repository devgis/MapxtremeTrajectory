using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using MapInfo.Data;
using MapInfo.Engine;
using MapInfo.Mapping;
using MapInfo.Geometry;
using MapInfo.Windows;
using MapInfo.Styles;
using System.IO;

namespace Devgis.Trajectory
{
    public partial class MainMapForm : Form
    {
        List<CarPoint> listCarPoint = new List<CarPoint>();
        int iPlaySpeed = 20; //播放间隔mm
        bool iTrackBarCanChange = true;
        //DataTable dt;
        Feature fMove;
        CoordSys cs;
        Table CarTable,HistoryLine;
        int iLength;
        MapInfo.Styles.SimpleLineStyle slsLine = new MapInfo.Styles.SimpleLineStyle(new LineWidth(3, LineWidthUnit.Pixel), 2, System.Drawing.Color.OrangeRed);
        BitmapPointStyle carStyle = new BitmapPointStyle("AMBU1-32.BMP", BitmapStyles.None, Color.Aqua, 20);
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public MainMapForm()
        {
            InitializeComponent();
            mapControl1.Map.ViewChangedEvent += new MapInfo.Mapping.ViewChangedEventHandler(Map_ViewChangedEvent);
            Map_ViewChangedEvent(this, null);
        }
        /// <summary>
        /// 地图视图改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Map_ViewChangedEvent(object sender, MapInfo.Mapping.ViewChangedEventArgs e)
        {
            // Display the zoom level
            Double dblZoom = System.Convert.ToDouble(String.Format("{0:E2}", mapControl1.Map.Zoom.Value));
            if (statusStrip1.Items.Count > 0)
            {
                statusStrip1.Items[0].Text = "缩放: " + dblZoom.ToString() + " " + MapInfo.Geometry.CoordSys.DistanceUnitAbbreviation(mapControl1.Map.Zoom.Unit);
            }
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MapForm1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(PublicDim.DataPath))
            {
                throw new Exception("路径不存在！");
            }
            else
            {
                string[] fList = Directory.GetFiles(PublicDim.DataPath, "*.txt");
                foreach (string fname in fList)
                {
                    List<CarPoint> listPoint = new List<CarPoint>();
                    string CarNo = Path.GetFileNameWithoutExtension(fname);

                    StreamReader sr = new StreamReader(fname);

                    while (true)
                    {
                        string str = sr.ReadLine();
                        if (String.IsNullOrEmpty(str))
                        {
                            break;
                        }
                        //PublicDim.ShowInfoMessage(str);
                        String[] s = str.Split(',');
                        CarPoint cPoint = new CarPoint();
                        cPoint.PosID = CarNo;
                        cPoint.PTime = DateTime.Parse(s[0]);
                        cPoint.PosX = double.Parse(s[1]);
                        cPoint.PosY = double.Parse(s[2]);
                        listPoint.Add(cPoint);
                    }
                    PublicDim.ShowInfoMessage(listPoint.Count.ToString());

                }
            }
            return;

            //初始化背景地图信息
            comboBox1.SelectedIndex = 0;
            //string MapPath = @"E:\CODE\SALE\功能\Data\World.mws";
            string MapPath = Path.Combine(Application.StartupPath, @"Map\map.mws");
            MapWorkSpaceLoader mwsLoader = new MapWorkSpaceLoader(MapPath);
            mapControl1.Map.Load(mwsLoader);
            mapControl1.Map.Center = new DPoint(108.5555,34.5555);
            cs = mapControl1.Map.GetDisplayCoordSys();

            loadCarHistoryLayer();//初始化轨迹图层
        }

        private void btPlay_Click(object sender, EventArgs e)
        {
            //dt = DBHelper.Instance.GetDataTable(String.Format("select posx,posy,time from t_Trajectory where historyid='{0}' order by time", ID));
            trackBar.Maximum = listCarPoint.Count-1;
            if (trackBar.Maximum < 0)
                trackBar.Maximum = 0;

            if (listCarPoint == null || listCarPoint.Count <= 0)
            {
                PublicDim.ShowErrorMessage("无数据，请初始化数据！");
                return;
            }

            timerTrack.Interval = Convert.ToInt32(comboBox1.Text);
            btPlay.Enabled = false;
            ckTrace.Enabled = false;
            trackBar.Enabled = true;
            iLength = listCarPoint.Count;
            if (iLength <= 0)
            {
                ckTrace.Enabled = true;
                btPlay.Enabled = true;
                trackBar.Enabled = false;
                return;
            }

            //清除原有数据
            CarTable = Session.Current.Catalog.GetTable("CarHistory");
            (CarTable as ITableFeatureCollection).Clear();
            HistoryLine = Session.Current.Catalog.GetTable("HistoryLine");
            (HistoryLine as ITableFeatureCollection).Clear();
            fMove = new Feature(CarTable.TableInfo.Columns);
            timerTrack.Start();
        }

        /// <summary>
        /// 初始化图层信息（车辆图层和轨迹图层）
        /// </summary>
        private void loadCarHistoryLayer()
        {
            //加载临时图层用于回放车辆历史轨迹
            TableInfoMemTable ti = new TableInfoMemTable("CarHistory");
            ti.Temporary = true;

            //   add   columns   
            Column column;
            column = new GeometryColumn(mapControl1.Map.GetDisplayCoordSys());
            column.Alias = "MI_Geometry";
            column.DataType = MIDbType.FeatureGeometry;
            ti.Columns.Add(column);

            column = new Column();
            column.Alias = "MI_Style";
            column.DataType = MIDbType.Style;
            ti.Columns.Add(column);

            column = new Column();
            column.Alias = "MI_Prinx";
            column.DataType = MIDbType.Int;
            ti.Columns.Add(column);

            //   create   table   and   feature   layer 
            Table table = Session.Current.Catalog.GetTable("CarHistory");
            if (table == null)
            {
                table = Session.Current.Catalog.CreateTable(ti);
            }
            FeatureLayer flCarHistory = new FeatureLayer(table, "CarHistory ", "aCarHistory");

            //临时线路图层
            TableInfoMemTable tiLine = new TableInfoMemTable("HistoryLine");
            Column columnLine = new GeometryColumn(mapControl1.Map.GetDisplayCoordSys());
            columnLine.Alias = "MI_Geometry";
            columnLine.DataType = MIDbType.FeatureGeometry;
            tiLine.Columns.Add(columnLine);

            columnLine = new Column();
            columnLine.Alias = "MI_Style";
            columnLine.DataType = MIDbType.Style;
            tiLine.Columns.Add(columnLine);

            columnLine = new Column();
            columnLine.Alias = "MI_Prinx";
            columnLine.DataType = MIDbType.Int;
            tiLine.Columns.Add(columnLine);

            Table tableLine = Session.Current.Catalog.CreateTable(tiLine);
            FeatureLayer flHistoryLine = new FeatureLayer(tableLine, "HistoryLine ", "aHistoryLine");

            mapControl1.Map.Layers.Add(flHistoryLine);
            mapControl1.Map.Layers.Add(flCarHistory);
        }

        private void timerTrack_Tick(object sender, EventArgs e)
        {
            if (!iTrackBarCanChange)
                return;
            mapControl1.Refresh();
            timerTrack.Interval = iPlaySpeed;
            (CarTable as ITableFeatureCollection).Clear();
            
            creatPoint(trackBar.Value);
            if (ckTrace.Checked)
            {
                creatLine(trackBar.Value);
            }

            trackBar.Value++;
            if (trackBar.Value == listCarPoint.Count - 1)
            { 
                btPlay.Enabled = true;
                ckTrace.Enabled = true;
                trackBar.Enabled = false;
                trackBar.Value = 0;
                timerTrack.Stop();
            }
        }

        private DPoint getDP(int index)
        {
            DPoint dp = new DPoint(listCarPoint[index].PosX, listCarPoint[index].PosY);
            return dp;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            iTrackBarCanChange = true;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                iPlaySpeed = Convert.ToInt32(comboBox1.Text);
            }
            catch
            {
                iPlaySpeed = 100;
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            timerTrack.Stop();
            trackBar.Value = 0;
            ckTrace.Enabled = true;
            trackBar.Enabled = false;
            btPlay.Enabled = true;
            trackBar.Value = 0;
            tsslTime.Text = string.Empty;
            try
            {
                (CarTable as ITableFeatureCollection).Clear();
                (HistoryLine as ITableFeatureCollection).Clear();
            }
            catch
            { }
        }

        /// <summary>
        /// 绘制轨迹信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btHistoryLine_Click(object sender, EventArgs e)
        {
            int iPointNo = listCarPoint.Count;
            if (iPointNo <= 1)
            {
                PublicDim.ShowErrorMessage("无历史轨迹数据！");
                return;
            }

            //清除原有数据
            CarTable = Session.Current.Catalog.GetTable("CarHistory");
            (CarTable as ITableFeatureCollection).Clear();
            HistoryLine = Session.Current.Catalog.GetTable("HistoryLine");
            (HistoryLine as ITableFeatureCollection).Clear();

            DPoint[] dp = new DPoint[listCarPoint.Count];
            for (int i = 0; i < iPointNo; i++)
            {
                dp[i] = new DPoint(listCarPoint[i].PosX, listCarPoint[i].PosY);
            }
            FeatureGeometry pgLine = new MultiCurve(cs, CurveSegmentType.Linear, dp);
            Feature ftr = new Feature(pgLine, slsLine);
            HistoryLine.InsertFeature(ftr);

        }
        /// <summary>
        /// 刷新车辆位置信息
        /// </summary>
        /// <param name="index"></param>
        private void creatPoint(int index)
        {
            tsslTime.Text = listCarPoint[index].PTime.ToString();
            (CarTable as ITableFeatureCollection).Clear(); //清除老数据
            fMove = new Feature(CarTable.TableInfo.Columns);
            MapInfo.Geometry.Point pt;
            pt = new MapInfo.Geometry.Point(cs, getDP(index));
            fMove.Geometry = pt;
            fMove.Style = carStyle;
            
            CarTable.InsertFeature(fMove);
            this.mapControl1.Refresh();

        }
        /// <summary>
        /// 绘制线路轨迹
        /// </summary>
        /// <param name="index"></param>
        private void creatLine(int index)
        {
            if (index == trackBar.Maximum - 1)
                return;
            HistoryLine = Session.Current.Catalog.GetTable("HistoryLine");
            DPoint dStart = getDP(index);
            DPoint dEnd = getDP(index + 1);
            FeatureGeometry pgLine = MultiCurve.CreateLine(cs, dStart, dEnd);
            Feature ftr = new Feature(pgLine, slsLine);
            HistoryLine.InsertFeature(ftr);
        }
        private void MainMapForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.Start("http://flysoft.taobao.com/");
        }
    }
}
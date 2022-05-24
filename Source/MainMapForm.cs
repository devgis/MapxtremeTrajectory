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
        //DataTable dt;
        Feature fMove;
        CoordSys cs;
        Table CarTable,HistoryLine;
        SimpleLineStyle slsLine = new SimpleLineStyle(new LineWidth(3, LineWidthUnit.Pixel), 2, System.Drawing.Color.Red);
        SimpleVectorPointStyle NodeStyle = new SimpleVectorPointStyle(34, Color.Green, 6);
        BitmapPointStyle carStyle = new BitmapPointStyle("AMBU1-32.BMP", BitmapStyles.None, Color.Aqua, 20);
        double MaxX = double.MinValue;
        double MinX = double.MaxValue;
        double MaxY = double.MinValue;
        double MinY = double.MaxValue;
        
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
            WaitForm.ShowWaite("加载数据中...");
            //初始化背景地图信息
            //string MapPath = @"E:\CODE\SALE\功能\Data\World.mws";
            //string MapPath = Path.Combine(Application.StartupPath, @"Map\map.mws");
            string MapPath = @"E:\WORK\Map\mapnew\全国电子地图\map.mws";
            MapWorkSpaceLoader mwsLoader = new MapWorkSpaceLoader(MapPath);
            mapControl1.Map.Load(mwsLoader);
            mapControl1.Map.Center = new DPoint(108.5555,34.5555);
            cs = mapControl1.Map.GetDisplayCoordSys();
            //WaitForm.ShowWaite("准备创建图层...");
            loadCarHistoryLayer();//初始化轨迹图层

            CarTable = Session.Current.Catalog.GetTable("CarHistory");
            HistoryLine = Session.Current.Catalog.GetTable("HistoryLine");
            //WaitForm.ShowWaite("加载数据中...");
            string[] fList = Directory.GetFiles(PublicDim.DataPath, "*.txt");
            foreach (string fname in fList)
            {
                listCarPoint.Clear();
                string CarNo = Path.GetFileNameWithoutExtension(fname);
                //WaitForm.ShowWaite(String.Format("加载{0}数据中...", CarNo));
                StreamReader sr = new StreamReader(fname);

                while (true)
                {
                    string str = sr.ReadLine();
                    if (String.IsNullOrEmpty(str))
                    {
                        break;
                    }
                    String[] s = str.Split(',');
                    CarPoint cPoint = new CarPoint();
                    cPoint.PosID = CarNo;
                    cPoint.PTime = DateTime.Parse(s[0]);
                    cPoint.PosX = double.Parse(s[1]);
                    cPoint.PosY = double.Parse(s[2]);
                    listCarPoint.Add(cPoint);

                    if (cPoint.PosX > MaxX)
                    {
                        MaxX = cPoint.PosX;
                    }
                    if (cPoint.PosY > MaxY)
                    {
                        MaxY = cPoint.PosY;
                    }
                    if (cPoint.PosX< MinX)
                    {
                        MinX = cPoint.PosX;
                    }
                    if (cPoint.PosY < MinY)
                    {
                        MinY = cPoint.PosY;
                    }
                }
                creatOneLine();
                for (int i = 0; i < listCarPoint.Count - 1; i++)
                {
                    creatPoint(listCarPoint[i].PosX, listCarPoint[i].PosY, listCarPoint[i].PTime.ToString());
                }
                    creatPoint(listCarPoint.Count - 1, CarNo);

            }
            mapControl1.Map.SetView(new DRect(MinX, MinY, MaxX, MaxY), cs);
            WaitForm.InvokCloseWait(this);
        }

        /// <summary>
        /// 初始化图层信息（车辆图层和轨迹图层）
        /// </summary>
        private void loadCarHistoryLayer()
        {
            //加载临时图层用于回放  以及时间车次
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

            column = new Column();
            column.Alias = "CarNO";
            column.DataType = MIDbType.String;
            ti.Columns.Add(column);

            //   create   table   and   feature   layer 
            Table table = Session.Current.Catalog.GetTable("CarHistory");
            if (table == null)
            {
                table = Session.Current.Catalog.CreateTable(ti);
            }
            FeatureLayer flCarHistory = new FeatureLayer(table, "CarHistory ", "aCarHistory");

            //加载车辆图层标签
            LabelSource sourceCar = new LabelSource(table);//绑定Table
            sourceCar.DefaultLabelProperties.Caption = "CarNO";//指定哪个字段作为显示标注
            sourceCar.DefaultLabelProperties.Style.Font.ForeColor = Color.Red;
            sourceCar.DefaultLabelProperties.CalloutLine.Use = false;  //是否使用标注线  
            sourceCar.DefaultLabelProperties.Layout.Offset = 5;//标注偏移   
            sourceCar.DefaultLabelProperties.Style.Font = new MapInfo.Styles.Font("宋体", 10);
            LabelLayer carLabelLayer = new LabelLayer();
            carLabelLayer.Sources.Append(sourceCar);//加载指定数据

            

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

            columnLine = new Column();
            columnLine.Alias = "TheTime";
            columnLine.DataType = MIDbType.String;
            tiLine.Columns.Add(columnLine);

            Table tableLine = Session.Current.Catalog.CreateTable(tiLine);
            FeatureLayer flHistoryLine = new FeatureLayer(tableLine, "HistoryLine ", "aHistoryLine");

            mapControl1.Map.Layers.Add(flHistoryLine);
            mapControl1.Map.Layers.Add(flCarHistory);

            //加载标签图层
            mapControl1.Map.Layers.Add(carLabelLayer);

            
        }

        private DPoint getDP(int index)
        {
            DPoint dp = new DPoint(listCarPoint[index].PosX, listCarPoint[index].PosY);
            return dp;
        }

        /// <summary>
        /// 刷新车辆位置信息
        /// </summary>
        /// <param name="index"></param>
        private void creatPoint(int index,String CarNo)
        {
            //tsslTime.Text = listCarPoint[index].PTime.ToString();
            //(CarTable as ITableFeatureCollection).Clear(); //清除老数据
            fMove = new Feature(CarTable.TableInfo.Columns);
            MapInfo.Geometry.Point pt;
            pt = new MapInfo.Geometry.Point(cs, getDP(index));
            fMove.Geometry = pt;
            fMove.Style = carStyle;
            fMove["CarNO"] = CarNo;
            CarTable.InsertFeature(fMove);
        }

        /// <summary>
        /// 创建车辆时间信息
        /// </summary>
        /// <param name="index"></param>
        private void creatPoint(double x,double y, String pTime)
        {
            fMove = new Feature(CarTable.TableInfo.Columns);
            MapInfo.Geometry.Point pt;
            pt = new MapInfo.Geometry.Point(cs,x,y);
            fMove.Geometry = pt;
            fMove.Style = NodeStyle;
            fMove["CarNO"] = pTime;
            CarTable.InsertFeature(fMove);
        }
        /// <summary>
        /// 绘制线路轨迹
        /// </summary>
        /// <param name="index"></param>
        private void creatLine(int index)
        {
            if (index == listCarPoint.Count - 1)
                return;
            DPoint dStart = getDP(index);
            DPoint dEnd = getDP(index + 1);
            FeatureGeometry pgLine = MultiCurve.CreateLine(cs, dStart, dEnd);
            Feature ftr = new Feature(pgLine, slsLine);
            HistoryLine.InsertFeature(ftr);
        }
        Random rd = new Random();
        /// <summary>
        /// 绘制线路轨迹
        /// </summary>
        /// <param name="index"></param>
        private void creatOneLine()
        {
            DPoint[] dpPoint = new DPoint[listCarPoint.Count];
            for (int i = 0; i < listCarPoint.Count; i++)
            {
                dpPoint[i]=new DPoint(listCarPoint[i].PosX,listCarPoint[i].PosY);
            }
            FeatureGeometry pgLine = new MultiCurve(cs, CurveSegmentType.Linear, dpPoint);
            Feature ftr = new Feature(HistoryLine.TableInfo.Columns);
            ftr.Geometry = pgLine;
            ftr.Style = new MapInfo.Styles.SimpleLineStyle(new LineWidth(3, LineWidthUnit.Pixel), 2, Color.FromArgb(rd.Next(20, 200),rd.Next(20, 200),rd.Next(20, 200)));
            HistoryLine.InsertFeature(ftr);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Devgis.Trajectory
{
    /// <summary>
    /// 点坐标信息
    /// </summary>
    public class CarPoint
    {
        private string _PosID;
        /// <summary>
        /// ID
        /// </summary>
        public string PosID
        {
            get
            {
                return _PosID;
            }
            set
            {
                _PosID = value;
            }
        }
        
        private double _PosX;
        /// <summary>
        /// X坐标
        /// </summary>
        public double PosX
        {
            get
            {
                return _PosX;
            }
            set
            {
                _PosX = value;
            }
        }
        private double _PosY;
        /// <summary>
        /// Y坐标
        /// </summary>
        public double PosY
        {
            get
            {
                return _PosY;
            }
            set
            {
                _PosY = value;
            }
        }

        private DateTime _PTime;
        /// <summary>
        /// 时间点
        /// </summary>
        public DateTime PTime
        {
            get
            {
                return _PTime;
            }
            set
            {
                _PTime = value;
            }
        }
    }
}

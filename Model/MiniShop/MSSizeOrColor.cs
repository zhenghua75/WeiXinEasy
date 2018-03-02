using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 产品尺寸、颜色分类
    /// </summary>
    public class MSSizeOrColor
    {
        private string iD;
        private int sizeColor;
        private string pID;
        private string scname;
        private string scimg;
        private int stock;
        private int isDel;
        private DateTime addTime;

        /// <summary>
        /// 编号
        /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        /// <summary>
        /// 尺寸或颜色 1表示尺寸 -1表示颜色  0表示 只有库存
        /// </summary>
        public int SizeColor
        {
            get { return sizeColor; }
            set { sizeColor = value; }
        }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string PID
        {
            get { return pID; }
            set { pID = value; }
        }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string Scname
        {
            get { return scname; }
            set { scname = value; }
        }
        /// <summary>
        /// 颜色图像
        /// </summary>
        public string Scimg
        {
            get { return scimg; }
            set { scimg = value; }
        }
        /// <summary>
        /// 库存
        /// </summary>
        public int Stock
        {
            get { return stock; }
            set { stock = value; }
        }
        /// <summary>
        /// 状态
        /// </summary>
        public int IsDel
        {
            get { return isDel; }
            set { isDel = value; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 产品图集
    /// </summary>
    public class MSProductAtlas
    {
        private string iD;
        private string pID;
        private string atlasName;
        private string pimgUrl;
        private int imgState;
        private int isDefault;
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
        /// 产品编号
        /// </summary>
        public string PID
        {
            get { return pID; }
            set { pID = value; }
        }
        /// <summary>
        /// 图像名称
        /// </summary>
        public string AtlasName
        {
            get { return atlasName; }
            set { atlasName = value; }
        }
        /// <summary>
        /// 图像链接地址
        /// </summary>
        public string PimgUrl
        {
            get { return pimgUrl; }
            set { pimgUrl = value; }
        }
        /// <summary>
        /// 状态 0为有效 1表示无效
        /// </summary>
        public int ImgState
        {
            get { return imgState; }
            set { imgState = value; }
        }
        /// <summary>
        /// 是否默认为产品首图
        /// </summary>
        public int IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
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

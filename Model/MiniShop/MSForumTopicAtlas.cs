using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 帖子图集
    /// </summary>
   public class MSForumTopicAtlas
    {
        private string iD;
        private string tID;
        private string imgName;
        private string imgUrl;
        private int imgState;
        private DateTime addTime;
       /// <summary>
       /// 自编号
       /// </summary>
        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
       /// <summary>
       /// 帖子编号
       /// </summary>
        public string TID
        {
            get { return tID; }
            set { tID = value; }
        }
       /// <summary>
       /// 图像名称
       /// </summary>
        public string ImgName
        {
            get { return imgName; }
            set { imgName = value; }
        }
       /// <summary>
       /// 图像链接
       /// </summary>
        public string ImgUrl
        {
            get { return imgUrl; }
            set { imgUrl = value; }
        }
       /// <summary>
       /// 图像状态 默认0：正常 1：删除
       /// </summary>
        public int ImgState
        {
            get { return imgState; }
            set { imgState = value; }
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

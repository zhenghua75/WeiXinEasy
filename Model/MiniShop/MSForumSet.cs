using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.MiniShop
{
    /// <summary>
    /// 论坛设置
    /// </summary>
   public class MSForumSet
    {
        private string iD;
        private string fTitle;
        private int visit;
        private string logoImg;
        private string backImg;
        private int fstate;
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
       /// 论坛名称
       /// </summary>
        public string FTitle
        {
            get { return fTitle; }
            set { fTitle = value; }
        }
       /// <summary>
       /// 浏览次数
       /// </summary>
        public int Visit
        {
            get { return visit; }
            set { visit = value; }
        }
       /// <summary>
       /// logo
       /// </summary>
        public string LogoImg
        {
            get { return logoImg; }
            set { logoImg = value; }
        }
       /// <summary>
       /// 背景
       /// </summary>
        public string BackImg
        {
            get { return backImg; }
            set { backImg = value; }
        }
       /// <summary>
       /// 状态 0为正常 1为删除
       /// </summary>
        public int Fstate
        {
            get { return fstate; }
            set { fstate = value; }
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

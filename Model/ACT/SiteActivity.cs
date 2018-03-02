/* ==============================================================================
 * 类名称：SiteActivity
 * 类描述：
 * 创建人：yhn,51809571@qq.com
 * 创建时间：2014/3/22 14:50:38
 * 修改人：
 * 修改时间：
 * 修改备注：
 * @version 1.0
 * ==============================================================================*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.ACT
{
    public class SiteActivity
    {
        private string iD;
        private string siteCode;
        private string actTitle;
        private string photo;
        private string actContent;
        private string actType;
        private int actStatus;
        private string startTime;
        private string endTime;
        private string cutofftime;
        private string discount;
        private DateTime addTime;
        private string openTime;
        private string closeTime;
        private int dayLimit;
        private string remark;

        public string ID
        {
            get { return iD; }
            set { iD = value; }
        }
        public string SiteCode
        {
            get { return siteCode; }
            set { siteCode = value; }
        }
        public string ActTitle
        {
            get { return actTitle; }
            set { actTitle = value; }
        }
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }
        public string ActContent
        {
            get { return actContent; }
            set { actContent = value; }
        }
        public string ActType
        {
            get { return actType; }
            set { actType = value; }
        }
        public int ActStatus
        {
            get { return actStatus; }
            set { actStatus = value; }
        }
        public string StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        public string EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        public string CutOffTime
        {
            get { return cutofftime; }
            set { cutofftime = value; }
        }
        public string DisCount
        {
            get { return discount; }
            set { discount = value; }
        }
        public DateTime AddTime
        {
            get { return addTime; }
            set { addTime = value; }
        }

        public string OpenTime
        {
            get { return openTime; }
            set { openTime = value; }
        }
        public string CloseTime
        {
            get { return closeTime; }
            set { closeTime = value; }
        }
        public int DayLimit
        {
            get { return dayLimit; }
            set { dayLimit = value; }
        }
        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }
    }
}

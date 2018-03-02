using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.HoliDay;
using DAL.HoliDay;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class HoliDayReg : System.Web.UI.Page
    {
        string strSiteCode = string.Empty;
        string strID = string.Empty;
        string strAction = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["sitecode"] == null || Request["sitecode"] == "")
                {
                    return;
                }
                if (Request["hid"] == null || Request["hid"] == "")
                {
                    return;
                }
                strSiteCode = Common.Common.NoHtml(Request["sitecode"]);
                strID = Common.Common.NoHtml(Request["hid"]);
                if (Request["action"] != null && Request["action"] != "" && Request["action"] == "save")
                {
                    saveUserinfo();
                    Response.End();
                }
                else
                {
                    GetHolidayDetail();
                }
            }
        }
        #region 获取活动详细
        /// <summary>
        /// 获取活动详细
        /// </summary>
        void GetHolidayDetail()
        {
            if (strID.Trim() != null && strID.Trim() != "")
            {
                HoliDayDAL holidaydal = new HoliDayDAL();
                HD_HoliDay holidaymodel = new HD_HoliDay();
                DataSet ds = holidaydal.GetHoliDayDateil(strID);
                string starttime = string.Empty; string endtime = string.Empty;
                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    holidaymodel = DataConvert.DataRowToModel<HD_HoliDay>(ds.Tables[0].Rows[0]);
                }
                if (holidaymodel.HstartTime < DateTime.Now)
                {
                    starttime = holidaymodel.HstartTime.ToString();
                }
                else
                {
                    starttime = "";
                }
                if (holidaymodel.HendTime < DateTime.Now)
                {
                     endtime = "";
                }
                else
                {
                    endtime = holidaymodel.HendTime.ToString();
                }
                //读取模板内容 
                string text = string.Empty;
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/HoliDay/HolidayRsg.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["title"] = "活动详细信息";
                context.TempData["holidayDetail"] = holidaymodel;
                context.TempData["starttime"] = starttime;
                context.TempData["endtime"] = endtime;
                context.TempData["sitecode"] = strSiteCode;
                context.TempData["hid"] = strID;
                context.TempData["footer"] = "奥琦微商易";
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
        #endregion
        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        void saveUserinfo()
        {
            string nickname = string.Empty;
            string phone = string.Empty;
            string age = string.Empty;
            string married = string.Empty;
            if (Request["nickname"] != null && Request["nickname"] != "")
            {
                nickname = Common.Common.NoHtml(Request["nickname"]);
            }
            if (Request["phone"] != null && Request["phone"] != "")
            {
                phone = Common.Common.NoHtml(Request["phone"]);
            }
            if (Request["age"] != null && Request["age"] != "")
            {
                age = Common.Common.NoHtml(Request["age"]);
            }
            if (Request["married"] != null && Request["married"] != "")
            {
                married = Common.Common.NoHtml(Request["married"]);
            }
            HoliDayUsersDAL userdal = new HoliDayUsersDAL();
            HD_HoliDayUsers usermodel = new HD_HoliDayUsers();
            if (userdal.ExistHoliDayUsers("", phone, strSiteCode))
            {
                Response.Write("{\"message\":\"不能重复提交！\"}"); return;
            }
            if (nickname.Trim() != null && nickname.Trim() != "")
            {
                usermodel.NickName = nickname;
            }
            if (phone.Trim() != null && phone.Trim() != "")
            {
                usermodel.Phone = phone;
            }
            if (age.Trim() != null && age.Trim() != "")
            {
                usermodel.Age = Convert.ToInt32(age);
            }
            if (married.Trim() != null && married.Trim() != "")
            {
                usermodel.Married = married;
            }
            usermodel.IsDel = 0;
            usermodel.HID = strID;
            usermodel.OpenID = "";
            usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
            if (userdal.AddHolidayUsers(usermodel))
            {
                Response.Write("{\"message\":\"操作成功！\"}"); return;
            }
        }
        #endregion
    }
}
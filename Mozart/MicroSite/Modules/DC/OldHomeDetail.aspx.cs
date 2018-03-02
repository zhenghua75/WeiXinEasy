using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.DC;
using DAL.DC;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class OldHomeDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strHouseID = string.Empty;
            string strSiteCode = string.Empty;
            string siteid = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            if (Request["siteid"]!=null&&Request["siteid"]!="")
            {
                siteid=Request["siteid"];
            }
            #region 房屋详细
            strHouseID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            DC_HouseDAL dal = new DC_HouseDAL();
            DataSet ds = dal.GetDCHouseDetail(strHouseID);
            DC_House model = new DC_House();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<DC_House>(ds.Tables[0].Rows[0]);
                strSiteCode = model.SiteCode;
            }
            #endregion

            #region 相关房屋列表
            List<DC_House> HouseList = new List<DC_House>();
            DataSet RelevantHouseDs = dal.GetRelevantHouseList(3, strHouseID);
            if (null != RelevantHouseDs && RelevantHouseDs.Tables.Count > 0 && RelevantHouseDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in RelevantHouseDs.Tables[0].Rows)
                {
                    DC_House RelevantModel = DataConvert.DataRowToModel<DC_House>(row);
                    HouseList.Add(RelevantModel);
                }
            }
            #endregion

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Realty/OldHomeDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "房产详细信息";
            context.TempData["HouseDetail"] = model;
            context.TempData["siteid"] = siteid;
            context.TempData["RelevantHouse"] = HouseList;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
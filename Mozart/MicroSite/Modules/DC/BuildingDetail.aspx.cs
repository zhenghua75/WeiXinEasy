using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.DC;
using Model.DC;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class BuildingDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string strHouseID = string.Empty;
                string strSiteCode = string.Empty;
                string siteid = string.Empty;
                if (null == Request.QueryString["id"])
                {
                    return;
                }
                if (Request["siteid"] != null && Request["siteid"] != "")
                {
                    siteid = Request["siteid"];
                }
                #region 楼盘详细
                strHouseID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
                DC_BuildingsDAL dal = new DC_BuildingsDAL();
                DataSet ds = dal.GetDCBuildingDetail(strHouseID);
                DC_Building model = new DC_Building();
                if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    model = DataConvert.DataRowToModel<DC_Building>(ds.Tables[0].Rows[0]);
                    strSiteCode = model.SiteCode;
                }
                #endregion

                #region 相关楼盘列表
                List<DC_Building> BuildingList = new List<DC_Building>();
                DataSet RelevantBuildingDs = dal.GetDCBuildingList(3, strHouseID);
                if (null != RelevantBuildingDs && RelevantBuildingDs.Tables.Count > 0 && RelevantBuildingDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in RelevantBuildingDs.Tables[0].Rows)
                    {
                        DC_Building RelevantModel = DataConvert.DataRowToModel<DC_Building>(row);
                        BuildingList.Add(RelevantModel);
                    }
                }
                #endregion

                //读取模板内容 
                string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Realty/BuildingDetail.html"));

                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

                context.TempData["sitecode"] = strSiteCode;
                context.TempData["title"] = "楼盘详细信息";
                context.TempData["BuildingDetail"] = model;
                context.TempData["siteid"] = siteid;
                context.TempData["BuildingList"] = BuildingList;
                context.TempData["footer"] = "奥琦微商易";

                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
    }
}
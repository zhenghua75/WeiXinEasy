using DAL.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinCore.Models;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class Vcoin : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        string struid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] == null || Session["customerID"].ToString() == "")
                {
                    JQDialog.SetCookies("pageurl", "Vcoin.aspx", 2);
                    errormsg = JQDialog.alertOKMsgBox(5, "您还没有登<br/>请登录后再操作！",
                        "UserLogin.aspx", "error");
                }
                else
                {
                    struid = Session["customerID"].ToString();
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            int vcoin = 0; int IsReceive = 0;
            List<MSVAcctDetail> vcoindetaillist = new List<MSVAcctDetail>();
            if (struid != null && struid != "")
            {
                
                MSVAcctDAL VAD = new MSVAcctDAL();
                MSVAcctDetailDAL MVA = new MSVAcctDetailDAL();
                #region-获取V币详细
                //try
                //{
                //    IsReceive = Convert.ToInt32(MVA.GetMSVAcctDetailByUID("IsReceive", struid).ToString());
                //}
                //catch (Exception)
                //{
                //}
                try
                {
                    vcoin = Convert.ToInt32(VAD.GetMSVAcct("V_Amont", struid).ToString());
                }
                catch (Exception)
                {
                }
                //if (IsReceive == 0)
                //{
                //    vcoin = 0;
                //}
                #endregion
                #region-获取用户V币详情列表
                string vcoinwhere=" Where CustID='"+struid+"' ";
                DataSet detailds = MVA.GetMSVAcctDetailList(vcoinwhere);
                if (detailds != null && detailds.Tables.Count > 0 && detailds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in detailds.Tables[0].Rows)
                    {
                        MSVAcctDetail vcoinmodel = DataConvert.DataRowToModel<MSVAcctDetail>(item);
                        vcoindetaillist.Add(vcoinmodel);
                    }
                }
                #endregion
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/Vcoin.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;
            context.TempData["vcoin"] = vcoin;
            context.TempData["vcoinlist"] = vcoindetaillist;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
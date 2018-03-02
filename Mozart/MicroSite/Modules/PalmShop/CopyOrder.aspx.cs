using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;


namespace Mozart.PalmShop.ShopCode
{
    public partial class CopyOrder : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            string oid = string.Empty; MSOrderLogistics OLDetail = new MSOrderLogistics();
            if (Request["oid"] != null && Request["oid"] != "")
            {
                oid = Common.Common.NoHtml(Request["oid"]);
            }
            if (oid != null && oid != "")
            {
                MSOrderLogisticsDAL OLdetail = new MSOrderLogisticsDAL();
                DataSet ds = OLdetail.GetMSODetailByOID(oid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    OLDetail = DataConvert.DataRowToModel<MSOrderLogistics>(ds.Tables[0].Rows[0]);
                }
            }
            else
            {
                errormsg = JQDialog.alertOKMsgBox(5, "无效的请求方式","", "error");
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/CopyOrder.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;
            context.TempData["oldetail"] = OLDetail;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
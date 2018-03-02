using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Product;
using Model.SP;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyOrderDetail : System.Web.UI.Page
    {
        string strOrderID = string.Empty;
        int iCs = 0;
        int iPs = 0;
        decimal dSp = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null != Request.QueryString["id"])
            {
                strOrderID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            }
            else
            {
                if (null != Session["strSiteCode"])
                {
                    Response.Redirect("Login.aspx?SiteCode='" + Session["strSiteCode"].ToString() + "'", false);
                }
                return;
            }

            List<SP_MyOrderDetail> liOrder = new List<SP_MyOrderDetail>();

            CartDAL dal = new CartDAL();

            DataSet ds = dal.GetOrderList(strOrderID);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                iCs = ds.Tables[0].Rows.Count;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    //SP_ShoppingCart model = DataConvert.DataRowToModel<SP_ShoppingCart>(row);
                    SP_MyOrderDetail model = DataConvert.DataRowToModel<SP_MyOrderDetail>(row);
                    iPs = iPs + model.Quantity;
                    dSp = dSp + model.Quantity * model.UnitCost;
                    liOrder.Add(model);
                }
            }
            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Default/MyOrderDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "订单详细信息";
            if (null == Session["strName"])
            {
                context.TempData["username"] = "未登录";
            }
            else
            {
                context.TempData["username"] = CustomerSession.strName;
            }
            context.TempData["order_list"] = liOrder;
            context.TempData["footer"] = "奥琦微商易";

            context.TempData["cs"] = iCs.ToString();
            context.TempData["ps"] = iPs.ToString();
            context.TempData["sp"] = dSp.ToString();

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
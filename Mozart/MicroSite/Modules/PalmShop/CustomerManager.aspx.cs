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
    public partial class CustomerManager : System.Web.UI.Page
    {
        string errorscript = string.Empty;
        string cuid = string.Empty; string shopid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = ""; string pageurl = string.Empty;
                if (Request["sid"] != null && Request["sid"] != "")
                {
                    shopid = Common.Common.NoHtml(Request["sid"]);
                    pageurl = "?sid="+shopid;
                }
                else
                {
                    if (Session["SID"] != null && Session["SID"].ToString() != "")
                    {
                        shopid = Session["SID"].ToString();
                        pageurl = "?sid=" + shopid;
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "CustomerManager.aspx" + pageurl, 1);
                        errorscript = JQDialog.alertOKMsgBox(3, "操作失败，请登录后再操作", "UserLogin.aspx", "error");
                    }
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    cuid = Session["customerID"].ToString();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "CustomerManager.aspx" + pageurl, 2);
                    errorscript = JQDialog.alertOKMsgBox(2, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                }
                
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/CustomerManager.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            string strWhere = string.Empty;
            List<OrderCustomer> ordercustomerModel = new List<OrderCustomer>();
            if (shopid != null && shopid != "")
            {
                shopid = " and d.[SID]='"+shopid+"' ";
                strWhere = shopid;
                
                MSProductOrderDAL customerDal = new MSProductOrderDAL();
                DataSet orderds = customerDal.GetOrderUser(strWhere);
                if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in orderds.Tables[0].Rows)
                    {
                        OrderCustomer customermodel = DataConvert.DataRowToModel<OrderCustomer>(row);
                        ordercustomerModel.Add(customermodel);
                    }
                }
            }

            context.TempData["customerlist"] = ordercustomerModel;
            context.TempData["cuid"] = cuid;
            context.TempData["errorscript"] = errorscript;
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        public class OrderCustomer {
            private string nickName;

            public string NickName
            {
                get { return nickName; }
                set { nickName = value; }
            }
            private string headImg;

            public string HeadImg
            {
                get { return headImg; }
                set { headImg = value; }
            }
            private string phone;

            public string Phone
            {
                get { return phone; }
                set { phone = value; }
            }
            private string cuid;

            public string Cuid
            {
                get { return cuid; }
                set { cuid = value; }
            }
            private DateTime addTime;

            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }
        }
    }
}
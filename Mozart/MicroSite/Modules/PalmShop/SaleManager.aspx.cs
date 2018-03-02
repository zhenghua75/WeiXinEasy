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
    public partial class SaleManager : System.Web.UI.Page
    {
        string errorscript = string.Empty;
        string cuid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    cuid = Session["customerID"].ToString();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "SaleManager.aspx", 2);
                    errorscript = JQDialog.alertOKMsgBox(2, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            List<SaleManagerList> OrderModel = new List<SaleManagerList>();
            MSProductOrderDAL orderDal = new MSProductOrderDAL();
            string strWhere = string.Empty;
            if (cuid != null && cuid != "")
            {
                cuid = " and a.CustomerID='" + cuid + "' ";
            }
            strWhere = " and a.IsReceive=1 "+cuid;
            DataSet orderds = orderDal.GetSaleManager(strWhere);
            if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in orderds.Tables[0].Rows)
                {
                    SaleManagerList salemodel = DataConvert.DataRowToModel<SaleManagerList>(row);
                    OrderModel.Add(salemodel);
                }
            }

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/SaleManager.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["orderlist"] = OrderModel;
            context.TempData["cuid"] = cuid;
            context.TempData["errorscript"] = errorscript;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        public class SaleManagerList 
        {
            private string iD;

            public string ID
            {
                get { return iD; }
                set { iD = value; }
            }
            private string cartID;

            public string CartID
            {
                get { return cartID; }
                set { cartID = value; }
            }
            private string customerID;

            public string CustomerID
            {
                get { return customerID; }
                set { customerID = value; }
            }
            private string buyName;

            public string BuyName
            {
                get { return buyName; }
                set { buyName = value; }
            }
            private string phone;

            public string Phone
            {
                get { return phone; }
                set { phone = value; }
            }
            private string leaveMsg;

            public string LeaveMsg
            {
                get { return leaveMsg; }
                set { leaveMsg = value; }
            }
            private string payWay;

            public string PayWay
            {
                get { return payWay; }
                set { payWay = value; }
            }
            private string carryWay;

            public string CarryWay
            {
                get { return carryWay; }
                set { carryWay = value; }
            }
            private string reveiveAddress;

            public string ReveiveAddress
            {
                get { return reveiveAddress; }
                set { reveiveAddress = value; }
            }
            private string zipCode;

            public string ZipCode
            {
                get { return zipCode; }
                set { zipCode = value; }
            }
            private int isSend;

            public int IsSend
            {
                get { return isSend; }
                set { isSend = value; }
            }
            private int isReceive;

            public int IsReceive
            {
                get { return isReceive; }
                set { isReceive = value; }
            }
            private int orderState;

            public int OrderState
            {
                get { return orderState; }
                set { orderState = value; }
            }
            private int payState;

            public int PayState
            {
                get { return payState; }
                set { payState = value; }
            }
            private DateTime payTime;

            public DateTime PayTime
            {
                get { return payTime; }
                set { payTime = value; }
            }
            private DateTime addTime;

            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }
            private string ptitle;

            public string Ptitle
            {
                get { return ptitle; }
                set { ptitle = value; }
            }
            private string shopName;

            public string ShopName
            {
                get { return shopName; }
                set { shopName = value; }
            }
            private string atlasName;

            public string AtlasName
            {
                get { return atlasName; }
                set { atlasName = value; }
            }
            private string oID;

            public string OID
            {
                get { return oID; }
                set { oID = value; }
            }
            private string pID;

            public string PID
            {
                get { return pID; }
                set { pID = value; }
            }
            private int quantity;

            public int Quantity
            {
                get { return quantity; }
                set { quantity = value; }
            }
            private decimal unitCost;

            public decimal UnitCost
            {
                get { return unitCost; }
                set { unitCost = value; }
            }
            private string pnum;

            public string Pnum
            {
                get { return pnum; }
                set { pnum = value; }
            }
            private string pimgUrl;

            public string PimgUrl
            {
                get { return pimgUrl; }
                set { pimgUrl = value; }
            }
        }
    }
}
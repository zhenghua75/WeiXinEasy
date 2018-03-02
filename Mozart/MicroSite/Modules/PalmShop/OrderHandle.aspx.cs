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
    public partial class OrderHandle : System.Web.UI.Page
    {
        static string customerid = string.Empty;
        public static string errorscript = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    errorscript = "";
                    customerid = Session["customerID"].ToString();
                    GetOrderHandleList();
                }
                else
                {
                    setCookies();
                    errorscript = JQDialog.alertOKMsgBox(2, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                    GetOrderHandleList();
                    return;
                }
            }
        }
        /// <summary>
        /// 设置Cookies
        /// </summary>
        void setCookies()
        {
            HttpCookie delcookies = new HttpCookie("pageurl");
            delcookies.Expires = DateTime.Now.AddDays(-1);
            Response.AppendCookie(delcookies);
            HttpCookie cookies = new HttpCookie("pageurl","OrderHandle.aspx");
            cookies.Expires = DateTime.Now.AddMinutes(3);
            Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 获取订单处理信息
        /// </summary>
        void GetOrderHandleList()
        {
            string oid = string.Empty;//支付成功返回订单
            if (Request["oid"] != null && Request["oid"] != "")
            {
                oid = Common.Common.NoHtml(Request["oid"]);
            }
            List<OrderModel> orderlistModel = new List<OrderModel>();
            if (customerid.Trim() != null && customerid.Trim() != "")
            {
                #region 获取产品列表
                MSShoppingCartDAL orderlistDal = new MSShoppingCartDAL();
                string orderstate = string.Empty; string where = string.Empty;
                string paystate = string.Empty; string receive = string.Empty;
                if (Request["orderstate"] != null && Request["orderstate"] != "")
                {
                    orderstate = Common.Common.NoHtml(Request["orderstate"]);
                }
                if (Request["paystate"] != null && Request["paystate"] != "")
                {
                    paystate = Common.Common.NoHtml(Request["paystate"]);
                }
                if (Request["receive"] != null && Request["receive"] != "")
                {
                    receive = Common.Common.NoHtml(Request["receive"]);
                }
                where = " and a.CustomerID='" + customerid + "' ";
                if (orderstate.Trim() != null && orderstate.Trim() != "")
                {
                    where += " and OrderState=" + orderstate;
                }
                else
                {
                    where += " and OrderState=0 ";
                }
                if (paystate.Trim() != null && paystate.Trim() != "")
                {
                    where += " and PayState=" + paystate;
                }
                else
                {
                    where += " and PayState=0 ";
                }
                if (receive.Trim() != null && receive.Trim() != "")
                {
                    where += " and IsReceive=" + receive;
                }
                else
                {
                    where += " and IsReceive=0 ";
                }
                if (oid.Trim() != null && oid.Trim() != "")
                {
                    where += " and a.ID='"+oid+"' ";
                }
                DataSet orderds = orderlistDal.GetMyOrderListByWhere(where);
                if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in orderds.Tables[0].Rows)
                    {
                        OrderModel orderModel = DataConvert.DataRowToModel<OrderModel>(row);
                        orderlistModel.Add(orderModel);
                    }
                }
                #endregion
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/OrderHandle.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "订单处理";
            context.TempData["errorscript"] = errorscript;
            context.TempData["productlist"] = orderlistModel;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 产品订单Model
        /// </summary>
      public class OrderModel
        {
            private string iD;

            public string ID
            {
                get { return iD; }
                set { iD = value; }
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
            private string isSend;

            public string IsSend
            {
                get { return isSend; }
                set { isSend = value; }
            }
            private string isReceive;

            public string IsReceive
            {
                get { return isReceive; }
                set { isReceive = value; }
            }
            private string orderState;

            public string OrderState
            {
                get { return orderState; }
                set { orderState = value; }
            }
            private string payState;

            public string PayState
            {
                get { return payState; }
                set { payState = value; }
            }
            private string payTime;

            public string PayTime
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
            private string pID;

            public string PID
            {
                get { return pID; }
                set { pID = value; }
            }
            private string price;

            public string Price
            {
                get { return price; }
                set { price = value; }
            }
            private string quantity;

            public string Quantity
            {
                get { return quantity; }
                set { quantity = value; }
            }
            private string unitCost;

            public string UnitCost
            {
                get { return unitCost; }
                set { unitCost = value; }
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
            private string pimgUrl;

            public string PimgUrl
            {
                get { return pimgUrl; }
                set { pimgUrl = value; }
            }
            private string atlasName;

            public string AtlasName
            {
                get { return atlasName; }
                set { atlasName = value; }
            }
        }
    }
}
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
    public partial class OrderDetail : System.Web.UI.Page
    {
        string errormsg = string.Empty; string action = string.Empty;
        string oid = string.Empty; string struid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["oid"] != null && Request["oid"] != "")
                {
                    oid = Common.Common.NoHtml(Request["oid"]);
                }
                else
                {
                    errormsg = JQDialog.alertOKMsgBox(5, "无效的请求方式",
                        "CustomerOrder.aspx", "error");
                }
                if (oid != null && oid != "")
                {
                    try
                    {
                        if (Session["customerID"] != null || Session["customerID"].ToString() != "")
                    {
                        struid = Session["customerID"].ToString();
                    }
                    }
                    catch (Exception)
                    {
                    }
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]);
                }
                if (action != null && action != "")
                {
                    action = action.Trim().ToLower();
                    switch (action)
                    {
                        case "edite":
                            EditeOrder();
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            MSProductOrderDAL orderDal = new MSProductOrderDAL();
            MyOrderList myorderlistModel = new MyOrderList();
            string strWhere = string.Empty; string payway = string.Empty;
            if (struid != null && struid != "")
            {
                struid = " and a.CustomerID='" + struid + "' ";
            }
            strWhere =struid+ " and a.ID='"+oid+"'  ";
            DataSet orderds = orderDal.GetCustomerOrderList(strWhere);
            if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
            {
                myorderlistModel = DataConvert.DataRowToModel<MyOrderList>(orderds.Tables[0].Rows[0]);
                payway = myorderlistModel.PayWay;
                string[] way = payway.Split('|');
                try
                {
                    payway = way[1].ToString();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                errormsg = JQDialog.alertOKMsgBox(5, "无效的请求方式",
                                       "CustomerOrder.aspx", "error");
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/OrderDetail.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;
            context.TempData["uid"] = struid;
            context.TempData["payway"] = payway;
            context.TempData["orderdetail"] = myorderlistModel;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 收货信息修改
        /// </summary>
        void EditeOrder()
        {
            string buyname = string.Empty; string phone = string.Empty; string pac = string.Empty;
            string address = string.Empty; string leveamsg = string.Empty; string zipcode = string.Empty;
            #region-获取请求值
            if (Request["buyname"] != null && Request["buyname"] != "")
            {
                buyname = Request["buyname"];
            }
            if (Request["phone"] != null && Request["phone"] != "")
            {
                phone = Request["phone"];
            }
            if (Request["pac"] != null && Request["pac"] != "")
            {
                pac = Request["pac"];
            }
            if (Request["address"] != null && Request["address"] != "")
            {
                address = Request["address"];
            }
            if (Request["leveamsg"] != null && Request["leveamsg"] != "")
            {
                leveamsg = Request["leveamsg"];
            }
            if (Request["zipcode"] != null && Request["zipcode"] != "")
            {
                zipcode = Request["zipcode"];
            }
            #endregion
            MSProductOrderDAL OrderDal = new MSProductOrderDAL();
            if (OrderDal.UpdateCustomer(oid, phone, buyname, pac + "-" + address, leveamsg, zipcode))
            {
                Response.Write("{\"success\":true}");
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"收货信息修改失败，请核对后再操作\"}");
            }
            Response.End();
        }
        public class MyOrderList
        {
            private string iD;
            private string cartID;
            private string customerID;
            private string buyName;
            private string phone;
            private string leaveMsg;
            private string payWay;
            private string carryWay;
            private string reveiveAddress;
            private string zipCode;
            private int isSend;
            private int isReceive;
            private int orderState;
            private int payState;
            private DateTime payTime;
            private DateTime addTime;

            private string shopName;
            private string pID;
            private string ptitle;
            private decimal price;
            private string parName;
            private string pimg;
            private string pcontent;
            private string pnum;
            private int quantity;
            private decimal unitCost;
            private string mid;


            #region 订单编号
            /// <summary>
            /// 订单编号
            /// </summary>
            public string ID
            {
                get { return iD; }
                set { iD = value; }
            }
            #endregion
            #region 购物车编号
            /// <summary>
            /// 购物车编号
            /// </summary>
            public string CartID
            {
                get { return cartID; }
                set { cartID = value; }
            }
            #endregion
            #region 客户编号
            /// <summary>
            /// 客户编号
            /// </summary>
            public string CustomerID
            {
                get { return customerID; }
                set { customerID = value; }
            }
            #endregion
            #region 收件人姓名
            /// <summary>
            /// 收件人姓名
            /// </summary>
            public string BuyName
            {
                get { return buyName; }
                set { buyName = value; }
            }
            #endregion
            #region 收件人电话
            /// <summary>
            /// 收件人电话
            /// </summary>
            public string Phone
            {
                get { return phone; }
                set { phone = value; }
            }
            #endregion
            #region 留言信息
            /// <summary>
            /// 留言信息
            /// </summary>
            public string LeaveMsg
            {
                get { return leaveMsg; }
                set { leaveMsg = value; }
            }
            #endregion
            #region 支付方式
            /// <summary>
            /// 支付方式
            /// </summary>
            public string PayWay
            {
                get { return payWay; }
                set { payWay = value; }
            }
            #endregion 支付方式
            #region 运输方式
            /// <summary>
            /// 运输方式
            /// </summary>
            public string CarryWay
            {
                get { return carryWay; }
                set { carryWay = value; }
            }
            #endregion
            #region 收件人地址
            /// <summary>
            /// 收件人地址
            /// </summary>
            public string ReveiveAddress
            {
                get { return reveiveAddress; }
                set { reveiveAddress = value; }
            }
            #endregion
            #region 收件人邮编
            /// <summary>
            /// 收件人邮编
            /// </summary>
            public string ZipCode
            {
                get { return zipCode; }
                set { zipCode = value; }
            }
            #endregion
            #region 是否已发货 0表示未发货，1表示发货
            /// <summary>
            /// 是否已发货 0表示未发货，1表示发货
            /// </summary>
            public int IsSend
            {
                get { return isSend; }
                set { isSend = value; }
            }
            #endregion
            #region 是否签收 0表示未签收，1表示签收
            /// <summary>
            /// 是否签收 0表示未签收，1表示签收
            /// </summary>
            public int IsReceive
            {
                get { return isReceive; }
                set { isReceive = value; }
            }
            #endregion
            #region 订单状态 0表示正常 1表示已删
            /// <summary>
            /// 订单状态 0表示正常 1表示已删
            /// </summary>
            public int OrderState
            {
                get { return orderState; }
                set { orderState = value; }
            }
            #endregion
            #region 支付状态 0表示未支付，1表示已支付
            /// <summary>
            /// 支付状态 0表示未支付，1表示已支付
            /// </summary>
            public int PayState
            {
                get { return payState; }
                set { payState = value; }
            }
            #endregion
            #region 支付时间
            /// <summary>
            /// 支付时间
            /// </summary>
            public DateTime PayTime
            {
                get { return payTime; }
                set { payTime = value; }
            }
            #endregion
            #region 添加时间
            /// <summary>
            /// 添加时间
            /// </summary>
            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }
            #endregion


            public string PID
            {
                get { return pID; }
                set { pID = value; }
            }
            public string ShopName
            {
                get { return shopName; }
                set { shopName = value; }
            }
            public string Ptitle
            {
                get { return ptitle; }
                set { ptitle = value; }
            }
            public decimal Price
            {
                get { return price; }
                set { price = value; }
            }
            public string ParName
            {
                get { return parName; }
                set { parName = value; }
            }
            public string Pimg
            {
                get { return pimg; }
                set { pimg = value; }
            }
            public string Pcontent
            {
                get { return pcontent; }
                set { pcontent = value; }
            }
            public string Pnum
            {
                get { return pnum; }
                set { pnum = value; }
            }
            public int Quantity
            {
                get { return quantity; }
                set { quantity = value; }
            }
            public decimal UnitCost
            {
                get { return unitCost; }
                set { unitCost = value; }
            }
            public string Mid
            {
                get { return mid; }
                set { mid = value; }
            }
        }
    }
}
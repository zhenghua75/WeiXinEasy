using DAL.WeiXin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinCore.Models;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;
using System.Data;

namespace Mozart.PalmShop.ShopCode
{
    public partial class CustomerOrder : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        string action = string.Empty;
        string strOpenID = string.Empty;
        string strSiteCode = string.Empty;
        string customerid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customerid = Session["customerID"].ToString();
                    GetOpenId();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "CustomerOrder.aspx", 2);
                    errormsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                            "UserLogin.aspx", "error");
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    action = action.ToLower().Trim();
                    switch (action)
                    {
                        case "receive":
                            ReceiveOrder();
                            break;
                        case "pubol":
                            //submitOrder();
                            break;
                        case"delorder":
                            DelOrder();
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        void GetOpenId()
        {
            if (customerid != null && customerid != "")
            {
                MSCustomersDAL CustomerDal = new MSCustomersDAL();
                strOpenID = CustomerDal.GetCustomerValueByID("OpenID", customerid).ToString();
                Session["OpenID"] = strOpenID;
            }
            else
            {
                strOpenID = "";
            }
        }
        void GetHtmlPage()
        {
            string strWhere = string.Empty; string productmodelid = string.Empty;
            string orderstate = string.Empty; string paystate = string.Empty; string receive = string.Empty;
            string oid = string.Empty;//支付成功返回订单
            #region-获取用户信息
            if (strOpenID != null && strOpenID != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                try
                {
                    customerid = customerDal.GetCustomerValueByOpenID("ID", strOpenID).ToString();
                }
                catch (Exception)
                {
                }
            }
            else
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customerid = Session["customerID"].ToString();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "CustomerOrder.aspx", 2);
                    errormsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "UserLogin.aspx", "error");
                }
            }
            #endregion
            List<MyOrderList> myorderlistModel = new List<MyOrderList>();
            if (customerid != null && customerid != "")
            {
                MSProductOrderDAL orderDal = new MSProductOrderDAL();
                #region -获取请求信息
                if (Request["oid"] != null && Request["oid"] != "")
                {
                    oid = Common.Common.NoHtml(Request["oid"]);
                }
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
                strWhere = " and a.CustomerID='" + customerid + "' ";
                if (orderstate.Trim() != null && orderstate.Trim() != "")
                {
                    strWhere += " and OrderState=" + orderstate;
                }
                if (paystate.Trim() != null && paystate.Trim() != "")
                {
                    strWhere += " and PayState=" + paystate;
                }
                else
                {
                    strWhere += " and PayState=1 ";
                }
                if (receive.Trim() != null && receive.Trim() != "")
                {
                    strWhere += " and IsReceive=" + receive;
                }
                else
                {
                    strWhere += " and IsReceive=0 ";
                }
                if (oid.Trim() != null && oid.Trim() != "")
                {
                    strWhere += " and a.ID='" + oid + "' ";
                }
                #endregion
                strWhere  += " and a.CustomerID='"+customerid+"' ";
                DataSet orderds = orderDal.GetCustomerOrderList(strWhere);
                if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in orderds.Tables[0].Rows)
                    {
                        MyOrderList ordermodel=DataConvert.DataRowToModel<MyOrderList>(item);
                        productmodelid = ordermodel.Mid;
                        if (ordermodel.Ptitle.Length > 10)
                        {
                            ordermodel.Ptitle = ordermodel.Ptitle.ToString().Substring(0, 10) + "..";
                        }
                        myorderlistModel.Add(ordermodel);
                    }
                }
            }
            else
            {
                JQDialog.SetCookies("pageurl", "CustomerOrder.aspx", 2);
                errormsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "UserLogin.aspx", "error");
            }

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/CustomerOrder.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            //context.TempData["artList"] = liArtList;
            context.TempData["myorderlist"] = myorderlistModel;
            context.TempData["errormsg"] = errormsg;
            context.TempData["customerid"] = customerid;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 订单收货
        /// </summary>
        void ReceiveOrder()
        {
            string oid = string.Empty;
            if (Request["oid"] != null && Request["oid"] != "")
            {
                oid = Common.Common.NoHtml(Request["oid"]);
            }
            if (oid != null && oid != "")
            {
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                if (OrderDal.UpdateOrderState("isReceive", oid))
                {
                    string buyName = string.Empty;
                    try
                    {
                        buyName = OrderDal.GetOrderValueByID("BuyName", oid).ToString();
                        if (strOpenID == null || strOpenID == "")
                        {
                            string customerid = OrderDal.GetOrderDetailValueByOID("CustomerID", oid).ToString();
                            MSCustomersDAL CustomerDal = new MSCustomersDAL();
                            try
                            {
                                strOpenID = CustomerDal.GetCustomerValueByID("OpenID", customerid).ToString();
                            }
                            catch (Exception)
                            {
                            }
                        }
                        WeiXinCore.WeiXin wx = WXHelper.CreateWeiXinInstanceBySiteCode("VYIGO");
                        List<TemplateMessageParam> paramList = new List<TemplateMessageParam>();
                        paramList.Add(new TemplateMessageParam("first", "尊敬的" + buyName));
                        paramList.Add(new TemplateMessageParam("OrderSn", oid));
                        paramList.Add(new TemplateMessageParam("OrderStatus", "已收货"));
                        paramList.Add(new TemplateMessageParam("Remark", 
                            "请关注公众号【vgo2013】进入“服务中心进行查询”查看完整信息"));
                        wx.SendTemplateMessage(strOpenID, "wmrxCKRq1hG3cHR0BXsuUnNq1chcbVosqYLqlsBBRCc",
                            "http://www.vgo2013.com/PalmShop/ShopCode/CustomerOrder.aspx?receive=1",
                            paramList.ToArray(), "");
                        MSOrderLogDAL.AddMSOrderLog("提示客户订单【" + oid + 
                            "】已确认收货，发送模板消息到客户OpenID【" + strOpenID + "】");
                    }
                    catch (Exception)
                    {
                    }
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请稍后再操作\"}");
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请稍后再操作\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        void DelOrder()
        {
            string oid = string.Empty; string uid = string.Empty;
            if (Request["oid"] != null && Request["oid"] != "")
            {
                oid = Common.Common.NoHtml(Request["oid"]);
            }
            if (Request["uid"] != null && Request["uid"] != "")
            {
                uid = Common.Common.NoHtml(Request["uid"]);
            }
            if (oid != null && oid != "" && uid != null && uid != "")
            {
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                if (OrderDal.UpdateOrderStateByUID(uid, oid))
                {
                    int orderquantity = 0; string mid = string.Empty; int quantity = 0;
                    #region-更新库存
                    MSProductParaDAL paraDal = new MSProductParaDAL();
                    try
                    {
                        orderquantity = Convert.ToInt32(OrderDal.GetOrderDetailValueByOID("Quantity", oid).ToString());
                    }
                    catch (Exception)
                    {
                    }
                    if (orderquantity != null && orderquantity>0)
                    {
                        try
                        {
                            mid = OrderDal.GetOrderDetailValueByOID("MID", oid).ToString();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (mid != null && mid != "")
                    {
                        try
                        {
                            quantity = Convert.ToInt32(paraDal.GetMSPParaValueByID("Stock", mid).ToString());
                        }
                        catch (Exception)
                        {
                        }
                    }
                    #endregion
                    if (quantity >= 0 && orderquantity > 0)
                    {
                        quantity = quantity + orderquantity;
                        try
                        {
                            paraDal.UpdateStock(quantity, mid);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    MSOrderLogDAL.AddMSOrderLog("订单【" + oid +
                            "】已被客户【" + uid + "】OpenID-【" + strOpenID + "】取消，库存更新");
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请稍后再操作\"}");
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
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
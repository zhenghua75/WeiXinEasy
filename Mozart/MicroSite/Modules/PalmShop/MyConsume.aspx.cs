using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using WeiXinCore.Models;
using DAL.WeiXin;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class MyConsume : System.Web.UI.Page
    {
        string errormsg = string.Empty; string strOpenID = string.Empty;
        string strSiteCode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetHtmlPage();
            }
        }
        /// <summary>
        /// 获取用户OpenID
        /// </summary>
        void GetUserOpenID()
        {
            if (null == Request.QueryString["state"])
            {
                //return;
            }
            else
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
                Session["strSiteCode"] = strSiteCode;
            }
            string code = Request.QueryString["code"] as string;
            if (!string.IsNullOrEmpty(code))
            {
                WXConfigDAL dal = new WXConfigDAL();
                Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
                if (wxConfig != null)
                {
                    WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                    {
                        ID = wxConfig.WXID,
                        Name = wxConfig.WXName,
                        Token = wxConfig.WXToken,
                        AppId = wxConfig.WXAppID,
                        AppSecret = wxConfig.WXAppSecret
                    };
                    WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                    Oauth2AccessToken oauth2AccessToken = weixin.GetOauth2AccessToken(code);
                    if (oauth2AccessToken != null)
                    {
                        strOpenID = oauth2AccessToken.OpenID;
                    }
                }
                else
                {
                    strOpenID = code;
                }
            }
            if (strOpenID != null && strOpenID != "")
            {
                Session["OpenID"] = strOpenID;
            }
        }
        void GetHtmlPage()
        {
            string customerid = string.Empty; string strWhere = string.Empty;

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
            List<MSProductPara> paralist = new List<MSProductPara>();
            if (customerid != null && customerid != "")
            {
                MSProductOrderDAL orderDal = new MSProductOrderDAL();
                strWhere += " and a.CustomerID='" + customerid + "' and PayState=1 ";
                DataSet orderds = orderDal.GetCustomerOrderList(strWhere);
                if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow item in orderds.Tables[0].Rows)
                    {
                        MyOrderList ordermodel = DataConvert.DataRowToModel<MyOrderList>(item);
                        myorderlistModel.Add(ordermodel);
                    }
                }
            }
            else
            {
                JQDialog.SetCookies("pageurl", "MyConsume.aspx", 2);
                errormsg = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "UserLogin.aspx", "error");
            }

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/MyConsume.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            //context.TempData["artList"] = liArtList;
            context.TempData["errormsg"] = errormsg;
            context.TempData["myorderlist"] = myorderlistModel;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
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
            private string ptitle;
            private decimal price;
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
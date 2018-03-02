using Mozart.Payment;
using Mozart.Payment.Alipay.WapPay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.MiniShop;
using DAL.MiniShop;
using Model.SYS;
using DAL.SYS;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class PayWay : System.Web.UI.Page
    {
        string pname =string.Empty;
        string ordernum = string.Empty;
        string countcost =string.Empty;
        string customid = string.Empty;
        string action = string.Empty;
        string strIP = string.Empty;
        string strOpenID = string.Empty;
        string ptitle = string.Empty;
        string payway = string.Empty;
        string errormsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                #region------------获取请求信息----------------
                if (Request["pname"] != null && Request["pname"] != "")
                {
                    pname = Request["pname"];
                }
                if (Request["ordernum"] != null && Request["ordernum"] != "")
                {
                    ordernum = Request["ordernum"];
                }
                if (Request["countcost"] != null && Request["countcost"] != "")
                {
                    countcost = Request["countcost"];
                }
                if (Request["customid"] != null && Request["customid"] != "")
                {
                    customid = Request["customid"];
                }
                #endregion
                #region -获取用户openid
                if (Session["OpenID"] == null || Session["OpenID"].ToString() == "")
                {
                    if (customid != null && customid != "")
                    {
                        MSCustomersDAL customerDal = new MSCustomersDAL();
                        try
                        {
                            strOpenID = customerDal.GetCustomerValueByID("OpenID", customid).ToString();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else
                {
                    strOpenID = Session["OpenID"].ToString();
                }
                #endregion
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    string strSiteCode = "VYIGO";
                    WXJSAPIPay wxpay = new WXJSAPIPay(strSiteCode);
                    MSProductOrderDAL ptitleDal = new MSProductOrderDAL();
                    switch (action.Trim().ToLower())
                    {
                        case "alipay":
                            //ptitleDal.UpdateOrderPayWay(ordernum, "alipay");
                           //payway= WapPayHelper.BuildRequest(pname, ordernum, countcost, customid);
                            AliWapPay pay = new AliWapPay("VYIGO");
                            string notify_url = "http://www.vgo2013.com/PalmShop/ShopCode/NotifyUrl.aspx";
                            string call_back_url = "http://www.vgo2013.com/PalmShop/ShopCode/Alipayreturn.aspx";
                            pay.DirectAliWayPay(this.Response,pname,ordernum,countcost,customid,call_back_url,notify_url);
                            break;
                        case "wxpay":
                            ptitle= ptitleDal.GetOrderTitleByOID(ordernum);
                            strIP =System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                            decimal countcostx = decimal.Parse(countcost);
                            int v = (Int32)Math.Round(countcostx * 100,0);

                            try
                            {
                                wxpay.DirectWXJSAPIPay(this.Response, ptitle, ordernum, v,
                                strIP, strOpenID, "http://www.vgo2013.com/PalmShop/ShopCode/PayState.aspx", null, "");
                            }
                            catch (Exception emsg)
                            {
                                errormsg = JQDialog.alertOKMsgBox(5, emsg.Message,"", "error");
                            }
                            
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/PayWay.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            if (Session["customerID"] != null && Session["customerID"].ToString() != "")
            {
                context.TempData["uid"] = Session["customerID"].ToString();
            }
            context.TempData["title"] = "支付方式";
            context.TempData["pname"] = pname;
            context.TempData["openid"] = strOpenID;
            context.TempData["ordernum"] = ordernum;
            context.TempData["countcost"] = countcost;
            context.TempData["customid"] = customid;
            context.TempData["payway"] = payway;
            context.TempData["errormsg"] = errormsg;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 获取服务器异步通知页面完整URL
        /// </summary>
        /// <returns></returns>
        public string GetNotifyUrl(string url)
        {
            return string.Format("{0}{1}",
                HttpContext.Current.Request.Url.AbsoluteUri.Replace(HttpContext.Current.Request.Url.PathAndQuery, ""),
                url);
        }
    }
}
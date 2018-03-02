using DAL.MiniShop;
using DAL.SYS;
using DAL.WeiXin;
using Model.SYS;
using Mozart.Common;
using Mozart.Payment;
using Mozart.Payment.Alipay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class NotifyUrl : AliWapPayNotifyBasePage
    {
        public override string GetSiteCode()
        {
            return "VYIGO";
        }
        public override void OnPaySucceed(AliWapPayNotifyInfo info)
        {
            ExceptionLog log = new ExceptionLog();
            log.Message = string.Format("订单号：{0},支付宝交易号：{1}",
                info.out_trade_no, info.trade_no);
            ExceptionLogDAL.InsertExceptionLog(log);

            MSProductOrderDAL ptitleDal = new MSProductOrderDAL();
            int paystate = 0;
            try
            {
                paystate = Convert.ToInt32(ptitleDal.GetOrderValueByID("PayState", info.out_trade_no).ToString());
            }
            catch (Exception)
            {
            }
            if (paystate == 0)
            {
                bool updatepayway = ptitleDal.UpdateOrderPayWay(info.out_trade_no, "alipay");
                bool updateOrderNum = MSProductOrderDAL.UpdateOrderPayState(info.out_trade_no, "1");
                if (updatepayway == true && updateOrderNum == true)
                {
                    string countcost = string.Empty; string pid = string.Empty; string pname = string.Empty;
                    #region -获取用户数据
                    string strOpenID = string.Empty; string customerid = string.Empty;
                    try
                    {
                        customerid = ptitleDal.GetOrderDetailValueByOID("CustomerID", info.out_trade_no).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    if (customerid != null && customerid != "")
                    {
                        MSCustomersDAL CustomerDal = new MSCustomersDAL();
                        try
                        {
                            strOpenID = CustomerDal.GetCustomerValueByID("OpenID", customerid).ToString();
                        }
                        catch (Exception)
                        {
                        }
                    }
                    #endregion
                    #region-获取产品信息
                    try
                    {
                        countcost = ptitleDal.GetOrderDetailValueByOID("UnitCost", info.out_trade_no).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        pid = ptitleDal.GetOrderDetailValueByOID("PID", info.out_trade_no).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    if (pid != null && pid != "")
                    {
                        MSProductDAL ProductDal = new MSProductDAL();
                        pname = ProductDal.GetMSProductVaueByID("Ptitle", pid).ToString();
                    }
                    #endregion
                    string strSiteCode = GetSiteCode();
                    WXConfigDAL dal = new WXConfigDAL();

                    WeiXinCore.WeiXin wx = WXHelper.CreateWeiXinInstanceBySiteCode(strSiteCode);
                    List<TemplateMessageParam> paramList = new List<TemplateMessageParam>();
                    paramList.Add(new TemplateMessageParam("first", "我们已收到您的货款，订单号为：" + info.out_trade_no +
                        "；我们将尽快为您打包商品，请耐心等待: )"));
                    paramList.Add(new TemplateMessageParam("orderMoneySum", countcost));
                    paramList.Add(new TemplateMessageParam("orderProductName", pname));
                    paramList.Add(new TemplateMessageParam("Remark", "如有问题请致电400-885-5790或直接在微信留言，小V将第一时间为您服务！"));
                    wx.SendTemplateMessage(strOpenID, "IR3TlAC2Y3lW0jaksuPRwHrVHe5nmbWRcD6ZeUPZPlA",
                        "http://www.vgo2013.com/PalmShop/ShopCode/OrderDetail.aspx?oid=" + info.out_trade_no,
                        paramList.ToArray(), "");
                    //    JQDialog.SendWeiXinMsg(GetSiteCode(), strOpenID,
                    //        "您于" + DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") +
                    //        "购买的【" + pname + "】已付款成功，支付金额：" + countcost + "元；订单号【" + info.out_trade_no +
                    //        "】，在等待卖家发货，详情请进入我的订单查询!");
                }
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}
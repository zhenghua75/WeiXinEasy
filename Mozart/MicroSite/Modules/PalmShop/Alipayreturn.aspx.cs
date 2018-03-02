using DAL.SYS;
using Model.SYS;
using Mozart.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using DAL.WeiXin;
using WeiXinCore.Models;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class Alipayreturn : AliWapPayCallBackBasePage
    {
        public static string oid = string.Empty;
        public static string payid = string.Empty;
        public override void OnPaySucceed(AliWapPayCallBackInfo info)
        {
            ExceptionLog log = new ExceptionLog();
            log.Message = string.Format("订单号：{0},支付宝交易号：{1}",
                info.out_trade_no, info.trade_no);
            ExceptionLogDAL.InsertExceptionLog(log);

            if (info.result.ToLower() == "success")
            {
                oid = info.out_trade_no;
                payid = info.trade_no;

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
                    MSOrderLogDAL.AddMSOrderLog("订单【" + info.out_trade_no + "】支付成功，支付方式：支付宝支付");
                    if (updatepayway == true && updateOrderNum == true)
                    {
                        string countcost = string.Empty; string pid = string.Empty; string pname = string.Empty;
                        #region -获取用户数据
                        string strOpenID = string.Empty; string customerid = string.Empty;
                        try
                        {
                            customerid = ptitleDal.GetOrderValueByID("CustomerID", info.out_trade_no).ToString();
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
                        paramList.Add(new TemplateMessageParam("first",
                            "我们已收到您的货款，订单号为：" + info.out_trade_no +
                            "；我们将尽快为您打包商品，请耐心等待: )"));
                        paramList.Add(new TemplateMessageParam("orderMoneySum", countcost+" 元"));
                        paramList.Add(new TemplateMessageParam("orderProductName", pname));
                        paramList.Add(new TemplateMessageParam("Remark", "如有问题请致电400-885-5790或直接在微信留言，小V将第一时间为您服务！"));
                        wx.SendTemplateMessage(strOpenID, "IR3TlAC2Y3lW0jaksuPRwHrVHe5nmbWRcD6ZeUPZPlA",
                            "http://www.vgo2013.com/PalmShop/ShopCode/OrderDetail.aspx?oid=" + info.out_trade_no,
                            paramList.ToArray(), "");
                        MSOrderLogDAL.AddMSOrderLog("发送模板消息到客户OpenID为【" + strOpenID + "】大致为：我们已收到您的货款" + countcost + "元,产品名称【" + pname + "】我们将尽快为您打包商品，请耐心等待: )");
                    }
                }
            }
        }

        public override string GetSiteCode()
        {
            return "VYIGO";
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
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
using Mozart.Common;
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class PayState : WXJSAPIPayNotifyBasePage
    {
        public override void OnPaySucceed(WXJSAPIPayNotifyInfo info)
        {
            ExceptionLog log = new ExceptionLog();
            log.Message = string.Format("Openid:{0},订单号：{1},附加消息：{2}",
                info.OpenId, info.OutTradeNo, info.Attach);
            ExceptionLogDAL.InsertExceptionLog(log);

            MSProductOrderDAL ptitleDal = new MSProductOrderDAL();
            int paystate = 0;
            try
            {
                paystate = Convert.ToInt32(ptitleDal.GetOrderValueByID("PayState", info.OutTradeNo).ToString());
            }
            catch (Exception)
            {
            }
            if (paystate == 0)
            {
                bool updatepayway = ptitleDal.UpdateOrderPayWay(info.OutTradeNo, "wxpay");
                bool updateOrderNum=MSProductOrderDAL.UpdateOrderPayState(info.OutTradeNo, "1");
                MSOrderLogDAL.AddMSOrderLog("订单【" + info.OutTradeNo + "】支付成功，支付方式：微支付");
                if (updatepayway==true&&updateOrderNum==true)
                {
                    string countcost = ptitleDal.GetOrderDetailValueByOID("UnitCost", info.OutTradeNo).ToString();
                    string strSiteCode = "VYIGO";
                    string pid = string.Empty; string pname = string.Empty;

                    #region-获取产品信息
                    try
                    {
                        countcost = ptitleDal.GetOrderDetailValueByOID("UnitCost", info.OutTradeNo).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    try
                    {
                        pid = ptitleDal.GetOrderDetailValueByOID("PID", info.OutTradeNo).ToString();
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

                    WeiXinCore.WeiXin wx = WXHelper.CreateWeiXinInstanceBySiteCode(strSiteCode);
                    List<TemplateMessageParam> paramList = new List<TemplateMessageParam>();
                    paramList.Add(new TemplateMessageParam("first", "我们已收到您的货款，订单号为：" +
                        info.OutTradeNo + "；我们将尽快为您打包商品，请耐心等待: )"));
                    paramList.Add(new TemplateMessageParam("orderMoneySum", countcost + " 元"));
                    paramList.Add(new TemplateMessageParam("orderProductName", pname));
                    paramList.Add(new TemplateMessageParam("Remark", "如有问题请致电400-885-5790或直接在微信留言，小V将第一时间为您服务！"));
                    wx.SendTemplateMessage(info.OpenId, "IR3TlAC2Y3lW0jaksuPRwHrVHe5nmbWRcD6ZeUPZPlA",
                        "http://www.vgo2013.com/PalmShop/ShopCode/OrderDetail.aspx?oid=" + info.OutTradeNo,
                        paramList.ToArray(), "");
                    MSOrderLogDAL.AddMSOrderLog("发送模板消息到客户OpenID为【" + info.OpenId + "】大致为：我们已收到您的货款" + countcost + "元,产品名称【" + pname + "】我们将尽快为您打包商品，请耐心等待: )");
                    //JQDialog.SendWeiXinMsg(strSiteCode, info.OpenId,
                    //                       "您的订单号【" + info.OutTradeNo + "】已于" +
                    //                       DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") +
                    //                       "付款成功，支付金额：" + countcost + 
                    //                       "元；在等待卖家发货，详情请进入我的订单查询!");
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
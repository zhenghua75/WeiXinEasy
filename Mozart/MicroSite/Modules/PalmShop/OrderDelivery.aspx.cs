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
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class OrderDelivery : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        string action = string.Empty;
        string strSiteCode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    action = action.ToLower().Trim();
                    switch (action)
                    {
                        case "valiteordernum":
                            valiteordernum();
                            break;
                        case "pubol":
                            submitOrder();
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/OrderDelivery.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            //context.TempData["artList"] = liArtList;
            context.TempData["errormsg"] = errormsg;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        void valiteordernum()
        {
            string ordernum = string.Empty; string cid = string.Empty;
            if (Request["ordernum"] != null && Request["ordernum"] != "")
            {
                ordernum = Request["ordernum"];
            }
            if (Request["cid"] != null && Request["cid"] != "")
            {
                cid = Request["cid"];
            }
            if (ordernum != null && ordernum != "")
            {
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                MSOrderLogisticsDAL logisticsDal = new MSOrderLogisticsDAL();
                if (!OrderDal.ExsitOrderNum(ordernum))
                {
                    Response.Write("{\"error\":true,\"msg\":\"该订单无效\"}");
                }
                else
                {
                    if (logisticsDal.ExistOID(cid, ordernum))
                    {
                        Response.Write("{\"error\":true,\"msg\":\"该订单已提交过\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":true}");
                    }
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"请输入正确的订单\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 提交订单
        /// </summary>
        void submitOrder()
        {
            string oid = string.Empty; string cid = string.Empty; string cname = string.Empty;
            #region-获取页面请求值
            try
            {
                oid = Request.Form.Get("ordernum").ToString();
            }
            catch (Exception)
            {
                oid = "";
            }
            try
            {
                cname = Request.Form.Get("cname").ToString();
            }
            catch (Exception)
            {
                cname = "";
            }
            try
            {
                cid = Request.Form.Get("cid").ToString();
            }
            catch (Exception)
            {
                cid = "";
            }
            #endregion
            strSiteCode = "VYIGO"; string customerid = string.Empty; string openid = string.Empty;
            if (oid != null && oid != "" && cid != null && cid != "")
            {
                MSOrderLogistics olModel = new MSOrderLogistics();
                MSOrderLogisticsDAL olDal = new MSOrderLogisticsDAL();
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                olModel.ID = cid;olModel.OID = oid;olModel.CName = cname;

                customerid= OrderDal.GetOrderValueByID("CustomerID", oid).ToString();

                if (customerid != null && customerid != "")
                {
                    MSCustomersDAL customerDal = new MSCustomersDAL();
                    openid = customerDal.GetCustomerValueByID("OpenID", customerid).ToString();
                }

                if (olDal.AddOrderLogistics(olModel) && OrderDal.UpdateOrderState("IsSend", oid))
                {

                    string buyName = string.Empty;
                    try
                    {
                        buyName = OrderDal.GetOrderValueByID("BuyName", oid).ToString();
                        WeiXinCore.WeiXin wx = WXHelper.CreateWeiXinInstanceBySiteCode(strSiteCode);
                        List<TemplateMessageParam> paramList = new List<TemplateMessageParam>();
                        paramList.Add(new TemplateMessageParam("first", "亲，宝贝已经启程了，好想快点来到你身边"));
                        paramList.Add(new TemplateMessageParam("delivername", cname));
                        paramList.Add(new TemplateMessageParam("ordername", cid));
                        paramList.Add(new TemplateMessageParam("remark",
                            "请关注公众号【vgo2013】进入“快点我”查看完整物流信息"));
                        SendTemplateMessageReturnObj temobj = wx.SendTemplateMessage(openid, 
                            "k4eMeEtKReeDvDRFAf8-Li9FuuHkaFPP1xQI6t3Jomo",
                            "http://www.vgo2013.com/PalmShop/ShopCode/CopyOrder.aspx?oid=" + oid,
                            paramList.ToArray(), "");
                        MSOrderLogDAL.AddMSOrderLog("提示客户订单【" + oid + "】已经发货，发送模板消息到客户OpenID【" + openid + "】");
                        bool flag=true;
                        if (flag)
                        { }
                    }
                    catch { }

                    errormsg = JQDialog.alertOKMsgBox(3, "操作成功！", "OrderDelivery.aspx", "succeed");
                }
                else
                {
                    errormsg = JQDialog.alertOKMsgBoxGoBack(3, "操作失败，请重新操作！", false);
                }
            }
            else
            {
                errormsg = JQDialog.alertOKMsgBoxGoBack(3, "操作失败，请重新操作！", false);
            }
        }
    }
}
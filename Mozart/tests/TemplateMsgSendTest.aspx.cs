using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.tests
{
    public partial class TemplateMsgSendTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            WeiXinCore.WeiXin wx = WXHelper.CreateWeiXinInstanceBySiteCode("VYIGO");
            //List<WeiXinCore.Models.TemplateMessageParam> list=new List<WeiXinCore.Models.TemplateMessageParam>();
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("first", "您好"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("orderMoneySum", "12.5"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("orderProductName", "测试商品"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("Remark", "我是备注"));
            //WeiXinCore.Models.SendTemplateMessageReturnObj obj = wx.SendTemplateMessage(TextBox1.Text,
            //    "IR3TlAC2Y3lW0jaksuPRwHrVHe5nmbWRcD6ZeUPZPlA",
            //    "http://www.baidu.com",list.ToArray());
            List<WeiXinCore.Models.TemplateMessageParam> list = new List<WeiXinCore.Models.TemplateMessageParam>();
            list.Add(new WeiXinCore.Models.TemplateMessageParam("first", "您好"));
            list.Add(new WeiXinCore.Models.TemplateMessageParam("delivername", "快递公司测试"));
            list.Add(new WeiXinCore.Models.TemplateMessageParam("ordername", "快递单号测试"));
            list.Add(new WeiXinCore.Models.TemplateMessageParam("Remark", "我是备注"));
            WeiXinCore.Models.SendTemplateMessageReturnObj obj = wx.SendTemplateMessage(TextBox1.Text,
                "k4eMeEtKReeDvDRFAf8-Li9FuuHkaFPP1xQI6t3Jomo","http://www.baidu.com", list.ToArray());

            //List<WeiXinCore.Models.TemplateMessageParam> list = new List<WeiXinCore.Models.TemplateMessageParam>();
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("first", "您好"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("OrderSn", "快递公司测试"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("OrderStatus", "快递单号测试"));
            //list.Add(new WeiXinCore.Models.TemplateMessageParam("Remark", "其他订单信息，物流信息: 圆通速递(上海)，快递单号: 8031971890"));
            //WeiXinCore.Models.SendTemplateMessageReturnObj obj = wx.SendTemplateMessage(TextBox1.Text,
            //    "wmrxCKRq1hG3cHR0BXsuUnNq1chcbVosqYLqlsBBRCc",
            //    "http://www.baidu.com", list.ToArray());
            int i=4;
        }
    }
}
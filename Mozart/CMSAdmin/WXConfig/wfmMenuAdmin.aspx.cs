using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using DAL.WeiXin;
using WeiXinCore;

namespace Mozart.CMSAdmin.WXConfig
{
    public partial class wfmMenuAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCreateMenu_Click(object sender, EventArgs e)
        {
            string strAppID = string.Empty;
            string strSecret = string.Empty;

            Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
            WXConfigDAL wcdal = new WXConfigDAL();
            //wc = wcdal.GetWXConfigBySiteCode("KM_HLF");
            wc = wcdal.GetWXConfigBySiteCode("QJTV");
            if (null != wc)
            {
                strAppID = wc.WXAppID;
                strSecret = wc.WXAppSecret;
            }
            //http://116.52.251.251/Default
            //662DC897E4C9195AB3B0DEDEB5575782
            //string acc_token = this.txtAccessToken.Text;//此部分设置你的access_token
            //自助服务：余额查询-充值缴费-业务受理-水电缴费-Q币充值
            //网上商城：号码商城-手机商城
            //精彩无限：号码绑定-最新活动-天气资讯-帮助
            //string strAppID = string.Empty;
            //string strSecret = string.Empty;

            string acc_token = getAccToken(strAppID, strSecret);
            //acc_token = "DVYt4jXMDPGyE6gwm62LXQ0dmgxm8wTaW_5a1Bp6WoOWC5PwJUvE5ElkCyLpTfIiqOH_gZkUzyZk7Lz8Y2wnVSQMkyPd4CmLSLXDVvb3sXYpKgKZg7JTBBE4dAJ3pEqtPTYgw14jQM8ZLNBWiCpmLQ";
            string url = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token=" + acc_token;

            //奥琦便民服务
            //String responeJsonStr = "{\"button\":[" +
            //                            "{\"name\":\"自助服务\",\"sub_button\":[" +
            //                                "{\"type\":\"view\",\"name\":\"Q币充值\",\"url\":\"http://115.29.141.99:8080/ChargeFee/QQPayFee\"}," +
            //                                "{\"type\":\"view\",\"name\":\"水电缴费\",\"url\":\"http://115.29.141.99:8080/ChargeFee/OtherPayFee\"}," +
            //                                "{\"type\":\"click\",\"name\":\"业务受理\",\"key\":\".\"}," +
            //                                "{\"type\":\"click\",\"name\":\"余额查询\",\"key\":\"menu_service_queryFee\"}," +
            //                                "{\"type\":\"view\",\"name\":\"充值缴费\",\"url\":\"http://115.29.141.99:8080/ChargeFee/MobilePayFee\"}" +
            //                            "]}," +
            //                            "{\"name\":\"网上商城\",\"sub_button\":[" +
            //                                "{\"type\":\"view\",\"name\":\"手机商城\",\"url\":\"http://115.29.141.99:8080/MobileSell/MobileSell\"}," +
            //                                "{\"type\":\"view\",\"name\":\"号码商城\",\"url\":\"http://115.29.141.99:8080/NumberSell/NumberSell\"}" +
            //                            "]}," +
            //                            "{\"name\":\"更多\",\"sub_button\":[" +
            //                                "{\"type\":\"click\",\"name\":\"号码绑定\",\"key\":\"menu_my_binding\"}," +
            //                                "{\"type\":\"click\",\"name\":\"最新活动\",\"key\":\"menu_my_activity\"}," +
            //                                "{\"type\":\"view\",\"name\":\"天气资讯\",\"url\":\"http://115.29.141.99:8080/OtherService/GetWeather\"}," +
            //                                "{\"type\":\"click\",\"name\":\"帮助\",\"key\":\"menu_my_help\"}" +
            //                            "]}" +
            //                         "]}";
            
//            /*奥琦便民服务
//{"menu":{
//    "button":[
//        {"name":"自助服务","sub_button":[
//            {"type":"view","name":"Q币充值","url":"http://115.29.147.37:8080/ChargeFee/QQPayFee","sub_button":[]},
//            {"type":"view","name":"水电缴费","url":"http://115.29.147.37:8080/ChargeFee/OtherPayFee","sub_button":[]},
//            {"type":"click","name":"业务受理","key":".","sub_button":[]},
//            {"type":"click","name":"余额查询","key":"menu_service_queryFee","sub_button":[]},
//            {"type":"view","name":"充值缴费","url":"http://115.29.147.37:8080/ChargeFee/MobilePayFee","sub_button":[]}]},
//        {"name":"网上商城","sub_button":[
//            {"type":"view","name":"手机商城","url":"http://115.29.147.37:8080/MobileSell/MobileSell","sub_button":[]},
//            {"type":"view","name":"号码商城","url":"http://115.29.147.37:8080/NumberSell/NumberSell","sub_button":[]}]},
//        {"name":"更多","sub_button":[
//            {"type":"click","name":"号码绑定","key":"menu_my_binding","sub_button":[]},
//            {"type":"click","name":"最新活动","key":"menu_my_activity","sub_button":[]},
//            {"type":"view","name":"天气资讯","url":"http://115.29.147.37:8080/OtherService/GetWeather","sub_button":[]},
//            {"type":"click","name":"帮助","key":"menu_my_help","sub_button":[]}]}]}}
//             * /

            //海立方
            //String responeJsonStr = "{\"button\":[" +
            //                "{\"name\":\"海立方\",\"sub_button\":[" +
            //                    "{\"type\":\"click\",\"name\":\"公司介绍\",\"key\":\"xh_jieshao\"}," +
            //                    "{\"type\":\"click\",\"name\":\"SPA美甲\",\"key\":\"xh_anmo\"}," +
            //                    "{\"type\":\"click\",\"name\":\"餐饮娱乐\",\"key\":\"xh_chanyu\"}," +
            //                    "{\"type\":\"click\",\"name\":\"洗浴保健\",\"key\":\"xh_xiyu\"}," +
            //                    "{\"type\":\"click\",\"name\":\"酒店客房\",\"key\":\"xh_kefang\"}" +
            //                "]}," +
            //                "{\"name\":\"酒店预订\",\"sub_button\":[" +
            //                    "{\"type\":\"click\",\"name\":\"小海促销\",\"key\":\"xh_chuxiao\"}," +
            //                    "{\"type\":\"view\",\"name\":\"水汇预订\",\"url\":\"http://114.215.108.27/MicroSite/ProductList.aspx?SiteCode=KM_HLF&CatID=02F25D571BA6438AB965428160A0AA68\"}," +
            //                    "{\"type\":\"view\",\"name\":\"客房预订\",\"url\":\"http://114.215.108.27/MicroSite/ProductList.aspx?SiteCode=KM_HLF&CatID=31A10FB143C248A3A2BA5B451DE13276\"}" +
            //                "]}," +
            //                "{\"name\":\"我的海立方\",\"sub_button\":[" +
            //                    "{\"type\":\"click\",\"name\":\"便利服务\",\"key\":\"xh_chuxiao\"}," +
            //                    "{\"type\":\"click\",\"name\":\"我的优惠\",\"key\":\"my_coupon\"}," +
            //                    "{\"type\":\"view\",\"name\":\"我的订单\",\"url\":\"http://114.215.108.27/MicroSite/MyOrder.aspx?SiteCode=KM_HLF\"}," +
            //                    "{\"type\":\"view\",\"name\":\"我要注册\",\"url\":\"http://114.215.108.27/MicroSite/Reg.aspx?SiteCode=KM_HLF\"}," +
            //                    "{\"type\":\"click\",\"name\":\"我的会员卡\",\"key\":\"my_membership\"}" +
            //                "]}" +
            //             "]}";

            String responeJsonStr = "{\"button\":[" +
                            "{\"name\":\"海立方\",\"sub_button\":[" +
                                "{\"type\":\"click\",\"name\":\"公司介绍\",\"key\":\"xh_jieshao\"}," +
                                "{\"type\":\"click\",\"name\":\"SPA美甲\",\"key\":\"xh_anmo\"}," +
                                "{\"type\":\"click\",\"name\":\"餐饮娱乐\",\"key\":\"xh_chanyu\"}," +
                                "{\"type\":\"click\",\"name\":\"洗浴保健\",\"key\":\"xh_xiyu\"}," +
                                "{\"type\":\"click\",\"name\":\"酒店客房\",\"key\":\"xh_kefang\"}" +
                            "]}," +
                            "{\"name\":\"世界杯活动\",\"sub_button\":[" +
                                "{\"type\":\"view\",\"name\":\"水汇预订\",\"url\":\"http://114.215.108.27/MicroSite/ProductList.aspx?SiteCode=KM_HLF&CatID=02F25D571BA6438AB965428160A0AA68\"}," +
                                "{\"type\":\"view\",\"name\":\"客房预订\",\"url\":\"http://114.215.108.27/MicroSite/ProductList.aspx?SiteCode=KM_HLF&CatID=31A10FB143C248A3A2BA5B451DE13276\"}" +
                                "{\"type\":\"view\",\"name\":\"球赛竞猜\",\"url\":\"http://114.215.108.27/MicroSite/JCQuizDetail.aspx?OpenID=1111&SiteCode=KM_HLF\"}" +
                                "{\"type\":\"view\",\"name\":\"赛程魔方\",\"url\":\"http://114.215.108.27/MicroSite/themes/Quiz/WorldCupMoFang.html\"}" +
                            "]}," +
                            "{\"name\":\"我的海立方\",\"sub_button\":[" +
                                "{\"type\":\"click\",\"name\":\"便利服务\",\"key\":\"xh_chuxiao\"}," +
                                "{\"type\":\"click\",\"name\":\"我的优惠\",\"key\":\"my_coupon\"}," +
                                "{\"type\":\"view\",\"name\":\"我的订单\",\"url\":\"http://114.215.108.27/MicroSite/MyOrder.aspx?SiteCode=KM_HLF\"}," +
                                "{\"type\":\"view\",\"name\":\"我要注册\",\"url\":\"http://114.215.108.27/MicroSite/Reg.aspx?SiteCode=KM_HLF\"}," +
                                "{\"type\":\"click\",\"name\":\"我的会员卡\",\"key\":\"my_membership\"}" +
                            "]}" +
                         "]}";

            ////曲靖电视台
            //String responeJsonStr = "{\"button\":[" +
            //    "{\"type\":\"view\",\"name\":\"节目展示\",\"url\":\"http://114.215.108.27/MicroSite/ArticleList.aspx?sitecode=QJTV&Catid=4E43448EADB844819AED5523A2C13F03\"}," +
            //    "{\"name\":\"观众互动\",\"sub_button\":[" +
            //        "{\"type\":\"view\",\"name\":\"观众热线\",\"url\":\"http://114.215.108.27/MicroSite/Guestbook.aspx?ID=F9671002614D4A6CBEA6BFC8B1F281FC\"}," +
            //        "{\"type\":\"view\",\"name\":\"观众投票\",\"url\":\"http://114.215.108.27/MicroSite/Vote.aspx?ID=F9671002614D4A6CBEA6BFC8B1F281FC\"}" +
            //    "]}," +
            //    "{\"name\":\"第三方合作\",\"sub_button\":[" +
            //        "{\"type\":\"click\",\"name\":\"便利服务\",\"key\":\"ps_service\"}," +
            //        "{\"type\":\"click\",\"name\":\"关注有奖\",\"key\":\"my_coupon\"}" +
            //    "]}" +
            // "]}";

            txtResult.Text = PostWebRequest(url, responeJsonStr, Encoding.UTF8);
        }

        //定义post函数
        public string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
                // MessageBox.Show(ex.Message);
            }
            return ret;
        }

        protected string getAccToken(string strAppID, string strSecret)
        {
            //string strurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wx1ea30615a87dbfd1&secret=00749a70b6ceea7f1afd222f79300f70";
            string strurl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=" + strAppID + "&secret=" + strSecret;
            string acc_token = PostWebRequest(strurl, "", Encoding.UTF8);
            //string strTmp = "{\"access_token\":\"vaU-Ebd8URv27kWJ6v9cktxQNzBGJF9Q2DOBX2Rg0Velmf-NeB3RlC1g7t0Z0SRL2Ppu0BZsM0FAHKcuYdZM6Tdl8BOHtJW2in_jIV2Ovs5hPXTwCfYdR57ZTV0iLKu0Vf4IL2fe9vBSnByOuTTQ_A\",\"expires_in\":7200}";
            var KeyToken = new { access_token = "" };
            var b = JsonConvert.DeserializeAnonymousType(acc_token, KeyToken);
            string strResult = b.access_token;
            return strResult;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            WXConfigDAL wcdal = new WXConfigDAL();
            //Model.WeiXin.WXConfig wxConfig = wcdal.GetWXConfigBySiteCode("QJTV");
            //Model.WeiXin.WXConfig wxConfig = wcdal.GetWXConfigBySiteCode("KM_HLF");
            //Model.WeiXin.WXConfig wxConfig = wcdal.GetWXConfigBySiteCode("yn-zhbm");   
            Model.WeiXin.WXConfig wxConfig = wcdal.GetWXConfigBySiteCode("VYIGO"); 
            MenuDAL.CreateWeiXinMenu(wxConfig.ID);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            #region 消费完成发生消息
            string strAppID = string.Empty;
            string strSecret = string.Empty;
            Model.WeiXin.WXConfig wc = new Model.WeiXin.WXConfig();
            WXConfigDAL wcdal = new WXConfigDAL();
            //wc = wcdal.GetWXConfigBySiteCode("QJTV");
            wc = wcdal.GetWXConfigBySiteCode("vyigo");
            if (null != wc)
            {
                strAppID = wc.WXAppID;
                strSecret = wc.WXAppSecret;
            }
            string strToken = WeiXinHelper.GetAccessToken(strAppID, strSecret);

            var KeyToken = new { access_token = "" };
            var b = JsonConvert.DeserializeAnonymousType(strToken, KeyToken);
            string strRToken = b.access_token;

            //WeiXinHelper.SendCustomTextMessage(strRToken, "oScaMjtue4NyANx0xQzNvGYxAs8U", "测试功能");
            WeiXinHelper.SendCustomTextMessage(strRToken, "okNacjt93Jsmr84P9EbR66pIt-sc", "回复测试功，能看到消息吗？");
            #endregion
        }
    }
}
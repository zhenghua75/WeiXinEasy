using DAL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.tests
{
    public partial class CCPTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ret = null;

            //CCPRestSDK api = new CCPRestSDK();
            ////ip格式如下，不带https://
            //bool isInit = api.init("sandboxapp.cloopen.com", "8883");
            //api.setAccount("aaf98f89499d24b50149cb1353b91900", "f5241e29e403406aaeecad5ab4a3a68c");
            //api.setAppId("8a48b55149896cfd0149cb14a05429cf");
            CCPRestSDK api =  CCPRestSDK.GetInstance();

            try
            {
                if (api!=null)
                {
                    string code = DateTime.Now.ToString("HHmmss");
                    string tel = "13577073155";
                    //默认模板消息[1]：【云通讯】您使用的是云通讯短信模板，您的验证码是{1}，请于{2}分钟内正确输入
                    //参数Data对应模板消息的索引
                    Dictionary<string, object> retData = api.SendTemplateSMS(tel, "1", new string[] { code, "2" });
                    ret = getDictionaryData(retData);
                    //对响应解析后，statusCode为“000000”表示请求发送成功。statusCode不是“000000”，表示请求发送失败，客户服务端可以根据自己的逻辑进行重发或者其他处理。 
                    //statusCode=000000;statusMsg=成功;data={TemplateSMS={dateCreated=20141122134226;smsMessageSid=201411221342253809119;};};
                }
                else
                {
                    ret = "初始化失败";
                }
            }
            catch (Exception exc)
            {
                ret = exc.Message;
            }
            finally
            {
                Response.Write(ret);
            }
        }

        private string getDictionaryData(Dictionary<string, object> data)
        {
            string ret = null;
            foreach (KeyValuePair<string, object> item in data)
            {
                if (item.Value != null && item.Value.GetType() == typeof(Dictionary<string, object>))
                {
                    ret += item.Key.ToString() + "={";
                    ret += getDictionaryData((Dictionary<string, object>)item.Value);
                    ret += "};";
                }
                else
                {
                    ret += item.Key.ToString() + "=" + (item.Value == null ? "null" : item.Value.ToString()) + ";";
                }
            }
            return ret;
        }
    }
}
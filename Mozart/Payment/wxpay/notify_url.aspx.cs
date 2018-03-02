using DAL.SYS;
using Model.SYS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace Mozart.Payment.wxpay
{
    public partial class notify_url : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string return_code = string.Empty;
                string return_msg = string.Empty;
                Dictionary<string, string> dict = GetRequestPost();
                if (wxpayHelper.VerifyNotify(dict, dict["sign"]))
                {
                    return_code = "SUCCESS";
                    if (dict["return_code"] == "SUCCESS")
                    {
                        if (dict["result_code"] == "SUCCESS")
                        {
                            string openid=dict["openid"];
                            int total_fee=int.Parse(dict["total_fee"]);
                            string trade_type = dict["trade_type"];
                            string transaction_id = dict["transaction_id"];
                            string out_trade_no = dict["out_trade_no"];
                            string attach = dict["attach"];
                            if (!string.IsNullOrEmpty(openid) && 
                                total_fee > 0 && 
                                !string.IsNullOrEmpty(transaction_id) &&
                                !string.IsNullOrEmpty(out_trade_no))
                            {
                                ExceptionLog log = new ExceptionLog();
                                log.Message = string.Format("Openid:{0},订单号：{1},附加消息：{2}",
                                    openid,out_trade_no,attach);
                                ExceptionLogDAL.InsertExceptionLog(log);
                                //DisposeOrder(out_trade_no,attach);
                            }
                            else
                            {
                                return_code = "FAIL";
                                return_msg = "参数格式校验错误";
                            }
                        }
                    }
                }
                else
                {
                    return_code = "FAIL";
                    return_msg="签名失败";
                }
                string returnValue = string.Format("<xml><return_code>{0}</return_code><return_msg>{1}</return_msg></xml>",
                    return_code, return_msg);
            }
        }

        /// <summary>
        /// 处理订单
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <param name="attach"></param>
        private void DisposeOrder(string out_trade_no, string attach)
        {
           
        }

        public Dictionary<string, string> GetRequestPost()
        {
            Dictionary<string,string> res=null;
            byte[] byts = new byte[Request.InputStream.Length];
            Request.InputStream.Read(byts, 0, byts.Length);
            string req = System.Text.Encoding.UTF8.GetString(byts);
            //req = Server.UrlDecode(req);
            if (!string.IsNullOrEmpty(req))
            {
                res=new Dictionary<string,string>();
                XElement xml = XElement.Parse(req);
                foreach (XElement xe in xml.Elements())
                {
                    res.Add(xe.Name.ToString(), xe.Value);
                }
            }
            return res;
        }
    }
}
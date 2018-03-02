using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class ReleaseSetting
    {
        public bool weixin { get; set; }
        public string apiurl { get; set; }
        public string token { get; set; }
        public string appid { get; set; }
        public string appsecret { get; set; }
        public string paysignkey { get; set; }
        public string partnerid { get; set; }
        public string partnerkey { get; set; }
        public bool iphone { get; set; }
        public bool iphone_ec { get; set; }
        public bool clientcredentials { get; set; }
        public bool android { get; set; }
        public bool mp { get; set; }
        public bool sms { get; set; }
        public bool weixin1 { get; set; }
        public bool weixin2 { get; set; }
        public bool weixin3 { get; set; }
        public string logourl { get; set; }
        public string clientname { get; set; }
        public string clientdescription { get; set; }
        public string appdesc { get; set; }
        public Right rights { get; set; }
    }
}
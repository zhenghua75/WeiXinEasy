using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Mozart.CMSAdmin.TemplateManage
{
    public class TmpInfo
    {
        public int Id { get; set; }
        public string ShellType { get; set; }
        public string ImgUrl { get; set; }
    }
    /// <summary>
    /// changetmp 的摘要说明
    /// </summary>
    public class changetmp : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string curPage = context.Request["query4Page.curPage"];
            context.Response.ContentType = "text/html;charset=utf-8";
            string htmlFragment = Helper.GetHtmnlFragment(context, "changetmp_"+curPage);
            context.Response.Write(htmlFragment);
        }

        private void GetTmp()
        {
            PageInfo<TmpInfo> pi = new PageInfo<TmpInfo>();

            List<TmpInfo> tmps = new List<TmpInfo>();

            TmpInfo tmp = new TmpInfo { Id = 29, ShellType = "shellR", ImgUrl = "Mb24.png" };
            tmp = new TmpInfo { Id = 30, ShellType = "shellR", ImgUrl = "Mb25.png" };

            tmp = new TmpInfo { Id = 27, ShellType = "shellP", ImgUrl = "Mb22.png" };

            //XmlSerializer ser = new XmlSerializer(typeof(List<TmpInfo>));
            //ser.Serialize()
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
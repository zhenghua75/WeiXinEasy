using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class VotePhoto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strSiteCode = string.Empty;

            if (null == Request.QueryString["sitecode"])
            {
                return;
            }

            strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());

            //取站点相册列表
            DAL.Vote.OptionDAL dalVotePhoto = new DAL.Vote.OptionDAL();
            DataSet dsThumbList = dalVotePhoto.getVotePhoto(strSiteCode);
            List<VotePhoto> lstThumb = new List<VotePhoto>();
            foreach (DataRow row in dsThumbList.Tables[0].Rows)
            {
                VotePhoto model = DataConvert.DataRowToModel<VotePhoto>(row);
                lstThumb.Add(model);
            }


            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Vote/VotePhoto.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["sitecode"] = strSiteCode;
            context.TempData["title"] = "梦想展台";
            context.TempData["lstThumb"] = lstThumb;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }

    partial class VotePhoto
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Ico { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Vote;
using Model.Vote;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class VoteOptionDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strOptionID = string.Empty;
            if (null == Request.QueryString["id"])
            {
                return;
            }
            strOptionID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            OptionDAL dal = new OptionDAL();
            DataSet ds = dal.getOptionDetail(strOptionID);

            VOTE_Option model = new VOTE_Option();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<VOTE_Option>(ds.Tables[0].Rows[0]);
            }

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Vote/OptionDetail.html"));

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "微商易";
            context.TempData["ADetail"] = model;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
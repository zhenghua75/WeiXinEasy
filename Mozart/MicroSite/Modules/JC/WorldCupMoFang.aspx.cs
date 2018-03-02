using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.MicroSite
{
    public partial class WorldCupMoFang : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //读取模板内容
            string text = System.IO.File.ReadAllText(Server.MapPath("Themes/Quiz/WorldCupMoFang.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
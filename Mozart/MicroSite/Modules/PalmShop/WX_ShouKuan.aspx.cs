using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.PalmShop.ShopCode
{
    public partial class WX_ShouKuan : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/WX_ShouKuan.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
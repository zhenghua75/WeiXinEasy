using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.PublicService.ServiceCode
{
    public partial class ServiceList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<Model.PublicService.PS_Service> liService = new List<Model.PublicService.PS_Service>();

            DAL.PublicService.PS_Service dal = new DAL.PublicService.PS_Service();

            DataSet ds = dal.GetServiceList();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Model.PublicService.PS_Service model = DataConvert.DataRowToModel<Model.PublicService.PS_Service>(row);
                liService.Add(model);
            }

            //读取模板内容 
            string text = System.IO.File.ReadAllText(Server.MapPath("../ServicePage/ServiceList.html")); 
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "便捷服务列表";
            context.TempData["product_list"] = liService;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
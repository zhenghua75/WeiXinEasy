using Mozart.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Mozart.MicroSite
{
    public class BasePage : System.Web.UI.Page
    {
        public string SiteID { get; set; }
        public string SiteCode { get; set; }
        public string OpenID { get; set; }
        public string CouponID { get; set; }
        public List<ModuleField> ModuleFields { get; set; }
        public string TemplatePath { get; set; }
        public Module CurrentModule { get; set; }
        public string Theme { get; set; }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(!BeforeLoad()) return;
            string strTitle = string.Empty;
            string siteSettingJson = File.ReadAllText(Server.MapPath("/MicroSite/SiteSetting.json"), System.Text.Encoding.UTF8);

            SiteSetting siteSetting = JsonConvert.DeserializeObject<SiteSetting>(siteSettingJson);
            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DataSet ds;
            if(!string.IsNullOrEmpty(SiteCode))
            {
                ds = dalAccount.GetAExtDataBySiteCode(SiteCode);
            }
            else
            {
                SiteID = null == Request.QueryString["ID"] ? siteSetting.DefaultSiteId : Mozart.Common.Common.NoHtml(Request.QueryString["ID"].ToString());
                ds = dalAccount.GetAccountExtData(SiteID);
            }
            
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                var dynamicTable = ds.Tables[0].AsDynamicEnumerable();
                var dynamicObj = dynamicTable.First();
                Theme = dynamicObj.Themes;
                strTitle = dynamicObj.Name;
                SiteCode = dynamicObj.SiteCode;
                SiteID = dynamicObj.ID;
            }
            Session["siteid"] = SiteID;
            Session["strSiteCode"] = SiteCode;
            List<Module> modules = new List<Module>();
            foreach (Category category in siteSetting.Categories)
            {
                modules.AddRange(category.Modules);
            }
            CurrentModule = modules.FirstOrDefault(w => w.Path == Request.Path);            
            SetTemplatePath();
            string templateModuleJsonPath = string.Format(CurrentModule.Setting, "/MicroSite/Themes/" + Theme + "/");
            string ModuleJsonPath = string.Format(CurrentModule.Setting, "/MicroSite/Modules/");
            if(CurrentModule.IsHomePage)
            {
                ModuleJsonPath = string.Format(CurrentModule.Setting, "/MicroSite/");
            }
            string moduleJsonPhysicalPath = Server.MapPath(ModuleJsonPath);
            string templateModuleJsonPhysicalPath = Server.MapPath(templateModuleJsonPath);
            ModuleFields = new List<ModuleField>();
            if (File.Exists(templateModuleJsonPhysicalPath))
            {
                string moduleFieldJson = File.ReadAllText(templateModuleJsonPhysicalPath, System.Text.Encoding.UTF8);
                ModuleFields = JsonConvert.DeserializeObject<List<ModuleField>>(moduleFieldJson);
            }
            else
            {
                string moduleFieldJson = File.ReadAllText(moduleJsonPhysicalPath, System.Text.Encoding.UTF8);
                ModuleFields = JsonConvert.DeserializeObject<List<ModuleField>>(moduleFieldJson);
            }

            string text = System.IO.File.ReadAllText(Server.MapPath(TemplatePath));


            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            foreach (Module module in modules)
            {
                context.TempData[module.Code] = module.Path + "?ID=" + SiteID;
            }

            foreach (ModuleField moduleField in ModuleFields)
            {
                if (moduleField.CanModified)
                {
                    context.TempData[moduleField.Code] = moduleField.Value;
                }
            }
            context.TempData["siteid"] = SiteID;
            context.TempData["sitecode"] = SiteCode;
            context.TempData["SiteUrl"] = "/MicroSite/";
            context.TempData["SiteModuleUrl"] = "/MicroSite/Modules/";
            AddTempData(context);
            JinianNet.JNTemplate.Template template = new JinianNet.JNTemplate.Template(context, text);
            template.Render(Response.Output);
        }
        protected virtual void SetTemplatePath()
        {
            TemplatePath = string.Format(CurrentModule.Theme, "/MicroSite/Themes/" + Theme + "/");
        }
        protected virtual bool BeforeLoad()
        {
            return true;
        }
        protected virtual void AddTempData(JinianNet.JNTemplate.TemplateContext context)
        { }
        protected void SetQRCode(JinianNet.JNTemplate.TemplateContext context,string id)
        {
            QRCode qr = new QRCode();
            ModuleField moduleFieldQRCodeType = this.ModuleFields.FirstOrDefault(f => f.Code == "QRCodeType");
            ModuleField moduleFieldQRCodeUrl = this.ModuleFields.FirstOrDefault(f => f.Code == "QRCodeUrl");
            string url = "http://www.vgo2013.com/WebService/QR.aspx";
            if (moduleFieldQRCodeUrl != null)
            {
                url = moduleFieldQRCodeUrl.Value;
            }
            if (moduleFieldQRCodeType != null && moduleFieldQRCodeType.Value == "ImageQRCode")
            {
                context.TempData["qrcode"] = qr.GetImageQRCode(url + "?ID=" + id);
            }
            else
            {
                context.TempData["qrcode"] = qr.GetQRCode(url + "?ID=" + id);
            }
        }
    }
}
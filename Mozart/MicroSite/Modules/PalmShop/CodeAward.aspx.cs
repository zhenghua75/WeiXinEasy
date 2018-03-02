using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class CodeAward : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        string Extfildid = string.Empty; int Amount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            int receive = 0;
            if (Request["id"] != null && Request["id"] != "")
            {
                Extfildid = Common.Common.NoHtml(Request["id"]);
            }
            if (Extfildid != null && Extfildid != "")
            {
                MSVAcctDetailDAL msvadetailDal = new MSVAcctDetailDAL();
                try
                {
                    receive = Convert.ToInt32(msvadetailDal.GetMSVAcctDetailByFid("IsReceive", Extfildid).ToString());
                }
                catch (Exception)
                {
                    receive = -1;
                }
                if (receive == 0)
                {
                    msvadetailDal.UpdateMSVacct(Extfildid);
                }
                try
                {
                    Amount = Convert.ToInt32(msvadetailDal.GetMSVAcctDetailByFid("Amount", Extfildid).ToString());
                }
                catch (Exception)
                {
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/CodeAward.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;
            context.TempData["extfid"] = Extfildid;
            context.TempData["receive"] = receive;
            context.TempData["amount"] = Amount;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
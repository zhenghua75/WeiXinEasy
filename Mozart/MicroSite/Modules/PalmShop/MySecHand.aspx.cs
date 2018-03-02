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
    public partial class MySecHand : System.Web.UI.Page
    {
        string action = string.Empty;
        public static string errorscript = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    if (Request["action"] != null && Request["action"] != "")
                    {
                        action = Request["action"];
                        switch (action.Trim().ToLower())
                        {
                            case "del":
                                delProduct();
                                break;
                        }
                    }
                    errorscript = "";
                    GetInfo();
                }
                else
                {
                    setCookies();
                    errorscript = JQDialog.alertOKMsgBox(2, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                    GetInfo();
                }
            }
        }
        /// <summary>
        /// 设置Cookies
        /// </summary>
        void setCookies()
        {
            HttpCookie delcookies = new HttpCookie("pageurl");
            delcookies.Expires = DateTime.Now.AddDays(-1);
            Response.AppendCookie(delcookies);
            HttpCookie cookies = new HttpCookie("pageurl", "MySecHand.aspx");
            cookies.Expires = DateTime.Now.AddMinutes(2);
            Response.AppendCookie(cookies);
        }
        void GetInfo()
        {
            string like = string.Empty; string strwhere = string.Empty;
            strwhere = " and a.CustomerID='" + Session["customerID"] + "' ";
            if (Request["like"] != null && Request["like"] != "")
            {
                like = " and a.ptitle like '%" + Request["like"] + "%' ";
            }
            else
            {
                like = "";
            }
            strwhere = strwhere + like;
            MSProductDAL productDal = new MSProductDAL();
            List<SecHandModel> MySecHandModel = new List<SecHandModel>();
            DataSet sechandDs = productDal.GetSecHandProduct(strwhere);
            if (sechandDs != null && sechandDs.Tables.Count > 0 && sechandDs.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in sechandDs.Tables[0].Rows)
                {
                    SecHandModel sechandModel = DataConvert.DataRowToModel<SecHandModel>(row);
                    MySecHandModel.Add(sechandModel);
                }
            }
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/MySecHand.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "我的商品";
            context.TempData["errorscript"] = errorscript;
            context.TempData["productlist"] = MySecHandModel;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 更新信息状态
        /// </summary>
        void delProduct()
        {
            string pid=string.Empty;
            if(Request["pid"]!=null&&Request["pid"]!="")
            {
                pid = Common.Common.NoHtml(Request["pid"]);
            }
            if (pid.Trim() != null && pid.Trim() != "")
            {
                MSProductDAL productDal = new MSProductDAL();
                MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
                productDal.UpdateMSProductState(pid);
                atlasDal.UpdateAtlasByPID(pid);
                Response.Write("{\"success\":true}");
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 设置属性
        /// </summary>
        public class SecHandModel
        {
            private string iD;
            private string sID;
            private string cid;
            private string ptitle;
            private string pcontent;
            private string price;
            private string pimgUrl;
            private int isSecHand;
            private int pstate;
            private int review;
            private DateTime addTime;
            public string ID
            {
                get { return iD; }
                set { iD = value; }
            }

            public string SID
            {
                get { return sID; }
                set { sID = value; }
            }

            public string Cid
            {
                get { return cid; }
                set { cid = value; }
            }
            public string Ptitle
            {
                get { return ptitle; }
                set { ptitle = value; }
            }
            public string Pcontent
            {
                get { return pcontent; }
                set { pcontent = value; }
            }
            public string Price
            {
                get { return price; }
                set { price = value; }
            }
            public string PimgUrl
            {
                get { return pimgUrl; }
                set { pimgUrl = value; }
            }
            public int IsSecHand
            {
                get { return isSecHand; }
                set { isSecHand = value; }
            }
            public int Pstate
            {
                get { return pstate; }
                set { pstate = value; }
            }
            public int Review
            {
                get { return review; }
                set { review = value; }
            } 
            public DateTime AddTime
            {
                get { return addTime; }
                set { addTime = value; }
            }
        }
    }
}
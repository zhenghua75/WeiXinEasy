using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.MicroSite
{
    public partial class MemberShip : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strTitle = string.Empty;
            string strTheme = string.Empty;
            string strCustID = string.Empty;
            if (null == Request.QueryString["sitecode"])
            {
                return;
            }
            if (null == Request.QueryString["openid"])
            {
                return;
            }

            string strSiteCode = Common.Common.NoHtml(Request.QueryString["sitecode"].ToString());
            string strOpenID = string.Empty;
            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
                return;
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }

            DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
            DataSet ds = dalAccount.GetAExtDataBySiteCode(strSiteCode);
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                strTheme = ds.Tables[0].Rows[0]["Themes"].ToString();
                strTitle = ds.Tables[0].Rows[0]["Name"].ToString();
                Session["strSiteCode"] = ds.Tables[0].Rows[0]["SiteCode"].ToString();
            }
            //会员卡判断处理
            DAL.SYS.CustomerDAL dalCust = new DAL.SYS.CustomerDAL();
            DAL.Site.SiteMemberNoDAL dalMemNo = new DAL.Site.SiteMemberNoDAL();
            string strCustNo = dalCust.GetCustMemberShipNo(strSiteCode, strOpenID);
            string strMemNO = string.Empty;
            if (strCustNo == "0" || strCustNo == "-1")
            {
                strMemNO = dalMemNo.GetMemberShipNo(strSiteCode);
                if (strCustNo == "-1") //没有用户记录
                {
                    //插入用户记录                  
                    Model.SYS.SYS_Customer modelAdd = new Model.SYS.SYS_Customer
                    {
                        ID = Guid.NewGuid().ToString("N").ToUpper(),
                        Mobile = "",
                        Name = "",
                        PassWord = "",
                        OpenID = strOpenID,
                        SiteCode = strSiteCode,
                        MemberShipNo = strMemNO
                    };
                    if (!dalCust.AddCustomerData(modelAdd))
                    {
                        return;
                    }
                }
                if (strCustNo == "0") //有用户记录，没有会员账号
                {
                    //写入用户账号
                    strMemNO = dalMemNo.GetMemberShipNo(strSiteCode);
                    dalCust.UpdateCutMemberShipNo(strSiteCode, strOpenID, strMemNO);
                }
                //修改站点账户状态
                dalMemNo.UpdateMemNoState(strSiteCode, strMemNO, "1");

            }
            if (strCustNo == "0" || strCustNo == "-1")
            {
                strCustNo = dalCust.GetCustMemberShipNo(strSiteCode, strOpenID);
            }
            //取最近的消费记录

            //读取模板内容
            string text = string.Empty;
            if (!File.Exists(Server.MapPath("Themes/" + strTheme + "/MemberShip.html")))
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/MemberShip/MemberShip.html"));
            }
            else
            {
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/" + strTheme + "/MemberShip.html"));
            }

            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            Common.QRCode qr = new Common.QRCode();
            //context.TempData["qrcode"] = qr.GetQRCode("http://114.215.108.27/WebService/CustMemNo.aspx?SiteCode=" + strSiteCode + "&MemNo=" + strCustNo + "&OpenId=" + strOpenID);
            context.TempData["qrcode"] = qr.GetImageQRCode("http://114.215.108.27/WebService/CustMemNo.aspx?SiteCode=" + strSiteCode + "&MemNo=" + strCustNo + "&OpenId=" + strOpenID);         
            context.TempData["memno"] = strCustNo;
            context.TempData["title"] = strTitle;
            context.TempData["siteid"] = Session["siteid"];
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["sitecode"] = Session["strSiteCode"].ToString();

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
    }
}
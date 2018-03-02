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
    public partial class EditeLogPwd : System.Web.UI.Page
    {
        string errormsg = string.Empty;
        string action = string.Empty;
        string strOpenID = string.Empty;
        string customid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customid = Session["customerID"].ToString();
                    GetOpenId();
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    action = action.Trim().ToLower();
                    switch (action)
                    {
                        case"valitephone":
                            validatephone();
                            break;
                        case "wxphone":
                            validatephone();
                            break;
                        case"valitepwd":
                            valitepwd();
                            break;
                        case "editepwd":
                            EditePhone();
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/EditeLogPwd.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["errormsg"] = errormsg;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void GetOpenId()
        {
            if (customid != null && customid != "")
            {
                MSCustomersDAL CustomerDal = new MSCustomersDAL();
                strOpenID = CustomerDal.GetCustomerValueByID("OpenID", customid).ToString();
                Session["OpenID"] = strOpenID;
            }
            else
            {
                strOpenID = "";
            }
        }
        /// <summary>
        /// 验证电话是否正确
        /// </summary>
        void validatephone()
        {
            string valphone = string.Empty;
            if (Request["phone"] != null && Request["phone"] != "")
            {
                valphone = Common.Common.NoHtml(Request["phone"]);
            }
            if (valphone != null && valphone != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                    if (customerDal.ExistCustomer("", valphone, "", ""))
                    {
                        if (strOpenID != null && strOpenID != "")
                        {
                            if (customerDal.ExistCustomer(strOpenID, "", "", ""))
                            {
                                if (customerDal.ExistCustomer(strOpenID, valphone, "", ""))
                                {
                                    Response.Write("{\"success\":true}");
                                }
                                else
                                {
                                    Response.Write("{\"error\":true,\"msg\":\"该微信号与注册电话不一致\"}");
                                }
                            }
                            else
                            {
                                Response.Write("{\"error\":true,\"msg\":\"该微信还未注册\"}");
                            }
                        }
                        else
                        {
                            Response.Write("{\"success\":true}");
                        }
                    }
                    else
                    {
                        Response.Write("{\"error\":true,\"msg\":\"未找到该用户资料\"}");
                    }
            }
            else
            {
                Response.Write("{\"success\":true}");
            }
            Response.End();
        }
        /// <summary>
        /// 验证电话和旧密码
        /// </summary>
        void valitepwd()
        {
            string phone = string.Empty;
            string oldpwd = string.Empty;
            if (Request["phone"] != null && Request["phone"] != "")
            {
                phone = Request["phone"];
            }
            if (Request["oldpwd"] != null && Request["oldpwd"] != "")
            {
                oldpwd = Request["oldpwd"];
            }
            MSCustomersDAL customerDal = new MSCustomersDAL();
            if (phone != null && phone != "" && oldpwd != null && oldpwd != "")
            {
                if (customerDal.ExistPhoneAndPwd(phone, Common.Common.MD5(oldpwd)))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"原密码不正确\"}");
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"请正确输入信息后再操作\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        void EditePhone()
        {
            string phone = string.Empty;
            string newpwd = string.Empty;
            if (Request["phone"] != null && Request["phone"] != "")
            {
                phone = Request["phone"];
            }
            if (Request["newpwd"] != null && Request["newpwd"] != "")
            {
                newpwd = Request["newpwd"];
            }
            MSCustomersDAL customerDal = new MSCustomersDAL();
            if (phone != null && phone != "" && newpwd != null && newpwd != "")
            {
                if (customerDal.UpdatePwdByPhone(phone, Common.Common.MD5(newpwd)))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请重新操作\"}");
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"请正确输入信息后再操作\"}");
            }
            Response.End();
        }
    }
}
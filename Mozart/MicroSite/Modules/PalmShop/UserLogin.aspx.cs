using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using WeiXinCore;
using DAL.WeiXin;
using DAL.Common;
using Model.SYS;
using DAL.SYS;
using Mozart.Common;
using WeiXinCore.Models;
using Newtonsoft.Json;

namespace Mozart.PalmShop.ShopCode
{
    public partial class UserLogin : System.Web.UI.Page
    {
        public string action = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        public static string regNickName = string.Empty;
        public static int regSex = 0;
        public static string regHeadImg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["OpenID"] == null || Session["OpenID"].ToString()== "")
                {
                    GetUserOpenID();
                }
                else
                {
                    strOpenID = Session["OpenID"].ToString();
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]);
                    if (action.Trim() != null && action.Trim() != "")
                    {
                        switch (action.Trim().ToLower())
                        {
                            case "save":
                                ToSaveUserInfo();
                                break;
                            case "userlogin":
                                ToUserLogin();
                                break;
                            case"getmsg":
                                GetMsg();
                                break;
                            case "validatephone":
                                validatephone();
                                break;
                            case "wxphone":
                                validatephone();
                                break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取用户OpenID
        /// </summary>
        void GetUserOpenID()
        {
            if (null == Request.QueryString["state"])
            {
                //return;
            }
            else
            {
                strSiteCode = Common.Common.NoHtml(Request.QueryString["state"].ToString());
                Session["strSiteCode"] = strSiteCode;
            }
            string code = Request.QueryString["code"] as string;
            if (!string.IsNullOrEmpty(code))
            {
                WXConfigDAL dal = new WXConfigDAL();
                Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
                if (wxConfig != null)
                {
                    WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
                    {
                        ID = wxConfig.WXID,
                        Name = wxConfig.WXName,
                        Token = wxConfig.WXToken,
                        AppId = wxConfig.WXAppID,
                        AppSecret = wxConfig.WXAppSecret
                    };
                    WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
                    Oauth2AccessToken oauth2AccessToken = weixin.GetOauth2AccessToken(code);
                    if (oauth2AccessToken != null)
                    {
                        strOpenID = oauth2AccessToken.OpenID;
                    }
                    UserInfo userinfo = weixin.GetUserInfo(strOpenID);
                    if (userinfo != null)
                    {
                        regNickName = userinfo.NickName;
                        regSex = userinfo.Sex;
                        regHeadImg = userinfo.Headimgurl;
                    }
                }
                else
                {
                    strOpenID = code;
                }
            }
            if (strOpenID == null || strOpenID == "")
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    MSCustomersDAL CustomerDal = new MSCustomersDAL();
                    try
                    {
                        strOpenID = CustomerDal.GetCustomerValueByID("OpenID", Session["customerID"].ToString()).ToString();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (strOpenID != null && strOpenID != "")
            {
                Session["OpenID"] = strOpenID;
            }
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        void ToUserLogin()
        {
            string logphone = string.Empty; string logpwd = string.Empty;
            if (Request["logphone"] != null && Request["logphone"] != "")
            {
                logphone = Common.Common.NoHtml(Request["logphone"]);
            }
            if (Request["logpwd"] != null && Request["logpwd"] != "")
            {
                logpwd = Common.Common.NoHtml(Request["logpwd"]);
            }
            if (logphone.Trim() != null && logphone.Trim() != "" && logpwd.Trim() != null && logpwd.Trim() != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                MSShopDAL shopDal = new MSShopDAL();
                string usrsid = string.Empty;
                DataSet loginds = null;
                loginds = customerDal.CustomerLogin(strOpenID, logphone, Common.Common.MD5(logpwd));
                if (loginds != null && loginds.Tables.Count > 0 && loginds.Tables[0].Rows.Count > 0)
                {
                    string customerID = loginds.Tables[0].Rows[0]["ID"].ToString();
                    string useropenid = loginds.Tables[0].Rows[0]["OpenID"].ToString();
                    if (useropenid == null || useropenid == "")
                    {
                        if (strOpenID != null && strOpenID != "")
                        {
                            customerDal.UpdateUserOpenID(customerID, strOpenID);
                        }
                    }
                    Session["customerID"] = customerID;
                    Session["OpenID"] = strOpenID;
                    if (customerID != null && customerID != "")
                    {
                        try
                        {
                            usrsid = shopDal.GetSidByUid("ID", customerID).ToString();
                        }
                        catch (Exception)
                        {
                            usrsid = "";
                        }
                    }
                    if (usrsid != null && usrsid != "")
                    {
                        Session["SID"] = usrsid;
                    }
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"非法操作，请核对后再操作\"}");
                }
            }
            Response.End();
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        void ToSaveUserInfo()
        {
            string inputphone = string.Empty; string logpwd = string.Empty;
            if (Request["inputphone"] != null && Request["inputphone"] != "")
            {
                inputphone = Common.Common.NoHtml(Request["inputphone"]);
            }
            if (Request["logpwd"] != null && Request["logpwd"] != "")
            {
                logpwd = Common.Common.NoHtml(Request["logpwd"]);
            }
            if (inputphone.Trim() != null && inputphone.Trim() != "" && logpwd.Trim() != null && logpwd.Trim() != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                if (customerDal.ExistCustomer(strOpenID, inputphone, "", ""))
                {
                    Response.Write("{\"error\":true,\"msg\":\"该用户已经存在\"}");
                }
                else
                {
                    MSCustomers customerModel = new MSCustomers();
                    string strID = string.Empty;
                    strID = Guid.NewGuid().ToString("N").ToUpper(); 
                    customerModel.ID = strID;
                    customerModel.Phone = inputphone;
                    customerModel.OpenID = strOpenID;
                    customerModel.NickName = regNickName;
                    customerModel.Sex = regSex;
                    customerModel.HeadImg = regHeadImg;
                    customerModel.UserPwd = Common.Common.MD5(logpwd);
                    customerModel.IsDel = 0;

                    MSVAcctDetailDAL vacctdetailDal = new MSVAcctDetailDAL();
                    MSVAcctDAL vacctDal = new MSVAcctDAL();

                    if (customerDal.AddCustomers(customerModel))
                    {
                        int vcount = 2;
                        if (!vacctDal.ExistMSVAcct(strID, ""))
                        {
                            MSVAcct vaccModel = new MSVAcct();
                            vaccModel.CustID = strID;
                            vaccModel.SiteCode = "VYIGO";
                            vaccModel.V_Amont = vcount;
                            MSVAcctDetail vaccDetailModel = new MSVAcctDetail();
                            vaccDetailModel.Amount = vcount;
                            vaccDetailModel.ChargeType = "注册";
                            vaccDetailModel.CustID = strID;
                            vaccDetailModel.Ext_Fld1 = "";
                            vaccDetailModel.IsReceive = 1;
                            vaccDetailModel.SiteCode = "VYIGO";
                            //vacctDal.AddMSVAcct(vaccModel);
                            //vacctdetailDal.AddMSVAcctDetail(vaccDetailModel);
                        }
                        Response.Write("{\"success\":true,\"msg\":\"操作成功\"}");              
                    }
                    else
                    {
                        Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
                    }
                }
            }
            Response.End();
        }
        /// <summary>
        /// 获取短信验证
        /// </summary>
        void GetMsg()
        {
            string inputphone = string.Empty; string imgcode = string.Empty; string writemsg = string.Empty;
            string statusCode = string.Empty; string statusMsg = string.Empty;
            if (Request["inputphone"] != null && Request["inputphone"] != "")
            {
                inputphone = Common.Common.NoHtml(Request["inputphone"]);
            }
            if (Request["imgcode"] != null && Request["imgcode"] != "")
            {
                imgcode = Request["imgcode"].ToString();
            }
            if (inputphone.Trim() != null && inputphone.Trim() != "" && imgcode != null && imgcode != "")
            {
                CCPRestSDK api = CCPRestSDK.GetInstance();
                try
                {
                    if (api != null)
                    {

                        Dictionary<string, object> retData =
                            api.SendTemplateSMS(inputphone, "8281", new string[] { imgcode, "5" });
                        try
                        {
                            statusCode = retData["statusCode"].ToString();
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            statusMsg = retData["statusMsg"].ToString();
                        }
                        catch (Exception)
                        {
                        }
                        if (statusCode != null && statusCode != "" && statusCode.ToString().Trim().ToLower() == "000000")
                        {
                            Response.Write("{\"success\":true}");
                        }
                        else
                        {
                            Response.Write("{\"error\":true,\"msg\":\"" + statusMsg + "\"}");
                        }
                    }
                    else
                    {
                        Response.Write("{\"error\":true,\"msg\":\"初始化失败\"}");
                    }
                }
                catch (Exception exc)
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请重新操作\"}");
                }
            }
            Response.End();
        }
        /// <summary>
        /// 验证电话是否注册
        /// </summary>
        void validatephone()
        {
            string valphone = string.Empty;
            if (Request["inputphone"] != null && Request["inputphone"] != "")
            {
                valphone = Common.Common.NoHtml(Request["inputphone"]);
            }
            if (valphone != null && valphone != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                if (action != null && action != "" && action == "wxphone")
                {
                    if (strOpenID != null && strOpenID != "")
                    {
                        if (customerDal.ExistCustomer(strOpenID, "", "", ""))
                        {
                            Response.Write("{\"error\":true,\"msg\":\"该微信已被注册\"}");
                        }
                        else
                        {
                            Response.Write("{\"success\":true}");
                        }
                    }
                    else
                    {
                        Response.Write("{\"success\":true}");
                    }
                }
                else
                {
                    if (customerDal.ExistCustomer("", valphone, "", ""))
                    {
                        Response.Write("{\"error\":true,\"msg\":\"该号码已被注册\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":true}");
                    }
                }
            }
            else
            {
                Response.Write("{\"success\":true}");
            }
            Response.End();
        }

    }
}
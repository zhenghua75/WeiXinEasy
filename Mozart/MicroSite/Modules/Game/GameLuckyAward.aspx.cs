using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Game;
using Model.Game;
using Mozart.Common;
using DAL.WeiXin;
using WeiXinCore.Models;
using DAL.ACT;

namespace Mozart.MicroSite
{
    public partial class GameLuckyAward : System.Web.UI.Page
    {
        string strActID = string.Empty;
        string strSiteCode = string.Empty;
        string strAction = string.Empty;
        string strOpenID = string.Empty;
        LuckyAwardDAL AwardDal = new LuckyAwardDAL();
        LuckyAwardUsersDAL AwardUserDal = new LuckyAwardUsersDAL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //--菜单功能
                if (null == Request["state"] || Request["state"] == "")
                {
                    return;
                }
                else
                {
                    strSiteCode = Common.Common.NoHtml(Request["state"].ToString());
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
                    }
                }

                //--


                if (Request["actid"] == null || Request["actid"] == "")
                {
                    return;
                }
                //strSiteCode = Common.Common.NoHtml(Request["sitecode"]);
                strActID = Common.Common.NoHtml(Request["actid"]);
                //GetOpenID();
                if (Request["action"] != null && Request["action"] != "")
                {
                    strAction = Common.Common.NoHtml(Request["action"]);
                    switch (strAction.ToLower().Trim())
                    {
                        case "saveuser":
                            //Response.Write("{\"success\":true}");
                            saveUserinfo();
                            break;
                        case "setphone":
                            setPhone();
                            break;
                    }
                    Response.End();
                }
                else
                {
                    GetAwardList();
                }
            }
        }
        #region 获取奖项列表
        /// <summary>
        /// 获取奖项列表
        /// </summary>
        void GetAwardList()
        {
            if (strSiteCode.Trim() != null && strSiteCode.Trim() != ""&&
                strActID.Trim()!=null&&strActID.Trim()!="")
            {
                string where = string.Empty;
                where = " and SiteCode='" + strSiteCode + "' and a.ActID='"+strActID+"' ";
                List<LuckyAward> modelList = new List<LuckyAward>();
                //DataSet modellistds = AwardDal.GetAwardList(where);
                DataSet modellistds = AwardDal.GetActAwardList(strActID,0);
                foreach (DataRow row in modellistds.Tables[0].Rows)
                {
                    LuckyAward Awardmodel = DataConvert.DataRowToModel<LuckyAward>(row);
                    modelList.Add(Awardmodel);
                }
                int awardcount =modellistds.Tables[0].Rows.Count;
                string awardpre = string.Empty;
                //decimal awardpervalue =100/Convert.ToDecimal(awardcount);
                //if (awardpervalue.ToString().Contains("."))
                //{
                //    awardpre = awardpervalue.ToString().Substring(0, awardpervalue.ToString().IndexOf(".") + 3);
                //}
                //else
                //{
                //    awardpre = awardpervalue.ToString();
                //}

                //读取模板内容 
                string text = string.Empty;
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Game/award.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["title"] = "抽奖活动详细信息";
                context.TempData["awardmodellist"] = modelList;
                context.TempData["sitecode"] = strSiteCode;
                context.TempData["openid"] = strOpenID;
                context.TempData["awardcount"] = awardcount;
                //context.TempData["awardper"] = awardpre+ "%";
                context.TempData["footer"] = "奥琦微商易";
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
        #endregion
        #region 获取OpenID
        /// <summary>
        /// 获取OpenID
        /// </summary>
        void GetOpenID()
        {
            //if (!string.IsNullOrEmpty(strSiteCode))
            //{
            //    WXConfigDAL dal = new WXConfigDAL();
            //    Model.WeiXin.WXConfig wxConfig = dal.GetWXConfigBySiteCode(strSiteCode);
            //    if (wxConfig != null)
            //    {
            //        WeiXinCore.Models.WeiXinConfig weixinConfig = new WeiXinCore.Models.WeiXinConfig()
            //        {
            //            ID = wxConfig.WXID,
            //            Name = wxConfig.WXName,
            //            Token = wxConfig.WXToken,
            //            AppId = wxConfig.WXAppID,
            //            AppSecret = wxConfig.WXAppSecret
            //        };
            //        WeiXinCore.WeiXin weixin = new WeiXinCore.WeiXin(weixinConfig);
            //        Oauth2AccessToken oauth2AccessToken = weixin.GetOauth2AccessToken(strSiteCode);
            //        if (oauth2AccessToken != null)
            //        {
            //            strOpenID = oauth2AccessToken.OpenID;
            //        }
            //    }
            //}
            if (Request["openid"] != null && Request["openid"] != "")
            {
                strOpenID = Request["openid"].ToString();
            }
        }
        #endregion
        #region 用户注册
        /// <summary>
        /// 用户注册
        /// </summary>
        void saveUserinfo()
        {
            if (strOpenID != null && strOpenID != "")
            {
                if (!AwardUserDal.ExistAwardUser(strOpenID, "", "", "",""))
                {
                    LuckyAwardUsers usermodel = new LuckyAwardUsers();
                    usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                    usermodel.IsDel = 0;
                    usermodel.OpenID = strOpenID;
                    usermodel.SendAward = 0;
                    usermodel.ActID = strActID;

                    if (AwardUserDal.AddAwardUsers(usermodel))
                    {
                        Random ran = new Random();
                        int RandKey = ran.Next(0, 6);
                        string strGuid = Guid.NewGuid().ToString("N");
                        string strSN = GenerateCheckCode(7);
                        string strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D04";
                        switch (RandKey)
                        {
                            case 1:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D01";
                                break;
                            case 2:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D02";
                                break;
                            case 3:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D03";
                                break;
                            case 4:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D04";
                                break;
                            case 5:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D05";
                                break;
                            case 6:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D06";
                                break;
                            default:
                                strCouActID = "AC87CE288FC24F23B7B68CE8F3D93D04";
                                break;
                        }

                        CouponDAL cdal = new CouponDAL();
                        Model.ACT.Coupon coupon = null;
                        coupon = new Model.ACT.Coupon()
                        {
                            ID = strGuid,
                            SiteCode = strSiteCode,
                            SiteActivityID = strCouActID,
                            OpenID = strOpenID,
                            CouponCode = strSN, 
                            CouponStatus = 0
                        };

                        cdal.InsertInfo(coupon);

                        Response.Write("{\"success\":\"true\",\"prizetype\":\"" + RandKey.ToString() + "\",\"sn\":\"a" + strSN + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"error\":\"invalid\"}");
                    }
                }
                else
                {
                    //Response.Write("{\"prizetype\":\"null\",\"sn\":\"no\"}");
                    Response.Write("{\"error\":\"isdoing\"}");
                }
            }
            else
            {
                Response.Write("{\"prizetype\":\"null\",\"sn\":\"no\"}");
            }
        }
        #endregion
        void setPhone()
        {
            string SNCode = string.Empty;
            string Phone = string.Empty;
            if (Request["sncode"] != null && Request["sncode"] != "")
            {
                SNCode = Common.Common.NoHtml(Request["sncode"]);
            }
            if (Request["phone"] != null && Request["phone"] != "")
            {
                Phone = Common.Common.NoHtml(Request["phone"]);
            }
            //Response.Write("{\"success\":true}");
            if (strOpenID != null && strOpenID != "")
            {
                LuckyAwardUsers usermodel = new LuckyAwardUsers();
                
            }
            else
            {
                Response.Write("{\"error\":\"操作失败，请重新操作\"}");
            }
        }

        #region 生成随机位数的数字串
        /// <summary>
        /// 生成随机校验码字符串
        /// </summary>
        /// <param name="iAmount">生成数字串的位数</param>
        /// <returns>生成的随机校验码字符串</returns>
        public string GenerateCheckCode(int iAmount)
        {
            int number;
            string strCode = string.Empty;

            //随机数种子
            Random random = new Random(GetRandomSeed());

            for (int i = 0; i < iAmount; i++) //校验码长度为4
            {
                //随机的整数
                number = random.Next();

                ////字符从0-9,A-Z中随机产生,对应的ASCII码分别为
                ////48-57,65-90
                //number = number % 36;
                //if (number < 10)
                //{
                //    number += 48;
                //}
                //else
                //{
                //    number += 55;
                //}
                //字符从0-9,A-Z中随机产生,对应的ASCII码分别为
                number = number % 10 + 48;
                strCode += ((char)number).ToString();
            }
            return strCode;
        }

        public int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }
        #endregion

        void GetPrizeProbability()
        {
            Random rd = new Random();
            int probability = rd.Next(100);
        }
    }
}
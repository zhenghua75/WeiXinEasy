using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.ACT;
using DAL.Game;
using Model.ACT;
using Model.Game;
using Mozart.Common;
using WeiXinCore.Models;
using DAL.WeiXin;

namespace Mozart.MicroSite
{
    public partial class GameScratchCard : System.Web.UI.Page
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
                strOpenID = "abc";
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

                if (Request["actid"] == null || Request["actid"] == "")
                {
                    return;
                }
                strActID = Common.Common.NoHtml(Request["actid"]);
                if (Request["action"] != null && Request["action"] != "")
                {
                    strAction = Common.Common.NoHtml(Request["action"]);
                    switch (strAction.ToLower().Trim())
                    {
                        case "saveuser":
                            SaveUser();
                            break;
                        case "getsn":
                            GetSNcode();
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
            if (strSiteCode.Trim() != null && strSiteCode.Trim() != "" &&
                strActID.Trim() != null && strActID.Trim() != "")
            {
                List<LuckyAward> modelList = new List<LuckyAward>();
                DataSet modellistds = AwardDal.GetActAwardList(strActID, 0);
                foreach (DataRow row in modellistds.Tables[0].Rows)
                {
                    LuckyAward Awardmodel = DataConvert.DataRowToModel<LuckyAward>(row);
                    modelList.Add(Awardmodel);
                }

                int awardcount = modellistds.Tables[0].Rows.Count;
                string awardtitle = string.Empty;
                string script = string.Empty;
                string peize=string.Empty;
                string peizeid=string.Empty;
                int peizenum = 0; int frontpeizenum = 0; int beforpeizenum = 0;
                for (int i = 0; i < awardcount; i++)
                {
                    try
                    {
                        peizenum = Convert.ToInt32(modellistds.Tables[0].Rows[i]["AwardPro"].ToString());
                    }
                    catch (Exception)
                    {
                    }
                    peize=modellistds.Tables[0].Rows[i]["Award"].ToString();
                    peizeid=modellistds.Tables[0].Rows[i]["ID"].ToString();
                    if (i == 0)
                    {
                        script += "\r\n                              if(randNum>=1&&randNum<=" + peizenum + ")" +
                            "\r\n                              {prize=\"" + peize + "\";zjl = true;peizeid=\"" + peizeid + "\";}";
                    }
                    else
                    {
                        peizenum = beforpeizenum + peizenum;
                        script += "\r\n                              if(randNum>=" + frontpeizenum + "&&randNum<=" + peizenum + ")" +
                            "\r\n                              {prize=\"" + peize + "\";zjl = true;peizeid=\"" + peizeid + "\";}";
                    }
                    frontpeizenum = peizenum + 1;
                    beforpeizenum = peizenum;
                }

                //读取模板内容 
                string text = string.Empty;
                text = System.IO.File.ReadAllText(Server.MapPath("Themes/Game/scratchcard.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
                context.TempData["title"] = "刮刮卡活动详细信息";
                context.TempData["awardmodellist"] = modelList;
                context.TempData["sitecode"] = strSiteCode;
                context.TempData["openid"] = strOpenID;
                context.TempData["awardcount"] = awardcount;
                context.TempData["actid"] = strActID;
                context.TempData["script"] = script;
                context.TempData["footer"] = "奥琦微商易";
                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
        }
        #endregion
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
        #region 添加用户
        /// <summary>
        /// 添加用户信息
        /// </summary>
        void SaveUser()
        {
            string sncode = string.Empty;
            if (strOpenID.Trim() != null && strOpenID.Trim() != "")
            {
                if (!AwardUserDal.ExistAwardUser(strOpenID, "", "","",strActID))
                {
                    LuckyAwardUsers usermodel = new LuckyAwardUsers();
                    usermodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                    usermodel.IsDel = 0;
                    usermodel.OpenID = strOpenID;
                    usermodel.SendAward = 0;
                    usermodel.ActID = strActID;
                    if (AwardUserDal.AddAwardUsers(usermodel))
                    {
                        Response.Write("{\"success\":true}");
                    }
                    else
                    {
                        Response.Write("{\"error\":\"invalid\"}");
                    }
                }
                else
                {
                    sncode = AwardUserDal.GetSNCodeByPhone(strOpenID, "", "", strActID);
                    if (sncode.Trim() != null && sncode.Trim() != "")
                    {
                        Response.Write("{\"error\":\"getsn\",\"sn\":\"" + sncode + "\"}");
                    }
                    else
                    {
                        //Response.Write("{\"error\":\"isdoing\"}");
                        Response.Write("{\"success\":true}");
                    }
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请重新操作\"}"); return;
            }
        }
        #endregion
        #region 获取兑奖SN码
        /// <summary>
        /// 获取兑奖SN码
        /// </summary>
        void GetSNcode()
        {
            string sncode = string.Empty;
            string peizeid = string.Empty;
            if (Request["peizeid"] != null && Request["peizeid"] != "")
            {
                peizeid = Common.Common.NoHtml(Request["peizeid"].ToString());
            }
            if (strOpenID.Trim() != null && strOpenID.Trim() != "")
            {
                if (!AwardUserDal.ExistAwardUser(strOpenID, "", "", peizeid, strActID))
                {
                    sncode = GenerateCheckCode(7);
                    if (AwardUserDal.UpdateUserSNCode(strOpenID,peizeid,strActID,sncode))
                    {
                        string strGuid = Guid.NewGuid().ToString("N");
                        CouponDAL cdal = new CouponDAL();
                        Model.ACT.Coupon coupon = null;
                        coupon = new Model.ACT.Coupon()
                        {
                            ID = strGuid,
                            SiteCode = strSiteCode,
                            SiteActivityID = strActID,
                            OpenID = strOpenID,
                            CouponCode = sncode,
                            CouponStatus = 0
                        };
                        cdal.InsertInfo(coupon);
                        Response.Write("{\"success\":true,\"sn\":\""+sncode+"\"}");
                    }
                    else
                    {
                        Response.Write("{\"error\":\"invalid\"}");
                    }
                }
                else
                {
                    sncode = AwardUserDal.GetSNCodeByPhone(strOpenID, "", "", strActID);
                    if (sncode.Trim() != null && sncode.Trim() != "")
                    {
                        Response.Write("{\"error\":\"getsn\",\"sn\":\"" + sncode + "\"}");
                    }
                    else
                    {
                        Response.Write("{\"error\":\"isdoing\"}");
                    }
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请重新操作\"}"); return;
            }
        }
        #endregion
    }
}
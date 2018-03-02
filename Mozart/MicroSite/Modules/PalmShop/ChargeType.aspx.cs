using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ChargeType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strChargeType = string.Empty;
            string strChargeNo = string.Empty;
            string strChargeAmount = string.Empty;
            string strOpenID = string.Empty;
            string strCelWhere = string.Empty;

            try
            {
                if (null == Request.QueryString["openid"])
                {
                    if (Session["OpenID"] != null && Session["OpenID"].ToString() != "")
                    {
                        strOpenID = Session["strOpenID"].ToString();
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
                    {
                        return;
                    }
                    else
                    {
                        strOpenID = Request.QueryString["openid"].ToString();
                    }
                }


                if (null == Request.QueryString["type"])
                {
                    return;
                }
                else
                {
                    switch (Request.QueryString["type"].ToString())
                    {
                        case "0":
                            strChargeType = "话费充值";
                            break;
                        case "1":
                            strChargeType = "Q币充值";
                            break;
                        default:
                            strChargeType = "话费充值";
                            break;
                    }
                }

                strChargeNo = Common.Common.NoHtml(Request.Form["ChargeNumber"].ToString());
                if (string.IsNullOrEmpty(strChargeNo.Trim()) || strChargeNo == "")
                {
                    Response.Write("<script>alert('充值号码不能为空！');history.back();</script>");
                    return;
                }

                strChargeAmount = Common.Common.NoHtml(Request.Form["PayFee"].ToString());

                string strOtherPay = "0";

                if (strChargeType == "话费充值")
                {
                    strOtherPay = Common.Common.NoHtml(Request.Form["other_price"].Trim());

                    if (!string.IsNullOrWhiteSpace(strOtherPay))
                    {
                        strChargeAmount = "其它";
                    }

                    if (strChargeAmount == "其它")
                    {
                        strChargeAmount = Common.Common.NoHtml(Request.Form["other_price"].ToString());
                    }
                }
                else
                {
                    strOtherPay = Common.Common.NoHtml(Request.Form["other_QQprice"].Trim());

                    if (!string.IsNullOrWhiteSpace(strOtherPay))
                    {
                        strChargeAmount = "其它";
                    }

                    if (strChargeAmount == "其它")
                    {
                        strChargeAmount = Common.Common.NoHtml(Request.Form["other_QQprice"].ToString());
                    }
                }

                if (string.IsNullOrEmpty(strChargeAmount.Trim()) || strChargeNo == "")
                {
                    Response.Write("<script>alert('充值金额不能为空！');history.back();</script>");
                    return;
                }
                if (strChargeType == "话费充值")
                {
                    string strYNLocal = "0691,0692,0870,0871,0872,0873,0874,0875,0876,0877,0878,0879,0883,0886,0887,0888";
                    string strMobieNO = "134、135、136、137、138、139、147、150、151、152、157、158、159、182、183、184、187、188";
                    string strMobieCUNO = "130、131、132、145、155、156、185、186";
                    string strMobieCTNO = "133、153、189、181、180";
                    strCelWhere = "CMCC";


                    if (strYNLocal.IndexOf(strChargeNo.Substring(0, 4)) > -1)
                    {
                        strCelWhere = "CT";
                    }
                    else
                    {
                        if (!IsMobilePhone(strChargeNo))
                        {
                            Response.Write("<script>alert('请输入正确的缴费号码！');history.back();</script>");
                            return;
                        }

                        if (strMobieNO.IndexOf(strChargeNo.Substring(0, 3)) > -1)
                        {
                            strCelWhere = "CMCC";
                            DAL.PublicService.aq_CeleWhere dal = new DAL.PublicService.aq_CeleWhere();
                            string strLocal = dal.GetLocalStateName(strChargeNo);
                            if (strLocal != "昆明")
                            {
                                Response.Write("<script>alert('移动只能缴昆明本地电话费！');history.back();</script>");
                                return;
                            }
                        }

                        if (strMobieCUNO.IndexOf(strChargeNo.Substring(0, 3)) > -1)
                        {
                            strCelWhere = "CU";
                        }

                        if (strMobieCTNO.IndexOf(strChargeNo.Substring(0, 3)) > -1 || strChargeNo.Substring(0, 1) != "1")
                        {
                            strCelWhere = "CT";
                        }

                        if (strCelWhere != "CMCC")
                        {
                            if (int.Parse(strChargeAmount.Trim()) < 10)
                            {
                                Response.Write("<script>alert('非移动缴费金额不能小于10元！');history.back();</script>");
                                return;
                            }

                            DAL.PublicService.aq_CeleWhere dal = new DAL.PublicService.aq_CeleWhere();
                            string strLocal = dal.GetLocalProvinceName(strChargeNo);
                            if (strLocal != "云南")
                            {
                                Response.Write("<script>alert('联通电信只能缴云南省电话费！');history.back();</script>");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    if (!IsInteger(strChargeNo))
                    {
                        Response.Write("<script>alert('请输入正确的缴费号码！');history.back();</script>");
                        return;
                    }
                }

                if (strChargeAmount == "其它")
                {
                    strChargeAmount = Common.Common.NoHtml(Request.Form["other_price"].ToString());
                }

                string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ChargeType.html"));
                JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

                context.TempData["ChargeType"] = strChargeType;
                context.TempData["ChargeNo"] = strChargeNo;
                context.TempData["ChargeAmount"] = strChargeAmount;
                context.TempData["CelWhere"] = strCelWhere;
                context.TempData["OpenID"] = strOpenID;

                JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
                t.Render(Response.Output);
            }
            catch (Exception ex)
            {
                Model.SYS.ExceptionLog log = new Model.SYS.ExceptionLog();
                log.Message = string.Format("Q币充值出错：" + ex.Message);
                ExceptionLogDAL.InsertExceptionLog(log);
            }
        }

        #region 判断充值号码是否有效 

        /// 名称：IsMobilePhone
        /// 功能：判断输入的是否为手机号码
        /// </summary>
        /// <param name="input">传入一个字符串</param>
        /// <returns>bool值</returns>
        public static bool IsMobilePhone(string input)
        {
            Regex regex = new Regex("^1\\d{10}$");
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 

        /// 名称：IsKmTelephone
        /// 功能：判断输入的是否为昆明固定号码
        /// </summary>
        /// <param name="input">传入一个字符串</param>
        /// <returns>bool值</returns>
        public static bool IsKmTelephone(string input)
        {
            Regex regex = new Regex("^(0871)?([2-9][0-9]{6,7})$");
            return regex.IsMatch(input);
        }
        /// <summary>
        /// 

        /// <summary>
        /// 判断一个字符串是否为合法整数(不限制长度)
        /// </summary>
        /// <param name="s">字符串</param>
        /// <returns></returns>
        public static bool IsInteger(string s)
        {
            string pattern = @"^\d*$";
            return Regex.IsMatch(s, pattern);
        }

        #endregion
    }
}
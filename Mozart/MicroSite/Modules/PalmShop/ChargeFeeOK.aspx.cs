using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.MiniShop;
using Mozart.Payment;
using Mozart.Payment.wxpay;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ChargeFeeOK : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string strOpenID = string.Empty;
            string strIP = string.Empty;
            string strChargeType = string.Empty;
            string strCustNo = string.Empty;
            string strChargeAmount = string.Empty;
            string strCelWhere = string.Empty;
            string strDiscount = string.Empty;

            //ChargeFeeOK.aspx?type=$chargetype&openid=$OpenID&chargeno=$ChargeNo&chargeamount=$ChargeAmount
            if (null == Request.QueryString["type"])
            {
                return;
            }

            if (null == Request.QueryString["openid"])
            {
                return;
            }

            if (null == Request.QueryString["chargeno"])
            {
                return;
            }

            if (null == Request.QueryString["chargeamount"])
            {
                return;
            }
            if (null == Request.QueryString["celWhere"])
            {
                return;
            }

            if (Request.QueryString["openid"].ToString().Length > 29 || Request.QueryString["openid"].ToString().Length < 25)
            {
                strOpenID = Common.Common.NoHtml(Request.QueryString["openid"].ToString());
            }
            else
            {
                strOpenID = Request.QueryString["openid"].ToString();
            }

            strIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            strChargeType = Common.Common.NoHtml(Request.QueryString["type"].ToString());
            strCustNo = Common.Common.NoHtml(Request.QueryString["chargeno"].ToString());
            strChargeAmount = Common.Common.NoHtml(Request.QueryString["chargeamount"].ToString());
            strCelWhere = Common.Common.NoHtml(Request.QueryString["celWhere"].ToString());
            
            //插入订单数据

            //调用支付接口返回成功写数据到缴费平台
            //DAL.MiniShop.MSChargeFeeOrder dal = new DAL.MiniShop.MSChargeFeeOrder();
            //Model.MiniShop.MSChargeFeeOrder model = new Model.MiniShop.MSChargeFeeOrder();

            //model.ID = Guid.NewGuid().ToString("N").ToUpper();
            //model.OpenID = strOpenID;
            //model.CustNo = strCustNo;
            //model.ChargeAmount = int.Parse(strChargeAmount + "000");
            //model.ChargeIP = strIP;
            //model.State = 0;
            DAL.SYS.SYSDictionaryDAL dicDal = new DAL.SYS.SYSDictionaryDAL();
            DAL.MiniShop.MSShoppingCartDAL dal = new MSShoppingCartDAL();
            string strPID = "VH20141104210211";
            string strCType = "话费充值";
            if (strChargeType == "Q币充值")
            {
                strCType = "qbcz";
                strPID = "VQ20141104210212";
                strDiscount = dicDal.GetDictionaryValues("WXCZ_QB");
            }
            else
            {
                strCType = "hfcz";
                strPID = "VH20141104210211";
                switch (strCelWhere)
                {
                    case "CMCC":
                        strDiscount = dicDal.GetDictionaryValues("WXCZ_YD");
                        break;
                    case "CT":
                        strDiscount = dicDal.GetDictionaryValues("WXCZ_DX");
                        break;
                    case "CU":
                        strDiscount = dicDal.GetDictionaryValues("WXCZ_LT");
                        break;
                    default:
                        strDiscount = "100";
                        break;
                }
            }

            string strOrderID = dal.SubVirtualProductOrder(strOpenID, strCustNo, strPID, strChargeAmount);

            if (!string.IsNullOrEmpty(strOrderID))
            {                
                WXJSAPIPay wxpay = new WXJSAPIPay("VYIGO");
                wxpay.DirectWXJSAPIPay(this.Response, strCType, strOrderID, int.Parse(strChargeAmount + "00") * int.Parse(strDiscount) / 100, strIP, strOpenID, "http://www.vgo2013.com/PalmShop/ShopCode/ChargeFeeOver.aspx", null, strCType + "|" + strCustNo + "|" + strChargeAmount);
            }
            else
            {
 
            }
        }
    }
}
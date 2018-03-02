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
using Mozart.Payment.Alipay.WapPay;
using System.Text;
using WeiXinCore.Models;

namespace Mozart.PalmShop.ShopCode
{
    public partial class setOrder : System.Web.UI.Page
    {
        static string strpid = string.Empty;
        static string action = string.Empty;
        static string customid = string.Empty;
        static string quantity = string.Empty;
        static string mid = string.Empty;
        public static string errorscript = string.Empty;
        string openid = string.Empty;
        string strSiteCode = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["mid"] != null && Request["mid"] != "")
                {
                    mid = Common.Common.NoHtml(Request["mid"]);
                }
                if (Request["pid"] != null && Request["pid"] != "" && Request["num"] != null && Request["num"] != "")
                {
                    errorscript = "";
                    strpid = Common.Common.NoHtml(Request["pid"]);
                    quantity = Common.Common.NoHtml(Request["num"]);
                    if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                    {
                        customid = Session["customerID"].ToString();
                        GetOpenId();
                        if (Request["action"] != null && Request["action"] != "")
                        {
                            action = Request["action"];
                            action=action.ToLower().Trim();
                            switch (action)
                            {
                                case"setorder":
                                    setAddOrder(); return;
                                case "setda":
                                    setUserAddress(); return;
                                case"updalist":
                                    setUserAddress();
                                    return;
                                case"getdalist":
                                    GetUserAddress();
                                    return;
                            }
                        }
                    }
                    else
                    {
                        setCookies();
                        errorscript = JQDialog.alertOKMsgBox(3, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                    }
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(3, "操作失败<br/>您暂时没有权限访问！", true);
                }
                GetInfo();
            }
        }
        void GetOpenId()
        {
            if (customid != null && customid != "")
            {
                MSCustomersDAL CustomerDal = new MSCustomersDAL();
                openid = CustomerDal.GetCustomerValueByID("OpenID", customid).ToString();
                Session["OpenID"] = openid;
            }
            else
            {
                openid = "";
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        void setCookies()
        {
            HttpCookie delcookies = new HttpCookie("pageurl");
            delcookies.Expires = DateTime.Now.AddDays(-1);
            Response.AppendCookie(delcookies);
            HttpCookie cookies = new HttpCookie("pageurl","setOrder.aspx?pid="+strpid+"&num="+quantity+"&mid="+mid);
            cookies.Expires = DateTime.Now.AddMinutes(3);
            Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 获取购物信息
        /// </summary>
        void GetInfo()
        {
            #region 产品详细
            MSProductDAL productDal = new MSProductDAL();
            MSProduct productModel = new MSProduct();
            DataSet productds = productDal.GetProductDetail(strpid);
            MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
            MSProductAtlas atlasdetailmodel=null;
            DataSet atlasDs = null;
            if (null != productds && productds.Tables.Count > 0 && productds.Tables[0].Rows.Count > 0)
            {
                productModel = DataConvert.DataRowToModel<MSProduct>(productds.Tables[0].Rows[0]);
            }
            #endregion
            #region 获取产品默认展示图
            atlasDs = atlasDal.GetDefaultAtlasByPid(strpid);
            foreach (DataRow atlasrow in atlasDs.Tables[0].Rows)
            {
                 atlasdetailmodel = DataConvert.DataRowToModel<MSProductAtlas>(atlasrow);
            }
            #endregion
            #region -----------获取型号尺码----------------
            MSProductPara paraModel = new MSProductPara();
            MSProductParaDAL paraDal = new MSProductParaDAL();
            DataSet parads = paraDal.GetParaDetail(mid);
            if (parads != null && parads.Tables.Count > 0 && parads.Tables[0].Rows.Count > 0)
            {
                paraModel = DataConvert.DataRowToModel<MSProductPara>(parads.Tables[0].Rows[0]);
            }
            #endregion
            #region 获取收货地址
            MSDeliveryAddressDAL addressDal = new MSDeliveryAddressDAL();
            MSDeliveryAddress defaultadModel = new MSDeliveryAddress();
            List<MSDeliveryAddress> damodellist = new List<MSDeliveryAddress>();
            string stradwhere = string.Empty;
            stradwhere = "and [UID]='" + customid + "' ";
            DataSet addressds = addressDal.GetDAList(3, stradwhere);
            if (null != addressds && addressds.Tables.Count > 0 && addressds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in addressds.Tables[0].Rows)
                {
                    MSDeliveryAddress damodel = DataConvert.DataRowToModel<MSDeliveryAddress>(row);
                    damodellist.Add(damodel);
                }
            }
            #endregion
            string customerphone = string.Empty;
            MSCustomers customerModel = new MSCustomers();
            if (customid != null && customid != "")
            {
                MSCustomersDAL CustomerDal = new MSCustomersDAL();
                DataSet customerds = CustomerDal.GetCustomerDetail(customid);
                if (customerds != null && customerds.Tables.Count > 0 && customerds.Tables[0].Rows.Count > 0)
                {
                    customerModel = DataConvert.DataRowToModel<MSCustomers>(customerds.Tables[0].Rows[0]);
                }
            }

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/setOrder.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["productdetail"] = productModel;
            context.TempData["atlas"] = atlasdetailmodel;
            context.TempData["paramodel"] = paraModel;
            context.TempData["errorscript"] = errorscript;
            context.TempData["dalist"] = damodellist;
            context.TempData["customer"] = customerModel;
            context.TempData["openid"] = openid;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 把购物车添加到订单
        /// </summary>
        void setAddOrder()
        {
            string uname = string.Empty;string uphone = string.Empty;string address = string.Empty;
            string payway = string.Empty; string carryway = string.Empty; string countcost = string.Empty;
            string leavemsg = string.Empty; string zipcode = string.Empty; string num = string.Empty;
            #region --------------获取请求信息---------------
            if (Request["num"] != null && Request["num"] != "")
            {
                num =Request["num"];
            }
            if (Request["uname"] != null && Request["uname"] != "")
            {
                uname = Common.Common.NoHtml(Request["uname"]);
            }
            if (Request["uphone"] != null && Request["uphone"] != "")
            {
                uphone = Common.Common.NoHtml(Request["uphone"]);
            }
            if (Request["address"] != null && Request["address"] != "")
            {
                address = Request["address"];
            }
            if (Request["payway"] != null && Request["payway"] != "")
            {
                payway = Common.Common.NoHtml(Request["payway"]);
            }
            if (Request["carryway"] != null && Request["carryway"] != "")
            {
                carryway = Common.Common.NoHtml(Request["carryway"]);
            }
            if (Request["countcost"] != null && Request["countcost"] != "")
            {
                countcost = Common.Common.NoHtml(Request["countcost"]);
            }
            if (Request["leavemsg"] != null && Request["leavemsg"] != "")
            {
                leavemsg = Common.Common.NoHtml(Request["leavemsg"]);
            }
            if (Request["zipcode"] != null && Request["zipcode"] != "")
            {
                zipcode = Common.Common.NoHtml(Request["zipcode"]);
            }
            #endregion
            if (customid.Trim() != null && customid.Trim() != "")
            {
                MSShoppingCartDAL cartDal = new MSShoppingCartDAL();
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                if (OrderDal.ExistOrderByUID(customid, strpid))
                {
                    Response.Write("{\"error\":true,\"exist\":true}");
                }
                else
                {
                    try
                    {
                        string strOrdernum = cartDal.SubOrder(customid, uname, uphone, leavemsg,
                            address, zipcode, strpid, countcost, num, mid);
                        if (strOrdernum != null && strOrdernum != "")
                        {
                            strSiteCode = "VYIGO";
                            MSOrderLogDAL.AddMSOrderLog("客户【" + customid + "】下单成功，订单号为【" + strOrdernum + "】");
                            MSProductDAL productDal = new MSProductDAL();
                            string pnam = string.Empty;
                            if (strpid != null && strpid != "")
                            {
                                pnam = productDal.GetMSProductVaueByID("Ptitle", strpid).ToString();
                            }

                            #region-发消息到店铺注册用户
                            string p_uid = string.Empty; string p_openid = string.Empty;
                            if (strpid != null && strpid != "")
                            {
                                try
                                {
                                    p_uid = productDal.GetMSProductVaueByID("CustomerID", strpid).ToString();
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if (p_uid != null && p_uid != "")
                            {
                                MSCustomersDAL P_UDal = new MSCustomersDAL();
                                try
                                {
                                    p_openid = P_UDal.GetCustomerValueByID("OpenID", p_uid).ToString();
                                }
                                catch (Exception)
                                {
                                }
                            }
                            if (p_openid != null && p_openid != "" && strSiteCode != null && strSiteCode!="")
                            {
                                JQDialog.SendWeiXinMsg(strSiteCode, p_openid,
                                           "亲爱的店长大人，您的宝贝【" + pnam + "】已于" +
                                           DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss") +
                                           "被" + uname + "成功买下，单号【" + strOrdernum + "】赶紧去小店看看吧!");
                            }
                            MSOrderLogDAL.AddMSOrderLog("下单成功，通知到OpenID为【" + p_openid + "】的店长");
                            #endregion

                            #region-产品属性
                            MSProductParaDAL paraDal = new MSProductParaDAL();
                            int stock = 0;
                            if (paraDal.ExistMSPPara("", strpid) && mid != null && mid != "")
                            {
                                try
                                {
                                    stock = int.Parse(paraDal.GetMSPParaValueByID("Stock", mid).ToString());
                                }
                                catch (Exception)
                                {
                                }
                                if (stock > 0)
                                {
                                    int buynum = 0;
                                    try
                                    {
                                        buynum = Convert.ToInt32(quantity);
                                    }
                                    catch (Exception)
                                    {
                                    }
                                    if (buynum != 0 && buynum < stock)
                                    {
                                        stock = stock - buynum;
                                        paraDal.UpdateStock(stock, mid);
                                    }
                                }
                            }
                            #endregion
                            DataSet orderds = cartDal.GetMyOrderDetail(strOrdernum);
                            string payinfo = string.Empty;
                            string productname = string.Empty;//用户购买的商品名称
                            string ordernum = string.Empty;//订单号
                            if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
                            {
                                productname = orderds.Tables[0].Rows[0]["Ptitle"].ToString();
                                ordernum = orderds.Tables[0].Rows[0]["ID"].ToString();
                                //payinfo= WapPayHelper.BuildRequest(productname, ordernum, countcost, customid);
                            }
                            if (productname.Trim() != null && productname.Trim() != "" && ordernum != null && ordernum != "")
                            {
                                //Response.Write("{\"success\":true,\"payinfo\":\"" + payinfo + "\"}");
                                Response.Write("{\"success\":true,\"pname\":\"" + productname +
                                    "\",\"ordernum\":\"" + ordernum + "\",\"countcost\":\"" + countcost +
                                    "\",\"customid\":\"" + customid + "\",\"openid\":\"" + openid + "\"}");
                            }
                        }
                        else
                        {
                            Response.Write("{\"error\":true,\"msg\":\"操作失败，重新操作\"}");
                        }
                    }
                    catch (Exception)
                    {
                        Response.Write("{\"error\":true,\"msg\":\"操作失败，重新操作\"}");
                    }
                }
            }
            else
            {
                setCookies();
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请登录后再操作\",\"loginurl\":\"UserLogin.aspx\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 设置收货地址
        /// </summary>
        void setUserAddress()
        {
            string uname = string.Empty; string uphone = string.Empty; string address = string.Empty;
            string zipcode = string.Empty; string dadetail = string.Empty; string updaid = string.Empty;
            #region-------------------获取请求信息----------------------
            if (Request["uname"] != null && Request["uname"] != "")
            {
                uname = Common.Common.NoHtml(Request["uname"]);
            }
            if (Request["uphone"] != null && Request["uphone"] != "")
            {
                uphone = Common.Common.NoHtml(Request["uphone"]);
            }
            if (Request["address"] != null && Request["address"] != "")
            {
                address = Request["address"];
            }
            if (Request["dadetail"] != null && Request["dadetail"] != "")
            {
                dadetail = Common.Common.NoHtml(Request["dadetail"]);
            }
            if (Request["zipcode"] != null && Request["zipcode"] != "")
            {
                zipcode = Common.Common.NoHtml(Request["zipcode"]);
            }
            if (Request["daid"] != null && Request["daid"] != "")
            {
                updaid = Common.Common.NoHtml(Request["daid"]);
            }
            #endregion
            MSDeliveryAddress daModel = new MSDeliveryAddress();
            MSDeliveryAddressDAL daDal = new MSDeliveryAddressDAL();
            #region ---------------设置Model属性---------------------------
            if (uname != null && uname != "")
            {
                daModel.DaName = uname;
            }
            if (uphone != null && uphone != "")
            {
                daModel.DaPhone = uphone;
            }
            if (address != null && address != "")
            {
                daModel.DaAddress = address;
            }
            if (dadetail != null && dadetail != "")
            {
                daModel.AddressDetail = dadetail;
            }
            if (zipcode != null && zipcode != "")
            {
                daModel.DaZipCode = zipcode;
            }
            daModel.UID = customid;
            #endregion
            if (action != null && action != "" && action.Trim().ToLower() == "updalist" &&
                updaid != null && updaid != "")
            {
                daModel.ID = updaid;
                if (daDal.UpdateDA(daModel))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true}");
                }
            }
            else
            {
                daModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                if (daDal.AddDA(daModel))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true}");
                }
            }
            Response.End();
        }
        /// <summary>
        /// 获取地址详细
        /// </summary>
        void GetUserAddress()
        {
            string daid = string.Empty;
            if (Request["daid"] != null && Request["daid"] != "")
            {
                daid = Common.Common.NoHtml(Request["daid"]);
            }
            if (daid != null && daid != "")
            {
                MSDeliveryAddressDAL adressDal = new MSDeliveryAddressDAL();
                DataSet ds = new DataSet();
                ds = adressDal.GetDADetail(daid);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Response.Write(Dataset2Json(ds));
                }
                else
                {
                    Response.Write("{\"error\":true}");
                }
            }
            else
            {
                Response.Write("{\"error\":true}");
            }
            Response.End();
        }
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTable2Json(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }
    }
}
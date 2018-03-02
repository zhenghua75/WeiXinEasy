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
    public partial class Product_detail : System.Web.UI.Page
    {
        public string strpid = string.Empty;
        string action = string.Empty;
        static string customid = string.Empty;
        public static string errorscript = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customid = Session["customerID"].ToString();
                }
                if (Request["pid"] != null && Request["pid"] != "")
                {
                    strpid = Common.Common.NoHtml(Request["pid"]);
                    errorscript = "";
                    if (Request["action"] != null && Request["action"] != "")
                    {
                        action = Common.Common.NoHtml(Request["action"]);
                        if (action.ToLower().Trim() == "setcart")
                        {
                            if (customid != null && customid.ToString() != "")
                            {
                                setAddCart();
                            }
                            else
                            {
                                setCookies();
                                Response.Write("{\"error\":true,\"msg\":\"操作失败，"+
                                    "请登录后再操作\",\"loginurl\":\"UserLogin.aspx\"}");
                            }
                            Response.End();
                        }
                    }
                    else
                    {
                        GetProductDetail();
                    }
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBoxGoBack(2, "操作失败<br/>您暂时没有权限访问该页面！", true);
                    GetProductDetail();
                    return;
                }
            }
        }

        /// 设置Cookie
        /// </summary>
        void setCookies()
        {
            HttpCookie delcookies = new HttpCookie("pageurl");
            delcookies.Expires = DateTime.Now.AddDays(-1);
            Response.AppendCookie(delcookies);
            HttpCookie cookies = new HttpCookie("pageurl", "Product_detail.aspx?pid="+strpid);
            cookies.Expires = DateTime.Now.AddMinutes(3);
            Response.AppendCookie(cookies);
        }
        /// <summary>
        /// 添加购物车
        /// </summary>
        void setAddCart()
        {
            if (customid.Trim() != null && customid.Trim() != "")
            {
                string countcost = string.Empty; string quantity = string.Empty;
                if (Request["countcost"] != null && Request["countcost"] != "")
                {
                    countcost = Common.Common.NoHtml(Request["countcost"]);
                }
                if (Request["quantity"] != null && Request["quantity"] != "")
                {
                    quantity = Common.Common.NoHtml(Request["quantity"]);
                }
                if (quantity.Trim() == null || quantity.Trim() == "")
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
                }
                if (countcost.Trim() == null || countcost.Trim() == "")
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
                }
                MSProductOrderDAL OrderDal = new MSProductOrderDAL();
                if (OrderDal.ExistOrderByUID(customid,strpid))
                {
                    Response.Write("{\"error\":true,\"exist\":true}");
                }
                else
                {
                    Response.Write("{\"success\":true}");
                }
                //MSShoppingCartDAL cartDal = new MSShoppingCartDAL();
                //if (!cartDal.ExistCart(strpid, customid))
                //{
                //    MSShoppingCart cartModel = new MSShoppingCart();
                //    cartModel.ID = Guid.NewGuid().ToString("N");
                //    cartModel.Pid = strpid;
                //    cartModel.CustomerID = customid;
                //    cartModel.UnitCost = decimal.Parse(countcost);
                //    cartModel.Quantity = int.Parse(quantity);
                //    if (!cartDal.AddShoppingCart(cartModel))
                //    {
                //        Response.Write("{\"error\":true,\"msg\":\"操作失败，请重新操作\"}");
                //    }
                //    else
                //    {
                //        Response.Write("{\"success\":true}");
                //    }
                //}
                //else
                //{
                //    Response.Write("{\"success\":true}");
                //}
            }
            else
            {
                setCookies();
                Response.Write("{\"error\":true,\"msg\":\"操作失败，请登录后再操作\",\"loginurl\":\"UserLogin.aspx\"}");
            }
        }
        void GetProductDetail()
        {
            string ptitle = string.Empty;
            string shopID = string.Empty;
            string puid = string.Empty;
            int paracount = 0;
            #region 产品详细
            MSProductDAL productDal = new MSProductDAL();
            MSProduct productModel = new MSProduct();
            DataSet productds = productDal.GetProductDetail(strpid);
            if (null != productds && productds.Tables.Count > 0 && productds.Tables[0].Rows.Count > 0)
            {
                productModel = DataConvert.DataRowToModel<MSProduct>(productds.Tables[0].Rows[0]);
                ptitle = productModel.Ptitle;
                puid = productModel.CustomerID;
                if (productModel.SID != null && productModel.SID != "")
                {
                    shopID = productModel.SID;
                }
            }
            #endregion
            #region 店铺详细
            MSShop shopModel = new MSShop();
            if (shopID!= null && shopID!= "")
            {
                MSShopDAL shopDal = new MSShopDAL();
                DataSet shopds = shopDal.GetMSShopDetail(shopID);
                if (shopds != null && shopds.Tables.Count > 0 && shopds.Tables[0].Rows.Count > 0)
                {
                    shopModel = DataConvert.DataRowToModel<MSShop>(shopds.Tables[0].Rows[0]);
                }
            }
            #endregion
            #region 图集列表
            MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
            List<MSProductAtlas> AtlasListModel = new List<MSProductAtlas>();
            DataSet atlasds = atlasDal.GetProductAtlasByPID(strpid);
            if (null != atlasds && atlasds.Tables.Count > 0 && atlasds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in atlasds.Tables[0].Rows)
                {
                    MSProductAtlas atlasModel = DataConvert.DataRowToModel<MSProductAtlas>(row);
                    AtlasListModel.Add(atlasModel);
                }
            }
            #endregion
            #region 产品参数列表
            //MSProductParaDAL paraDal = new MSProductParaDAL();
            //DataSet paramds = paraDal.GetProductParamByPID(strpid);
            //string paramlist = string.Empty;
            //if (null != paramds && paramds.Tables.Count > 0 && paramds.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < paramds.Tables[0].Rows.Count; i++)
            //    {
            //        paramlist += "<tr>\r\n";
            //        string paraname = paramds.Tables[0].Rows[i]["ParName"].ToString();
            //        string paravalue = paramds.Tables[0].Rows[i]["ParValue"].ToString();
            //        paramlist += "<td class=\"td_title\">" + paraname + "</td><td>" + paravalue + "</td>\r\n";
            //        try
            //        {
            //            i = i + 1;
            //            paraname = paramds.Tables[0].Rows[i]["ParName"].ToString();
            //            paravalue = paramds.Tables[0].Rows[i]["ParValue"].ToString();
            //            paramlist += "<td class=\"td_title\">" + paraname + "</td><td>" + paravalue + "</td>\r\n";
            //        }
            //        catch (Exception)
            //        {
            //            paramlist += "<td class=\"td_title\">&nbsp;&nbsp;</td><td>&nbsp;&nbsp;</td>\r\n";
            //        }
            //        paramlist += "</tr>\r\n";
            //    }
            //}
            #endregion
            #region-------获取产品型号及价格------------
            ProductPara ParaModel = new ProductPara();
            List<MSProductPara> paralistmodel = new List<MSProductPara>();
            MSProductParaDAL paraDal = new MSProductParaDAL();
            DataSet parads = paraDal.GetMaxMinPrice(strpid);
            if (parads != null && parads.Tables.Count > 0 && parads.Tables[0].Rows.Count > 0)
            {
                paracount = parads.Tables[0].Rows.Count;
                ParaModel = DataConvert.DataRowToModel<ProductPara>(parads.Tables[0].Rows[0]);
            }
            parads = null;
            parads = paraDal.GetProductParamByPID(strpid);
            if (parads != null && parads.Tables.Count > 0 && parads.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow item in parads.Tables[0].Rows)
                {
                    MSProductPara paramodel = DataConvert.DataRowToModel<MSProductPara>(item);
                    paralistmodel.Add(paramodel);
                }
            }
            #endregion

            #region ----------------根据产品编号获取联系方式--------------------
            MSShopContactsDAL contactDal = new MSShopContactsDAL();
            MSShopContacts contactModel = new MSShopContacts();
            DataSet contactDs = contactDal.GetContactDetailByPID(strpid);

            MSCustomersDAL CustomerDal = new MSCustomersDAL();
            MSCustomers CustomerModel = new MSCustomers();
            DataSet PuidDs = null; int contactcount = 0;

            if (contactDs != null && contactDs.Tables.Count > 0 && contactDs.Tables[0].Rows.Count > 0)
            {
                contactModel = DataConvert.DataRowToModel<MSShopContacts>(contactDs.Tables[0].Rows[0]);
                contactcount = 1;
            }
            else
            {
                if (puid != null && puid != "")
                {
                  PuidDs=  CustomerDal.GetCustomerDetail(puid);
                }
                if (PuidDs != null && PuidDs.Tables.Count > 0 && PuidDs.Tables[0].Rows.Count > 0)
                {
                    CustomerModel = DataConvert.DataRowToModel<MSCustomers>(PuidDs.Tables[0].Rows[0]);
                }
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/Product_detail.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = ptitle;
            if (shopModel != null)
            {
                context.TempData["shopdetail"] = shopModel;
            }
            context.TempData["productdetail"] = productModel;
            context.TempData["atlaslist"] = AtlasListModel;
            if (contactcount>0)
            {
                context.TempData["contactdetail"] = contactModel;
            }
            else
            {
                context.TempData["contactdetail"] = CustomerModel;
            }
            context.TempData["customid"] = customid;
            //context.TempData["paramlist"] = paramlist;
            context.TempData["paracount"] = paracount;
            context.TempData["paramodel"] = ParaModel;
            context.TempData["paralist"] = paralistmodel;
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["errorscript"] = errorscript;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 产品型号属性
        /// </summary>
        public class ProductPara
        {
            private string pID;

            public string PID
            {
                get { return pID; }
                set { pID = value; }
            }
            private string mprice;

            public string Mprice
            {
                get { return mprice; }
                set { mprice = value; }
            }
            private string sprice;

            public string Sprice
            {
                get { return sprice; }
                set { sprice = value; }
            }
        }
    }
}
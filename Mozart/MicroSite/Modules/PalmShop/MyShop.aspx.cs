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
using System.Text.RegularExpressions;

namespace Mozart.PalmShop.ShopCode
{
    public partial class MyShop : System.Web.UI.Page
    {
        public string shopid = string.Empty;
        public string UrlPageName = string.Empty;
        string errormsg = string.Empty;
        string action = string.Empty;
        string pid = string.Empty;
        string customerid = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customerid = Session["customerID"].ToString();
                }
                if (Request["sid"] != null && Request["sid"] != "")
                {
                    shopid = Common.Common.NoHtml(Request["sid"]);
                }
                else
                {
                    if (Session["SID"] != null && Session["SID"].ToString() != "")
                    {
                        shopid = Session["SID"].ToString();
                    }
                    else
                    {
                        GetShopID();
                        if (shopid == null || shopid == "")
                        {
                            JQDialog.SetCookies("pageurl", "MyShop.aspx", 1);
                            errormsg = JQDialog.alertOKMsgBox(5, "您还没开通微店，请开通后再操作",
                                "ApplyShop.aspx?action=apply", "error");
                        }
                    }
                }
                if (Request["pid"] != null && Request["pid"] != "")
                {
                    pid = Common.Common.NoHtml(Request["pid"]);
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    switch (action.ToLower().Trim())
                    {
                        case "delproduct":
                            DelProduct();
                            break;
                    }
                }
                GetInfo();
            }
        }
        /// <summary>
        /// 获取店铺编号
        /// </summary>
        void GetShopID()
        {
            MSShopDAL shopDal = new MSShopDAL();
            if (customerid != null && customerid != "")
            {
                try
                {
                    shopid = shopDal.GetSidByUid("ID", customerid).ToString();
                }
                catch (Exception)
                {
                }
            }
        }
        /// <summary>
        /// 获取店铺商品信息
        /// </summary>
        void GetInfo()
        {
            #region 获取店铺详细
            MSShopDAL shopdal = new MSShopDAL();
            DataSet shopds = shopdal.GetMSShopDetail(shopid);
            MSShop shopmodel = new MSShop();
            string pagetitle = string.Empty;
            if (null != shopds && shopds.Tables.Count > 0 && shopds.Tables[0].Rows.Count > 0)
            {
                shopmodel = DataConvert.DataRowToModel<MSShop>(shopds.Tables[0].Rows[0]);
                pagetitle = shopmodel.ShopName;
            }
            #endregion

            List<MSProduct> ProductModel = new List<MSProduct>();
            MSProductDAL productdal = new MSProductDAL();

            MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
            List<MSProductAtlas> AtlasModel = new List<MSProductAtlas>();

            List<ProductPara> ParaListModel = new List<ProductPara>();
            MSProductParaDAL paraDal = new MSProductParaDAL();

            string thpid = string.Empty;
            #region 获取产品列表
            DataSet productds = null; string cid = string.Empty; string like = string.Empty;
            if (Request["cid"] != null && Request["cid"] != "")
            {
                cid = Common.Common.NoHtml(Request["cid"]);
            }
            if (cid.Trim() != null && cid.Trim() != "")
            {
                cid = " and a.[CID]='" + cid + "' ";
            }
            if (Request["like"] != null && Request["like"] != "")
            {
                like = Common.Common.NoHtml(Request["like"]);
            }
            if (like.Trim() != null && like.Trim() != "")
            {
                like = " and a.Ptitle like '%" + like + "%' ";
            }
            productds = productdal.GetProductList(" and a.[SID]='" + shopid + "' " + cid + like);
            DataSet atlasDs = null;
            foreach (DataRow row in productds.Tables[0].Rows)
            {
                MSProduct model = DataConvert.DataRowToModel<MSProduct>(row);
                string pdesc = model.Pcontent;
                pdesc = JQDialog.GetTextFromHTML(pdesc);
                if (pdesc.Length > 50)
                {
                    pdesc = pdesc.ToString().Substring(0,50) + "...";
                }
                model.Pcontent = pdesc;
                ProductModel.Add(model);
                thpid = model.ID;

                #region 获取产品默认展示图
                atlasDs = atlasDal.GetDefaultAtlasByPid(thpid);
                foreach (DataRow atlasrow in atlasDs.Tables[0].Rows)
                {
                    MSProductAtlas atlasdetailmodel = DataConvert.DataRowToModel<MSProductAtlas>(atlasrow);
                    AtlasModel.Add(atlasdetailmodel);
                }
                #endregion

                #region-------获取产品型号及价格------------
                DataSet parads = paraDal.GetMaxMinPrice(thpid);
                if (parads != null && parads.Tables.Count > 0 && parads.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow pararow in parads.Tables[0].Rows)
                    {
                        ProductPara paramodel = DataConvert.DataRowToModel<ProductPara>(pararow);
                        ParaListModel.Add(paramodel);
                    }
                }
                #endregion
            }
            #endregion

            #region 获取产品类别
            List<MSProductCategory> CategoryModel = new List<MSProductCategory>();
            MSProductCategoryDAL categorydal = new MSProductCategoryDAL();
            DataSet categoryds = categorydal.GetMSPCList(" and a.[SID]='" + shopid + "' ");
            foreach (DataRow row in categoryds.Tables[0].Rows)
            {
                MSProductCategory model = DataConvert.DataRowToModel<MSProductCategory>(row);
                CategoryModel.Add(model);
            }
            #endregion

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/MyShop.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = pagetitle;
            context.TempData["ShopDetail"] = shopmodel;
            context.TempData["shopid"] = shopid;
            context.TempData["productlist"] = ProductModel;
            context.TempData["defaultatlas"] = AtlasModel;
            context.TempData["categorylist"] = CategoryModel;
            context.TempData["paralist"] = ParaListModel;
            context.TempData["errormsg"] = errormsg;
            context.TempData["customerid"] = customerid;
            if (Session["customerID"] != null && Session["customerID"].ToString() != "")
            {
                context.TempData["uid"] = Session["customerID"].ToString();
            }
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 产品删除
        /// </summary>
        void DelProduct()
        {
            if (pid != null && pid != "")
            {
                MSProductDAL productDal = new MSProductDAL();
                if (productDal.UpdateMSProductState(pid))
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
                Response.Write("{\"error\":true}");
            }
            Response.End();
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
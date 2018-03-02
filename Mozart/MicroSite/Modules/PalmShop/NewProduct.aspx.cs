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
    public partial class NewProduct : System.Web.UI.Page
    {
        string categoryhtml = string.Empty;
        string newpublist = string.Empty;
        public static int ishand = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["ishand"] != null && Request["ishand"] != "")
                {
                    try
                    {
                        ishand = Convert.ToInt32(Request["ishand"]);
                    }
                    catch (Exception)
                    {
                    }
                }
                GetInfo();
            }
        }
        void GetInfo()
        {
            GetCategoryList();
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/newproduct.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["title"] = "批发市场";
            context.TempData["categoryhtml"] = categoryhtml;
            context.TempData["newpublist"] = newpublist;
            context.TempData["footer"] = "奥琦微商易";

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void GetCategoryList()
        {
            #region -------------------获取类别---------------------
            MSProductCategory CategoryModel = new MSProductCategory();
            MSProductCategoryDAL CategoryDal = new MSProductCategoryDAL();
            DataSet BigCategoryds = CategoryDal.GetSecHandCategoryList(" and UpID='' and CsecHand=" + ishand);
            DataSet SmallCategoryds = null;
            if (BigCategoryds != null && BigCategoryds.Tables.Count > 0 && BigCategoryds.Tables[0].Rows.Count > 0)
            {
                categoryhtml = "";
                for (int i = 0; i < BigCategoryds.Tables[0].Rows.Count; i++)
                {
                    string bigcname = BigCategoryds.Tables[0].Rows[i]["cname"].ToString();
                    string bigID = BigCategoryds.Tables[0].Rows[i]["ID"].ToString();

                    categoryhtml += "<ul class=\"list-unstyled pro_list\">";
                    categoryhtml += "\r\n<h4 class=\"col-lg-12\">" + bigcname + "</h4>";

                    SmallCategoryds = CategoryDal.GetSecHandCategoryList(" and UpID='" + bigID + "' ");
                    if (SmallCategoryds != null && SmallCategoryds.Tables.Count > 0
                        && SmallCategoryds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < SmallCategoryds.Tables[0].Rows.Count; j++)
                        {
                            string smallcname = SmallCategoryds.Tables[0].Rows[j]["cname"].ToString();
                            string smallID = SmallCategoryds.Tables[0].Rows[j]["ID"].ToString();
                            categoryhtml += "\r\n<li class=\"col-lg-4 col-md-4 col-sm-4 col-xs-4 text-center\">\r\n" +
                                "<a href=\"SecHandList.aspx?scid=" + smallID + "\">" + smallcname + "</a>\r\n" +
                                "</li>";
                        }
                        categoryhtml += "</ul>";
                    }
                }
            }
            #endregion

            #region -----------------获取最新发布的产品列表-------------
            MSProductDAL productDal = new MSProductDAL();
            DataSet productds = productDal.GetProductByTop(30, " and a.IsSecHand=" + ishand + " and a.[SID]='' ");
            if (productds != null && productds.Tables.Count > 0 && productds.Tables[0].Rows.Count > 0)
            {
                string newpubpid = string.Empty; string newpubpname = string.Empty;
                string newpubdate;
                newpublist = "";
                for (int i = 0; i < productds.Tables[0].Rows.Count; i++)
                {
                    newpubpid = newpubpname = newpubdate = "";
                    newpubpid = productds.Tables[0].Rows[i]["ID"].ToString();
                    newpubpname = productds.Tables[0].Rows[i]["Ptitle"].ToString();
                    newpubdate = productds.Tables[0].Rows[i]["AddTime"].ToString();
                    newpubdate = Convert.ToDateTime(newpubdate).ToString("yy-MM-dd mm:ss");
                    newpubpname = Common.Common.NoHtml(newpubpname);
                    if (newpubpname.ToString().Length > 15)
                    {
                        newpubpname = newpubpname.ToString().Substring(0, 13) + "...";
                    }
                    newpublist +=
                        "<li title=\"" + newpubpname + "\">\r\n<a href=\"Product_detail.aspx?pid=" + newpubpid + "\">" +
                        newpubpname + "<span>" + newpubdate + "</span></a>\r\n</li>\r\n";
                }
            }
            #endregion
        }
    }
}
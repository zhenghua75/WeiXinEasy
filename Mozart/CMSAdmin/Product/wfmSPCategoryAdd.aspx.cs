using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.CMSAdmin.Product
{
    public partial class wfmSPCategoryAdd : System.Web.UI.Page
    {
        string strMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }

            DAL.Product.CategoryDAL dal = new DAL.Product.CategoryDAL();
            Model.SP.SP_Category modelAdd = new Model.SP.SP_Category
            {
                ID = Guid.NewGuid().ToString("N").ToUpper(),
                //文章标题
                Name = this.txtTitle.Text,
                SiteCode = Session["strSiteCode"].ToString()
            };
            if (dal.InsertInfo(modelAdd) > 0)
            {
                strMessage = "商品分类添加成功！";
            }
            else
            {
                strMessage = "商品分类添加失败！";
            }
            MessageBox.Show(this, strMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            this.txtTitle.Text = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.CMSAdmin.Product
{
    public partial class wfmSPCategoryUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Login.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面
                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null !=Common.Common.NoHtml( Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }

                ShowSPCategoryInfo(strID);
                #endregion
            }
        }

        public void ShowSPCategoryInfo(string strID)
        {
            DAL.Product.CategoryDAL dal = new DAL.Product.CategoryDAL();
            DataSet ds = dal.GetSPCategoryDetail(strID);
            Model.SP.SP_Category model = DataConvert.DataRowToModel<Model.SP.SP_Category>(ds.Tables[0].Rows[0]);
            this.txtTitle.Text = model.Name;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }

            DAL.Product.CategoryDAL dal = new DAL.Product.CategoryDAL();
            Model.SP.SP_Category modelUpdate = new Model.SP.SP_Category
            {
                ID = strID,
                Name = this.txtTitle.Text               
            };
            if (dal.UpdateSPCategoryData(modelUpdate))
            {
                MessageBox.Show(this, "修改成功！");
            }
            else
            {
                MessageBox.Show(this, "修改失败！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            this.txtTitle.Text = "";
        }
    }
}
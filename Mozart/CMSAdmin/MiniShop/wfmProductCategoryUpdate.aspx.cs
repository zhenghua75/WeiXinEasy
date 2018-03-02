using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using Mozart.Common;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductCategoryUpdate : System.Web.UI.Page
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
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
                {
                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    showdetailinfo();
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }
        void showdetailinfo()
        {
            ddlcategorylist.Items.Clear();
            MSProductCategoryDAL selectcategoryDal = new MSProductCategoryDAL();
            DataSet ds = new DataSet();
            ds = selectcategoryDal.GetCategoryList(" and UpID='' and [SID]!='' ");
            DataTable dt = ds.Tables[0];
            DataRow dr = ds.Tables[0].NewRow();
            DataRow firstdr = ds.Tables[0].NewRow();
            firstdr["ID"] = "0";
            firstdr["Cname"] = "顶级导航";
            dt.Rows.InsertAt(firstdr, 0);
            ddlcategorylist.DataSource = ds.Tables[0].DefaultView;
            ddlcategorylist.DataTextField = "Cname";
            ddlcategorylist.DataValueField = "ID";
            ddlcategorylist.DataBind();

            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet categoryDs = categoryDal.GetCategoryDetail(strID);
            MSProductCategory categoryModel = DataConvert.DataRowToModel<MSProductCategory>(categoryDs.Tables[0].Rows[0]);
            cname.Text = categoryModel.Cname;
            string parm = string.Empty;
            if (categoryModel.UpID.Trim() != null && categoryModel.UpID.Trim() != "")
            {
                parm = categoryModel.UpID;
            }
            else
            {
                parm = "0";
            }
            ddlcategorylist.SelectedIndex =
                ddlcategorylist.Items.IndexOf(ddlcategorylist.Items.FindByValue(parm));
            sortin.Text = categoryModel.Sortin.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (cname.Text.Trim() != null && cname.Text.Trim() != "")
                {
                    MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                    MSProductCategory categoryModel = new MSProductCategory();
                    categoryModel.Cname = cname.Text;
                    string parm = "";
                    categoryModel.SID = "vyigo";
                    if (ddlcategorylist.SelectedValue.Trim() != "0")
                    {
                        parm = ddlcategorylist.SelectedValue;
                    }
                    categoryModel.UpID = parm;
                    if (sortin.Text.Trim() != null && sortin.Text.Trim() != "")
                    {
                        categoryModel.Sortin = Convert.ToInt32(sortin.Text);
                    }
                    categoryModel.Cstate = 0;
                    categoryModel.ID = strID;
                    if (categoryDal.UpdateMSPCategory(categoryModel))
                    {
                        MessageBox.Show(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.Show(this, "操作失败！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请输入相应名称或店铺！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            cname.Text = "";
        }
    }
}
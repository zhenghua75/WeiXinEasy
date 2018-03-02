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

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductCategoryAdd : System.Web.UI.Page
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
                    ddlcategorylist.Items.Clear();
                    MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                    DataSet ds = new DataSet();
                    ds = categoryDal.GetCategoryList(" and UpID='' and [SID]!='' ");
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["Cname"] = "--请选择类别--";
                    dt.Rows.InsertAt(dr, 0);
                    DataRow firstdr = ds.Tables[0].NewRow();
                    firstdr["ID"] = "0";
                    firstdr["Cname"] = "--添加顶级导航--";
                    dt.Rows.InsertAt(firstdr, 1);
                    ddlcategorylist.DataSource = ds.Tables[0].DefaultView;
                    ddlcategorylist.DataTextField = "Cname";
                    ddlcategorylist.DataValueField = "ID";
                    ddlcategorylist.DataBind();

                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (ddlcategorylist.SelectedValue.Trim() != null && ddlcategorylist.SelectedValue.Trim() != "")
                {
                    if (cname.Text.Trim() != null && cname.Text.Trim() != "")
                    {
                        MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                        if (categoryDal.ExistMSPC("vyigo", ddlcategorylist.SelectedValue.Trim(), cname.Text))
                        {
                            MessageBox.Show(this, "该类别已经存在！");
                        }
                        else
                        {
                            MSProductCategory categoryModel = new MSProductCategory();
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
                            categoryModel.Cname = cname.Text;
                            categoryModel.Cstate = 0;
                            categoryModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                            if (categoryDal.AddMSPCategory(categoryModel))
                            {
                                MessageBox.Show(this, "操作成功！");
                            }
                            else
                            {
                                MessageBox.Show(this, "操作失败！");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "请输入相应名称！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请选择相应的类别！");
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
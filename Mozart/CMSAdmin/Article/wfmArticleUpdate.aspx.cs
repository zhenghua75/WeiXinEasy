using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Article
{
    public partial class wfmArticleUpdate : System.Web.UI.Page
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
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面
                this.ddlCategory.Items.Clear();
                DAL.CMS.CategoryDAL dal = new DAL.CMS.CategoryDAL();
                DataSet ds = new DataSet();
                if (GlobalSession.strRoleCode == "ADMIN")
                {
                    ds = dal.GetAllCategory("");
                }
                else
                {
                    ds = dal.GetAllCategory(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "";
                dt.Rows.InsertAt(dr, 0);

                this.ddlCategory.DataSource = ds.Tables[0].DefaultView;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "ID";
                this.ddlCategory.DataBind();

                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                ShowArticleInfo(strID);
                #endregion
            }
        }

        public void ShowArticleInfo(string strID)
        {
            DAL.CMS.ArticleDAL dal = new DAL.CMS.ArticleDAL();
            DataSet ds = dal.GetArticleDetail(strID);
            Model.CMS.CMS_Article model = DataConvert.DataRowToModel<Model.CMS.CMS_Article>(ds.Tables[0].Rows[0]);
            this.txtArticleTitle.Text = model.Title;
            this.hd_content.Value = model.Content;
            this.ddlCategory.SelectedIndex = this.ddlCategory.Items.IndexOf(this.ddlCategory.Items.FindByValue(model.Category));
            this.ddlArticleIsTop.SelectedIndex = this.ddlArticleIsTop.Items.IndexOf(this.ddlArticleIsTop.Items.FindByValue(model.IsTop ? "是" : "否"));
            this.txtSummary.Text = model.Summary;
            this.txtOrder.Text = model.Order.ToString();
            this.img0.Src = "../../" +model.Pic;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }

        /// <summary>
        /// 单击"保存"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            //上传图标
            string strIconFileName = string.Empty;//图标路径
            string strIconSaveFileName = string.Empty;//网址路径
            try
            {
                if (!string.IsNullOrEmpty(this.file0.PostedFile.FileName))
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Images"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Images");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strIconFileName = String.Format(@"{0}Images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strIconSaveFileName = String.Format(@"Images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file0.PostedFile.SaveAs(strIconFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            DAL.CMS.ArticleDAL dal = new DAL.CMS.ArticleDAL();
            Model.CMS.CMS_Article modelUpdate = new Model.CMS.CMS_Article()
            {
                //文章ID
                ID = strID,
                //文章标题
                Title = this.txtArticleTitle.Text,
                //图标路径
                Pic = strIconSaveFileName,
                Summary = this.txtSummary.Text,
                //文章内容
                Content = this.hd_content.Value,
                //文章类别
                Category = this.ddlCategory.SelectedValue,
                //是否置顶
                IsTop = (this.ddlArticleIsTop.Text == "是") ? true : false ,
                Order = int.Parse(this.txtOrder.Text.ToString())

            };
            if (dal.UpdateArticleData(modelUpdate))
            {
                MessageBox.Show(this, "修改成功！");
            }
            else
            {
                MessageBox.Show(this, "修改失败！");
            }
        }

        /// <summary>
        /// 单击"重置"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {

        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.CMS;
using Model.CMS;
using Mozart.Common;

namespace Mozart.CMSAdmin.Article
{
    public partial class wfmCategoryUpdate : System.Web.UI.Page
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
                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                ShowCategoryInfo(strID);
                #endregion
            }
        }

        public void ShowCategoryInfo(string strID)
        {
            DAL.CMS.CategoryDAL dal = new DAL.CMS.CategoryDAL();
            DataSet ds = dal.GetCategoryByID(strID);
            Model.CMS.CMS_Category model = DataConvert.DataRowToModel<Model.CMS.CMS_Category>(ds.Tables[0].Rows[0]);
            this.txtName.Text = model.Name;
            this.hd_content.Value = model.Content;
            this.txtSummary.Text = model.Summary;
            this.txtLink.Text = model.Link;
            this.txtOrder.Text = model.Order.ToString();
            this.img0.Src = "../../" + model.Pic;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //上传图标
            string strIconFileName = string.Empty;//图标路径
            try
            {
                if (!string.IsNullOrEmpty(this.file0.PostedFile.FileName))
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Images"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Images");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), GlobalSession.strSiteCode)))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), GlobalSession.strSiteCode));
                    }
                    string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strIconFileName = String.Format(@"{0}Images/{1}/{2}", Server.MapPath("~"), GlobalSession.strSiteCode, newName);
                    file0.PostedFile.SaveAs(strIconFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            CategoryDAL dal = new CategoryDAL();
            CMS_Category modelUpdate = new CMS_Category()
            {
                //类别ID
                ID = strID,
                //站点代码
                SiteCode = GlobalSession.strSiteCode,
                //类别名称
                Name = this.txtName.Text,
                //图标路径
                Pic = string.IsNullOrEmpty(strIconFileName) ? strIconFileName : strIconFileName.Substring(Server.MapPath("~").Length),
                //简要说明
                Summary = this.txtSummary.Text,
                //类别内容
                Content = this.hd_content.Value,
                //链接
                Link = this.txtLink.Text,
                //排序
                Order = int.Parse(this.txtOrder.Text)
            };
            if (dal.UpdateCategoryData(modelUpdate))
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
            this.txtName.Text = "";
            this.hd_content.Value = "";
            this.txtLink.Text = "";
            this.txtOrder.Text = "0";
        }
    }
}
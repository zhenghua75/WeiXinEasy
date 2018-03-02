using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Album;
using Model.PA;
using System.Data;
using Mozart.Common;

namespace Mozart.CMSAdmin.Album
{
    public partial class wfmAlbumAdd : System.Web.UI.Page
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
                this.ddlAlbumType.Items.Clear();
                AlbumTypeDAL dal = new AlbumTypeDAL();
                DataSet ds = new DataSet();
                if (GlobalSession.strRoleCode == "ADMIN")
                {
                    ds = dal.GetAlbumTypeByIsDel("");
                }
                else
                {
                    ds = dal.GetAlbumTypeByIsDel(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "请选择相关类别";
                dt.Rows.InsertAt(dr, 0);

                this.ddlAlbumType.DataSource = ds.Tables[0].DefaultView;
                this.ddlAlbumType.DataTextField = "Name";
                this.ddlAlbumType.DataValueField = "ID";
                this.ddlAlbumType.DataBind();

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
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            //上传图像
            string strIconFileName = string.Empty;//图像路径
            string strIconSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file0.PostedFile.FileName == "")
                {
                    strIconSaveFileName = "";
                }
                else
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
            AlbumDAL dal = new AlbumDAL();
            PA_Album model = new PA_Album();
            if (AlbumName.Text.Trim() != null && AlbumName.Text.Trim() != "")
            {
                model.Name = AlbumName.Text;
                model.Note = hd_content.Value;
                model.Photo = strIconSaveFileName;
                model.Type = ddlAlbumType.SelectedValue;
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                if (dal.InsertAlbum(model))
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
                MessageBox.Show(this, "请输入相应标题名称！");
            }

        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            AlbumName.Text = "";
            hd_content.Value = "";
        }
    }
}
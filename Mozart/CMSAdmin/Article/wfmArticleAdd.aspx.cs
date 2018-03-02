using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using Model;
using DAL.CMS;

namespace Mozart.CMSAdmin.Article
{
    public partial class wfmArticleAdd : System.Web.UI.Page
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
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.ddlCategory.Items.Add(dt.Rows[i]["Name"].ToString());
                }

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "";
                dt.Rows.InsertAt(dr, 0);

                this.ddlCategory.DataSource = ds.Tables[0].DefaultView;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "ID";
                this.ddlCategory.DataBind();
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
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            //上传图标
            string strIconFileName = string.Empty;//图标路径
            string strIconSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file0.PostedFile.FileName == "")
                {
                    MessageBox.Show(this, "请选择上传文件！");
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
            ArticleDAL dal = new ArticleDAL();
            Model.CMS.CMS_Article modelAdd = new Model.CMS.CMS_Article()
            {
                ID = Guid.NewGuid().ToString("N").ToUpper(),
                //文章标题
                Title = this.txtArticleTitle.Text,
                //图标路径
                Pic = strIconSaveFileName,//strIconFileName,
                //简要说明
                Summary = this.txtSummary.Text,
                //文章内容
                Content = this.hd_content.Value,
                //用户名称
                Author = Session["strSiteName"].ToString(),
                //类别
                Category = this.ddlCategory.SelectedValue,
                //点击次数
                ClickNum = 0,
                //是否置顶
                IsTop = this.txtArticleIsTop.Text == "是" ? true : false,
                //是否删除
                IsDel = false,
                //创建时间
                CreateTime = DateTime.Now,
                //用户登录名
                CreateUser = Session["strLoginName"].ToString(),
                //站点代码
                SiteCode = Session["strSiteCode"].ToString()
            };
            if (dal.AddArticleData(modelAdd))
            {
                strMessage = "文章添加成功！";
            }
            else
            {
                strMessage = "文章添加失败！";
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
            this.txtArticleTitle.Text = "";
            this.hd_content.Value = "";
        }
    }
}
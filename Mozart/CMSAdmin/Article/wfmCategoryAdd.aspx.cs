using DAL.CMS;
using Model.CMS;
using Mozart.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.Article
{
    public partial class wfmCategoryAdd : System.Web.UI.Page
    {
        string strMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
        }

        /// <summary>
        /// 保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSava_Click(object sender, EventArgs e)
        {
            //上传图标
            string strIconFileName = string.Empty;//图标路径
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
            CMS_Category modelAdd = new CMS_Category()
            {
                ID = Guid.NewGuid().ToString("N").ToUpper(),
                //站点代码
                SiteCode = GlobalSession.strSiteCode,
                //类别名称
                Name = this.txtName.Text,
                //图标路径
                Pic = strIconFileName.Substring(Server.MapPath("~").Length),
                //简要说明
                Summary = this.txtSummary.Text,
                //类别内容
                Content = this.hd_content.Value,
                //链接
                Link = this.txtLink.Text,
                //排序
                Order = int.Parse(this.txtOrder.Text)
            };
            if (dal.AddCategoryData(modelAdd))
            {
                MessageBox.Show(this, "类别添加成功！");
            }
            else
            {
                MessageBox.Show(this, "类别添加失败！");
            }
        }

        /// <summary>
        /// 重置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnResett_Click(object sender, EventArgs e)
        {
            this.txtName.Text = "";
            this.hd_content.Value = "";
            this.txtLink.Text = "";
        }
    }
}
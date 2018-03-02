using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Vote;
using Model.Vote;
using Mozart.Common;

namespace Mozart.CMSAdmin.Vote
{
    public partial class wfmOptonUpdate : System.Web.UI.Page
    {
        static string subID = string.Empty;
        static string strID = string.Empty;
        string strAction = string.Empty;
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
                if (null != Common.Common.NoHtml(Request.QueryString["subid"]))
                {
                    subID = Common.Common.NoHtml(Request.QueryString["subid"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                }
                ShowUpdateInfo();
                #endregion
            }
        }

        void ShowUpdateInfo()
        {
            if (strID.Trim() != null && strID.Trim() != "")
            {
                OptionDAL dal = new OptionDAL();
               DataSet ds= dal.getOptionDetail(strID);
               if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
               {
                   VOTE_Option model = DataConvert.DataRowToModel<VOTE_Option>(ds.Tables[0].Rows[0]);
                   hd_content.Value = model.Contentdesc;
                   optitle.Text = model.Title;
                   oporder.Text = model.Order.ToString();
                   if (model.Ico!= null && model.Ico!= "")
                   {
                       img0.Src = "../../" + model.Ico;
                   }
                   if (strAction == "show")
                   {
                       this.btnReset.Visible = false;
                       this.btnSave.Visible = false;
                   }
               }
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
            OptionDAL dal = new OptionDAL();
            VOTE_Option model = new VOTE_Option();
            if (optitle.Text.Trim() != null && optitle.Text.Trim() != "")
            {
                model.Title = optitle.Text;
                model.Contentdesc = hd_content.Value;
                model.Ico = strIconSaveFileName;
                model.SubjectID = subID;
                model.IsDel = 1;
                if (oporder.Text.Trim() != null && oporder.Text.Trim() != "")
                {
                    model.Order = Convert.ToInt32(oporder.Text);
                }
                model.ID =strID;
                if (dal.UpdateOption(model))
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
                MessageBox.Show(this, "请输入相应名称！");
            }

        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            optitle.Text = ""; oporder.Text = "";
            hd_content.Value = "";
        }
    }
}
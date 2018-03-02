using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.JC;
using DAL.JC;
using Mozart.Common;

namespace Mozart.CMSAdmin.JC
{
    public partial class wfmJCQuizUpdate : System.Web.UI.Page
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
                    ShowActivityInfo(strID);
                    #endregion
                }
            }
        }
        public void ShowActivityInfo(string strID)
        {
            JC_QuizDAL dal = new JC_QuizDAL();
            DataSet ds = dal.GetJCQuizDetail(strID);
            JC_Quiz model = DataConvert.DataRowToModel<JC_Quiz>(ds.Tables[0].Rows[0]);
            this.txtName.Text = model.Name;
            this.hd_content.Value = model.MatchDesc;
            starttime.Text = model.StartTime.ToString();
            img0.Src = "../../" + model.HomeTeamImg;
            img1.Src = "../../" + model.VisitingTeamImg;
            hometeam.Text = model.HomeTeam;
            VisitingTeam.Text = model.VisitingTeam;
            RightScore.Text = model.RightScore;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                starttime.ReadOnly = true;
                txtName.ReadOnly = true;
            }
            else
            {
                starttime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                //上传主队图像
                string strIconFileName0 = string.Empty;//图像路径
                string strIconSaveFileName0 = string.Empty;//网址路径
                try
                {
                    if (this.file0.PostedFile.FileName == "")
                    {
                        strIconSaveFileName0 = "";
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
                        strIconFileName0 = String.Format(@"{0}Images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                        strIconSaveFileName0 = String.Format(@"Images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                        file0.PostedFile.SaveAs(strIconFileName0);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                }

                //上传客队图像
                string strIconFileName1 = string.Empty;//图像路径
                string strIconSaveFileName1 = string.Empty;//网址路径
                try
                {
                    if (this.file1.PostedFile.FileName == "")
                    {
                        strIconSaveFileName1 = "";
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
                        string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                        string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                        if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                        {
                            MessageBox.Show(this, "文件格式有误！");
                            return;

                        }//检查文件格式
                        string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength, extendName);//对文件进行重命名
                        strIconFileName1 = String.Format(@"{0}Images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                        strIconSaveFileName1 = String.Format(@"Images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                        file1.PostedFile.SaveAs(strIconFileName1);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                }

                if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
                {
                    JC_Quiz model = new JC_Quiz();
                    JC_QuizDAL dal = new JC_QuizDAL();
                    model.Name = txtName.Text;
                    model.HomeTeam = hometeam.Text;
                    model.HomeTeamImg = strIconSaveFileName0;
                    model.VisitingTeam = VisitingTeam.Text;
                    model.VisitingTeamImg = strIconSaveFileName1;
                    model.StartTime = Convert.ToDateTime(starttime.Text);
                    model.MatchDesc = hd_content.Value;
                    model.ID = strID;
                    model.State = 0;
                    model.RightScore = RightScore.Text;
                    model.QuizType = "1";
                    model.SiteCode = Session["strSiteCode"].ToString();
                    if (dal.UpDateJCQuiz(model))
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
                    MessageBox.Show(this, "请输入相关名称！");
                }
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            starttime.Text = ""; hd_content.Value = ""; RightScore.Text = "";
            txtName.Text = ""; img0.Src = ""; img1.Src = ""; hometeam.Text = ""; VisitingTeam.Text = "";
        }
    }
}
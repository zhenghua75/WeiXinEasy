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
    public partial class wfmMSForumSetUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string oldimg = string.Empty;
        static string oldlogoimg = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                    && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
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
            MSForumSetDAL ForumDal = new MSForumSetDAL();
            DataSet ds = ForumDal.GetMSForumSetDetail(strID);
            MSForumSet ForumModel = DataConvert.DataRowToModel<MSForumSet>(ds.Tables[0].Rows[0]);
            Ftitle.Text = ForumModel.FTitle;
            if (ForumModel.BackImg != null && ForumModel.BackImg != "")
            {
                img0.Src = "../../Comment/" + ForumModel.BackImg;
                oldimg = ForumModel.BackImg;
            }
            if (ForumModel.LogoImg != null && ForumModel.LogoImg != "")
            {
                img1.Src = "../../Comment/" + ForumModel.LogoImg;
                oldlogoimg = ForumModel.LogoImg;
            }
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            MSForumSetDAL ForumDal = new MSForumSetDAL();
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (Ftitle.Text.Trim() != null && Ftitle.Text.Trim() != "")
                {
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
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Comment/ForumImg"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Comment/ForumImg");
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = "backimg"+String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}Comment/ForumImg/{1}", Server.MapPath("~"),newName);
                            strIconSaveFileName = String.Format(@"ForumImg/{0}",newName);
                            file0.PostedFile.SaveAs(strIconFileName);
                            if (oldimg.Trim() != null && oldimg.Trim() != "" && oldimg.Trim().ToLower() != "forumimg/2.png")
                            {
                                System.IO.File.Delete(String.Format(@"{0}Comment/{1}", Server.MapPath("~"), oldimg));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }
                    //上传logo
                    string strlogoFileName = string.Empty;//图像路径
                    string strlogoSaveFileName = string.Empty;//网址路径
                    try
                    {
                        if (this.file1.PostedFile.FileName == "")
                        {
                            if (oldlogoimg != null && oldlogoimg != "")
                            {
                                strlogoSaveFileName = oldlogoimg;
                            }
                            else
                            {
                                strlogoSaveFileName = "";
                            }
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Comment/ForumImg"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Comment/ForumImg");
                            }
                            string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = "logo" + String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strlogoFileName = String.Format(@"{0}Comment/ForumImg/{1}", Server.MapPath("~"), newName);
                            strlogoSaveFileName = String.Format(@"ForumImg/{0}", newName);
                            file1.PostedFile.SaveAs(strlogoFileName);
                            if (oldlogoimg.Trim() != null && oldlogoimg.Trim() != "" && oldlogoimg.Trim().ToLower() != "forumimg/2.png")
                            {
                                System.IO.File.Delete(String.Format(@"{0}Comment/{1}", Server.MapPath("~"), oldlogoimg));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }

                    MSForumSet ForumModel = new MSForumSet();
                    ForumModel.ID =strID;
                    ForumModel.FTitle = Ftitle.Text;
                    ForumModel.Fstate = 0;
                    if (strlogoSaveFileName.Trim() == null || strlogoSaveFileName.Trim() == "")
                    {
                        strlogoSaveFileName = "ForumImg/2.png";
                    }
                    ForumModel.LogoImg = strlogoSaveFileName;
                    if (strIconSaveFileName.Trim() != null || strIconSaveFileName.Trim() != "")
                    {
                        ForumModel.BackImg = strIconSaveFileName;
                    }
                    
                    if (ForumDal.UpdateMSForumSet(ForumModel))
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
                    MessageBox.Show(this, "请输入相应标题！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Ftitle.Text = "";
        }
    }
}
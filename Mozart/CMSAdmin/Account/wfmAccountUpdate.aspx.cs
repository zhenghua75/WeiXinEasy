using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using DAL.SYS;
using Model.SYS;

namespace Mozart.CMSAdmin.Account
{
    public partial class wfmAccountUpdate : System.Web.UI.Page
    {
       static string strID = string.Empty;
        string strMessage = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GlobalSession.strAccountID) || string.IsNullOrEmpty(GlobalSession.strRoleCode))
            {
                Response.Write("<script language=JavaScript>;parent.parent.parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面
                //代理商
                DAL.SYS.AccountDAL dalAccount = new DAL.SYS.AccountDAL();
                DataSet dsAccount = null;
                switch (GlobalSession.strRoleCode)
                {
                    case "ADMIN":
                        dsAccount = dalAccount.GetAllAccount(" RoleID = 'AGENT' ");
                        break;
                    case "AGENT":
                        DataRow drAccount = dsAccount.Tables[0].NewRow();
                        drAccount["ID"] = GlobalSession.strAccountID;
                        drAccount["Name"] = GlobalSession.strName;
                        dsAccount.Tables[0].Rows.InsertAt(drAccount, 0);
                        break;
                    default:
                        break;
                }
                this.ddlAgent.DataSource = dsAccount.Tables[0].DefaultView;
                this.ddlAgent.DataTextField = "Name";
                this.ddlAgent.DataValueField = "ID";
                this.ddlAgent.DataBind();


                //账户状态
                DAL.SYS.SYSDictionaryDAL dalState = new DAL.SYS.SYSDictionaryDAL();
                DataSet dsState = dalState.GetDictionaryData("ZHZT");
                this.ddlState.DataSource = dsState.Tables[0].DefaultView;
                this.ddlState.DataTextField = "Remark";
                this.ddlState.DataValueField = "ID";
                this.ddlState.DataBind();

                //角色
                DAL.SYS.MenuRoleDAL dalRole = new DAL.SYS.MenuRoleDAL();
                DataSet dsRole = null;
                switch (GlobalSession.strRoleCode)
                {
                    case "ADMIN":
                        dsRole = dalRole.GetRoleList();
                        break;
                    case "AGENT":
                        DataRow drRole = dsRole.Tables[0].NewRow();
                        drRole["No"] = "PTKH";
                        drRole["Name"] = "普通客户";
                        dsRole.Tables[0].Rows.InsertAt(drRole, 0);
                        break;
                    default:
                        break;
                }
                this.ddlRole.DataSource = dsRole.Tables[0].DefaultView;
                this.ddlRole.DataTextField = "Name";
                this.ddlRole.DataValueField = "No";
                this.ddlRole.DataBind();

                //站点类别
                ddlsitecategory.Items.Clear();
                SysCategoryDAL sitecategoryDal = new SysCategoryDAL();
                DataSet sitecategoryDs = sitecategoryDal.GetNoDelSysCateGoryLsit("");
                ddlsitecategory.DataSource = sitecategoryDs.Tables[0].DefaultView;
                ddlsitecategory.DataTextField = "SiteName";
                ddlsitecategory.DataValueField = "Id";
                ddlsitecategory.DataBind();

                //主题
                ddlthemeslist.Items.Clear();
                SysThemesDAL themesdal = new SysThemesDAL();
                DataSet themesds = themesdal.GetSysThemesListByState("");
                this.ddlthemeslist.DataSource = themesds.Tables[0].DefaultView;
                this.ddlthemeslist.DataTextField = "Name";
                this.ddlthemeslist.DataValueField = "value";
                this.ddlthemeslist.DataBind();

                //设置主题的可见性
                if (GlobalSession.strRoleCode == "ADMIN" || GlobalSession.strRoleCode == "AGENT" ||
                    GlobalSession.strRoleCode.ToLower() == "admin" || GlobalSession.strRoleCode.ToLower() == "agent")
                {
                    SummaryTb.Visible = true;
                }
                else
                {
                    SummaryTb.Visible = false;
                }

                #endregion

                if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    ShowAccountInfo(strID);
                }
            }
        }

        public void ShowAccountInfo(string strID)
        {
            ///账户信息
            DAL.SYS.AccountDAL dal = new DAL.SYS.AccountDAL();
            DataSet ds = dal.GetAccountByID(strID);
            Model.SYS.SYS_Account model = DataConvert.DataRowToModel<Model.SYS.SYS_Account>(ds.Tables[0].Rows[0]);
            this.txtID.Value = model.ID;
            this.txtAccountID.Text = model.LoginName;
            this.ddlAgent.SelectedIndex = this.ddlAgent.Items.IndexOf(this.ddlAgent.Items.FindByValue(model.AgentID));
            this.txtAccountName.Text = model.Name;
            this.ddlRole.SelectedIndex = this.ddlRole.Items.IndexOf(this.ddlRole.Items.FindByValue(model.RoleID));
            this.txtEmail.Text = model.Email;
            this.txtAddress.Text = model.Address;
            this.txtMobile.Text = model.Mobile;
            this.txtTel.Text = model.Telphone;
            this.ddlState.SelectedIndex = this.ddlState.Items.IndexOf(this.ddlState.Items.FindByValue(model.Status));
            this.txtSitCode.Text = model.SiteCode;

            ////站点信息
            Account_ExtDAL accountdal = new Account_ExtDAL();
           DataSet accountds= accountdal.GetAccountds(strID);
           if (accountds != null && accountds.Tables.Count > 0 && accountds.Tables[0].Rows.Count > 0)
           {
               Account_Ext accountmodel = DataConvert.DataRowToModel<Account_Ext>(accountds.Tables[0].Rows[0]);
               if (accountmodel.Photo != null && accountmodel.Photo != "")
               {
                   img0.Src = "../../" + accountmodel.Photo;
               }
               if (accountmodel.CodeImg != null && accountmodel.CodeImg != "")
               {
                   img1.Src = "../../HPPrint/" + accountmodel.CodeImg;
               }
               if (accountmodel.PrintImg1 != null && accountmodel.PrintImg1 != "")
               {
                   img2.Src = "../../HPPrint/" + accountmodel.PrintImg1;
               }
               if (accountmodel.PrintImg2 != null && accountmodel.PrintImg2 != "")
               {
                   img3.Src = "../../HPPrint/" + accountmodel.PrintImg2;
               }
               if (accountmodel.PrintImg3 != null && accountmodel.PrintImg3 != "")
               {
                   img4.Src = "../../HPPrint/" + accountmodel.PrintImg3;
               }
               if (accountmodel.PrintImg4 != null && accountmodel.PrintImg4 != "")
               {
                   img5.Src = "../../HPPrint/" + accountmodel.PrintImg4;
               }
               summary.Text = accountmodel.Summary;
               hd_remark.Value = accountmodel.Remark;
               ddlsitecategory.SelectedIndex = 
                   ddlsitecategory.Items.IndexOf(ddlsitecategory.Items.FindByValue(accountmodel.SiteCategory));
               this.ddlthemeslist.SelectedIndex = 
                   this.ddlthemeslist.Items.IndexOf(this.ddlthemeslist.Items.FindByValue(accountmodel.Themes.ToString()));
           }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.txtAccountID.Text.Equals(""))
            {
                MessageBox.Show(this, "申请开户账号不能为空！");
                return;
            }
            if (this.txtAccountName.Text.Equals(""))
            {
                MessageBox.Show(this, "申请开户名称不能为空！");
                return;
            }
            DAL.SYS.AccountDAL dal = new DAL.SYS.AccountDAL();

            Model.SYS.SYS_Account modelUpdate = new Model.SYS.SYS_Account
            {
                ID = this.txtID.Value,
                Name = this.txtAccountName.Text,
                RoleID = this.ddlRole.SelectedValue.ToString(),
                Email = this.txtEmail.Text,
                Address = this.txtAddress.Text,
                Mobile = this.txtMobile.Text,
                Telphone = this.txtTel.Text,
                Status = this.ddlState.SelectedValue.ToString(),
                SiteCode = this.txtSitCode.Text
            };

            #region 上传站点图标
            //上传图标
            string strIconFileName = string.Empty;//图标路径
            string strIconSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file0.PostedFile.FileName == "")
                {
                    //MessageBox.Show(this, "请选择上传文件！");
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
            #endregion

            #region 上传二维码图标
            //上传二维码图标
            string strCodeFileName = string.Empty;//图标路径
            string strCodeSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file1.PostedFile.FileName == "")
                {
                    strCodeSaveFileName = "";
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/HPPrint/printimg"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/HPPrint/printimg");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strCodeFileName = String.Format(@"{0}HPPrint/printimg/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strCodeSaveFileName = String.Format(@"printimg/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file1.PostedFile.SaveAs(strCodeFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            #endregion

            #region 上传打印机图片1
            //上传打印机图片1
            string strPrintFileName = string.Empty;//图标路径
            string strPrintSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file2.PostedFile.FileName == "")
                {
                    strPrintSaveFileName = "";
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/HPPrint/printimg"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/HPPrint/printimg");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file2.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file2.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strPrintFileName = String.Format(@"{0}HPPrint/printimg/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strPrintSaveFileName = String.Format(@"printimg/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file2.PostedFile.SaveAs(strPrintFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            #endregion

            #region 上传打印机图片2
            //上传打印机图片2
            string strPrint2FileName = string.Empty;//图标路径
            string strPrint2SaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file3.PostedFile.FileName == "")
                {
                    strPrint2SaveFileName = "";
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/HPPrint/printimg"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/HPPrint/printimg");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file3.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file3.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strPrint2FileName = String.Format(@"{0}HPPrint/printimg/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strPrint2SaveFileName = String.Format(@"printimg/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file3.PostedFile.SaveAs(strPrint2FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            #endregion

            #region 上传打印机图片3
            //上传打印机图片3
            string strPrint3FileName = string.Empty;//图标路径
            string strPrint3SaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file4.PostedFile.FileName == "")
                {
                    strPrint3SaveFileName = "";
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/HPPrint/printimg"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/HPPrint/printimg");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file4.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file4.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strPrint3FileName = String.Format(@"{0}HPPrint/printimg/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strPrint3SaveFileName = String.Format(@"printimg/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file4.PostedFile.SaveAs(strPrint3FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            #endregion

            #region 上传打印机图片4
            //上传打印机图片4
            string strPrint4FileName = string.Empty;//图标路径
            string strPrint4SaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file5.PostedFile.FileName == "")
                {
                    strPrint4SaveFileName = "";
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/HPPrint/printimg"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/HPPrint/printimg");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/HPPrint/printimg/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file5.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file5.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strPrint4FileName = String.Format(@"{0}HPPrint/printimg/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strPrint4SaveFileName = String.Format(@"printimg/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file5.PostedFile.SaveAs(strPrint4FileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            #endregion

            Account_Ext accmodel = new Account_Ext();
            Account_ExtDAL accdal = new Account_ExtDAL();
            if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
            {
                accmodel.Photo = strIconSaveFileName;
            }
            if (strCodeSaveFileName.Trim() != null && strCodeSaveFileName.Trim() != "")
            {
                accmodel.CodeImg = strCodeSaveFileName;
            }
            if (strPrintSaveFileName.Trim() != null && strPrintSaveFileName.Trim() != "")
            {
                accmodel.PrintImg1 = strPrintSaveFileName;
            }
            if (strPrint2SaveFileName.Trim() != null && strPrint2SaveFileName.Trim() != "")
            {
                accmodel.PrintImg2 = strPrint2SaveFileName;
            }
            if (strPrint3SaveFileName.Trim() != null && strPrint3SaveFileName.Trim() != "")
            {
                accmodel.PrintImg3 = strPrint3SaveFileName;
            }
            if (strPrint4SaveFileName.Trim() != null && strPrint4SaveFileName.Trim() != "")
            {
                accmodel.PrintImg4 = strPrint4SaveFileName;
            }
            if (summary.Text.Trim() != null && summary.Text.Trim() != "")
            {
                accmodel.Summary = summary.Text;
            }
            if (hd_remark.Value.Trim() != null && hd_remark.Value.Trim() != "")
            {
                accmodel.Remark = hd_remark.Value;
            }
            if (ddlthemeslist.SelectedValue != null && ddlthemeslist.SelectedValue != "" && ddlthemeslist.SelectedValue != "0")
            {
                accmodel.Themes = ddlthemeslist.SelectedValue;
            }
            else
            {
                accmodel.Themes ="";
            }
            if (ddlsitecategory.SelectedValue != null && ddlsitecategory.SelectedValue != "" && ddlsitecategory.SelectedValue != "0")
            {
                accmodel.SiteCategory = ddlsitecategory.SelectedValue;
            }
            accmodel.AccountID = strID;
            if (accdal.IsExsit(strID))
            {
                accdal.UpdateAccount_Ext(accmodel);
            }
            else
            {
                accdal.SaveAccount_Ext(accmodel);
            }

            if (dal.UpdateAccountData(modelUpdate))
            {
                strMessage = "修改成功！";
            }
            else
            {
                strMessage = "修改失败！";
            }

            MessageBox.Show(this, strMessage);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            this.txtAccountID.Text = "";
            this.txtAddress.Text = "";
            this.txtTel.Text = "";
            this.txtAccountName.Text = "";
            this.txtMobile.Text = "";
            summary.Text = "";
            hd_remark.Value = "";
        }
    }
}
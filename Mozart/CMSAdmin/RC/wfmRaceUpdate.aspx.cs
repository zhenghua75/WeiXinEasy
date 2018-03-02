using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.RC;
using Model.RC;
using Mozart.Common;

namespace Mozart.CMSAdmin.RC
{
    public partial class wfmRaceUpdate : System.Web.UI.Page
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
                showracelistinfo();
                #endregion
            }
        }
        void showracelistinfo()
        {
            RC_RaceDAL dal = new RC_RaceDAL();
            DataSet ds = dal.GetRaceDetail(strID);
            RC_Race model = DataConvert.DataRowToModel<RC_Race>(ds.Tables[0].Rows[0]);
            Rtitle.Text = model.Rtitle;
            hd_content.Value = model.RaceDesc;
            starttime.Text = model.StartTime;
            movenum.Text = model.MoveNum.ToString();
            endtime.Text = model.EndTime;
            appid.Text = model.AppID;
            if (model.CodeImg != null && model.CodeImg != "")
            {
                img0.Src = "../../WXWall/" + model.CodeImg;
            }
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                starttime.ReadOnly = true;
                endtime.ReadOnly = true;
                movenum.ReadOnly = true;
            }
            else
            {
                starttime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                endtime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
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
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/WXWall/images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strIconFileName = String.Format(@"{0}WXWall/images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strIconSaveFileName = String.Format(@"images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file0.PostedFile.SaveAs(strIconFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }

            if (Rtitle.Text.Trim() != null && Rtitle.Text.Trim() != "")
            {
                RC_RaceDAL dal = new RC_RaceDAL();
                RC_Race model = new RC_Race();
                string strSiteCode = Session["strSiteCode"].ToString();
                model.SiteCode = strSiteCode;
                model.Rtitle = Rtitle.Text;
                model.RaceDesc = hd_content.Value;
                if (starttime.Text.Trim() != null && starttime.Text.Trim() != "")
                {
                    model.StartTime =starttime.Text;
                }
                if (endtime.Text.Trim() != null && endtime.Text.Trim() != "")
                {
                    model.EndTime = endtime.Text;
                }
                if (appid.Text.Trim() != null && appid.Text.Trim() != "")
                {
                    model.AppID = appid.Text;
                }
                if (movenum.Text.Trim() != null && movenum.Text.Trim() != "")
                {
                    model.MoveNum = Convert.ToInt32(movenum.Text);
                }
                if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                {
                    model.CodeImg = strIconSaveFileName;
                }
                model.IsDel = 0;
                model.ID = strID;
                if (dal.UpdateRCRace(model))
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
            Rtitle.Text = ""; starttime.Text = ""; endtime.Text = "";
            hd_content.Value = ""; movenum.Text = "";
        }
    }
}
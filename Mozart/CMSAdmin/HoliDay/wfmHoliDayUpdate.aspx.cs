using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.HoliDay;
using DAL.HoliDay;
using Mozart.Common;

namespace Mozart.CMSAdmin.HoliDay
{
    public partial class wfmHoliDayUpdate : System.Web.UI.Page
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
            HoliDayDAL dal = new HoliDayDAL();
            DataSet ds = dal.GetHoliDayDateil(strID);
            HD_HoliDay model = DataConvert.DataRowToModel<HD_HoliDay>(ds.Tables[0].Rows[0]);
            txtName.Text = model.Htitle;
            hd_content.Value = model.Hcontent;
            starttime.Text = model.HstartTime.ToString();
            endtime.Text = model.HendTime.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                starttime.ReadOnly = true;
                endtime.ReadOnly = true;
                txtName.ReadOnly = true;
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
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                HD_HoliDay model = new HD_HoliDay();
                HoliDayDAL dal = new HoliDayDAL();
                model.Htitle = txtName.Text;
                model.HstartTime = Convert.ToDateTime(starttime.Text);
                model.HendTime = Convert.ToDateTime(endtime.Text);
                model.Hcontent = hd_content.Value;
                model.ID = strID;
                model.Hisdel = 0;
                model.SiteCode = Session["strSiteCode"].ToString();
                if (dal.UpdateHoliday(model))
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

        protected void btnReset_Click(object sender, EventArgs e)
        {
            starttime.Text = ""; hd_content.Value = "";
            txtName.Text = ""; endtime.Text = "";
        }
    }
}
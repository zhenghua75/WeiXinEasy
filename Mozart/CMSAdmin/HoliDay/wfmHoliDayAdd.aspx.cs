using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.HoliDay;
using Model.HoliDay;
using Mozart.Common;

namespace Mozart.CMSAdmin.HoliDay
{
    public partial class wfmHoliDayAdd : System.Web.UI.Page
    {
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
                starttime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                endtime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
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

            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                HD_HoliDay model = new HD_HoliDay();
                HoliDayDAL dal=new HoliDayDAL ();
                model.SiteCode = Session["strSiteCode"].ToString();
                model.Htitle = txtName.Text;
                model.Himg = "";
                model.HstartTime = Convert.ToDateTime(starttime.Text);
                model.HendTime = Convert.ToDateTime(endtime.Text);
                model.Hcontent = hd_content.Value;
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                model.Hisdel = 0;
                if (dal.AddHoliday(model))
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
                MessageBox.Show(this, "请输入信息名称后再操作！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = ""; hd_content.Value = ""; starttime.Text = ""; 
            endtime.Text = "";
        }
    }
}
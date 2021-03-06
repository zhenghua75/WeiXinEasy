﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Album;
using Model.PA;

namespace Mozart.CMSAdmin.Album
{
    public partial class wfmAlbumTypeList : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }
                ViewState[vsKey] = s;
                LoadData(s);
            }
        }

        /// <summary>
        /// 单击"查询"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            if (!string.IsNullOrEmpty(this.txtTitle.Text))
            {
                strWhere = strWhere + " AND Name LIKE '%" + this.txtTitle.Text + "%' ";
            }
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;
            LoadData(strWhere);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        void LoadData(string strWhere)
        {
            txtTitle.Text = "";
            AlbumTypeDAL dal = new AlbumTypeDAL();
            DataSet ds = dal.GetAlbumTypeBySiteCode(strWhere);
            DataView dv = ds.Tables[0].DefaultView;

            AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            Repeater1.DataSource = pds;
            Repeater1.DataBind();
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb_no = (Label)e.Item.FindControl("no");
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();
                Label lb_isdel = (Label)e.Item.FindControl("IsDel");
                Label lb_thestate = (Label)e.Item.FindControl("thestate");
                if (lb_isdel.Text != null && lb_isdel.Text != "")
                {
                    switch (lb_isdel.Text.Trim())
                    {
                        case "1":
                            lb_isdel.Text = "生效";
                            lb_thestate.Text = "失效";
                            break;
                        default:
                            lb_isdel.Text = "失效";
                            lb_thestate.Text = "生效";
                            break;
                    }
                }
            }
        }

        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            LoadData((string)ViewState[vsKey]);
        }

        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmShopContacts : System.Web.UI.Page
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
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
                {

                    ddlShoplist.Items.Clear();
                    MSShopDAL actDal = new MSShopDAL();
                    DataSet ds = new DataSet();
                    ds = actDal.GetMSShopList("");
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["ShopName"] = "--全部--";
                    dt.Rows.InsertAt(dr, 0);
                    ddlShoplist.DataSource = ds.Tables[0].DefaultView;
                    ddlShoplist.DataTextField = "ShopName";
                    ddlShoplist.DataValueField = "ID";
                    ddlShoplist.DataBind();

                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
                    ViewState[vsKey] = s;
                    LoadData(s);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 单击"查询"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere = " AND a.[NickName] LIKE '%" + txtName.Text + "%' ";
            }
            if (ddlShoplist.SelectedValue.Trim() != null && ddlShoplist.SelectedValue.Trim() != "")
            {
                strWhere = strWhere + " AND a.[SID]='" + ddlShoplist.SelectedValue + "' ";
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
            MSShopContactsDAL shopdal = new MSShopContactsDAL();
            DataSet ds = shopdal.GetMSShopContactsList(strWhere);
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
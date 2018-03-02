﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using DAL.MiniShop;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmPcustomers : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        static string sechand = string.Empty;
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
                    if (Request["sch"] != null && Request["sch"] != "")
                    {
                        sechand = " and c.CsecHand=" + Request["sch"] + " ";
                    }
                    else
                    {
                        sechand = "";
                    }
                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
                    s = sechand;
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
            if (sechand != null && sechand != "")
            {
                strWhere += sechand;
            }
            if (cphone.Text != null && cphone.Text != "" && cphone.Text != "请输入正确的电话")
            {
                strWhere += " AND b.[Phone] = '" + cphone.Text + "' ";
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere  += " AND b.[NickName] LIKE '%" + txtName.Text + "%' ";
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
            MSProductDAL customerDal = new MSProductDAL();
            DataSet ds = customerDal.GetProductCustomerList(strWhere);
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
                Label lb_shoplogo = (Label)e.Item.FindControl("headimg");
                if (lb_shoplogo.Text != null && lb_shoplogo.Text != "")
                {
                    lb_shoplogo.Text = "<img src=\"../../PalmShop/ShopCode/" + lb_shoplogo.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
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
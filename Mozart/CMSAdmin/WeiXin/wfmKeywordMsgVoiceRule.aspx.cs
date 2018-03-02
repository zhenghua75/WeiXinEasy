using DAL.WeiXin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Mozart.CMSAdmin.WeiXin
{
    public partial class wfmKeywordMsgVoiceRule : System.Web.UI.Page
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
            string strWhere = "  ";
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (!string.IsNullOrEmpty(this.txtName.Text) &&
                txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " [MatchPattern] LIKE '%" + txtName.Text + "%' ";
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
            txtName.Text = "";
            MsgAutoRuleDAL dal = new MsgAutoRuleDAL();
            DataSet ds = dal.GetKeywordVoiceMsgAutoRuleList(strWhere);
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
                Label lb_msgtype = (Label)e.Item.FindControl("msgtype");//
                if (lb_msgtype.Text.Trim().ToLower() == "voice" )
                {
                    lb_msgtype.Text = "语音格式";
                }
                Label lb_MatchType = (Label)e.Item.FindControl("MatchType");
                if (lb_MatchType.Text.Trim().ToLower() == "keywords" || lb_MatchType.Text.Trim().ToLower() == "keywords")
                {
                    lb_MatchType.Text = "关键字";
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
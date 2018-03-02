using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.CR;
using Model.CR;

namespace Mozart.CMSAdmin.CR
{
    public partial class wfmChatMsg : System.Web.UI.Page
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

                ddlRoomList.Items.Clear();
                ChatRoomDAL rmdal = new ChatRoomDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = rmdal.GetChatRoomList("");
                }
                else
                {
                    ds = rmdal.GetChatRoomList("  AND SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = 0;
                dr["RoomName"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                this.ddlRoomList.DataSource = ds.Tables[0].DefaultView;
                this.ddlRoomList.DataTextField = "RoomName";
                this.ddlRoomList.DataValueField = "ID";
                this.ddlRoomList.DataBind();

                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strSiteCode"].ToString() != "ADMIN")
                {
                    s = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }
                s = s + " and a.MsgState=0 ";
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
            if (Session["strSiteCode"].ToString() != "ADMIN")
            {
                strWhere = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (MsgText.Text.Trim() != null && MsgText.Text.Trim() != "")
            {
                strWhere = strWhere + " AND a.MsgText  like '%" + MsgText.Text + "%' ";
            }
            if (ddlRoomList.SelectedValue.Trim() != null && ddlRoomList.SelectedValue.Trim() != ""
                && ddlRoomList.SelectedValue.Trim() != "0")
            {
                strWhere = strWhere + " AND c.ID =" + ddlRoomList.SelectedValue + " ";
            }
            if (ddlmsgstate.SelectedValue.Trim() != null && ddlmsgstate.SelectedValue.Trim() != "")
            {
                strWhere = strWhere + " AND a.MsgState=" + ddlmsgstate.SelectedValue + " ";
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
            MsgText.Text = "";
            ChatMessageDAL dal = new ChatMessageDAL();
            DataSet ds = dal.GetChatMsgList(strWhere);
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
                string strID = lb_no.Text;
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();

                Label lb_msgstate = (Label)e.Item.FindControl("msgstate");
                Label lb_msgtype = (Label)e.Item.FindControl("msgtype");
                Label lb_state = (Label)e.Item.FindControl("state");
                Label lb_msgtext = (Label)e.Item.FindControl("msgtext");
                if (lb_msgstate.Text.Trim() != null && lb_msgstate.Text.Trim() != "")
                {
                    switch (lb_msgstate.Text.Trim())
                    {
                        case"0":
                            lb_state.Text = "<a href=\"javascript:\" onclick=\"IsPass('wfmChatMsgAdmin.aspx?action=state&id=" + strID + "');\">上墙</a>";
                            break;
                        default:
                            lb_state.Text = "<a href=\"javascript:\" onclick=\"IsPass('wfmChatMsgAdmin.aspx?action=state&id=" + strID + "');\">冻结</a>";
                            break;
                    }
                }
                if (lb_msgtype.Text.Trim() != null && lb_msgtype.Text.Trim() != "")
                {
                    switch (lb_msgtype.Text.ToLower().Trim())
                    {
                        case"text":
                            break;
                        case "img":
                            lb_msgtext.Text = "<img src=\"../../WXWall/" + lb_msgtext.Text + "\" style=\"border:0px;width:30px;height:30px;\" />";
                            break;
                        case"image":
                            lb_msgtext.Text = "<img src=\"../../WXWall/" + lb_msgtext.Text + "\" style=\"border:0px;width:30px;height:30px;\" />";
                            break;
                        default:
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

        protected void nopassbtn_Click(object sender, EventArgs e)
        {
            string strWhere = "  ";
            if (Session["strSiteCode"].ToString() != "ADMIN")
            {
                strWhere = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            strWhere = strWhere + " AND a.MsgState =0 ";
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;
            LoadData(strWhere);
        }

        protected void passbtn_Click(object sender, EventArgs e)
        {
            string strWhere = "  ";
            if (Session["strSiteCode"].ToString() != "ADMIN")
            {
                strWhere = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            strWhere = strWhere + " AND a.MsgState =1 ";
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;
            LoadData(strWhere);
        }
    }
}
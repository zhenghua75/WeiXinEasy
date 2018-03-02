using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Game;
using DAL.ACT;
using System.Text;

namespace Mozart.CMSAdmin.Game
{
    public partial class wfmLuckyAwardUsers : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        public string action = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]);
                }
                if (action.Trim() != null && action.Trim() != "" && action.Trim().ToLower() == "getoption")
                {
                    GetOptionList();
                }
                ddlactlist.Items.Clear();
                SiteActivityDAL actDal = new SiteActivityDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = actDal.GetActivityListByState("");
                }
                else
                {
                    ds = actDal.GetActivityListByState("  AND SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "";
                dr["ActTitle"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);
                ddlactlist.DataSource = ds.Tables[0].DefaultView;
                ddlactlist.DataTextField = "ActTitle";
                ddlactlist.DataValueField = "ID";
                ddlactlist.DataBind();
                ddlawardlist.Items.Clear();

                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
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
            string strWhere = string.Empty; ;
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere  += " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (ddlactlist.SelectedValue.Trim() != null && ddlactlist.SelectedValue.Trim() != "")
            {
                strWhere += " AND a.ActID = '" + ddlactlist.SelectedValue + "' ";
            }
            if (setoptionvalue.Value.Trim() != null && setoptionvalue.Value.Trim() != "" && setoptionvalue.Value.Trim() != "0")
            {
                strWhere = strWhere + " AND a.AID= '" + setoptionvalue.Value + "' ";
            }
            if (ddlsendaward.SelectedValue.Trim() != null && ddlsendaward.SelectedValue.Trim() != "")
            {
                strWhere += " AND a.SendAward = '" + ddlsendaward.SelectedValue + "' ";
            }
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                strWhere  += " AND a.[NickName] LIKE '%" + txtName.Text + "%' ";
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
            LuckyAwardUsersDAL dal = new LuckyAwardUsersDAL();
            DataSet ds = dal.GetAwardUserList(strWhere);
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
                Label lb_sendaward = (Label)e.Item.FindControl("sendaward");
                if (lb_sendaward.Text != null && lb_sendaward.Text != "")
                {
                    if (lb_sendaward.Text == "0")
                    {
                        lb_sendaward.Text = "未发放";
                    }
                    else
                    {
                        lb_sendaward.Text = "已发放";
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

        void GetOptionList()
        {
            string subid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }
            if (subid.Trim() != null && subid.Trim() != "")
            {
                LuckyAwardDAL luckdal = new LuckyAwardDAL();
                DataSet optionds;
                if (Session["strSiteCode"].ToString() == "ADMIN")
                {
                    optionds = luckdal.GetAwardList(" and a.ActID='" + subid + "' ");
                }
                else
                {
                    optionds = luckdal.GetAwardList(" and b.SiteCode='" + Session["strSiteCode"].ToString() +
                        "' and a.ActID='" + subid + "' ");
                }
                Response.Write(Dataset2Json(optionds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }

        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTable2Json(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\"")); //对于特殊字符，还应该进行特别的处理。 
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        } 
    }
}
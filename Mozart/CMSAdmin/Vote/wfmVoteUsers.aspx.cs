using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.Vote;
using Model.Vote;
using System.Text;

namespace Mozart.CMSAdmin.Vote
{
    public partial class wfmVoteUsers : System.Web.UI.Page
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
                string optionid = string.Empty;
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]);
                }
                if (action.Trim() != null && action.Trim() != "" && action.Trim().ToLower() == "getoption")
                {
                    GetOptionList();
                }
               #region 页面加载数据绑定
                    ddlSubjectList.Items.Clear();
                    SubjectDAL subdal = new SubjectDAL();
                    DataSet ds = new DataSet();
                    if (Session["strSiteCode"].ToString() == "ADMIN")
                    {
                        ds = subdal.GetVoteDataList("");
                    }
                    else
                    {
                        ds = subdal.GetVoteDataList("   SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                    }
                    DataTable dt = ds.Tables[0];

                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] ="";
                    dr["Subject"] = "--全部--";
                    dt.Rows.InsertAt(dr, 0);

                    ddlSubjectList.DataSource = ds.Tables[0].DefaultView;
                    ddlSubjectList.DataTextField = "Subject";
                    ddlSubjectList.DataValueField = "ID";
                    ddlSubjectList.DataBind();

                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
                    if (Session["strSiteCode"].ToString() != "ADMIN")
                    {
                        s = " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                    }
                    if (Request["option"] != null && Request["option"] != "")
                    {
                        optionid = Common.Common.NoHtml(Request["option"]);
                    }
                    if (optionid.Trim() != null && optionid.Trim() != "")
                    {
                        s += " and a.VoteID='"+optionid+"' ";
                    }
                    ViewState[vsKey] = s;
                    LoadData(s);
                    #endregion 页面加载数据绑定
            }
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
                OptionDAL optiondal = new OptionDAL();
                DataSet optionds;
                if (Session["strSiteCode"].ToString() == "ADMIN")
                {
                    optionds = optiondal.GetOptionDataList(" and a.SubjectID='"+subid+"' ");
                }
                else
                {
                    optionds = optiondal.GetOptionDataList(" and b.SiteCode='" + Session["strSiteCode"].ToString() +
                        "' and a.SubjectID='" + subid + "' ");
                }
                Response.Write(Dataset2Json(optionds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
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
                strWhere = strWhere + " AND c.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }
            if (ddlSubjectList.SelectedValue.Trim() != null && ddlSubjectList.SelectedValue.Trim() != ""
                && ddlSubjectList.SelectedValue.Trim() != "0")
            {
                strWhere = strWhere + " AND a.SubjectID= '" + ddlSubjectList.SelectedValue + "' ";
            }
            if (setoptionvalue.Value.Trim() != null && setoptionvalue.Value.Trim() != "" && setoptionvalue.Value.Trim() != "0")
            {
                strWhere = strWhere + " AND a.VoteID= '" + setoptionvalue.Value + "' ";
            }
            if (txtTitle.Text.Trim() != null && txtTitle.Text.Trim() != "")
            {
                strWhere = strWhere + " AND  UserName like '%" + txtTitle.Text + "%' ";
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
            VoteUsersDAL dal = new VoteUsersDAL();
            DataSet ds = dal.GetVoteUserlist(strWhere);
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
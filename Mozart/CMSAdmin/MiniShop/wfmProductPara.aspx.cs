using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using System.Text;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductPara : System.Web.UI.Page
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
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
                {
                    if (Request["action"] != null && Request["action"] != "")
                    {
                        action = Common.Common.NoHtml(Request["action"]);
                    }
                    if (action.Trim() != null && action.Trim() != "")
                    {
                        switch (action.Trim().ToLower())
                        {
                            case "getcateoption":
                                GetOptionList();
                                break;
                            case "getpoption":
                                GetCatOptionList();
                                break;
                        }
                    }
                    else
                    {
                        ddlshoplist.Items.Clear();
                        MSShopDAL actDal = new MSShopDAL();
                        DataSet ds = new DataSet();
                        ds = actDal.GetMSShopList("");
                        DataTable dt = ds.Tables[0];
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["ID"] = "";
                        dr["ShopName"] = "--全部--";
                        dt.Rows.InsertAt(dr, 0);
                        ddlshoplist.DataSource = ds.Tables[0].DefaultView;
                        ddlshoplist.DataTextField = "ShopName";
                        ddlshoplist.DataValueField = "ID";
                        ddlshoplist.DataBind();

                        AspNetPager1.CurrentPageIndex = 1;
                        string s = "";
                        ViewState[vsKey] = s;
                        LoadData(s);
                    }
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
                strWhere = " AND a.[ParName] LIKE '%" + txtName.Text + "%' ";
            }
            if (ddlshoplist.SelectedValue.Trim() != null && ddlshoplist.SelectedValue.Trim() != "")
            {
                strWhere = strWhere + " AND b.[ID]='" + ddlshoplist.SelectedValue + "' ";
            }
            if (setsortvalue.Value.Trim() != null && setsortvalue.Value.Trim() != "")
            {
                strWhere = strWhere + " AND d.[ID]='" + setsortvalue.Value + "' ";
            }
            if (setpvalue.Value.Trim() != null && setpvalue.Value.Trim() != "")
            {
                strWhere = strWhere + " AND c.[ID]='" + setpvalue.Value + "' ";
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
            MSProductParaDAL paraDal = new MSProductParaDAL();
            DataSet ds = paraDal.GetMSPParaList(strWhere);
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
                MSProductCategoryDAL CateDal = new MSProductCategoryDAL();
                DataSet optionds;
                optionds = CateDal.GetMSPCList(" and a.[SID]='" + subid + "' ");
                Response.Write(Dataset2Json(optionds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        void GetCatOptionList()
        {
            string subid = string.Empty; string catid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }
            if (Request["catid"] != null && Request["catid"] != "")
            {
                catid = Common.Common.NoHtml(Request["catid"]);
            }
            else
            {
                return;
            }
            if (subid.Trim() != null && subid.Trim() != "" && catid.Trim() != null && catid.Trim() != "")
            {
                MSProductDAL productDal = new MSProductDAL();
                DataSet optionds;
                optionds = productDal.GetMSProductList(" and a.[SID]='" + subid + "' and a.[CID]='" + catid + "' ");
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
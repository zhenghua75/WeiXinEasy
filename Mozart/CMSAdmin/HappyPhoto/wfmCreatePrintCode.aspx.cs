using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.SYS;
using Mozart.Common;

namespace Mozart.CMSAdmin.HappyPhoto
{
    public partial class wfmCreatePrintCode : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {

                this.ddlClientID.Items.Clear();
                DAL.HP.HPClientDAL dalClient = new DAL.HP.HPClientDAL();
                DataSet dsClient = dalClient.GetPrintClient(Session["strSiteCode"].ToString());
                //DataTable dt = dsClient.Tables[0];
                //DataRow dr = dsClient.Tables[0].NewRow();
                //dr["ID"] = "0";
                //dr["ClientCode"] = "--全部--";
                //dt.Rows.InsertAt(dr, 0);

                this.ddlClientID.DataSource = dsClient.Tables[0].DefaultView;
                this.ddlClientID.DataTextField = "ClientCode";
                this.ddlClientID.DataValueField = "ClientCode";
                this.ddlClientID.DataBind();

                txtLoginName.Text = "";
                AspNetPager1.CurrentPageIndex = 1;
                string s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                ViewState[vsKey] = s;
                txtLoginName.Focus();
                LoadData(s);
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            if (!string.IsNullOrEmpty(txtLoginName.Text.Trim()))
            {
                strWhere = strWhere + " AND PrintCode = '" + this.txtLoginName.Text.Trim() +"' ";
            }

            if (ddlState.SelectedValue == "ZHZT_WX")
            {
                strWhere = strWhere + " AND [State] = 1 ";
            }

            if (ddlState.SelectedValue == "ZHZT_YX")
            {
                strWhere = strWhere + " AND [State] = 0 ";
            }
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;

            LoadData(strWhere);
        }

        void LoadData(string strWhere)
        {
            DAL.HP.PrintCodeDAL dalPrintCode = new DAL.HP.PrintCodeDAL();
            DataSet ds = dalPrintCode.GetPrintCodeByWhere(strWhere);
            
            DataView dv = ds.Tables[0].DefaultView;

            AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            Repeater1.DataSource = pds;
            Repeater1.DataBind();

            this.ddlState.Items.Clear();
            SYSDictionaryDAL didal = new SYSDictionaryDAL();
            DataSet dids = new DataSet();
            dids = didal.GetDictionaryList("");
            DataTable dt = dids.Tables[0];

            DataRow dr = dids.Tables[0].NewRow();
            dr["ID"] = "0";
            dr["Remark"] = "--全部--";
            dt.Rows.InsertAt(dr, 0);

            this.ddlState.DataSource = dids.Tables[0].DefaultView;
            this.ddlState.DataTextField = "Remark";
            this.ddlState.DataValueField = "ID";
            this.ddlState.DataBind();
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

        protected void btnCreatePrintCode_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAmount.Text) || txtAmount.Text == "0")
            {
                MessageBox.Show(this, "请输入打印数量！");
                return;
            }

            DAL.HP.PrintCodeDAL dalPrintCode = new DAL.HP.PrintCodeDAL();
            dalPrintCode.CreatePrintCode(int.Parse(txtAmount.Text), Session["strSiteCode"].ToString(), ddlClientID.SelectedValue.ToString(), "2014-01-01", "2019-12-31");
            btnQuery_Click(null, null);
            //Response.Write("<script language=JavaScript>__doPostBack('btnQuery', '')</script>");
            //Response.End();
        }
    }
}
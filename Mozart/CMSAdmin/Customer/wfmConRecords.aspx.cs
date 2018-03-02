using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace Mozart.CMSAdmin.Customer
{
    public partial class wfmConRecords : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                this.txtConFee.Text = "";
                this.txtMemNo.Text = "";

                AspNetPager1.CurrentPageIndex = 1;
                string s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' AND CreateTime BETWEEN '" + this.txtStartDate.Text + " 00:00:00' AND '" + this.txtEndDate.Text + " 23:59:59' ORDER BY CreateTime DESC ";
                ViewState[vsKey] = s;
                LoadData(" SiteCode = '" + Session["strSiteCode"].ToString() + "' AND CreateTime BETWEEN '" + this.txtStartDate.Text + " 00:00:00' AND '" + this.txtEndDate.Text + " 23:59:59' ORDER BY CreateTime DESC ");
            }
        }

        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = string.Empty;
            strWhere = " CreateTime BETWEEN '" + this.txtStartDate.Text + " 00:00:00' AND '" + this.txtEndDate.Text + " 23:59:59' ";
            if (!this.txtMemNo.Text.Trim().Equals(""))
            {
                strWhere = strWhere + " AND MemberShipNo = '" + this.txtMemNo.Text.Trim() + "'";
            }
            if (!this.txtConFee.Text.Trim().Equals(""))
            {
                strWhere = strWhere + " AND Price = " + this.txtConFee.Text.Trim();
            }
            strWhere = strWhere + " ORDER BY CreateTime DESC ";
            AspNetPager1.CurrentPageIndex = 1;
            ViewState[vsKey] = strWhere;
            LoadData(strWhere);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            GridView GridViewExport = new GridView();
            string strWhere = string.Empty;
            strWhere = " SiteCode = '" + Session["strSiteCode"].ToString() + "' AND CreateTime BETWEEN '" + this.txtStartDate.Text + " 00:00:00' AND '" + this.txtEndDate.Text + " 23:59:59' ";
            if (!this.txtMemNo.Text.Trim().Equals(""))
            {
                strWhere = strWhere + " AND MemberShipNo = '" + this.txtMemNo.Text.Trim() + "'";
            }
            if (!this.txtConFee.Text.Trim().Equals(""))
            {
                strWhere = strWhere + " AND Price = " + this.txtConFee.Text.Trim();
            }
            strWhere = strWhere + " ORDER BY CreateTime DESC ";

            DAL.Site.ConRecordsDAL dal = new DAL.Site.ConRecordsDAL();
            DataSet ds = dal.GetMemConRecord(strWhere);
            GridViewExport.DataSource = ds.Tables[0].DefaultView;
            GridViewExport.DataBind();

            if (GridViewExport.Rows.Count > 0)
            {
                //调用导出方法  
                //ExportGridViewForUTF8(GridViewExport, DateTime.Now.ToShortDateString() + ".xls");
                ExportGridViewForUTF8(GridViewExport, "ConRecord_" + DateTime.Now.ToString() + ".xls");
            }
            else
            {
                Common.MessageBox.Show(this, "没有数据可导出，请先查询数据!");
            }
        }

        void LoadData(string strWhere)
        {
            DAL.Site.ConRecordsDAL dal = new DAL.Site.ConRecordsDAL();
            DataSet ds = dal.GetMemConRecord(strWhere);
            DataView dv = ds.Tables[0].DefaultView;

            AspNetPager1.RecordCount = dv.Count;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            GridView1.DataSource = pds;
            GridView1.DataBind();
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

        /// <summary>  
        /// 导出方法  
        /// </summary>  
        /// <param name="GridView"></param>  
        /// <param name="filename">保存的文件名称</param>  
        private void ExportGridViewForUTF8(GridView GridView, string filename)
        {
            string attachment = "attachment; filename=" + filename;

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", attachment);

            Response.Charset = "UTF-8";
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/ms-excel";
            System.IO.StringWriter sw = new System.IO.StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            GridView.RenderControl(htw);

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
}

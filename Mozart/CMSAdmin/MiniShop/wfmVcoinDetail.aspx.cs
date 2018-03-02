using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmVcoinDetail : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        string custid = string.Empty;
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
                    string s = ""; 
                    if (Request["id"] != null && Request["id"] != "")
                    {
                        custid = Common.Common.NoHtml(Request["id"]);
                    }
                    if (custid != null && custid != "")
                    {
                        s = " and a.CustID='"+custid+"' ";
                    }
                    ViewState[vsKey] = s ;
                    LoadData(s);
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="strWhere">条件</param>
        void LoadData(string strWhere)
        {
            MSVAcctDetailDAL vcoinDal = new MSVAcctDetailDAL();
            DataSet ds = vcoinDal.GetVaccdetail(strWhere);
            DataView dv = ds.Tables[0].DefaultView;

            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;

            Repeater1.DataSource = pds;
            Repeater1.DataBind();

            if (custid != null && custid != "")
            {
                MSVAcctDAL vaccDal = new MSVAcctDAL();
                try
                {
                    vcoincount.Text = vaccDal.GetMSVAcct("V_Amont", custid).ToString();
                }
                catch (Exception)
                {
                }
            }
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb_no = (Label)e.Item.FindControl("no");
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();
            }
        }

        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
    }
}
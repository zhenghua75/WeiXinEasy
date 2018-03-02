using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;
using DAL.Vote;
using Model.Vote;

namespace Mozart.CMSAdmin.Vote
{
    public partial class wfmVotesubjectAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
                {
                    Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                    Response.End();
                }
                txtName.Focus();
                starttime.Attributes.Add("onclick", "WdatePicker()");
                endtime.Attributes.Add("onclick", "WdatePicker()");
                if (Common.Common.NoHtml(Request["action"]) == "save")
                {
                    saveupdate();
                }
            }
        }

        public void saveupdate()
        {
            string strID = Guid.NewGuid().ToString("N").ToUpper();
            VOTE_Subject model = new VOTE_Subject();
            model.ID = strID;
            model.Subject = Common.Common.NoHtml(Request["txtName"]);
            model.SiteCode = GlobalSession.strSiteCode;
            model.Content = Common.Common.NoHtml(Request["Content"]);
            model.BeginTime = Convert.ToDateTime(Request["BeginTime"]);
            model.EndTime = Convert.ToDateTime(Request["endtime"]);
            model.IsValid = Convert.ToInt32(Common.Common.NoHtml(Request["IsValid"]));
            SubjectDAL dal = new SubjectDAL();
            if (dal.AddVoteinfo(model))
            {
                if (Common.Common.NoHtml(Request["Option0"]) != null && Common.Common.NoHtml(Request["Option0"]) != "")
                {
                    OptionDAL optdal = new OptionDAL();
                    VOTE_Option optmoedl = new VOTE_Option();
                    for (int i = 0; i < 6; i++)
                    {
                        optmoedl.Title = Common.Common.NoHtml(Request["Option" + i]);
                        if (Common.Common.NoHtml(Request["Order" + i]) != null && Common.Common.NoHtml(Request["Order" + i]) != "")
                        {
                            optmoedl.Order = Convert.ToInt32(Common.Common.NoHtml(Request["Order" + i]));
                        }
                        optmoedl.ID = Guid.NewGuid().ToString("N").ToUpper();
                        optmoedl.SubjectID = strID;
                        if (Common.Common.NoHtml(Request["Option" + i]) != null && Common.Common.NoHtml(Request["Option" + i]) != "")
                        {
                            optdal.AddVoteOption(optmoedl);
                        }
                    }
                }
                Response.Write("{\"success\":\"true\"}");
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }


        protected void btnReset_Click(object sender, EventArgs e)
        {
            starttime.Text = "";
            endtime.Text = "";
            txtName.Text = "";
        }
    }
}
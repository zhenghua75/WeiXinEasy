using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.Vote;
using Mozart.Common;
using Model.Vote;

namespace Mozart.CMSAdmin.Vote
{
    public partial class wfmVotesubjectUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        public string optionlist;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                #region 初始化界面
                if (null !=  Common.Common.NoHtml(Request.QueryString["action"]))
                {
                    strAction =  Common.Common.NoHtml(Request.QueryString["action"]);
                }
                if (null != Common.Common.NoHtml(Request.QueryString["id"]) && Common.Common.NoHtml(Request.QueryString["id"])!="")
                {
                    strID = Common.Common.NoHtml(Request.QueryString["id"]);
                }
                else
                {
                    strID = Common.Common.NoHtml(Request["upId"]);
                }

                if (strAction.ToLower().Trim() == "save")
                {
                    saveupdate();
                }
                else if (strAction.ToLower().Trim() == "delopid")
                {
                    deloption();
                }
                else
                {
                    ShowActivityInfo(strID);
                }
                #endregion
            }
        }
        public void ShowActivityInfo(string strID)
        {
            SubjectDAL dal = new SubjectDAL();
            DataSet ds = dal.GetVoteDetail(strID, Session["strSiteCode"].ToString());
            VOTE_Subject model = DataConvert.DataRowToModel<VOTE_Subject>(ds.Tables[0].Rows[0]);
            this.txtName.Text = model.Subject;
            this.hd_content.Value = model.Content;
            starttime.Text = model.BeginTime.ToString("yyyy-MM-dd");
            endtime.Text = model.EndTime.ToString("yyyy-MM-dd");

            optionlist = ""; DataSet optionds = dal.GetOptionsData(strID);
            optionlist += "<table id=\"MyOption\">";
            if (optionds != null && optionds.Tables.Count > 0 && optionds.Tables[0].Rows.Count > 0)
            {
                optionlist += "<script type=\"text/javascript\">";
                for (int i = 0; i < optionds.Tables[0].Rows.Count; i++)
                {
                    string opname = optionds.Tables[0].Rows[i]["Title"].ToString();
                    string opid = optionds.Tables[0].Rows[i]["ID"].ToString();
                    string order = optionds.Tables[0].Rows[i]["Order"].ToString();
                    if (optionds.Tables[0].Rows.Count > 2 && Common.Common.NoHtml(Request["action"]) != "show")
                    {
                        optionlist += "AddRow(\"" + opname + "\",\"" + opid + "\",\"" + order + "\",\"del\");";
                    }
                    else
                    {
                        optionlist += "AddRow(\"" + opname + "\",\"" + opid + "\",\"" + order + "\",\"\");";
                    }
                }
                optionlist += "</script>";
            }
            optionlist += "</table>";
            
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
                starttime.ReadOnly = true;
                endtime.ReadOnly = true;
                txtName.ReadOnly = true;
            }
            else
            {
                starttime.Attributes.Add("onclick", "WdatePicker()");
                endtime.Attributes.Add("onclick", "WdatePicker()");
            }
        }

        public void saveupdate()
        {
            VOTE_Subject model = new VOTE_Subject();
            model.ID = strID;
            model.Subject = Common.Common.NoHtml(Request["txtName"]);
            model.SiteCode = GlobalSession.strSiteCode;
            model.Content = Common.Common.NoHtml(Request["Content"]);
            model.BeginTime = Convert.ToDateTime(Request["BeginTime"]);
            model.EndTime = Convert.ToDateTime(Request["endtime"]);
            SubjectDAL dal = new SubjectDAL();
            if (dal.UpdateVoteInfo(model))
            {
                if (Common.Common.NoHtml(Request["Option0"]) != null && Common.Common.NoHtml(Request["Option0"]) != "")
                {
                    OptionDAL optdal = new OptionDAL();
                    VOTE_Option optmoedl = new VOTE_Option();
                    int listlength = 0; optmoedl.IsDel = 1;
                    if (Request["listlength"] != null && Request["listlength"] != "")
                    {
                        listlength = Convert.ToInt32(Common.Common.NoHtml(Request["listlength"]));
                    }
                    for (int i = 0; i < listlength; i++)
                    {
                        string optiontitle = string.Empty;
                        string optionID = string.Empty;
                        string optionOrder = string.Empty;
                        try
                        {
                            optiontitle = Common.Common.NoHtml(Request["Option" + i]);
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                            optionID = Common.Common.NoHtml(Request["Option" + i + "ID"]);
                        }
                        catch (Exception)
                        {
                        }
                        try
                        {
                          optionOrder=Common.Common.NoHtml(Request["Order" + i]);
                        }
                        catch (Exception)
                        {}
                        if (optiontitle != null && optiontitle != "")
                        {
                            optmoedl.Title = optiontitle;
                            if (optionOrder != null && optionOrder != "")
                            {
                                optmoedl.Order = Convert.ToInt32(optionOrder);
                            }
                            if (optdal.IsexistOption(optionID))
                            {
                                optmoedl.ID = optionID;
                                optdal.UpdateOption(optmoedl);
                            }
                            else if (optdal.ExistOptionTile(optiontitle))
                            {
                                Response.Write("{\"success\":\"该选项已经存在\"}");
                            }
                            else
                            {
                                optmoedl.ID = Guid.NewGuid().ToString("N").ToUpper();
                                optmoedl.SubjectID = strID;
                                if (optiontitle != null && optiontitle != "")
                                {
                                    optdal.AddVoteOption(optmoedl);
                                }
                            }
                        }
                    }
                }
                Response.Write("{\"success\":\"true\"}");
            }
            else
            {
                Response.Write("{\"success\":\"修改失败\"}");
            }
            Response.End();
        }
        public void deloption()
        {
            OptionDAL dal = new OptionDAL();
            string deloptionID = string.Empty;
            try
            {
                deloptionID=Common.Common.NoHtml(Request["delopid"]);
            }
            catch (Exception)
            {
            }
            if (deloptionID != null && deloptionID != "")
            {
                if (dal.DelOption(deloptionID))
                {
                    Response.Write("{\"success\":\"true\"}");
                }
                else
                {
                    Response.Write("{\"success\":\"操作失败，请重新操作\"}");
                }
            }
            else
            {
                Response.Write("{\"success\":\"true\"}");
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.JC;
using Model.JC;

namespace Mozart.CMSAdmin.JC
{
    public partial class wfmJCQuizWin : System.Web.UI.Page
    {
        const string vsKey = "searchCriteria"; //ViewState key
        static string quizID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["quizid"] == null && Request["quizid"] == "")
                {
                    return;
                }
                else
                {
                    AspNetPager1.CurrentPageIndex = 1;
                    string s = "";
                    if (Session["strRoleCode"].ToString() != "ADMIN")
                    {
                        s = " SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                    }
                    ViewState[vsKey] = s;
                    quizID = Request["quizid"];
                    losewindatalistbind();
                }
            }
        }

        void losewindatalistbind()
        {
            JC_ScoreDAL dal = new JC_ScoreDAL();
            DataSet ds = null;
            if (Session["losewin"] != null && Session["losewin"].ToString() != "")
            {
                ds = dal.GetLoseWin(quizID, GetLoseWin());
                GuessTooTip.Text = "竞猜输赢";
            }
            else
            {
                ds = dal.GetRightGuessTop(quizID);
                GuessTooTip.Text = "竞猜比分";
            }
            DataView dv = ds.Tables[0].DefaultView;
            AspNetPager1.RecordCount = dv.Count;
            PagedDataSource pds = new PagedDataSource();
            pds.DataSource = dv;
            pds.AllowPaging = true;
            pds.CurrentPageIndex = AspNetPager1.CurrentPageIndex - 1;
            pds.PageSize = AspNetPager1.PageSize;
            sortrepeat.DataSource = pds;
            sortrepeat.DataBind();
        }

        string GetLoseWin()
        {
            JC_QuizDAL dal = new JC_QuizDAL();
            string Score = string.Empty;
            try
            {
                Score = dal.GetQuizValueById("RightScore", quizID).ToString();
            }
            catch (Exception)
            {
            }
            int HomeNum = 0; int VisitNum = 0;
            string[] sArray = Score.Split(':');
            string[] sArray1 = Score.Split('：');
            try
            {
                HomeNum = Convert.ToInt32(sArray[0].ToString());
                VisitNum = Convert.ToInt32(sArray[1].ToString());
            }
            catch (Exception)
            {
                HomeNum = Convert.ToInt32(sArray1[0].ToString());
                VisitNum = Convert.ToInt32(sArray1[1].ToString());
            }
            if (HomeNum > VisitNum)
            {
                return ">";
            }
            else if (HomeNum == VisitNum)
            {
                return "=";
            }
            else
            {
                return "<";
            }
        }

        protected void sortrepeat_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb_no = (Label)e.Item.FindControl("no");
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();
            }
        }
        protected void AspNetPager1_PageChanged(object src, EventArgs e)
        {
            losewindatalistbind();
        }

        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }
        
        protected void sortbtn_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            Session["losewin"] = "losewin";
            losewindatalistbind();
        }

        protected void guessscore_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            Session.Remove("losewin");
            losewindatalistbind();
        }
    }
}
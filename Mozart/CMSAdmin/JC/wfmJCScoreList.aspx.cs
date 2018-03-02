using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.JC;
using Model.JC;
using Mozart.Common;
using DAL.ACT;

namespace Mozart.CMSAdmin.JC
{
    public partial class wfmJCScoreList : System.Web.UI.Page
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
                AspNetPager1.CurrentPageIndex = 1;
                string s = "";
                if (Session["strRoleCode"].ToString() != "ADMIN")
                {
                    s = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
                }
                ViewState[vsKey] = s;

                ddlselectName.Items.Clear();
                JC_QuizDAL jcquizdal = new JC_QuizDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = jcquizdal.GetJCQuizDataList("");
                }
                else
                {
                    ds = jcquizdal.GetJCQuizDataList(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                ddlselectName.DataSource = ds.Tables[0].DefaultView;
                ddlselectName.DataTextField = "Name";
                ddlselectName.DataValueField = "ID";
                ddlselectName.DataBind();

                LoadData(s);
            }
        }


        string GetLoseWin(string quizID)
        {
            JC_QuizDAL dal = new JC_QuizDAL();
            string Score = string.Empty;
            try
            {
                Score = dal.GetQuizValueById("RightScore", quizID).ToString();
            }
            catch (Exception)
            {
                return "0";
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

        /// <summary>
        /// 单击"查询"按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnQuery_Click(object sender, EventArgs e)
        {
            string strWhere = "  "; 
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }

            if (ddlselectName.SelectedValue.Trim() != null && ddlselectName.SelectedValue.Trim() != ""&&
                ddlselectName.SelectedValue.Trim() !="0")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " b.ID='" + ddlselectName.SelectedValue + "' ";
            }
            if (ddlselectlosewin.SelectedValue.Trim() != null && ddlselectlosewin.SelectedValue.Trim() != "" &&
                ddlselectlosewin.SelectedValue.Trim() != "0")
            {
                if (ddlselectName.SelectedValue.Trim() != null && ddlselectName.SelectedValue.Trim() != "" &&
                               ddlselectName.SelectedValue.Trim() != "0")
                {
                    if (strWhere.Trim() != null && strWhere.Trim() != "")
                    {
                        strWhere = strWhere + " AND ";
                    }
                    string arrow = GetLoseWin(ddlselectName.SelectedValue);

                    if (arrow == "0")
                    {
                        MessageBox.Show(this, "请设置正确的比赛结果再做统计!");
                        return;
                    }

                    switch (ddlselectlosewin.SelectedValue.Trim())
                    {
                        case "2":
                            if (arrow.Trim() == ">" || arrow.Trim() == "=")
                            {
                                arrow = "<";
                            }
                            else
                            {
                                arrow = ">";
                            }
                            break;
                        default:
                            break;
                    }
                    strWhere = strWhere + " [dbo].[split](GuessScore,':',0) " + arrow + " [dbo].[split](GuessScore,':',1) ";
                }
                else
                {
                    MessageBox.Show(this, "操作失败，请选择相应的赛事!"); return;
                }
            }
            if (ddlselectscore.SelectedValue.Trim() != null && ddlselectscore.SelectedValue.Trim() != ""&&
                ddlselectscore.SelectedValue.Trim() !="0")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                string scoreright = string.Empty;
                switch (ddlselectscore.SelectedValue)
                {
                    case "1":
                        scoreright =  " = ";
                        break;
                    default:
                        scoreright = " <> ";
                        break;
                }
                strWhere = strWhere + " GuessScore"+scoreright+"RightScore ";
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
            Session.Remove("sort");
            JC_ScoreDAL dal = new JC_ScoreDAL();
            DataSet ds = dal.GetJCScoreListByState(strWhere);
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
        string losewin(string guessscore,string rightscore)
        {
            string[] arry = rightscore.Split(':');
            string[] arry1 = rightscore.Split('：');
            string[] guessarry = guessscore.Split(':');
            string[] guessarry1 = guessscore.Split('：');
            int score1 = 0; int score2 = 0; int score3 = 0; int score4 = 0;
            try
            {
                score1 = Convert.ToInt32(arry[0]);
            }
            catch (Exception)
            {
                score1 = Convert.ToInt32(arry1[0]);
            }
            try
            {
                score2 = Convert.ToInt32(arry[1]);
            }
            catch (Exception)
            {
                score2 = Convert.ToInt32(arry1[1]);
            }
            try
            {
                score3 = Convert.ToInt32(guessarry[0]);
            }
            catch (Exception)
            {
                score3 = Convert.ToInt32(guessarry1[0]);
            }
            try
            {
                score4 = Convert.ToInt32(guessarry[1]);
            }
            catch (Exception)
            {
                score4= Convert.ToInt32(guessarry1[1]);
            }
            string lowsewin = "";
            if (score1 > score2)
            {
                if (score3 > score4)
                {
                    lowsewin = "胜";
                }
                else 
                {
                    lowsewin = "负";
                }
            }
            else if (score1 == score2)
            {
                if (score3 == score4)
                {
                    lowsewin = "胜";
                }
                else
                {
                    lowsewin = "负";
                }
            }
            else
            {
                if (score3<score4)
                {
                    lowsewin = "胜";
                }
                else
                {
                    lowsewin = "负";
                }
            }
            return lowsewin;
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Label lb_no = (Label)e.Item.FindControl("no");
                lb_no.Text = (1 + e.Item.ItemIndex).ToString();
                Label lb_HomeTeamImg = (Label)e.Item.FindControl("HomeTeamImg");
                Label lb_VisitingTeamImg = (Label)e.Item.FindControl("VisitingTeamImg");
                if (lb_HomeTeamImg.Text != null && lb_HomeTeamImg.Text != "")
                {
                    lb_HomeTeamImg.Text = "<img src=\"../../" + lb_HomeTeamImg.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }
                if (lb_VisitingTeamImg.Text != null && lb_VisitingTeamImg.Text != "")
                {
                    lb_VisitingTeamImg.Text = "<img src=\"../../" + lb_VisitingTeamImg.Text +
                        "\" width=\"20px\" height=\"20px\" border=\"0px\">";
                }

                Label lb_guessscore = (Label)e.Item.FindControl("GuessScore");
                Label lb_rightscore = (Label)e.Item.FindControl("RightScore");
                Label lb_losewin = (Label)e.Item.FindControl("losewin");
                Label lb_score = (Label)e.Item.FindControl("score");
                if (lb_guessscore.Text.Trim() != null && lb_guessscore.Text.Trim() != "" &&
                    lb_rightscore.Text.Trim() != null && lb_rightscore.Text.Trim() != "")
                {
                    lb_losewin.Text = losewin(lb_guessscore.Text, lb_rightscore.Text);
                    if (lb_guessscore.Text.Trim() == lb_rightscore.Text.Trim())
                    {
                        lb_score.Text = "是";
                    }
                    else
                    {
                        lb_score.Text = "否";
                    }
                }
                else
                {
                    lb_losewin.Text = ""; lb_score.Text = "";
                }

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
            LoadData((string)ViewState[vsKey]);
        }

        /// </summary>
        /// <param name="control"></param>
        public override void VerifyRenderingInServerForm(Control control)
        {
            //base.VerifyRenderingInServerForm(control);
        }

        protected void btnCoupon_Click(object sender, EventArgs e)
        {
            string strWhere = "  ";
            if (Session["strRoleCode"].ToString() != "ADMIN")
            {
                strWhere = " a.SiteCode = '" + Session["strSiteCode"].ToString() + "' ";
            }

            if (ddlselectName.SelectedValue.Trim() != null && ddlselectName.SelectedValue.Trim() != "" &&
                ddlselectName.SelectedValue.Trim() != "0")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                strWhere = strWhere + " b.ID='" + ddlselectName.SelectedValue + "' ";
            }

            if (ddlselectName.SelectedValue.Trim() != null && ddlselectName.SelectedValue.Trim() != "" &&
                           ddlselectName.SelectedValue.Trim() != "0")
            {
                if (strWhere.Trim() != null && strWhere.Trim() != "")
                {
                    strWhere = strWhere + " AND ";
                }
                string arrow = GetLoseWin(ddlselectName.SelectedValue);

                if (arrow == "0")
                {
                    MessageBox.Show(this, "请设置正确的比赛结果再做统计!");
                    return;
                }

                strWhere = strWhere + " [dbo].[split](GuessScore,':',0) " + arrow + " [dbo].[split](GuessScore,':',1) ";
            }
            else
            {
                MessageBox.Show(this, "操作失败，请选择相应的赛事!"); return;
            }

            JC_ScoreDAL dal = new JC_ScoreDAL();
            DataSet ds = dal.GetJCScoreListByStateNO(strWhere);

            //取正确比分
            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string strOpenID = row["OpenID"].ToString();
                string strScoreID = row["ID"].ToString();
                string strGuessScore = row["GuessScore"].ToString();
                string strRightScore = row["RightScore"].ToString();
                //修改竞猜状态;
                dal.UpdateJCScoreState(strScoreID);
                //写入优惠券
                //AC87CE288FC24F23B7B68CE8F3D93D13
                string strGuid = Guid.NewGuid().ToString("N");
                CouponDAL cdal = new CouponDAL();
                string strSiteCode = Session["strSiteCode"].ToString();
                Model.ACT.Coupon coupon = null;
                if (strGuessScore == strRightScore)
                {
                    coupon = new Model.ACT.Coupon()
                    {
                        ID = strGuid,
                        SiteCode = Session["strSiteCode"].ToString(),
                        SiteActivityID = "AC87CE288FC24F23B7B68CE8F3D93D18",
                        OpenID = strOpenID,
                        CouponStatus = 0
                    };
                }
                else
                {
                   coupon = new Model.ACT.Coupon()
                   {
                       ID = strGuid,
                       SiteCode = Session["strSiteCode"].ToString(),
                       SiteActivityID = "AC87CE288FC24F23B7B68CE8F3D93D22",
                       OpenID = strOpenID,
                       CouponStatus = 0
                   };
                }
                cdal.InsertInfo(coupon);

            }

            MessageBox.Show(this, "优惠券发放完成!");

        }
        
    }
}
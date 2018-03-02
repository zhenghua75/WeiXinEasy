using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Model.MiniShop;
using Mozart.Common;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductParaUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
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
                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    showdetailinfo();
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }
        void showdetailinfo()
        {
            MSProductParaDAL ParaDal = new MSProductParaDAL();
            DataSet ParaDs = ParaDal.GetParaDetail(strID);
            MSProductPara ParaModel = DataConvert.DataRowToModel<MSProductPara>(ParaDs.Tables[0].Rows[0]);
            paraname.Text = ParaModel.ParName;
            //paravalue.Text = ParaModel.ParValue;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        } 
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (paraname.Text.Trim() != null && paraname.Text.Trim() != "" &&
                    paravalue.Text.Trim() != null && paravalue.Text.Trim() != "")
                {
                    MSProductParaDAL ParaDal = new MSProductParaDAL();
                    MSProductPara paraModel = new MSProductPara();
                    paraModel.ParName = paraname.Text;
                    paraModel.ParState = 0;
                    paraModel.ID = strID;
                    //paraModel.ParValue = paravalue.Text;
                    if (ParaDal.UpdateMSPPara(paraModel))
                    {
                        MessageBox.Show(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.Show(this, "操作失败！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请输入相应名称或参数值！");
                }
            }
            else
            {
                return;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            paravalue.Text = ""; paraname.Text = ""; 
        }
    }
}
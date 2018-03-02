using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using Mozart.Common;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductOrderUpdate : System.Web.UI.Page
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
            MSShoppingCartDAL orderDal = new MSShoppingCartDAL();
            string strWhere = string.Empty;
            strWhere = " and a.ID='"+strID+"' ";
            DataSet orderds = orderDal.GetCustomerOrderList(strWhere);
            string paytime = string.Empty;
            if (orderds != null && orderds.Tables.Count > 0 && orderds.Tables[0].Rows.Count > 0)
            {
                ptitle.Text = orderds.Tables[0].Rows[0]["Ptitle"].ToString();
                Quantity.Text = orderds.Tables[0].Rows[0]["Quantity"].ToString();
                UnitCost.Text = orderds.Tables[0].Rows[0]["UnitCost"].ToString();
                shopname.Text = orderds.Tables[0].Rows[0]["ShopName"].ToString();
                paytime=orderds.Tables[0].Rows[0]["PayTime"].ToString();
                buyname.Text = orderds.Tables[0].Rows[0]["buyname"].ToString();
                zipcode.Text = orderds.Tables[0].Rows[0]["zipcode"].ToString();
                phone.Text = orderds.Tables[0].Rows[0]["phone"].ToString();
                ReveiveAddress.Text = orderds.Tables[0].Rows[0]["ReveiveAddress"].ToString();
                string pay = orderds.Tables[0].Rows[0]["PayWay"].ToString();
                string[] way = pay.Split('|');
                try
                {
                    pay = way[1].ToString();
                }
                catch (Exception)
                {
                }
                payway.Text = pay;
                
            }
            if (paytime.ToString().Trim() != "1900-01-0100:00:00.000")
            {
                PayTime.Text = orderds.Tables[0].Rows[0]["PayTime"].ToString();
            }
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
                
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            
        }
    }
}
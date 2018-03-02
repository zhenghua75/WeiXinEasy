using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using System.Text;
using Mozart.Common;
using Model.MiniShop;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmSecHandProductView : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        public static string atlaslist = string.Empty;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != ""
                    && Session["strLoginName"].ToString().ToLower().Trim() == "vyigo")
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
            MSProductDAL productDal = new MSProductDAL();
            DataSet productDs = productDal.GetProductDetail(strID);
            MSProduct productModel = DataConvert.DataRowToModel<MSProduct>(productDs.Tables[0].Rows[0]);
            price.Text = productModel.Price.ToString();
            pname.Text = productModel.Ptitle;
            hd_content.Value = productModel.Pcontent;
            if (productModel.IsSecHand == 0)
            {
                isstateno.Checked = true;
            }
            else
            {
                isstateyes.Checked = true;
            }
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
            MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
            DataSet atlasds = atlasDal.GetProductAtlasByPID(strID);
            if (atlasds != null && atlasds.Tables.Count > 0 && atlasds.Tables[0].Rows.Count > 0)
            {
                atlaslist = ""; string atlasimg = string.Empty;
                for (int i = 0; i < atlasds.Tables[0].Rows.Count; i++)
                {
                    atlasimg = atlasds.Tables[0].Rows[i]["PimgUrl"].ToString();
                    atlaslist += "<img src=\"../../PalmShop/ShopCode/"+atlasimg+"\" />";
                }
            }
            MSShopContactsDAL contactDal = new MSShopContactsDAL();
            DataSet contactDs = contactDal.GetContactDetailByPID(strID);
            if (contactDs != null && contactDs.Tables.Count > 0 && contactDs.Tables[0].Rows.Count > 0)
            {
                string uphone = string.Empty; string uname = string.Empty;
                uphone = contactDs.Tables[0].Rows[0]["Phone"].ToString();
                uname = contactDs.Tables[0].Rows[0]["NickName"].ToString();
                UserContact.Text = "联系电话：" + uphone + " &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                    "联系人：" + uname;

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (pname.Text.Trim() != null && pname.Text.Trim() != "" &&
                    price.Text.Trim() != null && price.Text.Trim() != "")
                {
                    MSProductDAL productdal = new MSProductDAL();
                    MSProduct productmodel = new MSProduct();
                    productmodel.Pstate = 0;
                    productmodel.ID = strID;
                    productmodel.Ptitle = pname.Text;
                    productmodel.Price = decimal.Parse(price.Text);
                    productmodel.Pcontent = hd_content.Value;
                    productmodel.IsSecHand = isstateno.Checked ? 0 : 1;
                    if (productdal.UpdateMSProduct(productmodel))
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
                    MessageBox.Show(this, "请输入相应标题或价格！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            pname.Text = ""; price.Text = "";
            hd_content.Value = "";
        }
    }
}
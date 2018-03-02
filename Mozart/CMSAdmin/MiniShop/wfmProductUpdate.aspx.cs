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
    public partial class wfmProductUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        public static string pataslist = string.Empty;
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
            MSProductDAL productDal = new MSProductDAL();
            DataSet productDs = productDal.GetProductDetail(strID);
            MSProduct productModel = DataConvert.DataRowToModel<MSProduct>(productDs.Tables[0].Rows[0]);
            price.Text = productModel.Price.ToString();
            pname.Text = productModel.Ptitle;
            hd_content.Value=productModel.Pcontent;
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

            MSProductAtlasDAL AtlasDal = new MSProductAtlasDAL();
            DataSet atlasds = AtlasDal.GetProductAtlasList(" AND PID='" + strID + "' ");
            if (atlasds != null && atlasds.Tables.Count > 0 && atlasds.Tables[0].Rows.Count > 0)
            {
                pataslist = ""; string atlasimg = "";
                for (int i = 0; i < atlasds.Tables[0].Rows.Count; i++)
                {
                    atlasimg = atlasds.Tables[0].Rows[i]["PimgUrl"].ToString();
                    pataslist += "<img src=\"../../PalmShop/ShopCode/" + atlasimg + "\" />";
                }
            }
 
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                bool rowcount = false;
                MSProductParaDAL paraDal = new MSProductParaDAL();
                rowcount = paraDal.ExistMSPPara("", strID);
                if (pname.Text.Trim() != null && pname.Text.Trim() != "" &&
                    price.Text.Trim() != null && price.Text.Trim() != "")
                {
                    MSProductDAL productdal = new MSProductDAL();
                    MSProduct productmodel = new MSProduct();
                    productmodel.Pstate = 0;
                    productmodel.ID = strID;
                    productmodel.Ptitle = pname.Text;
                    if (rowcount == true)
                    {
                        productmodel.Price =0;
                    }
                    else
                    {
                        productmodel.Price = decimal.Parse(price.Text);
                    }
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
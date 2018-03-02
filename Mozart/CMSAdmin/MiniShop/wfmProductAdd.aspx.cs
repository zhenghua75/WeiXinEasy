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
using System.Text;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductAdd : System.Web.UI.Page
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
                    if (strAction.Trim() != null && strAction.Trim() != "" && strAction.Trim().ToLower() == "getoption")
                    {
                        GetOptionList();
                    }
                    else
                    {
                        ddlshoplist.Items.Clear();
                        MSShopDAL actDal = new MSShopDAL();
                        DataSet ds = new DataSet();
                        ds = actDal.GetMSShopList("");
                        DataTable dt = ds.Tables[0];
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["ID"] = "";
                        dr["ShopName"] = "--请选择相应店铺--";
                        dt.Rows.InsertAt(dr, 0);
                        ddlshoplist.DataSource = ds.Tables[0].DefaultView;
                        ddlshoplist.DataTextField = "ShopName";
                        ddlshoplist.DataValueField = "ID";
                        ddlshoplist.DataBind();
                    }
                    #endregion
                }
                else
                {
                    return;
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (ddlshoplist.SelectedValue.Trim() != null && ddlshoplist.SelectedValue.Trim() != "" &&
                        setoptionvalue.Value.Trim() != null && setoptionvalue.Value.Trim() != "")
                {
                if (pname.Text.Trim() != null && pname.Text.Trim() != "" &&
                    price.Text.Trim() != null && price.Text.Trim() != "" )
                {
                        MSProductDAL productdal = new MSProductDAL();
                        if (productdal.ExistMSProduct(pname.Text, setoptionvalue.Value, ddlshoplist.SelectedValue))
                        {
                            MessageBox.Show(this, "该商品已经存在！");
                        }
                        else
                        {
                            MSProduct productmodel = new MSProduct();
                            productmodel.Pstate = 0;
                            productmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                            productmodel.Ptitle = pname.Text;
                            productmodel.Price = decimal.Parse(price.Text);
                            productmodel.SID = ddlshoplist.SelectedValue;
                            productmodel.Pcontent = hd_content.Value;
                            productmodel.Cid = setoptionvalue.Value;
                            productmodel.IsSecHand = isstateno.Checked ? 0 : 1;
                            if (productdal.AddMSProduct(productmodel))
                            {
                                MessageBox.Show(this, "操作成功！");
                            }
                            else
                            {
                                MessageBox.Show(this, "操作失败！");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "请输入相应标题或价格！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请选择相应店铺和类别！");
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
        void GetOptionList()
        {
            string subid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }
            if (subid.Trim() != null && subid.Trim() != "")
            {
                MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                DataSet optionds;
                optionds = categoryDal.GetMSPCList(" and [SID]='" + subid + "' ");
                Response.Write(Dataset2Json(optionds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTable2Json(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\"")); //对于特殊字符，还应该进行特别的处理。 
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }
    }
}
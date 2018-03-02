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
using System.Text;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductParaAdd : System.Web.UI.Page
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

                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    if (strAction.Trim() != null && strAction.Trim() != "")
                    {
                        switch (strAction.Trim().ToLower())
                        {
                            case "getcateoption":
                                GetOptionList();
                                break;
                            case "getpoption":
                                GetCatOptionList();
                                break;
                        }
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
                if (paraname.Text.Trim() != null && paraname.Text.Trim() != "" &&
                    paravalue.Text.Trim()!=null&&paravalue.Text.Trim()!=""&&
                    setpvalue.Value.Trim() != null && setpvalue.Value.Trim() != "")
                {
                    MSProductParaDAL ParaDal = new MSProductParaDAL();
                    if (ParaDal.ExistMSPPara(paraname.Text, setpvalue.Value))
                    {
                        MessageBox.Show(this, "该参数已经存在！");
                    }
                    else
                    {
                        MSProductPara paraModel = new MSProductPara();
                        paraModel.PID = setpvalue.Value;
                        paraModel.ParName = paraname.Text;
                        //paraModel.ParValue = paravalue.Text;
                        paraModel.ParState = 0;
                        paraModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        if (ParaDal.AddMSPPara(paraModel))
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
                    MessageBox.Show(this, "请选择相应产品或参数值！");
                }
            }
            else
            {
                return;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            paravalue.Text = ""; paraname.Text = ""; setpvalue.Value = "";
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
                MSProductCategoryDAL CateDal = new MSProductCategoryDAL();
                DataSet optionds;
                optionds = CateDal.GetMSPCList(" and a.[SID]='" + subid + "' ");
                Response.Write(Dataset2Json(optionds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        void GetCatOptionList()
        {
            string subid = string.Empty; string catid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }
            if (Request["catid"] != null && Request["catid"] != "")
            {
                catid = Common.Common.NoHtml(Request["catid"]);
            }
            else
            {
                return;
            }
            if (subid.Trim() != null && subid.Trim() != "" && catid.Trim() != null && catid.Trim() != "")
            {
                MSProductDAL productDal = new MSProductDAL();
                DataSet optionds;
                optionds = productDal.GetMSProductList(" and a.[SID]='" + subid + "' and a.[CID]='" + catid + "' ");
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
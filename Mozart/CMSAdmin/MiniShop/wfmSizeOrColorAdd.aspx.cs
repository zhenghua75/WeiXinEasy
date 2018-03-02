﻿using System;
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
    public partial class wfmSizeOrColorAdd : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
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
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
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
                if (ddlshoplist.SelectedValue.Trim() != null && ddlshoplist.SelectedValue.Trim() != "")
                {
                    if (setsortvalue.Value.Trim() != null && setsortvalue.Value.Trim() != "")
                    {
                        if (setpvalue.Value.Trim() != null && setpvalue.Value.Trim() != "")
                        {
                            if (scname.Text.Trim() != null && scname.Text.Trim() != "")
                            {
                                #region 上传图像
                                string shopPhone = string.Empty;
                                MSShopDAL shopdal = new MSShopDAL();
                                shopPhone = shopdal.GetMSShopValueByID("Phone", ddlshoplist.SelectedValue).ToString();
                                //上传图像
                                string strIconFileName = string.Empty;//图像路径
                                string strIconSaveFileName = string.Empty;//网址路径
                                try
                                {
                                    if (this.file0.PostedFile.FileName == "")
                                    {
                                        strIconSaveFileName = "";
                                    }
                                    else
                                    {
                                        if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Atlas"))
                                        {
                                            System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Atlas");
                                        }
                                        if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/Atlas/{1}", Server.MapPath("~"), shopPhone)))
                                        {
                                            System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/Atlas/{1}", Server.MapPath("~"), shopPhone));
                                        }
                                        string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                                        string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                                        if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                                        {
                                            MessageBox.Show(this, "文件格式有误！");
                                            return;

                                        }//检查文件格式
                                        string newName = String.Format("{0}_{1}pcolor{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                                        strIconFileName = String.Format(@"{0}PalmShop/ShopCode/Atlas/{1}/{2}", Server.MapPath("~"), shopPhone, newName);
                                        strIconSaveFileName = String.Format(@"Atlas/{0}/{1}", shopPhone, newName);
                                        file0.PostedFile.SaveAs(strIconFileName);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                                }
                                #endregion
                                MSSizeOrColor sizeorcolorModel = new MSSizeOrColor();
                                MSSizeOrColorDAL sizeorcolorDal = new MSSizeOrColorDAL();
                                sizeorcolorModel.PID = setpvalue.Value;
                                sizeorcolorModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                                sizeorcolorModel.SizeColor = Convert.ToInt32(sizeorcolor.SelectedValue);
                                sizeorcolorModel.IsDel = 0;
                                sizeorcolorModel.Scname = scname.Text;
                                if (stock.Text.Trim() != null && stock.Text.Trim() != "")
                                {
                                    sizeorcolorModel.Stock = Convert.ToInt32(stock.Text);
                                }
                                if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                                {
                                    sizeorcolorModel.Scimg = strIconSaveFileName;
                                }
                                if (sizeorcolorDal.AddSizeOrColor(sizeorcolorModel))
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
                                MessageBox.Show(this, "请填写相应的名称！");
                            }
                        }
                        else
                        {
                            MessageBox.Show(this, "请选择相应的产品！");
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "请选择相应的类别！");
                    }
                }
                else
                {
                    MessageBox.Show(this, "请选择相应的店铺！");
                }
            }
            else
            {
                return;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            scname.Text = ""; stock.Text = "";
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.DC;
using Model.DC;
using Mozart.Common;

namespace Mozart.CMSAdmin.DC
{
    public partial class wfmDCHouseUpdate : System.Web.UI.Page
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
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
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
                ShowArticleInfo(strID);
                #endregion
            }
        }

        public void ShowArticleInfo(string strID)
        {
            DC_HouseDAL dal = new DC_HouseDAL();
            DataSet ds= dal.GetDCHouseDetail(strID);
            DC_House model = DataConvert.DataRowToModel<DC_House>(ds.Tables[0].Rows[0]);
            Summary.Text = model.Summary;
            img0.Src = "../../" + model.Photo;
            ddlCategory.SelectedIndex = ddlCategory.Items.IndexOf(ddlCategory.Items.FindByValue(model.SaleRental.ToString()));
            Price.Text = model.Price.ToString();
            HouseType.Text = model.HouseType;
            Faces.Text = model.Faces;
            Area.Text = model.Area.ToString();
            Renovation.Text = model.Renovation;
            Floor.Text = model.Floor;
            UseType.Text = model.UseType;
            Buildings.Text = model.Buildings;
            CreateYear.Text = model.CreateYear;
            Regions.Text = model.Regions;
            Address.Text = model.Address;
            hd_content.Value = model.Content;
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Index.aspx';</script>");
                Response.End();
            }
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
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Images"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Images");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/Images/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strIconFileName = String.Format(@"{0}Images/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strIconSaveFileName = String.Format(@"Images/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file0.PostedFile.SaveAs(strIconFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }

            DC_HouseDAL dal = new DC_HouseDAL();
            DC_House model = new DC_House();
            model.ID = strID;
            model.SiteCode = Session["strSiteCode"].ToString();
            if (Summary.Text.Trim() != null && Summary.Text.Trim() != "")
            {
                model.Summary = Summary.Text;
            }
            if (strIconSaveFileName != null && strIconSaveFileName != "")
            {
                model.Photo = strIconSaveFileName;
            }
            if (ddlCategory.SelectedValue != null && ddlCategory.SelectedValue != "")
            {
                model.SaleRental = Convert.ToInt32(ddlCategory.SelectedValue);
            }
            if (Price.Text.Trim() != null && Price.Text.Trim() != "")
            {
                model.Price = Convert.ToDecimal(Price.Text);
            }
            if (HouseType.Text.Trim() != null && HouseType.Text.Trim() != "")
            {
                model.HouseType = HouseType.Text;
            }
            if (Faces.Text.Trim() != null && Faces.Text.Trim() != "")
            {
                model.Faces = Faces.Text;
            }
            if (Area.Text.Trim() != null && Area.Text.Trim() != "")
            {
                model.Area = Convert.ToDecimal(Area.Text);
            }
            if (Renovation.Text.Trim() != null && Renovation.Text.Trim() != "")
            {
                model.Renovation = Renovation.Text;
            }
            if (Floor.Text.Trim() != null && Floor.Text.Trim() != "")
            {
                model.Floor = Floor.Text;
            }
            if (UseType.Text.Trim() != null && UseType.Text.Trim() != "")
            {
                model.UseType = UseType.Text;
            }
            if (Buildings.Text.Trim() != null && Buildings.Text.Trim() != "")
            {
                model.Buildings = Buildings.Text;
            }
            if (CreateYear.Text.Trim() != null && CreateYear.Text.Trim() != "")
            {
                model.CreateYear = CreateYear.Text;
            }
            if (Regions.Text.Trim() != null && Regions.Text.Trim() != "")
            {
                model.Regions = Regions.Text;
            }
            if (Address.Text.Trim() != null && Address.Text.Trim() != "")
            {
                model.Address = Address.Text;
            }
            if (hd_content.Value.Trim() != null && hd_content.Value.Trim() != "")
            {
                model.Content = hd_content.Value;
            }
            if (dal.UpdateDCHouse(model))
            {
                MessageBox.Show(this, "操作成功！");
            }
            else
            {
                MessageBox.Show(this, "操作失败！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            Summary.Text = "";
            Price.Text = ""; HouseType.Text = ""; Faces.Text = ""; Area.Text = "";
            Renovation.Text = ""; Floor.Text = ""; UseType.Text = ""; Buildings.Text = "";
            CreateYear.Text = ""; Regions.Text = ""; Address.Text = "";
            hd_content.Value = "";
        }
    }
}
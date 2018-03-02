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
    public partial class wfmDCBuildingsUpdate : System.Web.UI.Page
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
            DC_BuildingsDAL dal = new DC_BuildingsDAL();
            DataSet ds = dal.GetDCBuildingDetail(strID);
            DC_Building model = DataConvert.DataRowToModel<DC_Building>(ds.Tables[0].Rows[0]);
            txtName.Text = model.Name;
            img0.Src =  "../../"+model.Photo;
            AVEPrice.Text = model.AVEPrice.ToString();
            GreenRate.Text = model.GreenRate.ToString();
            VolumeRate.Text = model.VolumeRate.ToString();
            School.Text = model.School;
            ParkingSpaces.Text = model.ParkingSpaces;
            PropertyDevelopers.Text = model.PropertyDevelopers;
            PropertyCompany.Text = model.PropertyCompany;
            Regions.Text = model.Regions;
            hd_content.Value = model.Content;
            BusLine.Text = model.BusLine;
            Address.Text = model.Address;
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

            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                DC_Building model = new DC_Building();
                DC_BuildingsDAL dal = new DC_BuildingsDAL();
                if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
                {
                    model.Name = txtName.Text;
                }
                if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                {
                    model.Photo = strIconSaveFileName;
                }
                if (AVEPrice.Text.Trim() != null && AVEPrice.Text.Trim() != "")
                {
                    model.AVEPrice = Convert.ToDecimal(AVEPrice.Text);
                }
                if (GreenRate.Text.Trim() != null && GreenRate.Text.Trim() != "")
                {
                    model.GreenRate = Convert.ToDecimal(GreenRate.Text);
                }
                if (VolumeRate.Text.Trim() != null && VolumeRate.Text.Trim() != "")
                {
                    model.VolumeRate = Convert.ToDecimal(VolumeRate.Text);
                }
                if (School.Text.Trim() != null && School.Text.Trim() != "")
                {
                    model.School = School.Text;
                }
                if (ParkingSpaces.Text.Trim() != null && ParkingSpaces.Text.Trim() != "")
                {
                    model.ParkingSpaces = ParkingSpaces.Text;
                }
                if (PropertyDevelopers.Text.Trim() != null && PropertyDevelopers.Text.Trim() != "")
                {
                    model.PropertyDevelopers = PropertyDevelopers.Text;
                }
                if (PropertyCompany.Text.Trim() != null && PropertyCompany.Text.Trim() != "")
                {
                    model.PropertyCompany = PropertyCompany.Text;
                }
                if (Regions.Text.Trim() != null && Regions.Text.Trim() != "")
                {
                    model.Regions = Regions.Text;
                }
                if (hd_content.Value.Trim() != null && hd_content.Value.Trim() != "")
                {
                    model.Content = hd_content.Value;
                }
                if (Address.Text.Trim() != null && Address.Text.Trim() != "")
                {
                    model.Address = Address.Text;
                }
                if (BusLine.Text.Trim() != null && BusLine.Text.Trim() != "")
                {
                    model.BusLine = BusLine.Text;
                }
                model.ID =strID;
                model.SiteCode = Session["strSiteCode"].ToString();
                if (dal.UpdateDCBuilding(model))
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
                MessageBox.Show(this, "请输入信息名称后再操作！");
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = ""; AVEPrice.Text = ""; GreenRate.Text = ""; VolumeRate.Text = ""; School.Text = "";
            ParkingSpaces.Text = ""; PropertyDevelopers.Text = ""; PropertyCompany.Text = ""; Regions.Text = "";
            BusLine.Text = ""; Address.Text = ""; hd_content.Value = "";
        }
    }
}
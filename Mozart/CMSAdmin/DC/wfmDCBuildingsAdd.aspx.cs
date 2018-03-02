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
    public partial class wfmDCBuildingsAdd : System.Web.UI.Page
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
                #endregion
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
                model.Name = txtName.Text;
                model.Photo = strIconSaveFileName;
                model.AVEPrice = Convert.ToDecimal(AVEPrice.Text);
                model.GreenRate = Convert.ToDecimal(GreenRate.Text);
                model.VolumeRate = Convert.ToDecimal(VolumeRate.Text);
                model.School = School.Text;
                model.ParkingSpaces = ParkingSpaces.Text;
                model.PropertyDevelopers = PropertyDevelopers.Text;
                model.PropertyCompany = PropertyCompany.Text;
                model.Regions = Regions.Text;
                model.Content = hd_content.Value;
                model.Address = Address.Text;
                model.BusLine = BusLine.Text;
                model.SiteCode = Session["strSiteCode"].ToString();
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                if (dal.AddDCBuilding(model)>0)
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
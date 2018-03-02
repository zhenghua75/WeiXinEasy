using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.ACT;
using Model.ACT;
using Mozart.Common;

namespace Mozart.CMSAdmin.Coupon
{
    public partial class wfmSiteActivityAdd : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
                {
                    Response.Write("<script language=JavaScript>parent.location.href='../Index.aspx';</script>");
                    Response.End();
                }
                txtName.Focus();
                starttime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                endtime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                cutTime.Attributes.Add("onclick", "WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})");
                opentime.Attributes.Add("onclick", "WdatePicker({dateFmt:'HH:mm:ss'})");
                closetime.Attributes.Add("onclick", "WdatePicker({dateFmt:'HH:mm:ss'})");
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtName.Text.Trim() != null && txtName.Text.Trim() != "")
            {
                //上传图标
                string strIconFileName = string.Empty;//图标路径
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

                SiteActivity model = new SiteActivity();
                model.ID = Guid.NewGuid().ToString("N").ToUpper();
                model.ActTitle = txtName.Text.Trim();
                model.SiteCode = GlobalSession.strSiteCode;
                model.ActContent = hd_content.Value;
                model.StartTime = starttime.Text;
                model.EndTime = endtime.Text;
                model.Remark = txtSummary.Text;
                model.CutOffTime = cutTime.Text;
                model.DisCount = Discount.Text;
                model.Photo = strIconSaveFileName;
                model.OpenTime = opentime.Text;
                model.CloseTime = closetime.Text;
                if (daylimit.Text.Trim() != null && daylimit.Text.Trim() != "" && Convert.ToInt32(daylimit.Text) > -1)
                {
                    model.DayLimit = Convert.ToInt32(daylimit.Text);
                }
                model.ActStatus =ActStatusyes.Checked?1:0;//默认1 生效
                SiteActivityDAL dal = new SiteActivityDAL();
                if (dal.SaveActivity(model)>0)
                {
                    MessageBox.Show(this, "添加成功！");
                }
                else
                {
                    MessageBox.Show(this, "添加失败！");
                }
            }
            else
            {
                MessageBox.Show(this, "请输入相关名称！");
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            starttime.Text = "";
            endtime.Text = "";
            txtName.Text = "";
            txtSummary.Text = "";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.CMSAdmin.Product
{
    public partial class wfmProductAdd : System.Web.UI.Page
    {
        string strMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Session["strSiteCode"].ToString()) && Session["strRoleCode"].ToString() != "ADMIN")
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }
            if (!IsPostBack)
            {
                this.ddlCategory.Items.Clear();
                DAL.Product.CategoryDAL dal = new DAL.Product.CategoryDAL();
                DataSet ds = new DataSet();
                if (Session["strRoleCode"].ToString() == "ADMIN")
                {
                    ds = dal.GetSPCategory("");
                }
                else
                {
                    ds = dal.GetSPCategory(" SiteCode ='" + Session["strSiteCode"].ToString() + "' ");
                }
                DataTable dt = ds.Tables[0];

                DataRow dr = ds.Tables[0].NewRow();
                dr["ID"] = "0";
                dr["Name"] = "--全部--";
                dt.Rows.InsertAt(dr, 0);

                this.ddlCategory.DataSource = ds.Tables[0].DefaultView;
                this.ddlCategory.DataTextField = "Name";
                this.ddlCategory.DataValueField = "ID";
                this.ddlCategory.DataBind();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (null == Session["strSiteName"] || null == Session["strSiteCode"] || null == Session["strLoginName"])
            {
                Response.Write("<script language=JavaScript>;parent.location.href='../Login.aspx';</script>");
                Response.End();
            }
            //上传图标
            string strIconFileName = string.Empty;//图标路径
            string strIconSaveFileName = string.Empty;//网址路径
            try
            {
                if (this.file0.PostedFile.FileName == "")
                {
                    MessageBox.Show(this, "请选择上传文件！");
                }
                else
                {
                    if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/Photo"))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/Photo");
                    }
                    if (!System.IO.Directory.Exists(String.Format(@"{0}/Photo/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString())))
                    {
                        System.IO.Directory.CreateDirectory(String.Format(@"{0}/Photo/{1}", Server.MapPath("~"), Session["strSiteCode"].ToString()));
                    }
                    string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                    string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                    if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                    {
                        MessageBox.Show(this, "文件格式有误！");
                        return;

                    }//检查文件格式
                    string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                    strIconFileName = String.Format(@"{0}Photo/{1}/{2}", Server.MapPath("~"), Session["strSiteCode"].ToString(), newName);
                    strIconSaveFileName = String.Format(@"Photo/{0}/{1}", Session["strSiteCode"].ToString(), newName);
                    file0.PostedFile.SaveAs(strIconFileName);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
            }
            DAL.Product.ProductDAL dal = new DAL.Product.ProductDAL();
            Model.SP.SP_Product modelAdd = new Model.SP.SP_Product
            {
                ID = Guid.NewGuid().ToString("N").ToUpper(),
                //文章标题
                Name = this.txtArticleTitle.Text,
                //图标路径
                Photo = strIconSaveFileName,//strIconFileName,
                //文章内容
                Desc = this.hd_content.Value,
                //站点代码
                SiteCode = Session["strSiteCode"].ToString(),
                //类别
                CatID = this.ddlCategory.SelectedValue,
                //积分
                Credits = int.Parse(this.txtCredits.Text),
                //单位
                Unit = this.txtUnit.Text,
                //普通价格
                NormalPrice = int.Parse(this.txtNPrice.Text),
                //会员价格
                MemberPrice = int.Parse(this.txtMPrice.Text),
                //是否置顶
                IsTop = this.txtArticleIsTop.Text == "是" ? 1 : 0,
                //是否删除
                State = 0,
                //创建时间
                Pdate = DateTime.Now,
                //有效开始时间
                StartTime = DateTime.Parse("2000-01-01 00:00:00.000"),
                //有效结束时间
                EndTime = DateTime.Parse("2099-01-01 00:00:00.000"),
                //排序号
                Order = int.Parse(this.txtOrder.Text)
            };
            if (dal.InsertInfo(modelAdd) > 0 )
            {
                strMessage = "商品添加成功！";
            }
            else
            {
                strMessage = "商品添加失败！";
            }
            MessageBox.Show(this, strMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            this.txtArticleTitle.Text = "";
            this.hd_content.Value = "";
        }
    }
}
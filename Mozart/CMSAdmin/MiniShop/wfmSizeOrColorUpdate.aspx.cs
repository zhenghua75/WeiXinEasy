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

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmSizeOrColorUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string oldimg = string.Empty;
        static string shopPhone = string.Empty;
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
            sizeorcolor.Items.Clear();
            sizeorcolor.Items.Insert(0, new ListItem("尺寸", "1"));
            sizeorcolor.Items.Insert(1, new ListItem("颜色", "-1"));
            sizeorcolor.Items.Insert(2, new ListItem("没有", "0"));
            MSSizeOrColorDAL sizeorcolorDal = new MSSizeOrColorDAL();
            DataSet sizeds = sizeorcolorDal.GetSizeOrColorDetail(strID);
            MSSizeOrColor sizeorcolorModel =
                DataConvert.DataRowToModel<MSSizeOrColor>(sizeds.Tables[0].Rows[0]);
            sizeorcolor.SelectedIndex =
                sizeorcolor.Items.IndexOf(sizeorcolor.Items.FindByValue(sizeorcolorModel.SizeColor.ToString()));
            scname.Text = sizeorcolorModel.Scname;
            if (sizeorcolorModel.Scimg != null && sizeorcolorModel.Scimg != "")
            {
                oldimg = sizeorcolorModel.Scimg;
                img0.Src = "../../PalmShop/ShopCode/" + oldimg;
            }
            stock.Text = sizeorcolorModel.Stock.ToString();
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
            else
            {
                string pid = sizeorcolorModel.PID;
                string sid = string.Empty;
                MSProductDAL pDal = new MSProductDAL();
                MSShopDAL shopdal = new MSShopDAL();
                try
                {
                    sid = pDal.GetMSProductVaueByID("[SID]", pid).ToString();
                    shopPhone = shopdal.GetMSShopValueByID("Phone", sid).ToString();
                }
                catch (Exception)
                {
                    shopPhone = "";
                }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (scname.Text.Trim() != null && scname.Text.Trim() != "")
                {
                    #region 上传图像
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
                            if (oldimg.Trim() != null && oldimg.Trim() != "" && oldimg.Trim().ToLower() != "shoplogo/default.png")
                            {
                                System.IO.File.Delete(String.Format(@"{0}PalmShop/ShopCode/{1}", Server.MapPath("~"), oldimg));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }
                    #endregion
                    MSSizeOrColor sizeorcolorModel = new MSSizeOrColor();
                    MSSizeOrColorDAL sizeorcolorDal = new MSSizeOrColorDAL();
                    sizeorcolorModel.ID =strID;
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
                    if (sizeorcolorDal.UpdateSizeOrColor(sizeorcolorModel))
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
                return;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            scname.Text = ""; stock.Text = "";
        }
    }
}
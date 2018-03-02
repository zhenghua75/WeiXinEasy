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

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmProductAtlasUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string ShopPhone = string.Empty;
        static string oldimg = string.Empty;
        string pid = string.Empty;
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
            MSProductAtlasDAL AtlasDal = new MSProductAtlasDAL();
            DataSet AtlasDs = AtlasDal.GetAtlasDetail(strID);
            MSProductAtlas AtlasModel = DataConvert.DataRowToModel<MSProductAtlas>(AtlasDs.Tables[0].Rows[0]);
            atlasname.Text = AtlasModel.AtlasName;
            string img = string.Empty; string sid = string.Empty;
            img = AtlasModel.PimgUrl;
            pid = AtlasModel.PID;
            MSProductDAL productDal = new MSProductDAL();
            MSShopDAL shopDal = new MSShopDAL();
            if (AtlasModel.IsDefault == 0)
            {
                isno.Checked = true;
            }
            else
            {
                isyes.Checked = true;
            }
            if (pid.Trim() != null && pid.Trim() != "")
            {
                sid = productDal.GetMSProductVaueByID("[SID]", pid).ToString();
            }
            if (sid.Trim() != null && sid.Trim() != "")
            {
                ShopPhone = shopDal.GetMSShopValueByID("Phone", sid).ToString();
            }
            else
            {
                ShopPhone = pid;
            }
            if (ShopPhone.Trim() == null || ShopPhone.Trim() == "")
            {
                ShopPhone = AtlasModel.ID;
            }
            if (img.Trim() != null && img.Trim() != "")
            {
                img0.Src = "../../PalmShop/ShopCode/" + img;
                oldimg = img;
            }
            if (strAction == "show")
            {
                this.btnReset.Visible = false;
                this.btnSave.Visible = false;
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Session["strLoginName"].ToString() != null && Session["strLoginName"].ToString() != "")
            {
                if (atlasname.Text.Trim() != null && atlasname.Text.Trim() != "")
                {
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
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/Atlas/{1}", Server.MapPath("~"), ShopPhone)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/Atlas/{1}", Server.MapPath("~"), ShopPhone));
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}PalmShop/ShopCode/Atlas/{1}/{2}", Server.MapPath("~"), ShopPhone, newName);
                            strIconSaveFileName = String.Format(@"Atlas/{0}/{1}", ShopPhone, newName);
                            file0.PostedFile.SaveAs(strIconFileName);
                            if (oldimg.Trim() != null && oldimg.Trim() != "")
                            {
                                System.IO.File.Delete(String.Format(@"{0}PalmShop/ShopCode/{1}", Server.MapPath("~"), oldimg));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }

                    MSProductAtlasDAL AtlasDal = new MSProductAtlasDAL();
                    MSProductAtlas AtlasModel = new MSProductAtlas();
                    AtlasModel.PID = pid;
                    AtlasModel.AtlasName = atlasname.Text;
                    AtlasModel.ImgState = 0;
                    if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                    {
                        AtlasModel.PimgUrl = strIconSaveFileName;
                    }
                    AtlasModel.ID =strID;
                    AtlasModel.IsDefault = isyes.Checked ? 1 : 0;
                    if (AtlasDal.UpdateMSProductAtlas(AtlasModel))
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
                    MessageBox.Show(this, "请输入相应名称！");
                }
            }
            else
            {
                return;
            }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            atlasname.Text = ""; file0.Value = "";
        }
    }
}
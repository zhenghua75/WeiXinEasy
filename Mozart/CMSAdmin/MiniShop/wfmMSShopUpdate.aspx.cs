using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using Mozart.Common;
using Model.MiniShop;

namespace Mozart.CMSAdmin.MiniShop
{
    public partial class wfmMSShopUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string oldimg = string.Empty;
        static string oldbackimg = string.Empty;
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
            MSShopDAL shopdal = new MSShopDAL();
            DataSet ds = shopdal.GetMSShopDetail(strID);
            MSShop shopmodel = DataConvert.DataRowToModel<MSShop>(ds.Tables[0].Rows[0]);
            hd_content.Value = shopmodel.ShopDesc;
            MSCustomersDAL customerDal = new MSCustomersDAL();
            userphone.Text = customerDal.GetCustomerValueByID("Phone", shopmodel.UID).ToString();
            shopname.Text = shopmodel.ShopName;
            if (shopmodel.ShopLogo != null && shopmodel.ShopLogo != "")
            {
                img0.Src = "../../PalmShop/ShopCode/" + shopmodel.ShopLogo;
                oldimg = shopmodel.ShopLogo;
            }
            if (shopmodel.ShopBackImg != null && shopmodel.ShopBackImg != "")
            {
                img1.Src = "../../PalmShop/ShopCode/" + shopmodel.ShopBackImg;
                oldbackimg = shopmodel.ShopBackImg;
            }
            wxname.Text=shopmodel.WXName;
            wxnum.Text=shopmodel.WXNum;
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
                if (shopname.Text.Trim()!=null&&shopname.Text.Trim()!="")
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
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/ShopLogo"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/ShopLogo");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), userphone.Text)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), userphone.Text));
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}PalmShop/ShopCode/ShopLogo/{1}/{2}", Server.MapPath("~"), userphone.Text, newName);
                            strIconSaveFileName = String.Format(@"ShopLogo/{0}/{1}", userphone.Text, newName);
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
                    //上传背景图像
                    string strbackimgFileName = string.Empty;//图像路径
                    string strbackimgSaveFileName = string.Empty;//网址路径
                    try
                    {
                        if (this.file1.PostedFile.FileName == "")
                        {
                            strbackimgSaveFileName = "";
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/ShopLogo"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/ShopLogo");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), userphone.Text)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), userphone.Text));
                            }
                            string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength + "backimg", extendName);//对文件进行重命名
                            strbackimgFileName = String.Format(@"{0}PalmShop/ShopCode/ShopLogo/{1}/{2}", Server.MapPath("~"), userphone.Text, newName);
                            strbackimgSaveFileName = String.Format(@"ShopLogo/{0}/{1}", userphone.Text, newName);
                            file1.PostedFile.SaveAs(strbackimgFileName);
                            if (oldbackimg.Trim() != null && oldbackimg.Trim() != "" && oldbackimg.Trim().ToLower() != "shoplogo/default.png")
                            {
                                System.IO.File.Delete(String.Format(@"{0}PalmShop/ShopCode/{1}", Server.MapPath("~"), oldbackimg));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }
                    MSShopDAL shopdal = new MSShopDAL();
                    MSShop shopmodel = new MSShop();
                    shopmodel.ShopName = shopname.Text;
                    shopmodel.ShopDesc = hd_content.Value;
                    shopmodel.ShopState = 0;
                    shopmodel.ID = strID;
                    if (wxname.Text != null && wxname.Text != "")
                    {
                        shopmodel.WXName = wxname.Text;
                    }
                    else
                    {
                        shopmodel.WXName = "";
                    }
                    if (wxnum.Text != null && wxnum.Text != "")
                    {
                        shopmodel.WXNum = wxnum.Text;
                    }
                    else
                    {
                        shopmodel.WXNum = "";
                    }
                    if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                    {
                        shopmodel.ShopLogo = strIconSaveFileName;
                    }
                    if (strbackimgSaveFileName.Trim() != null && strbackimgSaveFileName.Trim() != "")
                    {
                        shopmodel.ShopBackImg = strbackimgSaveFileName;
                    }
                    if (shopdal.UpdateMSShop(shopmodel))
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
                    MessageBox.Show(this, "请输入相应店铺名称或电话！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            shopname.Text = "";hd_content.Value = "";
        }
    }
}
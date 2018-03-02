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
    public partial class wfmMSShopAdd : System.Web.UI.Page
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
                    #region 初始化界面
                    if (null != Common.Common.NoHtml(Request.QueryString["action"]))
                    {
                        strAction = Common.Common.NoHtml(Request.QueryString["action"]);
                    }
                    if (null != Common.Common.NoHtml(Request.QueryString["id"]))
                    {
                        strID = Common.Common.NoHtml(Request.QueryString["id"]);
                    }
                    ddlphonelist.Items.Clear();
                    MSCustomersDAL phoneDal = new MSCustomersDAL();
                    DataSet ds = new DataSet();
                    ds = phoneDal.GetCustomersList("");
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["Phone"] = "--请选择用户电话--";
                    dt.Rows.InsertAt(dr, 0);
                    ddlphonelist.DataSource = ds.Tables[0].DefaultView;
                    ddlphonelist.DataTextField = "Phone";
                    ddlphonelist.DataValueField = "ID";
                    ddlphonelist.DataBind();
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
                if (ddlphonelist.SelectedValue.Trim()!=null&&ddlphonelist.SelectedValue.Trim()!=""&&
                    shopname.Text.Trim()!=null&&shopname.Text.Trim()!="")
                {
                    //上传图像
                    string strIconFileName = string.Empty;//图像路径
                    string strIconSaveFileName = string.Empty;//网址路径
                    try
                    {
                        if (this.file0.PostedFile.FileName == "")
                        {
                            strIconSaveFileName = "ShopLogo/default.png";
                        }
                        else
                        {
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/ShopLogo"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/ShopLogo");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text));
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}PalmShop/ShopCode/ShopLogo/{1}/{2}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text, newName);
                            strIconSaveFileName = String.Format(@"ShopLogo/{0}/{1}", ddlphonelist.SelectedItem.Text, newName);
                            file0.PostedFile.SaveAs(strIconFileName);
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
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/ShopLogo/{1}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text));
                            }
                            string orignalName = this.file1.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file1.PostedFile.ContentLength + "backimg", extendName);//对文件进行重命名
                            strbackimgFileName = String.Format(@"{0}PalmShop/ShopCode/ShopLogo/{1}/{2}", Server.MapPath("~"), ddlphonelist.SelectedItem.Text, newName);
                            strbackimgSaveFileName = String.Format(@"ShopLogo/{0}/{1}", ddlphonelist.SelectedItem.Text, newName);
                            file1.PostedFile.SaveAs(strbackimgFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, "上传发生错误！原因是：" + ex.ToString());
                    }

                    MSShopDAL shopdal = new MSShopDAL();
                    if (shopdal.ExistMSShop(shopname.Text, ddlphonelist.SelectedValue))
                    {
                        MessageBox.Show(this, "该店铺已经存在！");
                    }
                    else
                    {
                        MSShop shopmodel = new MSShop();
                        shopmodel.ShopName = shopname.Text;
                        shopmodel.ShopDesc = hd_content.Value;
                        shopmodel.ShopState = 0;
                        shopmodel.UID = ddlphonelist.SelectedValue;
                        shopmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
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
                        if (shopdal.AddMSShop(shopmodel))
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
           shopname.Text = "";
            hd_content.Value = "";
        }
    }
}
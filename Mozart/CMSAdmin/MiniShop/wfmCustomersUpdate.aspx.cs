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
    public partial class wfmCustomersUpdate : System.Web.UI.Page
    {
        static string strID = string.Empty;
        string strMessage = string.Empty;
        string strAction = string.Empty;
        static string oldimg = string.Empty;
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
            MSCustomersDAL CustomerDal = new MSCustomersDAL();
            DataSet ds = CustomerDal.GetCustomerDetail(strID);
            MSCustomers CustomerModel = DataConvert.DataRowToModel<MSCustomers>(ds.Tables[0].Rows[0]);
            phone.Text = CustomerModel.Phone;
            hd_content.Value = CustomerModel.Pnote;
            NickName.Text = CustomerModel.NickName;
            email.Text = CustomerModel.Email;
            QQnum.Text = CustomerModel.QQnum;
            if (CustomerModel.HeadImg != null && CustomerModel.HeadImg != "")
            {
                img0.Src = "../../PalmShop/ShopCode/" + CustomerModel.HeadImg;
                oldimg = CustomerModel.HeadImg;
            }
            if (CustomerModel.Sex == 0)
            {
                isstateyes.Checked = true;
            }
            else
            {
                isstateno.Checked = true;
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
                if (phone.Text.Trim() != null && phone.Text.Trim() != "" &&
                    password.Text.Trim() != null && password.Text.Trim() != "" &&
                    NickName.Text.Trim() != null && NickName.Text.Trim() != "")
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
                            if (!System.IO.Directory.Exists(Server.MapPath("~") + @"/PalmShop"))
                            {
                                System.IO.Directory.CreateDirectory(Server.MapPath("~") + @"/PalmShop");
                            }
                            if (!System.IO.Directory.Exists(String.Format(@"{0}/PalmShop/ShopCode/HeadImg/{1}", Server.MapPath("~"), phone.Text)))
                            {
                                System.IO.Directory.CreateDirectory(String.Format(@"{0}/PalmShop/ShopCode/HeadImg/{1}", Server.MapPath("~"), phone.Text));
                            }
                            string orignalName = this.file0.PostedFile.FileName;//获取客户机上传文件的文件名
                            string extendName = orignalName.Substring(orignalName.LastIndexOf("."));//获取扩展名

                            if (extendName != ".gif" && extendName != ".jpg" && extendName != ".jpeg" && extendName != ".png")
                            {
                                MessageBox.Show(this, "文件格式有误！");
                                return;

                            }//检查文件格式
                            string newName = String.Format("{0}_{1}{2}", DateTime.Now.Millisecond, file0.PostedFile.ContentLength, extendName);//对文件进行重命名
                            strIconFileName = String.Format(@"{0}PalmShop/ShopCode/HeadImg/{1}/{2}", Server.MapPath("~"), phone.Text, newName);
                            strIconSaveFileName = String.Format(@"HeadImg/{0}/{1}", phone.Text, newName);
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
                    MSCustomersDAL CustomerDal = new MSCustomersDAL();
                    MSCustomers CustomerModel = new MSCustomers();
                    CustomerModel.Phone = phone.Text;
                    CustomerModel.NickName = NickName.Text;
                    CustomerModel.Pnote = hd_content.Value;
                    CustomerModel.Email = email.Text;
                    CustomerModel.QQnum = QQnum.Text;
                    CustomerModel.Sex = isstateyes.Checked ? 0 : 1;
                    CustomerModel.UserPwd = Common.Common.MD5(password.Text);
                    CustomerModel.IsDel = 0;
                    CustomerModel.ID = strID;
                    if (strIconSaveFileName.Trim() != null && strIconSaveFileName.Trim() != "")
                    {
                        CustomerModel.HeadImg = strIconSaveFileName;
                    }
                    if (CustomerDal.UpdateCustomers(CustomerModel))
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
            phone.Text = ""; NickName.Text = ""; email.Text = ""; QQnum.Text = "";
            hd_content.Value = ""; password.Text = "";
        }
    }
}
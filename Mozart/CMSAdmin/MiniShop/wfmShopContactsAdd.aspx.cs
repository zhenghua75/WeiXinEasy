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
    public partial class wfmShopContactsAdd : System.Web.UI.Page
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
                    ddlcontactlist.Items.Clear();
                    MSShopDAL actDal = new MSShopDAL();
                    DataSet ds = new DataSet();
                    ds = actDal.GetMSShopList("");
                    DataTable dt = ds.Tables[0];
                    DataRow dr = ds.Tables[0].NewRow();
                    dr["ID"] = "";
                    dr["ShopName"] = "--请选择相应店铺--";
                    dt.Rows.InsertAt(dr, 0);
                    ddlcontactlist.DataSource = ds.Tables[0].DefaultView;
                    ddlcontactlist.DataTextField = "ShopName";
                    ddlcontactlist.DataValueField = "ID";
                    ddlcontactlist.DataBind();

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
                if (nickname.Text.Trim() != null && nickname.Text.Trim() != "" &&
                    phone.Text.Trim() != null && phone.Text.Trim() != ""&&
                    ddlcontactlist.SelectedValue.Trim()!=null&&ddlcontactlist.SelectedValue.Trim()!="")
                {
                    MSShopContactsDAL contactdal = new MSShopContactsDAL();
                    if (contactdal.ExistContact(phone.Text,nickname.Text,ddlcontactlist.SelectedValue,""))
                    {
                        MessageBox.Show(this, "该联系方式已经存在！");
                    }
                    else
                    {
                        MSShopContacts contactmodel = new MSShopContacts();
                        contactmodel.Phone = phone.Text;
                        contactmodel.SID = ddlcontactlist.SelectedValue;
                        contactmodel.NickName = nickname.Text;
                        string cityselect = string.Empty;
                        if(qqnum.Text.Trim()!=null&&qqnum.Text.Trim()!="")
                        {
                            contactmodel.QQnum = Convert.ToInt32(qqnum.Text);
                        }
                        if (email.Text.Trim() != null && email.Text.Trim() != "")
                        {
                            contactmodel.Email = email.Text;
                        }
                        if (address.Value.Trim() != null && address.Value.Trim() != "")
                        {
                            contactmodel.Address = address.Value;
                        }
                        contactmodel.IsDefault = isyes.Checked ? 1 : 0;
                        contactmodel.IsDel = 0;
                        contactmodel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        if (contactdal.AddMSSContacts(contactmodel))
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
                    MessageBox.Show(this, "请输入相应名称或电话！");
                }
            }
            else
            {
                return;
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            phone.Text = ""; nickname.Text = ""; qqnum.Text = ""; email.Text = "";
            address.Value = "";
        }
    }
}
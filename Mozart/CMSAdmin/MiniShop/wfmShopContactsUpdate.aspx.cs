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
    public partial class wfmShopContactsUpdate : System.Web.UI.Page
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
            ddlcontactlist.Items.Clear();
            MSShopDAL actDal = new MSShopDAL();
            DataSet ds = new DataSet();
            ds = actDal.GetMSShopList("");
            ddlcontactlist.DataSource = ds.Tables[0].DefaultView;
            ddlcontactlist.DataTextField = "ShopName";
            ddlcontactlist.DataValueField = "ID";
            ddlcontactlist.DataBind();

            MSShopContactsDAL contactdal = new MSShopContactsDAL();
            DataSet contactds = contactdal.GetContactDetail(strID);
            MSShopContacts contactdel = DataConvert.DataRowToModel<MSShopContacts>(contactds.Tables[0].Rows[0]);
            phone.Text = contactdel.Phone;
            ddlcontactlist.SelectedIndex = 
                ddlcontactlist.Items.IndexOf(ddlcontactlist.Items.FindByValue(contactdel.SID));
            nickname.Text = contactdel.NickName;
            qqnum.Text = contactdel.QQnum.ToString();
            email.Text = contactdel.Email;
            address.Value = contactdel.Address;
            if (contactdel.IsDefault == 1)
            {
                isyes.Checked = true;
            }
            else
            {
                isno.Checked = true;
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
                if (nickname.Text.Trim() != null && nickname.Text.Trim() != "" &&
                    phone.Text.Trim() != null && phone.Text.Trim() != "" &&
                    ddlcontactlist.SelectedValue.Trim() != null && ddlcontactlist.SelectedValue.Trim() != "")
                {
                    MSShopContactsDAL contactdal = new MSShopContactsDAL();
                    MSShopContacts contactmodel = new MSShopContacts();
                    contactmodel.Phone = phone.Text;
                    contactmodel.SID = ddlcontactlist.SelectedValue;
                    if (qqnum.Text.Trim() != null && qqnum.Text.Trim() != "")
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
                    contactmodel.IsDel = 0;
                    contactmodel.IsDefault = isyes.Checked ? 1 : 0;
                    contactmodel.ID =strID;
                    if (contactdal.UpdateMSSContacts(contactmodel))
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
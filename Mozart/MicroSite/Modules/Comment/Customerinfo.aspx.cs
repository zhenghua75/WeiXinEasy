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
using System.IO;

namespace Mozart.Comment
{
    public partial class Customerinfo : System.Web.UI.Page
    {
        public string errorscript = string.Empty;
        public string strfid = string.Empty;
        public string strTid = string.Empty;
        public string topictitle = string.Empty;
        public string strUid = string.Empty;
        public static string oldimgname = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request["fid"] != null && Request["fid"] != "")
                {
                    errorscript = "";
                    strfid = Common.Common.NoHtml(Request["fid"]);
                }
                else
                {
                    errorscript = JQDialog.alertOkMsgBoxClearBody(2, "操作失败<br/>请核对后再操作！");
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Common.Common.NoHtml(Session["customerID"].ToString());
                    if (Request["action"] != null && Request["action"] != "" &&
                        Request["action"].ToString().ToLower() == "edite")
                    {
                        editeCustomerinfo();
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "../../Comment/Customerinfo.aspx?fid=" + strfid, 2);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "../PalmShop/ShopCode/UserLogin.aspx","error");
                }
                getTemplate();
            }
        }
        void getTemplate()
        {
            #region --------------获取客户详细-----------------
            MSCustomers customerModel = new MSCustomers();
            MSCustomersDAL customerDal = new MSCustomersDAL();
            DataSet customerDs = customerDal.GetCustomerDetail(strUid);
            if (null != customerDs && customerDs.Tables.Count > 0 && customerDs.Tables[0].Rows.Count > 0)
            {
                customerModel = DataConvert.DataRowToModel<MSCustomers>(customerDs.Tables[0].Rows[0]);
                if (customerModel.HeadImg != null && customerModel.HeadImg != "")
                {
                    oldimgname = customerModel.HeadImg;
                }
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("HtmlPage/customerinfo.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();
            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            context.TempData["customer"] = customerModel;
            context.TempData["errorscript"] = errorscript;
            context.TempData["fid"] = strfid;
            context.TempData["footer"] = "奥琦微商易";
            t.Render(Response.Output);
        }
        void editeCustomerinfo()
        {
            string nickname = string.Empty; string modelimgurl = string.Empty;
            try
            {
                nickname = HttpContext.Current.Request.Form.Get("nickname").ToString();
            }
            catch (Exception)
            {
                nickname = "";
            }
            MSCustomers customerModel = new MSCustomers();
            MSCustomersDAL customerDal = new MSCustomersDAL();
            customerModel.ID = strUid;
            if (nickname != null && nickname != "")
            {
                #region -----------------头像上传-------------------
                HttpFileCollection files = HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    //检查文件扩展名字
                    HttpPostedFile postedFile = files[0];
                    string fileName, fileExtension, file_oldid, file_id;
                    //取出精确到毫秒的时间做文件的名称
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = my_file_id + fileExtension;
                    if (fileName != "" && fileName != null && fileName.Length > 0)
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        string saveurl;
                        modelimgurl = "HeadImg/";
                        saveurl ="../PalmShop/ShopCode/" + modelimgurl;
                        saveurl = Server.MapPath(saveurl);
                        if (!Directory.Exists(saveurl))
                        {
                            Directory.CreateDirectory(saveurl);
                        }
                        int length = postedFile.ContentLength;
                        if (length > 512000)
                        {
                            file_oldid = "old" + file_id;
                            postedFile.SaveAs(saveurl + file_oldid);
                            JQDialog.ystp(saveurl + file_oldid, saveurl + file_id, 15);
                            File.Delete(saveurl + file_oldid);
                        }
                        else
                        {
                            postedFile.SaveAs(saveurl + file_id);
                        }
                        modelimgurl = modelimgurl + file_id;
                    }
                }
                #endregion
                customerModel.NickName = nickname;
                if (modelimgurl != null && modelimgurl != "")
                {
                    customerModel.HeadImg = modelimgurl;
                }
                customerModel.IsDel = 0;
                if (customerDal.UpdateCustomers(customerModel))
                {
                    if (modelimgurl != null && modelimgurl != "")
                    {
                        if (oldimgname != null && oldimgname != "")
                        {
                            File.Delete(String.Format(@"{0}PalmShop/ShopCode/{1}", 
                                Server.MapPath("~"), oldimgname));
                        }
                    }
                    errorscript = JQDialog.alertOKMsgBox(3, "操作成功", "MyTopicList.aspx?fid=" + strfid,
                        "succeed");
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBox(3, "操作失败，请核对后再操作", "Customerinfo.aspx?fid=" + strfid,
                        "error");
                }
            }
            else
            {
                errorscript = JQDialog.alertOKMsgBox(3, "操作失败，请核对后再操作", 
                    "Customerinfo.aspx?fid=" + strfid, "error");
            }
        }
    }
}
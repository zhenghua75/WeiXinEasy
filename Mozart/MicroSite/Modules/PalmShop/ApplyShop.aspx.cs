using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Model.MiniShop;
using DAL.MiniShop;
using System.IO;
using Mozart.Common;

namespace Mozart.PalmShop.ShopCode
{
    public partial class ApplyShop : System.Web.UI.Page
    {
        string errorscript = string.Empty;
        string action = string.Empty;
        string strUid = string.Empty;
        string strSID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    strUid = Common.Common.NoHtml(Session["customerID"].ToString());
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "ShopIndex.aspx", 2);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！",
                        "UserLogin.aspx", "error");
                }
                if (Request["sid"] != null && Request["sid"] != "")
                {
                    strSID =Common.Common.NoHtml( Request["sid"]);
                }
                else
                {
                    if (Session["SID"] != null && Session["SID"].ToString() != "")
                    {
                        strSID = Common.Common.NoHtml(Session["SID"].ToString());
                    }
                }
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Common.Common.NoHtml(Request["action"]);
                    action=action.Trim().ToLower();
                }
                if (action == "uapply" || action == "uedite")
                {
                    RegOrUpdateShop();
                }
                GetHtmlPage();
            }
        }
        void GetHtmlPage()
        {
            MSShop shopModel = new MSShop();
            MSCustomers UserModel = new MSCustomers();
            #region-----------------店铺详情-----------------------
            if (strSID != null && strSID != "")
            {
                MSShopDAL shopDal = new MSShopDAL();
                DataSet shopds = shopDal.GetMSShopDetail(strSID);
                if (shopds != null && shopds.Tables.Count > 0 && shopds.Tables[0].Rows.Count > 0)
                {
                    shopModel = DataConvert.DataRowToModel<MSShop>(shopds.Tables[0].Rows[0]);
                }
            }
            #endregion
            #region---------------店铺用户资料详情--------------------
            if (strUid != null && strUid != "")
            {
                MSCustomersDAL UserDal = new MSCustomersDAL();
                DataSet userds = UserDal.GetCustomerDetail(strUid);
                if (userds != null && userds.Tables.Count > 0 && userds.Tables[0].Rows.Count > 0)
                {
                    UserModel = DataConvert.DataRowToModel<MSCustomers>(userds.Tables[0].Rows[0]);
                }
            }
            #endregion
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/ApplyShop.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["footer"] = "奥琦微商易";
            context.TempData["errorscript"] = errorscript;
            context.TempData["shopdetail"] = shopModel;
            context.TempData["usredetail"] = UserModel;
            context.TempData["action"] = action;
            context.TempData["sid"] = strSID;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        void RegOrUpdateShop()
        {
            string shopname = string.Empty;
            string wxname = string.Empty;
            string wxnum = string.Empty;
            string isnull = "";
            string ShopLogo = string.Empty;
            string ShopBackImg = string.Empty;
            string RealName = string.Empty;
            string IDnum = string.Empty;
            string IDimg = string.Empty;
            #region --------------------获取请求参数值-------------------------------
            try
            {
                shopname =Request.Form.Get("shopname").ToString();
            }
            catch (Exception)
            {
                shopname = isnull;
            }
            try
            {
                wxname =Request.Form.Get("wxname").ToString();
            }
            catch (Exception)
            {
                wxname = isnull;
            }
            try
            {
                wxnum =Request.Form.Get("wxnum").ToString();
            }
            catch (Exception)
            {
                wxnum = isnull;
            }
            try
            {
                ShopLogo = Request.Files.Get("logoimg").FileName.ToString();
            }
            catch (Exception)
            {
                ShopLogo = isnull;
            }
            try
            {
                ShopBackImg = Request.Files.Get("backimg").FileName.ToString();
            }
            catch (Exception)
            {
                ShopBackImg = isnull;
            }
            try
            {
                RealName = Request.Files.Get("realname").FileName.ToString();
            }
            catch (Exception)
            {
                RealName = isnull;
            }
            try
            {
                IDnum =Request.Form.Get("idnum").ToString();
            }
            catch (Exception)
            {
                IDnum = isnull;
            }
            try
            {
                IDimg = Request.Files.Get("idimg").FileName.ToString();
            }
            catch (Exception)
            {
                IDimg = isnull;
            }
            #endregion
            #region ---------------------获取图像---------------------------
            if (ShopLogo != null && ShopLogo != "")
            {
                ShopLogo = "ShopLogo/" + UploadImg("logoimg");
            }
            if (ShopBackImg != null && ShopBackImg != "")
            {
                ShopBackImg ="ShopLogo/"+ UploadImg("backimg");
            }
            if (IDimg != null && IDimg != "")
            {
                IDimg = "ShopLogo/" + UploadImg("idimg");
            }
            #endregion
            MSShop shopModel = new MSShop();
            MSShopDAL shopDal = new MSShopDAL();
            #region ----------------------设置Model值-----------------------------------
            if (shopname != null && shopname != "")
            {
                shopModel.ShopName = shopname;
            }
            shopModel.WXName = wxname;
            shopModel.WXNum = wxnum;
            shopModel.ShopLogo = ShopLogo;
            shopModel.ShopBackImg = ShopBackImg;
            string regSid = string.Empty;
            if (strUid != null && strUid != "")
            {
                shopModel.UID = strUid;
            }
            if (strSID != null && strSID != "")
            {
                shopModel.ID = strSID;
            }
            else
            {
                regSid=Guid.NewGuid().ToString("N").ToUpper();
                shopModel.ID = regSid;
            }
            #endregion
            #region------------------更新注册店铺用户身份信息---------------------------
            if (strUid != null && strUid != "")
            {
                MSCustomersDAL UserDal = new MSCustomersDAL();
                UserDal.UpdateUserIDnum(strUid, RealName, IDnum, IDimg);
            }
            #endregion
            #region---------------------更新或注册店铺信息--------------------------------------
            if (strSID != null && strSID != "" && action == "uedite")
            {
                if (shopDal.UpdateMSShop(shopModel))
                {
                    errorscript = JQDialog.alertOKMsgBox(3, "操作成功！", "MyShop.aspx", "succeed");
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBox(3, "操作失败，请核对后再操作！", "ApplyShop.aspx", "error");
                }
            }
            else
            {
                if (!shopDal.ExistMSShop(shopname, strUid))
                {
                    if (shopDal.AddMSShop(shopModel))
                    {
                        Session["SID"] = regSid;
                        errorscript = JQDialog.alertOKMsgBox(3, "操作成功！", "MyShop.aspx", "succeed");
                    }
                    else
                    {
                        errorscript = JQDialog.alertOKMsgBox(3, "操作失败，请核对后再操作！", "ApplyShop.aspx", "error");
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "MyShop.aspx", 2);
                    errorscript = JQDialog.alertOKMsgBox(3, "改店铺已经存在，请登录后操作", "UserLogin.aspx", "error");
                }
            }
            #endregion
        }
        string UploadImg(string strImgName) {
            HttpFileCollection files =Request.Files;
            string fileName, fileExtension, file_oldid;
            string file_id=string.Empty;
            if (strImgName != null && strImgName!="")
            {
                //检查文件扩展名字
                HttpPostedFile postedFile = files[strImgName];
                //取出精确到毫秒的时间做文件的名称
                string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                fileName = System.IO.Path.GetFileName(postedFile.FileName);
                fileExtension = System.IO.Path.GetExtension(fileName);
                file_id = my_file_id + fileExtension;
                if (fileName != "" && fileName != null && fileName.Length > 0)
                {
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    string saveurl;
                    saveurl = "ShopLogo/";
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
                }
            }
            return  file_id;
        }
    }
}
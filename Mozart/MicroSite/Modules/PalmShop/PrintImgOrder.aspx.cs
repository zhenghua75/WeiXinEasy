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
    public partial class PrintImgOrder : System.Web.UI.Page
    {
        string action = string.Empty;
        string errormsg = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errormsg = "";
                if (Request["action"] != null && Request["action"] != "")
                {
                    action = Request["action"];
                    action = action.ToLower().Trim();
                    switch (action)
                    {
                        case "valiteordernum":
                            valiteordernum();
                            break;
                        case "ordernum":
                            submitOrder();
                            GetHtmlPage();
                            break;
                    }
                }
                GetHtmlPage();
            }
        }
        /// <summary>
        /// 获取模板
        /// </summary>
        void GetHtmlPage()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/PrintImgOrder.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            //context.TempData["artList"] = liArtList;
            context.TempData["errormsg"] = errormsg;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 验证订单
        /// </summary>
        void valiteordernum()
        {
            string ordernum = string.Empty;
            if (Request["ordernum"] != null && Request["ordernum"] != "")
            {
                ordernum = Request["ordernum"];
            }
            if (ordernum != null && ordernum != "")
            {
                MSPhotoSubmitDAL photoDal = new MSPhotoSubmitDAL();
                if (!photoDal.ExsitOrderNum(ordernum))
                {
                    Response.Write("{\"error\":true,\"msg\":\"该订单无效\"}");
                }
                else
                {
                    if (photoDal.ExistisSubmit(ordernum))
                    {
                        Response.Write("{\"error\":true,\"msg\":\"该订单已提交过\"}");
                    }
                    else
                    {
                        Response.Write("{\"success\":true}");
                    }
                }
            }
            else
            {
                Response.Write("{\"error\":true,\"msg\":\"请输入正确的订单\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 订单提交
        /// </summary>
        void submitOrder()
        {
            string ordernum = string.Empty; int imgcount = 0;
            string img1 = string.Empty; string img2 = string.Empty;
            string openid = string.Empty; string userid = string.Empty;
            if (Session["OpenID"] != null && Session["OpenID"].ToString() != "")
            {
                openid = Session["OpenID"].ToString();
            }
            if (openid != null && openid != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                try
                {
                    userid = customerDal.GetCustomerValueByOpenID("ID", openid).ToString();
                }
                catch (Exception)
                {
                }
            }
            #region-------------获取请求值------------
            try
            {
                ordernum = Request.Form.Get("ordernum").ToString();
            }
            catch (Exception)
            {
                ordernum = "";
            }
            #endregion
            try
            {
                imgcount = Request.Files.Count;
            }
            catch (Exception)
            {
            }
            if (imgcount == 2)
            {
                img1 = UploadImg(0, "printimg",ordernum);
                img2 = UploadImg(1,"shareimg", ordernum);
                if (userid == null || userid == "" && ordernum != null && ordernum != "")
                {
                    MSProductOrderDAL orderDal = new MSProductOrderDAL();
                    userid = orderDal.GetOrderValueByID("CustomerID", ordernum).ToString();
                }
                if (ordernum != null && ordernum != "")
                {
                    MSPhotoSubmit photoModel = new MSPhotoSubmit();
                    MSPhotoSubmitDAL photoDal = new MSPhotoSubmitDAL();
                    photoModel.OrderNum = ordernum;
                    photoModel.Img1 = img1;
                    photoModel.Img2 = img2;
                    photoModel.UID = userid;
                    photoModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                    if (!photoDal.AddPhotoSubmit(photoModel))
                    {
                        errormsg = JQDialog.alertOKMsgBoxGoBack(3, "操作失败，请重新操作！", false);
                    }
                    else
                    {
                        errormsg = JQDialog.alertOKMsgBox(3, "操作成功，请等待审核！", "PrintImgOrder.aspx", "succeed");
                    }
                }
                else
                {
                    errormsg = JQDialog.alertOKMsgBoxGoBack(3, "请输入正确的订单！", false);
                }
            }
            else
            {
                errormsg = JQDialog.alertOKMsgBoxGoBack(3, "请选择您要打印的照片和分享的照片！", false);
            }
        }
        string UploadImg(int fileindex,string filename, string ordernum)
        {
            string modelimgurl = string.Empty;
            #region -----------------上传图片-------------------
            HttpFileCollection files = HttpContext.Current.Request.Files;
            //检查文件扩展名字
            HttpPostedFile postedFile = files[fileindex];
            string fileName, fileExtension, file_id;
            //取出精确到毫秒的时间做文件的名称
            fileName = System.IO.Path.GetFileName(postedFile.FileName);
            fileExtension = System.IO.Path.GetExtension(fileName);
            file_id = filename + ordernum + fileExtension.ToLower();
            if (fileName != "" && fileName != null && fileName.Length > 0)
            {
                fileExtension = System.IO.Path.GetExtension(fileName);
                string saveurl;
                modelimgurl = "Active/";
                saveurl = modelimgurl;
                saveurl = Server.MapPath(saveurl);
                if (!Directory.Exists(saveurl))
                {
                    Directory.CreateDirectory(saveurl);
                }
                int length = postedFile.ContentLength;
                postedFile.SaveAs(saveurl + file_id);
                modelimgurl = modelimgurl + file_id;
            }
            #endregion
            return modelimgurl;
        }
    }
}
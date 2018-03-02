using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DAL.MiniShop;
using System.Text;
using Model.MiniShop;
using System.IO;
using Mozart.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mozart.PalmShop.ShopCode
{
    public partial class PubSecHandEdit : System.Web.UI.Page
    {
        string action = string.Empty;
        public static string pid = string.Empty;
        string phone = string.Empty;
        public static string atlaslist = string.Empty;
        public static int altascount = 0;
        public static string contactID = string.Empty;
        public static int Review = 0;
        public static string errorscript = string.Empty;
        public static int ishand = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    if (Request["pid"] != null && Request["pid"] != "")
                    {
                        pid = Common.Common.NoHtml(Request["pid"]);
                        if (Request["action"] != null && Request["action"] != "")
                        {
                            action = Request["action"];
                        }
                        if (Request["ishand"] != null && Request["action"] != "")
                        {
                            try
                            {
                                ishand = Convert.ToInt32(Request["ishand"]);
                            }
                            catch (Exception)
                            {
                            }
                        }
                        if (action.Trim() != null && action.Trim() != "")
                        {
                            switch (action.ToLower().Trim())
                            {
                                case "getcateoption":
                                    GetOptionList();
                                    break;
                                case "delimg":
                                    DelImg();
                                    break;
                            }
                        }
                        getinfo();
                        errorscript = "";
                    }
                    else
                    {

                        JQDialog.SetCookies("pageurl", "PubSecHandEdit.aspx?ishand=" + ishand, 2);
                        getinfo();
                        errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error"); 
                        return;
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "PubSecHandEdit.aspx?ishand=" + ishand, 2);
                    getinfo();
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error"); 
                   return;
                }
            }
        }
        /// <summary>
        /// 获取类别
        /// </summary>
        void getinfo()
        {
            #region -----------一级导航绑定-----------
            ddlbigcategorylist.Items.Clear();
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet ds = new DataSet();
            ds = categoryDal.GetSecHandCategoryList(" and UpID='' and CsecHand=" + ishand);
            ddlbigcategorylist.DataSource = ds.Tables[0].DefaultView;
            ddlbigcategorylist.DataTextField = "Cname";
            ddlbigcategorylist.DataValueField = "ID";
            ddlbigcategorylist.DataBind();
            #endregion
            string cid = string.Empty; string bigcid = string.Empty;
            if (pid.Trim() != null && pid.Trim() != "")
            {
                MSProductDAL productDal = new MSProductDAL();
                DataSet productds = productDal.GetProductDetail(pid);
                MSProduct productModel = DataConvert.DataRowToModel<MSProduct>(productds.Tables[0].Rows[0]);
                cid = productModel.Cid;
                ptitle.Value = productModel.Ptitle;
                pdesc.Value = productModel.Pcontent;
                price.Value = productModel.Price.ToString();
                Review = productModel.Review;
                #region -----------获取一级类别编号------------
                if (cid!= null && cid!= "")
                {
                    string upid = string.Empty;
                    try
                    {
                        upid = categoryDal.GetMSPCategoryValueByID("UpID", cid).ToString();
                    }
                    catch (Exception)
                    {
                    }
                    if (upid != null && upid != "")
                    {
                        bigcid =upid;
                    }
                    else
                    {
                        bigcid = cid;
                    }
                    setpvalue.Value = cid;
                }
                #endregion
                ddlbigcategorylist.SelectedIndex =
                ddlbigcategorylist.Items.IndexOf(ddlbigcategorylist.Items.FindByValue(bigcid));
                #region------------二级导航绑定------------------
                if (bigcid != null && bigcid != "")
                {
                    ddlsmallcategorylist.Items.Clear();
                    ds = categoryDal.GetSecHandCategoryList(" and UpID='" + bigcid + "' ");
                    ddlsmallcategorylist.DataSource = ds.Tables[0].DefaultView;
                    ddlsmallcategorylist.DataTextField = "Cname";
                    ddlsmallcategorylist.DataValueField = "ID";
                    ddlsmallcategorylist.DataBind();
                    if (cid != null && cid != "")
                    {
                        ddlsmallcategorylist.SelectedIndex =
                        ddlsmallcategorylist.Items.IndexOf(ddlsmallcategorylist.Items.FindByValue(cid));
                    }
                }
                #endregion
                #region -------------获取图集---------------
                MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
                DataSet atlasDs = atlasDal.GetProductAtlasByPID(pid);
                int rowcount = 0; atlaslist = "";
                if (atlasDs != null && atlasDs.Tables.Count > 0 && atlasDs.Tables[0].Rows.Count > 0)
                {
                   rowcount = atlasDs.Tables[0].Rows.Count;
                    for (int i = 0; i < rowcount; i++)
                    {
                        string imgurl = atlasDs.Tables[0].Rows[i]["PimgUrl"].ToString();
                        string imgid = atlasDs.Tables[0].Rows[i]["ID"].ToString();
                        string datarow = string.Empty;
                        atlaslist += "\r\n<dd type=\"image\">\r\n" +
                        "<input type=\"file\" accept=\"image/jpg, image/jpeg, image/png\" " +
                            "onchange=\"form_pics.addImg(this);\" name=\"pics" + i + "\"><img dataid=\"" + imgid + 
                            "\" src=\"" + imgurl + "\">" +
                        "\r\n<span onclick=\"form_pics.removeImg(this);\">&nbsp;</span>\r\n" +
                    "</dd>";
                    }
                }
                if (rowcount < 8)
                {
                    atlaslist += "\r\n<dd datacount=\"" + rowcount + "\">\r\n" +
                    "<input type=\"file\" accept=\"image/jpg, image/jpeg, image/png\" " +
                        "onchange=\"form_pics.addImg(this);\" name=\"pics" + rowcount+ "\"><img src=\"images/upload.png\">" +
                    "\r\n<span onclick=\"form_pics.removeImg(this);\">&nbsp;</span>\r\n" +
                "</dd>";
                }
                altascount = rowcount;
                #endregion
                #region ----------------根据产品编号获取联系方式--------------------
                MSShopContactsDAL contactDal = new MSShopContactsDAL();
               DataSet contactDs= contactDal.GetContactDetailByPID(pid);
               if (contactDs != null && contactDs.Tables.Count > 0 && contactDs.Tables[0].Rows.Count > 0)
               {
                   string uphone = string.Empty;string uname = string.Empty;
                   uphone = contactDs.Tables[0].Rows[0]["Phone"].ToString();
                   uname = contactDs.Tables[0].Rows[0]["NickName"].ToString();
                   contactID = contactDs.Tables[0].Rows[0]["ID"].ToString();
                   UserPhone.Value = uphone;
                   UserName.Value = uname;
                   
               }
                #endregion
            }
        }
        protected void uploadbtn_Click(object sender, EventArgs e)
        {
            #region 信息判断
            if (Session["customerID"] != null && Session["customerID"].ToString() != "")
            {
                MSCustomersDAL customerDal = new MSCustomersDAL();
                phone = customerDal.GetCustomerValueByID("phone", Session["customerID"].ToString()).ToString();
            }
            else
            {
                JQDialog.SetCookies("pageurl", "PubSecHandEdit.aspx?ishand=" + ishand, 2);
                errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error"); 
                return;
            }
            MSProduct productModel = new MSProduct();
            if (setpvalue.Value.Trim() != null && setpvalue.Value.Trim() != "")
            {
                productModel.Cid = setpvalue.Value;
            }
            if (pdesc.Value.Trim() != null && pdesc.Value.Trim() != "")
            {
                productModel.Pcontent = pdesc.Value;
            }
            if (price.Value.Trim() != null && price.Value.Trim() != "")
            {
                productModel.Price = Convert.ToDecimal(price.Value);
            }
            if (ptitle.Value.Trim() != null && ptitle.Value.Trim() != "")
            {
                productModel.Ptitle = ptitle.Value;
            }
            if (price.Value.Trim() != null && price.Value.Trim() != "")
            {
                productModel.Price = Convert.ToDecimal(price.Value);
            }
            #endregion
            productModel.ID = pid;
            productModel.IsSecHand = ishand;
            productModel.Pstate = 0;
            productModel.Review = Review;
            productModel.CustomerID = Session["customerID"].ToString();
            MSProductDAL productDal = new MSProductDAL();

            MSShopContacts contactModel = new MSShopContacts();
            MSShopContactsDAL contactDal = new MSShopContactsDAL();
            contactModel.PID = pid;
            contactModel.IsDel = 0;
            contactModel.SID = "";
            contactModel.NickName = UserName.Value;
            contactModel.Phone = UserPhone.Value;
            bool contactflag = false;
            if (contactDal.ExistContact("", "", "", pid))
            {
                contactModel.ID = contactID;
                contactflag = contactDal.UpdateMSSContacts(contactModel);
            }
            else
            {
                contactModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                contactflag = contactDal.AddMSSContacts(contactModel);
            }
            if (productDal.UpdateMSProduct(productModel) && SaveImages())
            {
                errorscript = JQDialog.alertOKMsgBox(3, "操作成功！", "MySecHand.aspx", "error"); 
            }
            else
            {
                errorscript = JQDialog.alertOKMsgBox(4, "操作失败，请核对后再操作！", "", "error"); 
                return;

            }
        }
        /// <summary>
        /// 图集上传
        /// </summary>
        /// <returns></returns>
        private Boolean SaveImages()
        {
            HttpFileCollection files = HttpContext.Current.Request.Files;
            try
            {
                MSProductAtlas atlasModel = new MSProductAtlas();
                atlasModel.PID = pid;
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //检查文件扩展名字
                    HttpPostedFile postedFile = files[iFile];
                    string fileName, fileExtension,file_oldid, file_id;
                    //取出精确到毫秒的时间做文件的名称
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff")+ iFile.ToString();
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = my_file_id + fileExtension;
                    if (fileName != "" && fileName != null)
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        string saveurl, modelimgurl;
                        saveurl = modelimgurl = "Atlas/" + phone + "/";
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
                            ystp(saveurl + file_oldid, saveurl + file_id, 15);
                            File.Delete(saveurl + file_oldid);
                        }
                        else
                        {
                            postedFile.SaveAs(saveurl + file_id);
                        }

                        atlasModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        if (iFile == 0)
                        {
                            atlasModel.IsDefault = 1;
                        }
                        else
                        {
                            atlasModel.IsDefault = 0;
                        }
                        atlasModel.ImgState = 0;
                        atlasModel.PimgUrl = modelimgurl + file_id;
                        atlasModel.AtlasName = "";
                        MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
                        atlasDal.AddMSProductAtlas(atlasModel);
                    }
                }
                return true;
            }
            catch (System.Exception Ex)
            {
                return false;
            }
        }
        /// <summary>
        /// 获取二级导航
        /// </summary>
        void GetOptionList()
        {
            string subid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
            }
            else
            {
                return;
            }
            if (subid.Trim() != null && subid.Trim() != "")
            {
                MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
                DataSet ds = new DataSet();
                ds = categoryDal.GetSecHandCategoryList(" and UpID='" + subid + "' ");
                Response.Write(Dataset2Json(ds));
            }
            else
            {
                Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        public static string Dataset2Json(DataSet ds)
        {
            StringBuilder json = new StringBuilder();
            json.Append("[");
            foreach (System.Data.DataTable dt in ds.Tables)
            {
                json.Append(DataTable2Json(dt));
                json.Append(",");
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");
            return json.ToString();
        }
        public static string DataTable2Json(DataTable dt)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(dt.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(dt.Rows[i][j].ToString().Replace("\"", "\\\""));
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            return jsonBuilder.ToString();
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        void DelImg()
        {
            string imgid = string.Empty;
            if (Request["img"] != null && Request["img"] != "")
            {
                imgid = Common.Common.NoHtml(Request["img"]);
            }
            MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
            if (imgid != null && imgid != "")
            {
                if (atlasDal.UpdateMSProductAtlasState(imgid))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true,\"msg\":\"操作失败，请核对后再操作\"}");
                }
            }
            Response.End();
        }
        #region 压缩图片
        /// <summary>
        /// 压缩图片
        /// </summary>
        /// <param name="filePath">要压缩的图片的路径</param>
        /// <param name="filePath_ystp">压缩后的图片的路径</param>
        /// <param name="quality">压缩后的图片质量（范围0-100）</param>
        public void ystp(string filePath, string filePath_ystp, int quality)
        {
            Bitmap bmp = null; ImageCodecInfo ici = null; EncoderParameters eptS = null;
            try
            {
                bmp = new Bitmap(filePath);
                ici = this.getImageCoderInfo("image/jpeg");
                eptS = new EncoderParameters(1);
                eptS.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);
                bmp.Save(filePath_ystp, ici, eptS);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                bmp.Dispose();
                eptS.Dispose();
            }
        }
        /// <summary>
        /// 获取图片编码类型信息
        /// </summary>
        /// <param name="coderType">编码类型</param>
        /// <returns>ImageCodecInfo</returns>
        private ImageCodecInfo getImageCoderInfo(string coderType)
        {
            ImageCodecInfo[] iciS = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo retIci = null;
            foreach (ImageCodecInfo ici in iciS)
            {
                if (ici.MimeType.Equals(coderType))
                    retIci = ici;
            }
            return retIci;
        }
        #endregion 
    }
}
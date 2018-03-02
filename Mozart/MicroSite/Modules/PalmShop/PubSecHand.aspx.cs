using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using DAL.MiniShop;
using System.Text;
using Model.MiniShop;
using System.IO;
using Mozart.Common;
using System.Drawing;
using System.Drawing.Imaging;

namespace Mozart.PalmShop.ShopCode
{
    public partial class PubSecHand : System.Web.UI.Page
    {
        string action = string.Empty;
        string pid = string.Empty;
        string phone = string.Empty;
        public static string errorscript = string.Empty;
        public static int ishand = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    if (Request["action"] != null && Request["action"] != "")
                    {
                        action = Request["action"];
                    }
                    if (Request["ishand"] != null && Request["ishand"] != "")
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
                        }
                    }
                    getinfo();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "PubSecHand.aspx?ishand=" + ishand, 2);
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
            ddlbigcategorylist.Items.Clear();
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet ds = new DataSet();
            ds = categoryDal.GetSecHandCategoryList(" and UpID='' and CsecHand=" + ishand);
            DataTable dt = ds.Tables[0];
            DataRow dr = ds.Tables[0].NewRow();
            dr["ID"] = "";
            dr["Cname"] = "--请选择类别--";
            dt.Rows.InsertAt(dr, 0);
            ddlbigcategorylist.DataSource = ds.Tables[0].DefaultView;
            ddlbigcategorylist.DataTextField = "Cname";
            ddlbigcategorylist.DataValueField = "ID";
            ddlbigcategorylist.DataBind();
            MSCustomersDAL customerDal = new MSCustomersDAL();
            if (Session["customerID"] != null && Session["customerID"].ToString() != "")
            {
                phone = customerDal.GetCustomerValueByID("Phone", Session["customerID"].ToString()).ToString();
            }
            UserPhone.Value = phone;
        }
        protected void uploadbtn_Click(object sender, EventArgs e)
        {
            #region 信息判断
            if (Session["customerID"] == null || Session["customerID"].ToString() == "")
            {
                JQDialog.SetCookies("pageurl", "PubSecHand.aspx?ishand=" + ishand, 2);
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

            Random random = new Random();
            int r1 = (int)(random.Next(0, 9));//产生2个0-9的随机数
            int r2 = (int)(random.Next(0, 9));
            string now = DateTime.Now.ToString("yyMMddhhmmssf");//一个13位的时间戳
            String paymentID = r1.ToString() + r2.ToString() + now;

            pid = "R" + paymentID;
            productModel.ID =pid;
            productModel.IsSecHand = ishand;
            productModel.Pstate = 0;
            productModel.Review = 1;
            productModel.CustomerID = Session["customerID"].ToString();
            MSProductDAL productDal = new MSProductDAL();
            MSShopContacts contactModel = new MSShopContacts();
            MSShopContactsDAL contactDal = new MSShopContactsDAL();
            contactModel.ID = Guid.NewGuid().ToString("N").ToUpper();
            contactModel.PID = pid;
            contactModel.IsDel = 0;
            contactModel.SID = "";
            contactModel.NickName = UserName.Value;
            contactModel.Phone = UserPhone.Value;
            string pageurl = "NewProduct.aspx?ishand=" + ishand;
            if (productDal.AddMSProduct(productModel) && SaveImages()&&contactDal.AddMSSContacts(contactModel))
            {
                if (ishand == 1)
                {
                    pageurl = "MySecHand.aspx";
                }
                errorscript = JQDialog.alertOKMsgBox(5, "操作成功！", pageurl, "succeed");
            }
            else
            {
                errorscript = JQDialog.alertOKMsgBox(3, "操作失败<br/>请核对后再操作！", "", "error");
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
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + iFile.ToString();
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = my_file_id + fileExtension;
                    if (fileName != "" && fileName!=null)
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        string saveurl,modelimgurl;
                        saveurl = modelimgurl = "Atlas/" + phone+"/";
                        saveurl = Server.MapPath(saveurl);
                        if (!Directory.Exists(saveurl))
                        {
                            Directory.CreateDirectory(saveurl);
                        }
                        
                        int length =postedFile.ContentLength;
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
                        atlasModel.ImgState = 0;
                        atlasModel.PimgUrl = modelimgurl+ file_id;
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
                ici = getImageCoderInfo("image/jpeg");
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
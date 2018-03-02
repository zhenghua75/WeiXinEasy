using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Model.MiniShop;
using DAL.MiniShop;
using System.IO;
using Mozart.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.Text;

namespace Mozart.PalmShop.ShopCode
{
    public partial class EditeProduct : System.Web.UI.Page
    {
        string action = string.Empty;
        string pid = string.Empty;
        string phone = string.Empty;
        public static string errorscript = string.Empty;
        string sid = string.Empty;
        string customerid = string.Empty;
        string atlaslist = string.Empty;
        int altascount = 0;
        int mcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Request["sid"] != null && Request["sid"].ToString() != "")
                {
                    sid = Request["sid"].ToString();
                }
                
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customerid = Session["customerID"].ToString();
                    if (Request["pid"] != null && Request["pid"].ToString() != "")
                    {
                        pid = Request["pid"].ToString();
                    }
                    else
                    {
                        JQDialog.SetCookies("pageurl", "MyShop.aspx", 1);
                        errorscript = JQDialog.alertOKMsgBoxGoBack(3, "操作失败<br/>请登录后再操作！",true);
                    }
                    if (Request["action"] != null && Request["action"] != "")
                    {
                        action = Request["action"];
                    }
                    if (action.Trim() != null && action.Trim() != "")
                    {
                        switch (action.ToLower().Trim())
                        {
                            case "getcateoption":
                                GetOptionList();
                                break;
                            case "edite":
                                ProductEdite();
                                break;
                            case"delimg":
                                DelImg();
                                break;
                            case "delpara":
                                DelPara();
                                break;
                        }
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "MyShop.aspx", 1);
                    errorscript = JQDialog.alertOKMsgBox(3, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                }
                GetHtmlInfo();
            }
        }
        /// <summary>
        /// 输出到页面
        /// </summary>
        void GetHtmlInfo()
        {
            MSProduct ProductModel=new MSProduct ();
            MSProductDAL ProductDal = new MSProductDAL();
            MSProductCategory categoryModel = new MSProductCategory();
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet detailds = null; string cid = string.Empty; string bigcid = string.Empty;
            #region------------------产品详细-------------------------
            if (pid.Trim() != null && pid.Trim() != "")
            {
                detailds = ProductDal.GetProductDetail(pid);
                if (detailds != null && detailds.Tables.Count > 0 && detailds.Tables[0].Rows.Count > 0)
                {
                     ProductModel = DataConvert.DataRowToModel<MSProduct>(detailds.Tables[0].Rows[0]);
                     cid = ProductModel.Cid;
                }
            }
            #endregion
            #region-----------类别绑定---------------
            if (cid != null && cid != "")
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
                    bigcid = upid;
                }
                else
                {
                    bigcid = cid;
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
                atlaslist += "\r\n<dd>\r\n" +
                "<input type=\"file\" accept=\"image/jpg, image/jpeg, image/png\" " +
                    "onchange=\"form_pics.addImg(this);\" name=\"pics" + rowcount + "\"><img src=\"images/upload.png\">" +
                "\r\n<span onclick=\"form_pics.removeImg(this);\">&nbsp;</span>\r\n" +
            "</dd>";
            }
            altascount = rowcount;
            #endregion
            #region------------------绑定型号------------------
            List<MSProductPara> paralistmodel = new List<MSProductPara>();
            MSProductParaDAL paraDal = new MSProductParaDAL();
            DataSet parads = paraDal.GetProductParamByPID(pid);
            if (parads != null && parads.Tables.Count > 0 && parads.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in parads.Tables[0].Rows)
                {
                    MSProductPara paramodel = DataConvert.DataRowToModel<MSProductPara>(row);
                    paralistmodel.Add(paramodel);
                }
            }
            #endregion

            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/EditeProduct.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["customid"] = customerid;
            context.TempData["atlaslist"] = atlaslist;
            context.TempData["altascount"] = altascount;
            context.TempData["cid"] = cid;
            context.TempData["bigcid"] = bigcid;
            context.TempData["pdetail"] = ProductModel;
            context.TempData["paralist"] = paralistmodel;
            context.TempData["footer"] = "奥琦微商易";
            context.TempData["errorscript"] = errorscript;

            JinianNet.JNTemplate.Template t = new JinianNet.JNTemplate.Template(context, text);
            t.Render(Response.Output);
        }
        /// <summary>
        /// 获取类别列表
        /// </summary>
        void GetOptionList()
        {
            string subid = string.Empty;
            if (Request["subid"] != null && Request["subid"] != "")
            {
                subid = Common.Common.NoHtml(Request["subid"]);
                subid = " and UpID='" + subid + "' ";
            }
            if (sid != null && sid != "")
            {
                sid = " and [SID]='" + sid + "' ";
            }
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet ds = new DataSet();
            ds = categoryDal.GetCategoryList(subid + sid + " and CsecHand=0 ");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Response.Write(Dataset2Json(ds));
            }
            Response.End();
        }
        void ProductEdite()
        {
            string cid = string.Empty; string ptitle = string.Empty; string pcontent = string.Empty;
            string price = string.Empty; string zipcode = string.Empty; string isnull = "";
            #region --------------获取值--------------------
            try
            {
                cid = HttpContext.Current.Request.Form.Get("setoptionvalue").ToString();
            }
            catch (Exception)
            {
                cid = isnull;
            }
            try
            {
                ptitle = HttpContext.Current.Request.Form.Get("ptitle").ToString();
            }
            catch (Exception)
            {
                ptitle = isnull;
            }
            try
            {
                pcontent = HttpContext.Current.Request.Form.Get("pdesc").ToString();
            }
            catch (Exception)
            {
                pcontent = isnull;
            }
            try
            {
                price = HttpContext.Current.Request.Form.Get("price").ToString();
            }
            catch (Exception)
            {
                price = isnull;
            }
            try
            {
                price = HttpContext.Current.Request.Form.Get("price").ToString();
            }
            catch (Exception)
            {
                price = isnull;
            }
            try
            {
                zipcode = HttpContext.Current.Request.Form.Get("zipcode").ToString();
            }
            catch (Exception)
            {
                zipcode = isnull;
            }
            #endregion

            if (ptitle != null && ptitle != "")
            {
                MSProduct ProductModel = new MSProduct();
                MSProductDAL ProductDal = new MSProductDAL();
                ProductModel.ID = pid;
                ProductModel.Cid = cid;
                ProductModel.Ptitle = ptitle;
                ProductModel.Pcontent = pcontent;
                if (price != null && price != "")
                {
                    ProductModel.Price = decimal.Parse(price);
                }
                ProductModel.CustomerID = customerid;
                ProductModel.SiteCode = "";
                ProductModel.SID = sid;
                ProductModel.IsSecHand = 0;
                ProductModel.Pstate = 0;
                ProductModel.ZipCode = (zipcode == "" || zipcode == null ? "包邮" : zipcode);

                if (EditeProductModel()&&SaveImages()&&ProductDal.UpdateMSProduct(ProductModel))
                {
                    errorscript = JQDialog.alertOKMsgBox(3, "操作成功！", "MyShop.aspx", "succeed");
                }
                else
                {
                    errorscript = JQDialog.alertOKMsgBox(3, "操作失败<br/>请核对后再操作！", "", "error");
                }
            }
        }
        /// <summary>
        /// 图集上传
        /// </summary>
        /// <returns></returns>
        bool SaveImages()
        {
            bool result = false;
            if (customerid != null && customerid != "")
            {
                MSCustomersDAL customer = new MSCustomersDAL();
                phone = customer.GetCustomerValueByID("Phone", customerid).ToString() + "/";
            }
            HttpFileCollection files = HttpContext.Current.Request.Files;
            try
            {
                MSProductAtlas atlasModel = new MSProductAtlas();
                MSProductAtlasDAL atlasDal = new MSProductAtlasDAL();
                atlasModel.PID = pid;
                for (int iFile = 0; iFile < files.Count; iFile++)
                {
                    //检查文件扩展名字
                    HttpPostedFile postedFile = files[iFile];
                    string fileName, fileExtension, file_oldid, file_id;
                    //取出精确到毫秒的时间做文件的名称
                    string my_file_id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + iFile.ToString();
                    fileName = System.IO.Path.GetFileName(postedFile.FileName);
                    fileExtension = System.IO.Path.GetExtension(fileName);
                    file_id = my_file_id + fileExtension;
                    if (fileName != "" && fileName != null)
                    {
                        fileExtension = System.IO.Path.GetExtension(fileName);
                        string saveurl, modelimgurl;
                        saveurl = modelimgurl = "Atlas/" + phone;
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
                        atlasModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        if (iFile == 0)
                        {
                            if (!atlasDal.IsExitDefaultImg(pid))
                            {
                                atlasModel.IsDefault = 1;
                            }
                        }
                        atlasModel.ImgState = 0;
                        atlasModel.PimgUrl = modelimgurl + file_id;
                        atlasModel.AtlasName = "";
                        atlasDal.AddMSProductAtlas(atlasModel);
                    }
                }
                result = true;
            }
            catch (System.Exception Ex)
            {
                result= false;
            }
            return result;
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
        /// 修改属性列表
        /// </summary>
        bool EditeProductModel()
        {
            string isnull = ""; bool result = false;
            if (Request["m"] != null && Request["m"] != "")
            {
                try
                {
                    mcount = Convert.ToInt32(Common.Common.NoHtml(Request["m"]));
                }
                catch (Exception)
                {
                }
            }
            if (mcount > 0)
            {
                MSProductParaDAL paraDal = new MSProductParaDAL();
                for (int i = 0; i < mcount; i++)
                {
                    string m_model = string.Empty;string m_price = string.Empty;
                    string m_stock = string.Empty; int stock = 0;
                    string editepara = string.Empty;
                    #region--------------------获取model值---------------------
                    try
                    {
                        m_model = HttpContext.Current.Request.Form.Get("model" + i).ToString();
                    }
                    catch (Exception)
                    {
                        m_model = isnull;
                    }
                    try
                    {
                        m_price = HttpContext.Current.Request.Form.Get("mprice" + i).ToString();
                    }
                    catch (Exception)
                    {
                        m_price = isnull;
                    }
                    try
                    {
                        m_stock = HttpContext.Current.Request.Form.Get("mstock" + i).ToString();
                    }
                    catch (Exception)
                    {
                        m_stock = isnull;
                    }
                    if (m_stock != null && m_stock != "")
                    {
                        try
                        {
                            stock = Convert.ToInt32(m_stock);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    try
                    {
                        editepara = HttpContext.Current.Request.Form.Get("editepara" + i).ToString();
                    }
                    catch (Exception)
                    {
                        editepara = isnull;
                    }
                    #endregion
                    MSProductPara paraModel = new MSProductPara();
                    paraModel.ParName = m_model;
                    paraModel.Price = Convert.ToDecimal(m_price);
                    paraModel.Stock = Convert.ToInt32(stock);
                    paraModel.PID = pid;
                    if (editepara != null && editepara != "")
                    {
                        paraModel.ID = editepara;
                        if (paraDal.UpdateMSPPara(paraModel))
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        paraModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                        if (paraDal.AddMSPPara(paraModel))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
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
                    Response.Write("{\"error\":true}");
                }
            }
            Response.End();
        }
        /// <summary>
        /// 删除型号
        /// </summary>
        void DelPara()
        {
            string pr = string.Empty;
            if (Request["pr"] != null && Request["pr"] != "")
            {
                pr = Common.Common.NoHtml(Request["pr"]);
            }
            MSProductParaDAL paraDal = new MSProductParaDAL();
            if (pr != null && pr != "")
            {
                if (paraDal.UpdateMSPParaState(pr))
                {
                    Response.Write("{\"success\":true}");
                }
                else
                {
                    Response.Write("{\"error\":true}");
                }
            }
            Response.End();
        }
    }
}
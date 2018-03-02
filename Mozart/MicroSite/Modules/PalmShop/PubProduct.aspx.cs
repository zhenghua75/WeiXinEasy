using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DAL.MiniShop;
using System.Text;
using Model.MiniShop;
using System.IO;
using Mozart.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;

namespace Mozart.PalmShop.ShopCode
{
    public partial class PubProduct : System.Web.UI.Page
    {
        string action = string.Empty;
        string pid = string.Empty;
        string phone = string.Empty;
        public static string errorscript = string.Empty;
        string sid = string.Empty;
        string customerid = string.Empty;
        int mcount = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                errorscript = "";
                if (Session["SID"] != null && Session["SID"].ToString() != "")
                {
                    sid = Session["SID"].ToString();
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "MyShop.aspx", 5);
                    errorscript = JQDialog.alertOKMsgBox(5, "您还没开通微店，请开通后再操作？", "ApplyShop.aspx?action=reg",
                        "error");
                }
                if (Session["customerID"] != null && Session["customerID"].ToString() != "")
                {
                    customerid = Session["customerID"].ToString();
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
                            case "publish":
                                PublicProduct();
                                break;
                        }
                    }
                }
                else
                {
                    JQDialog.SetCookies("pageurl", "MyShop.aspx", 1);
                    errorscript = JQDialog.alertOKMsgBox(5, "操作失败<br/>请登录后再操作！", "UserLogin.aspx", "error");
                }
                GetHtmlInfo();
            }
        }
        /// <summary>
        /// 输出到页面
        /// </summary>
        void GetHtmlInfo()
        {
            string text = System.IO.File.ReadAllText(Server.MapPath("../ShopPage/PubProduct.html"));
            JinianNet.JNTemplate.TemplateContext context = new JinianNet.JNTemplate.TemplateContext();

            context.TempData["customid"] = customerid;
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
            sid = " and [SID]!='' ";
            MSProductCategoryDAL categoryDal = new MSProductCategoryDAL();
            DataSet ds = new DataSet();
            ds = categoryDal.GetCategoryList(subid + sid + " and CsecHand=0 ");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Response.Write(Dataset2Json(ds));
            }
            else
            {
                //Response.Write("{\"success\":\"操作失败\"}");
            }
            Response.End();
        }
        /// <summary>
        /// 产品发布
        /// </summary>
        void PublicProduct()
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
            Random random = new Random();
            int r1 = (int)(random.Next(0, 9));
            int r2 = (int)(random.Next(0, 9));
            string now = DateTime.Now.ToString("yyMMddhhmmssf");
            String paymentID = r1.ToString() + r2.ToString() + now;
            pid = "R" + paymentID;

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
                ProductModel.Review =1;
                ProductModel.ZipCode = (zipcode == "" || zipcode == null ? "包邮" : zipcode);
                if (SaveImages()&&AddProductModel()&&ProductDal.AddMSProduct(ProductModel))
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
                phone = customer.GetCustomerValueByID("Phone", customerid).ToString()+"/";
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
                        if (atlasDal.AddMSProductAtlas(atlasModel))
                        {
                            result = true;
                        }
                    }
                }
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
        /// 添加属性列表
        /// </summary>
        bool AddProductModel() 
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
                    string m_model = string.Empty;
                    string m_price = string.Empty;
                    string m_stock = string.Empty; int stock = 0;
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
                    #endregion
                    MSProductPara paraModel = new MSProductPara();
                    paraModel.ParName = m_model;
                    paraModel.Price = Convert.ToDecimal(m_price);
                    paraModel.Stock = Convert.ToInt32(stock);
                    paraModel.ID = Guid.NewGuid().ToString("N").ToUpper();
                    paraModel.PID = pid;
                    if (paraDal.AddMSPPara(paraModel))
                    {
                        result = true;
                    }
                }
            }
            return result;
        }
    }
}
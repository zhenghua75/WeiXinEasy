using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Mozart.Common;

namespace Mozart.MicroSite
{
    public partial class MyThumbPrintTwo : System.Web.UI.Page
    {
        string strThumbID = string.Empty;
        string strThumbMsg = string.Empty;
        string strPrintCode = string.Empty;
        string strSiteCode = string.Empty;
        string strOpenID = string.Empty;
        string strClientID = string.Empty;
        string strFilePath = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (null == Request.QueryString["id"])
            {
                return;
            }

            strThumbID = Common.Common.NoHtml(Request.QueryString["id"].ToString());
            DAL.Album.UserPhotoDAL dal = new DAL.Album.UserPhotoDAL();
            DataSet ds = dal.GetMyThumb(strThumbID);

            Model.Album.UserPhoto model = new Model.Album.UserPhoto();
            if (null != ds && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                model = DataConvert.DataRowToModel<Model.Album.UserPhoto>(ds.Tables[0].Rows[0]);
            }

            strSiteCode = model.SiteCode.ToString();
            strOpenID = model.OpenId.ToString();
            strThumbMsg = Common.Common.NoHtml(Request.Form["thumbMsg"].ToString());
            strPrintCode = Common.Common.NoHtml(Request.Form["printCode"].ToString());

            //通过打印码得到打印端ID
            DAL.HP.HPClientDAL dalClient = new DAL.HP.HPClientDAL();
            DataSet dsClient = dalClient.GetPrintClient(strSiteCode, strPrintCode);

            if (null != dsClient && dsClient.Tables.Count > 0 && dsClient.Tables[0].Rows.Count > 0)
            {
                strClientID = dsClient.Tables[0].Rows[0]["ClientCode"].ToString();
            }
            else
            {
                DAL.HP.PrintCodeDAL dalCode = new DAL.HP.PrintCodeDAL();
                strClientID = dalCode.GetClientIDByPrintCode(strPrintCode, strSiteCode);
            }

            if (string.IsNullOrEmpty(strClientID))
            {
                Response.Write("<script>alert('打印码已经使用过!')</script>");
                Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                return;
            }

            //插入打印数据　ID,SiteCode,OpenId,ClientID,PrintCode,Img,State,AttachText
            DAL.HP.PhotoDAL dalPhoto = new DAL.HP.PhotoDAL();
            if (null != dsClient && dsClient.Tables.Count > 0 && dsClient.Tables[0].Rows.Count > 0)
            {
                int iPrintcount = 1;
                iPrintcount = int.Parse(dsClient.Tables[0].Rows[0]["FreeAmount"].ToString());
                if (dalPhoto.OpenIDPhotoCount(strOpenID, strPrintCode) > iPrintcount)
                {
                    Response.Write("<script>alert('你已经提交过打印照片!')</script>");
                    Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                }
                else
                {
                    //照片处理
                    string[] urls = model.FilePath.Split('.');
                    strFilePath = model.FilePath;
                    string _url = strSiteCode + "_" + strOpenID + "." + urls.Last();

                    string inputurl = Server.MapPath("~/User_Photo/") + strFilePath;
                    string outputurl = Server.MapPath("~/HP_Photo/") + _url;

                    int width = 260;
                    int height = 260;

                    System.IO.FileStream fs = new System.IO.FileStream(inputurl, System.IO.FileMode.Open);
                    CutForCustom(fs, outputurl, width, height, 100);
                    fs.Close();

                    Model.HP.Photo modelPhoto = new Model.HP.Photo()
                    {
                        SiteCode = strSiteCode,
                        OpenId = strOpenID,
                        ClientID = strClientID,
                        PrintCode = strPrintCode,
                        Img = strSiteCode + "_" + strOpenID + "." + model.FilePath.Split('.').Last(),
                        AttachText = strThumbMsg
                    };
                    dalPhoto.InsertInfo(modelPhoto);
                    Response.Write("<script>alert('提交成功!')</script>");
                    Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
                }
            }
            else
            {
                //照片处理
                string[] urls = model.FilePath.Split('.');
                strFilePath = model.FilePath;
                string _url = strSiteCode + "_" + strOpenID + "." + urls.Last();

                string inputurl = Server.MapPath("~/User_Photo/") + strFilePath;
                string outputurl = Server.MapPath("~/HP_Photo/") + _url;

                int width = 260;
                int height = 260;

                System.IO.FileStream fs = new System.IO.FileStream(inputurl, System.IO.FileMode.Open);
                CutForCustom(fs, outputurl, width, height, 100);
                fs.Close();

                Model.HP.Photo modelPhoto = new Model.HP.Photo()
                {
                    SiteCode = strSiteCode,
                    OpenId = strOpenID,
                    ClientID = strClientID,
                    PrintCode = strPrintCode,
                    Img = strSiteCode + "_" + strOpenID + "." + model.FilePath.Split('.').Last(),
                    AttachText = strThumbMsg
                };
                dalPhoto.InsertInfo(modelPhoto);
                Response.Write("<script>alert('提交成功!')</script>");
                Response.Write("<script>document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {	WeixinJSBridge.call('closeWindow');});</script>");
            }        
        }

        #region 自定义裁剪并缩放

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图Stream对象</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(System.IO.Stream fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(fromFile, true);

            //原图宽高均小于模版，不作处理，直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;
                //原图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;

                //原图与模版比例相等，直接缩放
                if (templateRate == initRate)
                {
                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                //原图与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象实例化
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪源定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);

                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //裁剪
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //关键质量控制
                    //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                    ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                    ImageCodecInfo ici = null;
                    foreach (ImageCodecInfo i in icis)
                    {
                        if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                        {
                            ici = i;
                        }
                    }
                    EncoderParameters ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                    //保存缩略图
                    templateImage.Save(fileSaveUrl, ici, ep);
                    //templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
        }
        #endregion
    }
}
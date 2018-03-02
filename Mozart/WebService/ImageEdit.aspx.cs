using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Design;

namespace Mozart.WebService
{
    public partial class ImageEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string picName = "http_imgload.jpg";
            string picPath = "/UploadFiles/" + picName;
            target.ImageUrl = picPath;
            preview.ImageUrl = picPath;
        }

        protected void saveImg(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                string[] urls = target.ImageUrl.Split('/');
                string _url = urls.Last();

                string tempurl = Path.Combine(Server.MapPath("~/UploadFiles/"), _url);
                int startX = int.Parse(x1.Text);
                int startY = int.Parse(y1.Text);
                int width = int.Parse(Iwidth.Text);
                int height = int.Parse(Iheight.Text);
                ImgReduceCutOut(startX, startY, width, height, tempurl, tempurl);
            }
        }
        /// <summary>
        /// 缩小裁剪图片
        /// </summary>
        /// <param name="int_Width">要缩小裁剪图片宽度</param>
        /// <param name="int_Height">要缩小裁剪图片长度</param>
        /// <param name="input_ImgUrl">要处理图片路径</param>
        /// <param name="out_ImgUrl">处理完毕图片路径</param>
        public void ImgReduceCutOut(int startX, int startY, int int_Width, int int_Height, string input_ImgUrl, string out_ImgUrl)
        {
            // ＝＝＝上传标准图大小＝＝＝
            int int_Standard_Width = 120;
            int int_Standard_Height = 120;

            int Reduce_Width = 0; // 缩小的宽度
            int Reduce_Height = 0; // 缩小的高度
            int CutOut_Width = 0; // 裁剪的宽度
            int CutOut_Height = 0; // 裁剪的高度
            int level = 100; //缩略图的质量 1-100的范围

            // ＝＝＝获得缩小，裁剪大小＝＝＝
            if (int_Standard_Height * int_Width / int_Standard_Width > int_Height)
            {
                Reduce_Width = int_Width;
                Reduce_Height = int_Standard_Height * int_Width / int_Standard_Width;
                CutOut_Width = int_Width;
                CutOut_Height = int_Height;
            }
            else if (int_Standard_Height * int_Width / int_Standard_Width < int_Height)
            {
                Reduce_Width = int_Standard_Width * int_Height / int_Standard_Height;
                Reduce_Height = int_Height;
                CutOut_Width = int_Width;
                CutOut_Height = int_Height;
            }
            else
            {
                Reduce_Width = int_Width;
                Reduce_Height = int_Height;
                CutOut_Width = int_Width;
                CutOut_Height = int_Height;
            }

            // ＝＝＝通过连接创建Image对象＝＝＝
            System.Drawing.Image oldimage = System.Drawing.Image.FromFile(input_ImgUrl);
            oldimage.Save(Server.MapPath("tepm.jpg"));
            oldimage.Dispose();

            //// ＝＝＝缩小图片＝＝＝
            //System.Drawing.Image thumbnailImage = oldimage.GetThumbnailImage(Reduce_Width, Reduce_Height, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            Bitmap bm = new Bitmap(Server.MapPath("tepm.jpg"));

            // ＝＝＝处理JPG质量的函数＝＝＝
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            ImageCodecInfo ici = null;
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.MimeType == "image/jpeg")
                {
                    ici = codec;
                    break;
                }

            }
            EncoderParameters ep = new EncoderParameters();
            ep.Param[0] = new EncoderParameter(Encoder.Quality, (long)level);



            // ＝＝＝裁剪图片＝＝＝
            Rectangle cloneRect = new Rectangle(startX, startY, CutOut_Width, CutOut_Height);
            PixelFormat format = bm.PixelFormat;
            Bitmap cloneBitmap = bm.Clone(cloneRect, format);

            // ＝＝＝保存图片＝＝＝
            cloneBitmap.Save(out_ImgUrl, ici, ep);
            bm.Dispose();

        }

        public bool ThumbnailCallback()
        {
            return false;
        }
    }
}
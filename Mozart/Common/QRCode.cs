using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThoughtWorks.QRCode.Codec;
using ThoughtWorks.QRCode.Codec.Data;
using ThoughtWorks.QRCode.Codec.Util;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Mozart.Common
{
    public class QRCode
    {
        #region 不带图标的二维码
        public string GetQRCode(string strCodetext)
        {
            string strImage = string.Empty;
            if (!string.IsNullOrEmpty(strCodetext))
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.H;
                Image qrImage = qrCodeEncoder.Encode(strCodetext);
                System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                qrImage.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);                
                byte[] arr = new byte[MStream.Length];
                MStream.Position = 0;
                MStream.Read(arr, 0, (int)MStream.Length);
                MStream.Close();
                MStream.Dispose();
                strImage = Convert.ToBase64String(arr);
            }
            return strImage;
        }
        #endregion

        #region 带图标的二维码
        public string GetImageQRCode(string strCodetext)
        {
            string strImage = string.Empty;
            if (!string.IsNullOrEmpty(strCodetext))
            {
                QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
                qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
                qrCodeEncoder.QRCodeScale = 4;
                qrCodeEncoder.QRCodeVersion = 8;
                qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
                Image qrImage = qrCodeEncoder.Encode(strCodetext);
                System.IO.MemoryStream MStream = new System.IO.MemoryStream();
                System.IO.MemoryStream MStreamCombin = new System.IO.MemoryStream();
                qrImage.Save(MStream, System.Drawing.Imaging.ImageFormat.Png);
                CombinImage(qrImage, HttpContext.Current.Server.MapPath("/WebService/images/hlfico.png")).Save(MStreamCombin, System.Drawing.Imaging.ImageFormat.Png);
                byte[] arr = new byte[MStreamCombin.Length];
                MStreamCombin.Position = 0;
                MStreamCombin.Read(arr, 0, (int)MStreamCombin.Length);
                MStreamCombin.Close();
                MStreamCombin.Dispose();
                strImage = Convert.ToBase64String(arr);
            }
            return strImage;
        }
        #endregion

        #region 图片转为base64编码的字符串
        //图片转为base64编码的字符串
        protected string ImgToBase64String(string Imagefilename)
        {
            try
            {
                Bitmap bmp = new Bitmap(Imagefilename);

                MemoryStream ms = new MemoryStream();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] arr = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(arr, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(arr);
            }
            catch
            {
                return null;
            }
        }

        //base64编码的字符串转为图片
        protected Bitmap Base64StringToImage(string strbase64)
        {
            try
            {
                byte[] arr = Convert.FromBase64String(strbase64);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                return bmp;
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 加个性图标处理
        /// <summary>  
        /// 调用此函数后使此两种图片合并，类似相册，有个  
        /// 背景图，中间贴自己的目标图片  
        /// </summary>  
        /// <param name="imgBack">粘贴的源图片</param>  
        /// <param name="destImg">粘贴的目标图片</param>  
        public static Image CombinImage(Image imgBack, string destImg)
        {
            Image img = Image.FromFile(destImg);        //照片图片    
            if (img.Height != 65 || img.Width != 65)
            {
                img = KiResizeImage(img, 65, 65, 0);
            }
            Graphics g = Graphics.FromImage(imgBack);
            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      
            g.FillRectangle(System.Drawing.Brushes.Black, imgBack.Width / 2 - img.Width / 2 - 1, imgBack.Width / 2 - img.Width / 2 - 1,1,1);//相片四周刷一层黑色边框  
            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);            
            GC.Collect();
            return imgBack;
        }


        /// <summary>  
        /// Resize图片  
        /// </summary>  
        /// <param name="bmp">原始Bitmap</param>  
        /// <param name="newW">新的宽度</param>  
        /// <param name="newH">新的高度</param>  
        /// <param name="Mode">保留着，暂时未用</param>  
        /// <returns>处理以后的图片</returns>  
        public static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                Image b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量  
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp, new Rectangle(0, 0, newW, newH), new Rectangle(0, 0, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch
            {
                return null;
            }
        }
        #endregion

    }       
}
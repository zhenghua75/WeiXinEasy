using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Mozart.Common
{
    public class DrawingHelper
    {
        /// <summary>
        /// 添加文字到图片
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        public static Bitmap AddTextToImage(Image image, string text,Font font,float x,float y)
        {
            Bitmap bitmap = new Bitmap(image, image.Width, image.Height);
            Graphics g = Graphics.FromImage(bitmap);
            Brush textBrush=new SolidBrush(Color.Black);
            g.DrawString(text, font, textBrush, x, y);
            return bitmap;
        }
    }
}
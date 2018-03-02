using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace HappyPhoto
{
    public partial class frmModelTest : Form
    {
        public frmModelTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MultiFormatWriter mutiWriter = new MultiFormatWriter();
            BitMatrix bm = mutiWriter.encode("http://www.vgo2013.com/WebService/QR.aspx?ID=" + "111111", BarcodeFormat.QR_CODE, 300, 300);
            ZXing.Rendering.BitmapRenderer br = new ZXing.Rendering.BitmapRenderer();
            Bitmap img = br.Render(bm, BarcodeFormat.QR_CODE, "http://www.vgo2013.com/WebService/QR.aspx?ID=" + "111111");
            img = KiResizeImage(img, 90, 90, 0);
            string filename = System.Environment.CurrentDirectory + "\\QR" + DateTime.Now.Ticks.ToString() + ".jpg";
            img.Save(filename, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        ///
        /// Resize图片
        ///
        /// 原始Bitmap
        /// 新的宽度
        /// 新的高度
        /// 保留着，暂时未用
        /// 处理以后的图片
        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH, int Mode)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
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
 
    }
}

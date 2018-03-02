using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;

namespace HappyPhoto
{
    public partial class frmGetPrintPhoto : Form
    {
        [DllImport("wininet")]
        private extern static bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        delegate void WriteInvoke();
        bool bGDateIsExit = false;
        private PrintPreviewDialog PrintPreview = new PrintPreviewDialog();
        PrintDocument printDocument = new PrintDocument();
        srHappyPhoto.wsHappyPhotoSoapClient wsGetPrindCode = new srHappyPhoto.wsHappyPhotoSoapClient();
        string strHappyPhotoID = string.Empty;
        string strPrintCode = string.Empty;
        string strImg = string.Empty;
        string strPrintImg = string.Empty;
        string strAttachText = string.Empty;
        string strSiteCode = string.Empty;
        string strClientID = string.Empty;
        string strShowPage = string.Empty;
        string strIsPrintTime = string.Empty;
        string strIsPrintAddress = string.Empty;
        frmMessage frmMessage = new frmMessage();

        public frmGetPrintPhoto()
        {
            Ping ping = new Ping();
            PingOptions poptions = new PingOptions();
            poptions.DontFragment = true;
            string data = string.Empty;
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            int iPingCount = 0;
            bool bNetCon = false;
            while (!bNetCon)
            {
                int i = 0;
                if (InternetGetConnectedState(out i, 0))
                {                    
                    InitializeComponent();
                    bNetCon = true;
                }
                else
                {
                    Thread.Sleep(6000); 
                    iPingCount++;
                    if (iPingCount > 100) MessageBox.Show("微打印启动失败，请确认网络是否正常连接！");
                }
            }
        }

        private void frmGetPrintPhoto_Load(object sender, EventArgs e)
        {
            strSiteCode = ConfigurationManager.AppSettings["SiteCode"];
            strClientID = ConfigurationManager.AppSettings["ClientCode"];
            strShowPage = ConfigurationManager.AppSettings["ShowPage"];

            this.webBrowser1.Navigate("http://114.215.108.27/HPPrint/" + strShowPage);

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
            }
            this.TopMost = true;
            labMessage.Location = new Point((this.Width - labMessage.Width) / 2, (this.Height - labMessage.Height) / 2);
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetCouponData));
        }


        private void GetCouponData(object strService)
        {
            Object o = new Object();
            lock (o)
            {
                while (!bGDateIsExit)
                {
                    try
                    {
                        Thread.Sleep(2000);
                        DataSet dsPhoto = wsGetPrindCode.GetPrintPhoto(strSiteCode, strClientID);
                        if (null != dsPhoto && dsPhoto.Tables.Count > 0 && dsPhoto.Tables[0].Rows.Count > 0)
                        {
                            wsGetPrindCode.UpdatePrintState(strHappyPhotoID, "2");

                            strHappyPhotoID = dsPhoto.Tables[0].Rows[0]["ID"].ToString();
                            strPrintCode = dsPhoto.Tables[0].Rows[0]["PrintCode"].ToString();
                            //strImg = "http://localhost:1156/HP_Photo/" + dsPhoto.Tables[0].Rows[0]["Img"].ToString();
                            strImg = "http://114.215.108.27//HP_Photo/" + dsPhoto.Tables[0].Rows[0]["Img"].ToString();
                            strAttachText = dsPhoto.Tables[0].Rows[0]["AttachText"].ToString();

                            strPrintImg = "PrintPhoto." + strImg.Split('.').Last();
                            //取打印的图片
                            using (System.Net.WebClient wc = new System.Net.WebClient())
                            {
                                wc.Headers.Add("User-Agent", "Chrome");
                                wc.DownloadFile(strImg, Application.StartupPath + "/" + strPrintImg);
                            }

                            #region 打印过程
                            //printDocument.PrinterSettings.PrinterName = "TM58";//设置打印机

                            //printDocument.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
                            printDocument.PrinterSettings.PrinterName = "PhotoPrint";
                            //设置页边距
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Right = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;

                            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 290, 414);//页面大小
                            printDocument.DefaultPageSettings.Landscape = false;//横向打印

                            //打印开始前
                            printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
                            //打印输出（过程）
                            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                            //打印结束
                            printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

                            printDocument.Print();
                            //PrintDialog pd = new PrintDialog();
                            //pd.Document = printDocument;
                            //if (DialogResult.OK == pd.ShowDialog())
                            //{
                            //    //预览
                            //    PrintPreviewDialog ppd = new PrintPreviewDialog();
                            //    ppd.Document = printDocument;
                            //    if (DialogResult.OK == ppd.ShowDialog()) printDocument.Print();
                            //}
                            #endregion

                            this.BeginInvoke(new Action(() =>
                            {

                            }));
                        }
                        Application.DoEvents();
                        Thread.Sleep(2000);
                    }
                    catch //(Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        Thread.Sleep(20000);
                    }
                }
            }
        }

        #region 打印功能

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            //修改打印码与图片状态为正在打印
            wsGetPrindCode.UpdatePrintState(strHappyPhotoID, "2");
            bGDateIsExit = true;
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //照片打印
            Image layoutImage = Image.FromFile(Application.StartupPath + "\\" + strPrintImg);
            e.Graphics.DrawImage(layoutImage, 0, 0);
            
            //打印时间
            //strIsPrintTime = ConfigurationManager.AppSettings["PrintTime"];
            //if (!string.IsNullOrEmpty(strIsPrintTime))
            //{
            //    var taFont = new Font("宋体", 8, System.Drawing.FontStyle.Regular);
            //    var taColor = System.Drawing.Brushes.Black;
            //    string strTA = DateTime.Now.ToString("yyyy年MM月dd日HH:mm");
            //    e.Graphics.DrawString(strTA, taFont, taColor, 0, 300);
            //}

            ////打印地点
            //strIsPrintAddress = ConfigurationManager.AppSettings["PrintAddress"];
            //if (!string.IsNullOrEmpty(strIsPrintAddress))
            //{
            //    var taFont = new Font("宋体", 8, System.Drawing.FontStyle.Regular);
            //    var taColor = System.Drawing.Brushes.Black;
            //    e.Graphics.DrawString("»  " + strIsPrintAddress, taFont, taColor, 0, 320);
            //}

            //备注打印
            if (!string.IsNullOrEmpty(strAttachText))
            {
                PrivateFontCollection privateFonts = new PrivateFontCollection();
                var printFont = new Font("宋体", 10, System.Drawing.FontStyle.Regular);
                try
                {
                    privateFonts.AddFontFile(System.IO.Directory.GetCurrentDirectory() + @"\wsy.ttf");//加载路径的字体
                    printFont = new Font(privateFonts.Families[0], 12);
                }
                catch
                {
                    printFont = new Font("宋体", 10, System.Drawing.FontStyle.Regular);
                }
                var printColor = System.Drawing.Brushes.Black;
                e.Graphics.DrawString(CutStr(strAttachText, 6), printFont, printColor, 20, 310);
            }

            //打印公司图标
            Image footImage = Image.FromFile(Application.StartupPath + "\\log.jpg");
            e.Graphics.DrawImage(footImage, 160, 285);

            if (System.IO.File.Exists(Application.StartupPath + "\\log1.jpg"))
            {
                Image footImage1 = Image.FromFile(Application.StartupPath + "\\log1.jpg");
                //e.Graphics.DrawImage(footImage1, 106, 315);
                e.Graphics.DrawImage(footImage1, 5, 285);
            }
            //MessageBox.Show("OK");
            //e.HasMorePages = false;
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //修改打印码与图片状态为正在打印
            wsGetPrindCode.UpdatePrintState(strHappyPhotoID, "1");
            bGDateIsExit = false;
            tPrintState.Enabled = true;
        }
        #endregion

        #region 字符串换行
        /// <summary>
        /// 截取字符串，不限制字符串长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="len">每行的长度，多于这个长度自动换行</param>
        /// <returns></returns>
        public string CutStr(string str, int len)
        {
            string s = "";
            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len; if (i != 0 && i <= last)
                {

                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "\r\n";
                    }

                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }

            }
            return s;
        }
        #endregion

        private void tPrintState_Tick(object sender, EventArgs e)
        {
            //string strPrintState = GetPrinterStat("Microsoft XPS Document Writer").ToString();
            string strPrintState = GetPrinterStat("PhotoPrint").ToString();
            if (strPrintState == "空闲")
            {
                tPrintState.Enabled = false;
                this.labMessage.Visible = false;
            }
            else
            {
                this.labMessage.Visible = true;
            }
        }


        #region 获取打印机状态
        enum PrinterStatus
        {
            其他状态 = 1,
            未知,
            空闲,
            正在打印,
            预热,
            停止打印,
            打印中,
            离线
        }

        /// <summary>
        /// 获取打印机的当前状态
        /// </summary>
        /// <param name="PrinterDevice">打印机设备名称</param>
        /// <returns>打印机状态</returns>
        private PrinterStatus GetPrinterStat(string PrinterDevice)
        {
            PrinterStatus ret = 0;
            string path = @"win32_printer.DeviceId='" + PrinterDevice + "'";
            ManagementObject printer = new ManagementObject(path);
            printer.Get();
            string strState = printer.Properties["PrinterStatus"].Value.ToString();
            ret = (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);
            return ret;
        }

        #endregion

    }
}

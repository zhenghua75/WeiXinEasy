using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace HappyPhoto
{
    public partial class frmPrintCodeService : Form
    {
        delegate void WriteInvoke();
        bool bGDateIsExit = false;
        private PrintPreviewDialog PrintPreview = new PrintPreviewDialog();
        PrintDocument printDocument = new PrintDocument();
        srHappyPhoto.wsHappyPhotoSoapClient wsHP = new srHappyPhoto.wsHappyPhotoSoapClient();
        DataSet dsPrintCode = new DataSet();
        string strExtracode = string.Empty;
        public frmPrintCodeService()
        {
            InitializeComponent();
        }

        private void frmPrintCodeService_Load(object sender, EventArgs e)
        {
            this.dlIcon.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;//使Form不在任务栏上显示
            this.dlIcon.Icon = HappyPhoto.Properties.Resources.printer_3;
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
                        //dsPrintCode = wsHP.GetPrintCode("KM_HLF", "0");
                        dsPrintCode = wsHP.GetPrintCode("VYIGO", "0");
                        if (null != dsPrintCode && dsPrintCode.Tables.Count > 0 && dsPrintCode.Tables[0].Rows.Count > 0)
                        {
                            strExtracode = dsPrintCode.Tables[0].Rows[0]["Extracode"].ToString();
                            #region 打印过程
                            printDocument.PrinterSettings.PrinterName = "TM58";//设置打印机
                            //printDocument.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
                            //设置页边距
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Right = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;

                            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 180, 0); //页面大小
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
                        Thread.Sleep(5000);
                    }
                    catch (Exception ex)
                    {                       
                        Thread.Sleep(20000);
                    }
                    //this.Invoke(new WriteInvoke(DataGetState));
                }
            }
        }

        #region 打印功能

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            wsHP.UpdatePrintCodeState(strExtracode, "2");
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //string strTitle = "欢迎使用易美印";
            var printFont = new Font("宋体", 14, System.Drawing.FontStyle.Bold);
            var printColor = System.Drawing.Brushes.Black;

            var pointY = 10f;
            //画字符串
            //e.Graphics.DrawString(strTitle, printFont, printColor, 10f, pointY);
            //画图像
            //e.Graphics.DrawImage(Image, 10, 50);

            //绘画的设置保存与恢复
            var status = e.Graphics.Save();
            e.Graphics.ScaleTransform(1.0f, 1.0f);
            //string[] strCode = strCodes.Split(',');
            //for (int i = 0; i < strCode.Length; i++)
            //{
            //    e.Graphics.DrawString("   " + strCode[i].ToString(), printFont, printColor, 10f, pointY += 20f);
            //}
            foreach (DataRow row in dsPrintCode.Tables[0].Rows)
            {
                e.Graphics.DrawString("打印码： " + row["PrintCode"].ToString() + " ", printFont, printColor, 10f, pointY += 20f);
                e.Graphics.DrawString("\r\n", printFont, printColor, 10f, pointY += 20f);
            }

            //string strEnd = "打印结束！";
            //e.Graphics.Restore(status);
            //e.Graphics.DrawString(strEnd, printFont, printColor, 10f, pointY += 20f);

            //如果打印还有下一页，将HasMorePages值置为true;
            e.HasMorePages = false;     
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            wsHP.UpdatePrintCodeState(strExtracode, "3");
        }
        #endregion

        private void exitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dlIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                dlMenu.Show();
            }

            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void frmPrintCodeService_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生           
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.Hide();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HappyPhoto
{
    public partial class frmCreatPrintCode : Form
    {
        private PrintPreviewDialog PrintPreview = new PrintPreviewDialog();
        PrintDocument printDocument = new PrintDocument();
        string strCodes = string.Empty;

        public frmCreatPrintCode()
        {
            InitializeComponent();
        }

        private void frmCreatPrintCode_Load(object sender, EventArgs e)
        {

        }

        private void btnPrintCode_Click(object sender, EventArgs e)
        {
            srHappyPhoto.wsHappyPhotoSoapClient wsGetPrindCode = new srHappyPhoto.wsHappyPhotoSoapClient();
            string strPrintCode = wsGetPrindCode.CreatePrintCode(3,"KM_HLF","wsy001", "", "");
            if (strPrintCode != "0")
            {
                DataTable dtCode = JsonConvert.DeserializeObject<DataTable>(strPrintCode);
                for (int i = 0; i < dtCode.Rows.Count; i++)
                {
                    DataRow dr = dtCode.Rows[i];
                    strCodes = strCodes + dr[0] + ",";
                }
                strCodes = strCodes.ToString().Remove(strCodes.Length - 1);

                printDocument.PrinterSettings.PrinterName = "ZDesigner 888-TT";//设置打印机
                printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("SpecimenLabel", 88, 120);//页面大小
                printDocument.DefaultPageSettings.Landscape = true;//横向打印

                //打印开始前
                printDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
                //打印输出（过程）
                printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
                //打印结束
                printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);

                //跳出打印对话框，提供打印参数可视化设置，如选择哪个打印机打印此文档等
                PrintDialog pd = new PrintDialog();
                pd.Document = printDocument;
                if (DialogResult.OK == pd.ShowDialog()) //如果确认，将会覆盖所有的打印参数设置
                {
                    //预览
                    PrintPreviewDialog ppd = new PrintPreviewDialog();
                    ppd.Document = printDocument;
                    if (DialogResult.OK == ppd.ShowDialog())
                    printDocument.Print();          //打印
                }
            }
            else
            {
                MessageBox.Show("打印码生成的误！");
            }
        }

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            //也可以把一些打印的参数放在此处设置
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string strTitle = "欢迎使用快乐照打印";
            var printFont = new Font("宋体", 12, System.Drawing.FontStyle.Bold);
            var printColor = System.Drawing.Brushes.Black;

            var pointY = 10f;
            //画字符串
            e.Graphics.DrawString(strTitle, printFont, printColor, 10f, pointY);
            //画图像
            //e.Graphics.DrawImage(Image, 10, 50);

            //绘画的设置保存与恢复
            var status = e.Graphics.Save();
            e.Graphics.ScaleTransform(1.0f, 1.0f);
            string[] strCode = strCodes.Split(',');
            for (int i = 0; i < strCode.Length; i++)
            {
                e.Graphics.DrawString("   " + strCode[i].ToString(), printFont, printColor, 10f, pointY += 20f);
            }

            string strEnd = "打印结束语";
            e.Graphics.Restore(status);
            e.Graphics.DrawString(strEnd, printFont, printColor, 10f, pointY += 20f);

            //如果打印还有下一页，将HasMorePages值置为true;
            e.HasMorePages = false;          
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //打印结束后相关操作
        }
    }
}

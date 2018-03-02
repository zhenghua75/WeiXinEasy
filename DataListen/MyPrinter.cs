using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataListen
{
    /// <summary>
    /// 打印类
    /// </summary>
    public class MyPrinter
    {
        /// <summary>
        /// 向打印机发送的对象
        /// </summary>
        private PrintDocument ThePrintDocument;

        /// <summary>
        /// 页面标题
        /// </summary>
        private Font titleFont;

        /// <summary>
        /// 字体
        /// </summary>
        private Font theFont;

        /// <summary>
        /// 字体颜色
        /// </summary>
        private Color FontColor;

        /// <summary>
        /// 当前Y坐标
        /// </summary>
        private float CurrentY;

        /// <summary>
        /// 页数
        /// </summary>
        static int PageNumber;

        /// <summary>
        /// 页宽
        /// </summary>
        private int PageWidth;

        /// <summary>
        /// 页高
        /// </summary>
        private int PageHeight;

        /// <summary>
        /// 左边距
        /// </summary>
        private int LeftMargin;

        /// <summary>
        /// 顶边距
        /// </summary>
        private int TopMargin;

        /// <summary>
        /// 右边距
        /// </summary>
        private int RightMargin;

        /// <summary>
        /// 底边距
        /// </summary>
        private int BottomMargin;

        /// <summary>
        /// 文档内容集合
        /// </summary>
        private ArrayList textList = new ArrayList();
        private int currentIndex = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_thePrintDocument"></param>向打印机发送的打印对象
        /// <param name="_theFont"></param>打印字体
        /// <param name="_titleFont"></param>标题字体
        /// <param name="_FontColor"></param>字体颜色
        /// <param name="_textList"></param>打印内容集合
        public MyPrinter(PrintDocument _thePrintDocument, Font _theFont, Font _titleFont, Color _FontColor, ArrayList _textList)
        {
            ThePrintDocument = _thePrintDocument;
            theFont = _theFont;
            titleFont = _titleFont;
            FontColor = _FontColor;
            PageNumber = 0;
            textList = _textList;
            if (!ThePrintDocument.DefaultPageSettings.Landscape)
            {
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                PageHeight = ThePrintDocument.DefaultPageSettings.PaperSize.Width;
                PageWidth = ThePrintDocument.DefaultPageSettings.PaperSize.Height;
            }
            LeftMargin = ThePrintDocument.DefaultPageSettings.Margins.Left;
            TopMargin = ThePrintDocument.DefaultPageSettings.Margins.Top;
            RightMargin = ThePrintDocument.DefaultPageSettings.Margins.Right;
            BottomMargin = ThePrintDocument.DefaultPageSettings.Margins.Bottom;
        }

        /// <summary>
        /// 绘制打印文档
        /// </summary>
        /// <param name="g"></param>Graphics对象
        /// <returns></returns>返回是否绘制成功
        public bool DrawDocument(Graphics g)
        {
            try
            {
                DrawHeader(g);
                bool bContinue = DrawItems(g);
                g.Dispose();
                return bContinue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("失败" + ex.Message.ToString(), " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                g.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 绘制打印文档
        /// </summary>
        /// <param name="g">Graphics对象</param>
        /// <param name="activityName">活动名称</param>
        /// <returns>返回是否绘制成功</returns>
        public bool DrawDocument(Graphics g, string activityName)
        {
            try
            {
                DrawHeader(g, activityName);
                bool bContinue = DrawItems(g, activityName);
                g.Dispose();
                return bContinue;
            }
            catch (Exception ex)
            {
                MessageBox.Show("失败" + ex.Message.ToString(), " - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                g.Dispose();
                return false;
            }
        }

        /// <summary>
        /// 绘制打印文档头部
        /// </summary>
        /// <param name="g"></param>
        public void DrawHeader(Graphics g)
        {
            CurrentY = (float)TopMargin;
            PageNumber++;
            string PageString = "第" + PageNumber + "页";
            StringFormat PageStringFormat = new StringFormat();
            PageStringFormat.Trimming = StringTrimming.Word;
            PageStringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            PageStringFormat.Alignment = StringAlignment.Far;
            RectangleF PageStringRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin - 5, g.MeasureString(PageString, theFont).Height);
            g.DrawString(PageString, theFont, new SolidBrush(Color.Black), PageStringRectangle, PageStringFormat);
            CurrentY += g.MeasureString(PageString, theFont).Height;
            StringFormat TitleFormat = new StringFormat();
            TitleFormat.Trimming = StringTrimming.Word;
            TitleFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            TitleFormat.Alignment = StringAlignment.Center;
            RectangleF TitleRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString("酒店收费单", titleFont).Height);
            g.DrawString("酒店预订单", titleFont, new SolidBrush(FontColor), TitleRectangle, TitleFormat);
            CurrentY += g.MeasureString("酒店预订单", titleFont).Height;
        }

        /// <summary>
        /// 绘制打印文档的项
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>返回是否绘制成功
        public bool DrawItems(Graphics g)
        {
            StringFormat TextFormat = new StringFormat();
            TextFormat.Trimming = StringTrimming.Word;
            TextFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            TextFormat.Alignment = StringAlignment.Near;
            for (int i = currentIndex; i < textList.Count; i++)
            {
                string var = textList[i].ToString();
                RectangleF TextRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString("预订单", titleFont).Height);
                g.DrawString(var, theFont, new SolidBrush(FontColor), TextRectangle, TextFormat);
                CurrentY = CurrentY + g.MeasureString(var, theFont).Height;
                if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
                {
                    currentIndex = i + 1;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 绘制打印文档头部
        /// </summary>
        /// <param name="g"></param>
        public void DrawHeader(Graphics g, string activityName)
        {
            CurrentY = (float)TopMargin;
            PageNumber++;
            string PageString = "第" + PageNumber + "页";
            StringFormat PageStringFormat = new StringFormat();
            PageStringFormat.Trimming = StringTrimming.Word;
            PageStringFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            PageStringFormat.Alignment = StringAlignment.Far;
            RectangleF PageStringRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin - 5, g.MeasureString(PageString, theFont).Height);
            g.DrawString(PageString, theFont, new SolidBrush(Color.Black), PageStringRectangle, PageStringFormat);
            CurrentY += g.MeasureString(PageString, theFont).Height;
            StringFormat TitleFormat = new StringFormat();
            TitleFormat.Trimming = StringTrimming.Word;
            TitleFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            TitleFormat.Alignment = StringAlignment.Center;
            RectangleF TitleRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(activityName, titleFont).Height);
            g.DrawString(activityName, titleFont, new SolidBrush(FontColor), TitleRectangle, TitleFormat);
            CurrentY += g.MeasureString(activityName, titleFont).Height;
        }

        /// <summary>
        /// 绘制打印文档的项
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>返回是否绘制成功
        public bool DrawItems(Graphics g, string activityName)
        {
            StringFormat TextFormat = new StringFormat();
            TextFormat.Trimming = StringTrimming.Word;
            TextFormat.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            TextFormat.Alignment = StringAlignment.Near;
            for (int i = currentIndex; i < textList.Count; i++)
            {
                string var = textList[i].ToString();
                RectangleF TextRectangle = new RectangleF((float)LeftMargin, CurrentY, (float)PageWidth - (float)RightMargin - (float)LeftMargin, g.MeasureString(activityName, titleFont).Height);
                g.DrawString(var, theFont, new SolidBrush(FontColor), TextRectangle, TextFormat);
                CurrentY = CurrentY + g.MeasureString(var, theFont).Height;
                if ((int)CurrentY > (PageHeight - TopMargin - BottomMargin))
                {
                    currentIndex = i + 1;
                    return true;
                }
            }
            return false;
        }
    }
}

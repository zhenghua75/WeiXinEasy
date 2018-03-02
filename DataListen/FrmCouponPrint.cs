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

namespace DataListen
{
    public partial class FrmCouponPrint : Form
    {
        delegate void WriteInvoke();
        bool bGDateIsExit = false;
        srCouponPrint.wsCouponPrintSoapClient wsGetCP = new srCouponPrint.wsCouponPrintSoapClient();
        private PrintPreviewDialog PrintPreview = new PrintPreviewDialog();
        PrintDocument printDocument = new PrintDocument();
        string strPrintCode = string.Empty;
        string strCouponID = string.Empty;
        string strActiveName = string.Empty;
        string strCheckDate = string.Empty;
        string strBillRMB = string.Empty;
        string strCheckRMB = string.Empty;
        string strInfo = string.Empty;
        DataListenDAL dal = new DataListenDAL();
        DataSet dsCoupon = new DataSet();

        int pageSize = 0;     //每页显示行数
        int nMax = 0;         //总记录数
        int pageCount = 0;    //页数＝总记录数/每页显示行数
        int pageCurrent = 0;   //当前页号
        int nCurrent = 0;      //当前记录行
        DataSet ds = new DataSet();
        DataTable dtInfo = new DataTable();


        public FrmCouponPrint()
        {
            InitializeComponent();
        }

        private void FrmCouponPrint_Load(object sender, EventArgs e)
        {
            this.dlIcon.Visible = true;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.ShowInTaskbar = false;//使Form不在任务栏上显示

            this.btnControl.Text = "停止监控";
            bGDateIsExit = false;
            this.dlIcon.Icon = DataListen.Properties.Resources.doing;
            ThreadPool.QueueUserWorkItem(new WaitCallback(GetCouponData));

            BindData();
            InitDataSet();
        }

        private void FrmCouponPrint_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized && this.Visible == true)
            {
                this.dlIcon.Visible = true;//在通知区显示Form的Icon
                this.WindowState = FormWindowState.Minimized;
                this.Visible = false;
                this.ShowInTaskbar = false;//使Form不在任务栏上显示
            }
        }

        private void FrmCouponPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)//当用户点击窗体右上角X按钮或(Alt + F4)时 发生           
            {
                e.Cancel = true;
                this.ShowInTaskbar = false;
                this.dlIcon.Icon = this.Icon;
                this.Hide();
            }
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

        private void exitItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
                        dsCoupon = wsGetCP.GetCouponData("KM_HLF", "1");
                        if (null != dsCoupon && dsCoupon.Tables.Count > 0 && dsCoupon.Tables[0].Rows.Count > 0)
                        {
                            this.dlIcon.Icon = DataListen.Properties.Resources.get;
                            strPrintCode = dsCoupon.Tables[0].Rows[0]["CouponCode"].ToString();
                            strCouponID = dsCoupon.Tables[0].Rows[0]["ID"].ToString();
                            strActiveName = dsCoupon.Tables[0].Rows[0]["ActTitle"].ToString();
                            strBillRMB = dsCoupon.Tables[0].Rows[0]["Discount"].ToString();
                            strCheckRMB = dsCoupon.Tables[0].Rows[0]["Discount"].ToString();
                            #region 打印过程
                            printDocument.PrinterSettings.PrinterName = "TM58";//设置打印机

                            //设置页边距
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Left = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Top = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Right = 0;
                            printDocument.PrinterSettings.DefaultPageSettings.Margins.Bottom = 0;

                            printDocument.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("Custom", 180, 320);//页面大小
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
                                ds = dal.GetCouponList("");
                                dtInfo = ds.Tables[0];
                                InitDataSet();
                            }));
                        }
                        Application.DoEvents();
                        Thread.Sleep(5000);
                    }
                    catch //(Exception ex)
                    {
                        //this.labInfo.Text = ex.Data.ToString() + ex.Message;
                        Thread.Sleep(20000);
                    }
                    //this.Invoke(new WriteInvoke(DataGetState));
                }
            }
        }

        private void btnControl_Click(object sender, EventArgs e)
        {
            if (this.btnControl.Text == "启动监控")
            {
                this.btnControl.Text = "停止监控";
                bGDateIsExit = false;
                this.dlIcon.Icon = DataListen.Properties.Resources.doing;
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetCouponData));
            }
            else
            {
                bGDateIsExit = true;
                this.dlIcon.Icon = DataListen.Properties.Resources.stop;
                this.btnControl.Text = "启动监控";
            }
        }

        #region 打印功能

        void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            this.dlIcon.Icon = DataListen.Properties.Resources.print;
            wsGetCP.UpdateCouponState(strCouponID, "2");
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            strCheckDate = DateTime.Now.ToString();
            StringBuilder sbHeader = new StringBuilder();
            StringBuilder sbContent = new StringBuilder();
            StringBuilder sbFooter = new StringBuilder();

            sbHeader.AppendLine("海立方ＳＰＡ渡假酒店");
            sbHeader.AppendLine("    欢迎您的光临    ");
            sbContent.AppendLine("---------------------------");
            sbContent.AppendLine("活动名称：" + strActiveName);
            sbContent.AppendLine("");
            sbContent.AppendLine("流水号：" + strPrintCode);
            sbContent.AppendLine("");
            sbContent.AppendLine("日期：" + strCheckDate);
            sbContent.AppendLine("");
            sbContent.AppendLine("票券金额：" + strBillRMB);
            sbContent.AppendLine("");
            sbContent.AppendLine("抵免金额：" + strCheckRMB);
            sbContent.AppendLine("");
            sbContent.AppendLine("数量：1张");
            sbContent.AppendLine("");
            sbContent.AppendLine("---------------------------");
            sbContent.AppendLine("");
            sbFooter.AppendLine("地址：昆明市西山区人民西路");
            sbFooter.AppendLine("      390号西苑立交旁");
            sbFooter.AppendLine("电话：0871-65332666");
            sbFooter.AppendLine("      0871-68330666");

            var printFont = new Font("黑体", 11, System.Drawing.FontStyle.Regular);
            var printColor = System.Drawing.Brushes.Black;

            //打印标题
            e.Graphics.DrawString(sbHeader.ToString(), printFont, printColor, 9f, 10f);
            //打印内容
            printFont = new Font("宋体", 9, System.Drawing.FontStyle.Regular);
            e.Graphics.DrawString(sbContent.ToString(), printFont, printColor, 4f, 50f);

            //打印脚标
            e.Graphics.DrawString(sbFooter.ToString(), printFont, printColor, 4f, 210f);

            e.HasMorePages = false;
        }

        void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            this.dlIcon.Icon = DataListen.Properties.Resources.doing;

            wsGetCP.UpdateCouponState(strCouponID, "3");
            try
            {
                bool bOK = DataListenDAL.AddCouponInfo(new DataListenMODEL.CouponInfo
                {
                    ID = dsCoupon.Tables[0].Rows[0]["ID"].ToString(),
                    OpenID = dsCoupon.Tables[0].Rows[0]["CouponCode"].ToString(),
                    AddTime = dsCoupon.Tables[0].Rows[0]["AddTime"].ToString(),
                    CheckTime = dsCoupon.Tables[0].Rows[0]["CheckTime"].ToString(),
                    ActTitle = dsCoupon.Tables[0].Rows[0]["ActTitle"].ToString(),
                    CouponNo = dsCoupon.Tables[0].Rows[0]["CouponCode"].ToString(),
                    ActContent = dsCoupon.Tables[0].Rows[0]["ActContent"].ToString()
                });
                //if (bOK)
                //{
                //    ds = dal.GetCouponList("");
                //    dtInfo = ds.Tables[0];
                //    InitDataSet();
                //}
            }
            catch
            {

            }
        }
        #endregion

        #region 打印状态显示
        /// <summary>
        /// 监控返回数据状态
        /// </summary>        
        private void DataGetState()
        {
            //刷新列表数据
        }

        #endregion

        public void BindData()
        {
            ds = dal.GetCouponList("");
            dtInfo = ds.Tables[0];
        }

        #region 查询信息与列表

        private void InitDataSet()
        {
            pageSize = 20;      //设置页面行数
            nMax = dtInfo.Rows.Count;

            pageCount = (nMax / pageSize);    //计算出总页数

            if ((nMax % pageSize) > 0) pageCount++;

            pageCurrent = 1;    //当前页数从1开始
            nCurrent = 0;       //当前记录数从0开始

            LoadData();
        }

        private void LoadData()
        {
            int nStartPos = 0;   //当前页面开始记录行
            int nEndPos = 0;     //当前页面结束记录行

            DataTable dtTemp = dtInfo.Clone();   //克隆DataTable结构框架

            if (pageCurrent == pageCount)
                nEndPos = nMax;
            else
                nEndPos = pageSize * pageCurrent;

            nStartPos = nCurrent;

            lblPageCount.Text = pageCount.ToString();
            txtCurrentPage.Text = Convert.ToString(pageCurrent);

            //从元数据源复制记录行
            if (dtInfo.Rows.Count > 0)
            {
                for (int i = nStartPos; i < nEndPos; i++)
                {
                    dtTemp.ImportRow(dtInfo.Rows[i]);
                    nCurrent++;
                }
            }
            bdsInfo.DataSource = dtTemp;
            bdnInfo.BindingSource = bdsInfo;
            dgvInfo.DataSource = bdsInfo;
        }
        #endregion

        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text == "上一页")
            {
                pageCurrent--;
                if (pageCurrent <= 0)
                {
                    MessageBox.Show("已经是第一页，请点击“下一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }

                LoadData();
            }

            if (e.ClickedItem.Text == "下一页")
            {
                pageCurrent++;
                if (pageCurrent > pageCount)
                {
                    MessageBox.Show("已经是最后一页，请点击“上一页”查看！");
                    return;
                }
                else
                {
                    nCurrent = pageSize * (pageCurrent - 1);
                }
                LoadData();
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            string strQuerySql = " 1 = 1 ";

            if (dtpBegin.Checked)
            {
                strQuerySql += " AND CheckTime >= '" + dtpBegin.Text.ToString() + " 00:00:00' ";
            }
            if (dtpEnd.Checked)
            {
                strQuerySql += " AND CheckTime <= '" + dtpEnd.Text.ToString() + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(this.txtQueryValue.Text.ToString()))
            {
                strQuerySql += " AND CouponNo = '" + this.txtQueryValue.Text.Trim() + "' ";
            }
            ds = dal.GetCouponList(strQuerySql);
            dtInfo = ds.Tables[0];
            InitDataSet();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string strQuerySql = " 1 = 1 ";

            if (dtpBegin.Checked)
            {
                strQuerySql += " AND CheckTime >= '" + dtpBegin.Text.ToString() + " 00:00:00' ";
            }
            if (dtpEnd.Checked)
            {
                strQuerySql += " AND CheckTime <= '" + dtpEnd.Text.ToString() + " 23:59:59' ";
            }
            if (!string.IsNullOrEmpty(this.txtQueryValue.Text.ToString()))
            {
                strQuerySql += " AND CouponNo = '" + this.txtQueryValue.Text.Trim() + "' ";
            }
            ds = dal.GetCouponList(strQuerySql);

            DataTable dt = null;
            if (ds.Tables[0] != null)
            {
                dt = ds.Tables[0];
            }
            else
            {
                MessageBox.Show("没有数据需要导出的数据", "温馨提示信息", MessageBoxButtons.OK);
                return;
            }

            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            folderBrowserDialog1.Description = "请选择保存输出图件的文件夹";
            folderBrowserDialog1.ShowNewFolderButton = true;
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop; //Environment.SpecialFolder.Personal;
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                string newFileName = "HLF_Coupon_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                //string newFileName = "aaa.xls";
                string strFileName = folderName + "\\" + newFileName;
                AsposeExcel tt = new AsposeExcel(strFileName, "");//不用模板, saveFileDialog1是什么？上面已经说过  
                bool OK_NO = tt.DatatableToExcel(dt);
                if (OK_NO)
                {
                    MessageBox.Show("数据导出成功", "温馨提示信息", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("数据导出失败，请与开发人员联系！", "温馨提示信息", MessageBoxButtons.OK);
                } 
            }            
        }
    }
}
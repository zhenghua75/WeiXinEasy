namespace DataListen
{
    partial class FrmCouponPrint
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCouponPrint));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnExport = new System.Windows.Forms.Button();
            this.dtpBegin = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtQueryValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.bdsInfo = new System.Windows.Forms.BindingSource(this.components);
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPageLast = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPageCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtCurrentPage = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnControl = new System.Windows.Forms.Button();
            this.dlIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.dlMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dgvInfo = new System.Windows.Forms.DataGridView();
            this.colID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGUID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colvcArtNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colvcChinaName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colvcOwnerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colvcIsdel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCouponContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.bdnInfo = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.lblPageFirst = new System.Windows.Forms.ToolStripLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).BeginInit();
            this.dlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).BeginInit();
            this.bdnInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(803, 124);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 43;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // dtpBegin
            // 
            this.dtpBegin.CustomFormat = "yyyy-MM-dd";
            this.dtpBegin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBegin.Location = new System.Drawing.Point(234, 125);
            this.dtpBegin.Name = "dtpBegin";
            this.dtpBegin.ShowCheckBox = true;
            this.dtpBegin.Size = new System.Drawing.Size(115, 21);
            this.dtpBegin.TabIndex = 41;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(359, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 40;
            this.label3.Text = "结束时间：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(173, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 39;
            this.label2.Text = "起始时间：";
            // 
            // txtQueryValue
            // 
            this.txtQueryValue.Location = new System.Drawing.Point(612, 125);
            this.txtQueryValue.Name = "txtQueryValue";
            this.txtQueryValue.Size = new System.Drawing.Size(100, 21);
            this.txtQueryValue.TabIndex = 38;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(540, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 37;
            this.label1.Text = "抵用券代码：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(720, 124);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 36;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "最后一条记录";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPageLast
            // 
            this.lblPageLast.Name = "lblPageLast";
            this.lblPageLast.Size = new System.Drawing.Size(44, 22);
            this.lblPageLast.Text = "下一页";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPageCount
            // 
            this.lblPageCount.Name = "lblPageCount";
            this.lblPageCount.Size = new System.Drawing.Size(22, 22);
            this.lblPageCount.Text = "20";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(13, 22);
            this.toolStripLabel2.Text = "/";
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.Size = new System.Drawing.Size(30, 25);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // btnControl
            // 
            this.btnControl.AutoSize = true;
            this.btnControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnControl.Font = new System.Drawing.Font("宋体", 48F);
            this.btnControl.Location = new System.Drawing.Point(0, 0);
            this.btnControl.Name = "btnControl";
            this.btnControl.Size = new System.Drawing.Size(968, 120);
            this.btnControl.TabIndex = 33;
            this.btnControl.Text = "启动监控";
            this.btnControl.UseVisualStyleBackColor = true;
            this.btnControl.Click += new System.EventHandler(this.btnControl_Click);
            // 
            // dlIcon
            // 
            this.dlIcon.ContextMenuStrip = this.dlMenu;
            this.dlIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("dlIcon.Icon")));
            this.dlIcon.Text = "微信打印信息监控";
            this.dlIcon.Visible = true;
            this.dlIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dlIcon_MouseClick);
            // 
            // dlMenu
            // 
            this.dlMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitItem});
            this.dlMenu.Name = "dlMenu";
            this.dlMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // exitItem
            // 
            this.exitItem.Name = "exitItem";
            this.exitItem.Size = new System.Drawing.Size(100, 22);
            this.exitItem.Text = "退出";
            this.exitItem.Click += new System.EventHandler(this.exitItem_Click);
            // 
            // dgvInfo
            // 
            this.dgvInfo.AllowUserToAddRows = false;
            this.dgvInfo.AllowUserToDeleteRows = false;
            this.dgvInfo.AllowUserToResizeColumns = false;
            this.dgvInfo.AllowUserToResizeRows = false;
            this.dgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colID,
            this.colGUID,
            this.colvcArtNo,
            this.colvcChinaName,
            this.colvcOwnerName,
            this.colvcIsdel,
            this.colCouponContent});
            this.dgvInfo.Location = new System.Drawing.Point(0, 149);
            this.dgvInfo.Name = "dgvInfo";
            this.dgvInfo.ReadOnly = true;
            this.dgvInfo.RowHeadersVisible = false;
            this.dgvInfo.RowHeadersWidth = 30;
            this.dgvInfo.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvInfo.RowTemplate.Height = 23;
            this.dgvInfo.Size = new System.Drawing.Size(968, 347);
            this.dgvInfo.TabIndex = 34;
            // 
            // colID
            // 
            this.colID.DataPropertyName = "ID";
            this.colID.HeaderText = "序号";
            this.colID.Name = "colID";
            this.colID.ReadOnly = true;
            this.colID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.colID.Visible = false;
            this.colID.Width = 40;
            // 
            // colGUID
            // 
            this.colGUID.DataPropertyName = "OpenID";
            this.colGUID.HeaderText = "openID";
            this.colGUID.Name = "colGUID";
            this.colGUID.ReadOnly = true;
            this.colGUID.Visible = false;
            // 
            // colvcArtNo
            // 
            this.colvcArtNo.DataPropertyName = "AddTime";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colvcArtNo.DefaultCellStyle = dataGridViewCellStyle1;
            this.colvcArtNo.HeaderText = "关注时间";
            this.colvcArtNo.Name = "colvcArtNo";
            this.colvcArtNo.ReadOnly = true;
            this.colvcArtNo.Width = 120;
            // 
            // colvcChinaName
            // 
            this.colvcChinaName.DataPropertyName = "CheckTime";
            this.colvcChinaName.HeaderText = "打印时间";
            this.colvcChinaName.Name = "colvcChinaName";
            this.colvcChinaName.ReadOnly = true;
            this.colvcChinaName.Width = 120;
            // 
            // colvcOwnerName
            // 
            this.colvcOwnerName.DataPropertyName = "CouponNo";
            this.colvcOwnerName.HeaderText = "抵用券代码";
            this.colvcOwnerName.Name = "colvcOwnerName";
            this.colvcOwnerName.ReadOnly = true;
            // 
            // colvcIsdel
            // 
            this.colvcIsdel.DataPropertyName = "CouponName";
            this.colvcIsdel.HeaderText = "抵用券名称";
            this.colvcIsdel.Name = "colvcIsdel";
            this.colvcIsdel.ReadOnly = true;
            this.colvcIsdel.Width = 180;
            // 
            // colCouponContent
            // 
            this.colCouponContent.DataPropertyName = "CouponContent";
            this.colCouponContent.HeaderText = "抵用券内容";
            this.colCouponContent.Name = "colCouponContent";
            this.colCouponContent.ReadOnly = true;
            this.colCouponContent.Width = 270;
            // 
            // dtpEnd
            // 
            this.dtpEnd.CustomFormat = "yyyy-MM-dd";
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEnd.Location = new System.Drawing.Point(419, 125);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowCheckBox = true;
            this.dtpEnd.Size = new System.Drawing.Size(115, 21);
            this.dtpEnd.TabIndex = 42;
            // 
            // bdnInfo
            // 
            this.bdnInfo.AddNewItem = null;
            this.bdnInfo.CountItem = null;
            this.bdnInfo.DeleteItem = null;
            this.bdnInfo.Dock = System.Windows.Forms.DockStyle.None;
            this.bdnInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorSeparator2,
            this.lblPageFirst,
            this.toolStripSeparator9,
            this.txtCurrentPage,
            this.toolStripLabel2,
            this.lblPageCount,
            this.toolStripSeparator10,
            this.lblPageLast,
            this.toolStripSeparator2,
            this.bindingNavigatorMoveLastItem,
            this.toolStripSeparator11});
            this.bdnInfo.Location = new System.Drawing.Point(-80, 121);
            this.bdnInfo.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bdnInfo.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bdnInfo.MoveNextItem = null;
            this.bdnInfo.MovePreviousItem = null;
            this.bdnInfo.Name = "bdnInfo";
            this.bdnInfo.PositionItem = null;
            this.bdnInfo.Size = new System.Drawing.Size(243, 25);
            this.bdnInfo.TabIndex = 35;
            this.bdnInfo.Text = "bindingNavigator1";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "第一条记录";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // lblPageFirst
            // 
            this.lblPageFirst.Name = "lblPageFirst";
            this.lblPageFirst.Size = new System.Drawing.Size(44, 22);
            this.lblPageFirst.Text = "上一页";
            // 
            // FrmCouponPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(968, 496);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.dtpBegin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtQueryValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.btnControl);
            this.Controls.Add(this.dgvInfo);
            this.Controls.Add(this.dtpEnd);
            this.Controls.Add(this.bdnInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmCouponPrint";
            this.Text = "优惠券监控打印";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmCouponPrint_FormClosing);
            this.Load += new System.EventHandler(this.FrmCouponPrint_Load);
            this.SizeChanged += new System.EventHandler(this.FrmCouponPrint_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.bdsInfo)).EndInit();
            this.dlMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).EndInit();
            this.bdnInfo.ResumeLayout(false);
            this.bdnInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.DateTimePicker dtpBegin;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtQueryValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.BindingSource bdsInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel lblPageLast;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripLabel lblPageCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtCurrentPage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.Button btnControl;
        private System.Windows.Forms.NotifyIcon dlIcon;
        private System.Windows.Forms.ContextMenuStrip dlMenu;
        private System.Windows.Forms.ToolStripMenuItem exitItem;
        private System.Windows.Forms.DataGridView dgvInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGUID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colvcArtNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colvcChinaName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colvcOwnerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colvcIsdel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCouponContent;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.BindingNavigator bdnInfo;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripLabel lblPageFirst;

    }
}
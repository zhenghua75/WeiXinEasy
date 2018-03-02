namespace HappyPhoto
{
    partial class frmPrintCodeService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPrintCodeService));
            this.label1 = new System.Windows.Forms.Label();
            this.dlMenu = new System.Windows.Forms.ContextMenuStrip();
            this.exitItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dlIcon = new System.Windows.Forms.NotifyIcon();
            this.dlMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 24F);
            this.label1.Location = new System.Drawing.Point(12, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(399, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "易美印打印码监控打印服务";
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
            // dlIcon
            // 
            this.dlIcon.ContextMenuStrip = this.dlMenu;
            this.dlIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("dlIcon.Icon")));
            this.dlIcon.Text = "打印码监控服务";
            this.dlIcon.Visible = true;
            this.dlIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dlIcon_MouseClick);
            // 
            // frmPrintCodeService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 92);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPrintCodeService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印码打印监控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrintCodeService_FormClosing);
            this.Load += new System.EventHandler(this.frmPrintCodeService_Load);
            this.dlMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip dlMenu;
        private System.Windows.Forms.ToolStripMenuItem exitItem;
        private System.Windows.Forms.NotifyIcon dlIcon;
    }
}
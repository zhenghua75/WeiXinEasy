namespace HappyPhoto
{
    partial class frmVYIGOPrint
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
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labMessage = new System.Windows.Forms.Label();
            this.tPrintState = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScrollBarsEnabled = false;
            this.webBrowser1.Size = new System.Drawing.Size(1228, 492);
            this.webBrowser1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labMessage);
            this.panel1.Controls.Add(this.webBrowser1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1228, 492);
            this.panel1.TabIndex = 1;
            // 
            // labMessage
            // 
            this.labMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labMessage.AutoSize = true;
            this.labMessage.Font = new System.Drawing.Font("宋体", 60F, System.Drawing.FontStyle.Bold);
            this.labMessage.ForeColor = System.Drawing.Color.Blue;
            this.labMessage.Location = new System.Drawing.Point(259, 144);
            this.labMessage.Name = "labMessage";
            this.labMessage.Size = new System.Drawing.Size(601, 80);
            this.labMessage.TabIndex = 1;
            this.labMessage.Text = "照片打印中……";
            this.labMessage.Visible = false;
            // 
            // tPrintState
            // 
            this.tPrintState.Interval = 2000;
            this.tPrintState.Tick += new System.EventHandler(this.tPrintState_Tick);
            // 
            // frmVYIGOPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1228, 492);
            this.Controls.Add(this.panel1);
            this.Name = "frmVYIGOPrint";
            this.Text = "微易购照片打印";
            this.Load += new System.EventHandler(this.frmVYIGOPrint_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labMessage;
        private System.Windows.Forms.Timer tPrintState;
    }
}
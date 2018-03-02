namespace HappyPhoto
{
    partial class frmCreatPrintCode
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
            this.btnPrintCode = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnPrintCode
            // 
            this.btnPrintCode.Location = new System.Drawing.Point(131, 203);
            this.btnPrintCode.Name = "btnPrintCode";
            this.btnPrintCode.Size = new System.Drawing.Size(75, 23);
            this.btnPrintCode.TabIndex = 0;
            this.btnPrintCode.Text = "打印码生成";
            this.btnPrintCode.UseVisualStyleBackColor = true;
            this.btnPrintCode.Click += new System.EventHandler(this.btnPrintCode_Click);
            // 
            // frmCreatPrintCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 259);
            this.Controls.Add(this.btnPrintCode);
            this.Name = "frmCreatPrintCode";
            this.Text = "照片打印机管理";
            this.Load += new System.EventHandler(this.frmCreatPrintCode_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnPrintCode;
    }
}
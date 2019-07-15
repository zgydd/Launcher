namespace PressureMonitorLauncherV2
{
    partial class bootstrap
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bootstrap));
            this.rtxtDetail = new System.Windows.Forms.RichTextBox();
            this.lblView = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // rtxtDetail
            // 
            this.rtxtDetail.Enabled = false;
            this.rtxtDetail.Location = new System.Drawing.Point(12, 12);
            this.rtxtDetail.Name = "rtxtDetail";
            this.rtxtDetail.ReadOnly = true;
            this.rtxtDetail.Size = new System.Drawing.Size(735, 27);
            this.rtxtDetail.TabIndex = 0;
            this.rtxtDetail.Text = "";
            this.rtxtDetail.Visible = false;
            // 
            // lblView
            // 
            this.lblView.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblView.BackColor = System.Drawing.Color.Transparent;
            this.lblView.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblView.ForeColor = System.Drawing.SystemColors.Control;
            this.lblView.Location = new System.Drawing.Point(8, 297);
            this.lblView.Name = "lblView";
            this.lblView.Size = new System.Drawing.Size(785, 82);
            this.lblView.TabIndex = 1;
            this.lblView.Text = "正在加载...";
            this.lblView.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(753, 12);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(24, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "X";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Visible = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // bootstrap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 400);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblView);
            this.Controls.Add(this.rtxtDetail);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "bootstrap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.Shown += new System.EventHandler(this.bootstrap_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtDetail;
        private System.Windows.Forms.Label lblView;
        private System.Windows.Forms.Button btnCancel;
    }
}


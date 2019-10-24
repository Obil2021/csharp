namespace MSDBTOEXCEL
{
    partial class MainWindow
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
            this.DB_LABLE = new MaterialSkin.Controls.MaterialLabel();
            this.EXCEL_TEXT = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.EXCEL_LABLE = new MaterialSkin.Controls.MaterialLabel();
            this.EXPORT_BUTTON = new MaterialSkin.Controls.MaterialRaisedButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ERROR_LABLE = new System.Windows.Forms.Label();
            this.DB_TEXT = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DB_LABLE
            // 
            this.DB_LABLE.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DB_LABLE.AutoSize = true;
            this.DB_LABLE.Depth = 0;
            this.DB_LABLE.Font = new System.Drawing.Font("Roboto", 11F);
            this.DB_LABLE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.DB_LABLE.Location = new System.Drawing.Point(64, 95);
            this.DB_LABLE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DB_LABLE.MouseState = MaterialSkin.MouseState.HOVER;
            this.DB_LABLE.Name = "DB_LABLE";
            this.DB_LABLE.Size = new System.Drawing.Size(183, 19);
            this.DB_LABLE.TabIndex = 0;
            this.DB_LABLE.Text = "DB CONNECTIONSTRING:";
            // 
            // EXCEL_TEXT
            // 
            this.EXCEL_TEXT.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EXCEL_TEXT.Depth = 0;
            this.EXCEL_TEXT.Hint = "";
            this.EXCEL_TEXT.Location = new System.Drawing.Point(68, 177);
            this.EXCEL_TEXT.Margin = new System.Windows.Forms.Padding(4);
            this.EXCEL_TEXT.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXCEL_TEXT.Name = "EXCEL_TEXT";
            this.EXCEL_TEXT.PasswordChar = '\0';
            this.EXCEL_TEXT.SelectedText = "";
            this.EXCEL_TEXT.SelectionLength = 0;
            this.EXCEL_TEXT.SelectionStart = 0;
            this.EXCEL_TEXT.Size = new System.Drawing.Size(614, 23);
            this.EXCEL_TEXT.TabIndex = 3;
            this.EXCEL_TEXT.UseSystemPasswordChar = false;
            // 
            // EXCEL_LABLE
            // 
            this.EXCEL_LABLE.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EXCEL_LABLE.AutoSize = true;
            this.EXCEL_LABLE.Depth = 0;
            this.EXCEL_LABLE.Font = new System.Drawing.Font("Roboto", 11F);
            this.EXCEL_LABLE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EXCEL_LABLE.Location = new System.Drawing.Point(64, 154);
            this.EXCEL_LABLE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EXCEL_LABLE.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXCEL_LABLE.Name = "EXCEL_LABLE";
            this.EXCEL_LABLE.Size = new System.Drawing.Size(206, 19);
            this.EXCEL_LABLE.TabIndex = 2;
            this.EXCEL_LABLE.Text = "EXCEL SAVEFULLFILENAME:";
            // 
            // EXPORT_BUTTON
            // 
            this.EXPORT_BUTTON.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.EXPORT_BUTTON.Depth = 0;
            this.EXPORT_BUTTON.Location = new System.Drawing.Point(582, 248);
            this.EXPORT_BUTTON.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXPORT_BUTTON.Name = "EXPORT_BUTTON";
            this.EXPORT_BUTTON.Primary = true;
            this.EXPORT_BUTTON.Size = new System.Drawing.Size(100, 23);
            this.EXPORT_BUTTON.TabIndex = 4;
            this.EXPORT_BUTTON.Text = "EXPORT";
            this.EXPORT_BUTTON.UseVisualStyleBackColor = true;
            this.EXPORT_BUTTON.Click += new System.EventHandler(this.EXPORT_BUTTON_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.DB_TEXT);
            this.panel1.Controls.Add(this.ERROR_LABLE);
            this.panel1.Controls.Add(this.DB_LABLE);
            this.panel1.Controls.Add(this.EXPORT_BUTTON);
            this.panel1.Controls.Add(this.EXCEL_LABLE);
            this.panel1.Controls.Add(this.EXCEL_TEXT);
            this.panel1.Location = new System.Drawing.Point(12, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(776, 365);
            this.panel1.TabIndex = 5;
            // 
            // ERROR_LABLE
            // 
            this.ERROR_LABLE.AutoSize = true;
            this.ERROR_LABLE.ForeColor = System.Drawing.Color.Red;
            this.ERROR_LABLE.Location = new System.Drawing.Point(69, 251);
            this.ERROR_LABLE.Name = "ERROR_LABLE";
            this.ERROR_LABLE.Size = new System.Drawing.Size(0, 16);
            this.ERROR_LABLE.TabIndex = 5;
            // 
            // DB_TEXT
            // 
            this.DB_TEXT.FormattingEnabled = true;
            this.DB_TEXT.Location = new System.Drawing.Point(68, 117);
            this.DB_TEXT.Name = "DB_TEXT";
            this.DB_TEXT.Size = new System.Drawing.Size(614, 24);
            this.DB_TEXT.TabIndex = 6;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MSDBTOEXCEL";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel DB_LABLE;
        private MaterialSkin.Controls.MaterialSingleLineTextField EXCEL_TEXT;
        private MaterialSkin.Controls.MaterialLabel EXCEL_LABLE;
        private MaterialSkin.Controls.MaterialRaisedButton EXPORT_BUTTON;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label ERROR_LABLE;
        private System.Windows.Forms.ComboBox DB_TEXT;
    }
}


namespace EXCELTOWEBAPICODE
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.EXPORT_BUTTON = new MaterialSkin.Controls.MaterialRaisedButton();
            this.EXCEL_TEXT = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.CS_TEXT = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.CE_LABLE = new MaterialSkin.Controls.MaterialLabel();
            this.EXCEL_LABLE = new MaterialSkin.Controls.MaterialLabel();
            this.ERROR_LABLE = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.ERROR_LABLE);
            this.panel1.Controls.Add(this.EXPORT_BUTTON);
            this.panel1.Controls.Add(this.EXCEL_TEXT);
            this.panel1.Controls.Add(this.CS_TEXT);
            this.panel1.Controls.Add(this.CE_LABLE);
            this.panel1.Controls.Add(this.EXCEL_LABLE);
            this.panel1.Location = new System.Drawing.Point(16, 75);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(771, 362);
            this.panel1.TabIndex = 0;
            // 
            // EXPORT_BUTTON
            // 
            this.EXPORT_BUTTON.Depth = 0;
            this.EXPORT_BUTTON.Location = new System.Drawing.Point(625, 259);
            this.EXPORT_BUTTON.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXPORT_BUTTON.Name = "EXPORT_BUTTON";
            this.EXPORT_BUTTON.Primary = true;
            this.EXPORT_BUTTON.Size = new System.Drawing.Size(75, 23);
            this.EXPORT_BUTTON.TabIndex = 4;
            this.EXPORT_BUTTON.Text = "EXPORT";
            this.EXPORT_BUTTON.UseVisualStyleBackColor = true;
            this.EXPORT_BUTTON.Click += new System.EventHandler(this.EXPORT_BUTTON_Click);
            // 
            // EXCEL_TEXT
            // 
            this.EXCEL_TEXT.Depth = 0;
            this.EXCEL_TEXT.Hint = "";
            this.EXCEL_TEXT.Location = new System.Drawing.Point(100, 98);
            this.EXCEL_TEXT.Margin = new System.Windows.Forms.Padding(4);
            this.EXCEL_TEXT.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXCEL_TEXT.Name = "EXCEL_TEXT";
            this.EXCEL_TEXT.PasswordChar = '\0';
            this.EXCEL_TEXT.SelectedText = "";
            this.EXCEL_TEXT.SelectionLength = 0;
            this.EXCEL_TEXT.SelectionStart = 0;
            this.EXCEL_TEXT.Size = new System.Drawing.Size(600, 23);
            this.EXCEL_TEXT.TabIndex = 3;
            this.EXCEL_TEXT.UseSystemPasswordChar = false;
            this.EXCEL_TEXT.Click += new System.EventHandler(this.EXCEL_TEXT_Click);
            // 
            // CS_TEXT
            // 
            this.CS_TEXT.Depth = 0;
            this.CS_TEXT.Hint = "";
            this.CS_TEXT.Location = new System.Drawing.Point(100, 173);
            this.CS_TEXT.Margin = new System.Windows.Forms.Padding(4);
            this.CS_TEXT.MouseState = MaterialSkin.MouseState.HOVER;
            this.CS_TEXT.Name = "CS_TEXT";
            this.CS_TEXT.PasswordChar = '\0';
            this.CS_TEXT.SelectedText = "";
            this.CS_TEXT.SelectionLength = 0;
            this.CS_TEXT.SelectionStart = 0;
            this.CS_TEXT.Size = new System.Drawing.Size(600, 23);
            this.CS_TEXT.TabIndex = 2;
            this.CS_TEXT.UseSystemPasswordChar = false;
            this.CS_TEXT.Click += new System.EventHandler(this.CS_TEXT_Click);
            // 
            // CE_LABLE
            // 
            this.CE_LABLE.AutoSize = true;
            this.CE_LABLE.Depth = 0;
            this.CE_LABLE.Font = new System.Drawing.Font("Roboto", 11F);
            this.CE_LABLE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.CE_LABLE.Location = new System.Drawing.Point(96, 150);
            this.CE_LABLE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CE_LABLE.MouseState = MaterialSkin.MouseState.HOVER;
            this.CE_LABLE.Name = "CE_LABLE";
            this.CE_LABLE.Size = new System.Drawing.Size(163, 19);
            this.CE_LABLE.TabIndex = 1;
            this.CE_LABLE.Text = "WEBAPICSSAVEPATH:";
            // 
            // EXCEL_LABLE
            // 
            this.EXCEL_LABLE.AutoSize = true;
            this.EXCEL_LABLE.Depth = 0;
            this.EXCEL_LABLE.Font = new System.Drawing.Font("Roboto", 11F);
            this.EXCEL_LABLE.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.EXCEL_LABLE.Location = new System.Drawing.Point(96, 75);
            this.EXCEL_LABLE.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EXCEL_LABLE.MouseState = MaterialSkin.MouseState.HOVER;
            this.EXCEL_LABLE.Name = "EXCEL_LABLE";
            this.EXCEL_LABLE.Size = new System.Drawing.Size(164, 19);
            this.EXCEL_LABLE.TabIndex = 0;
            this.EXCEL_LABLE.Text = "EXCELFULLFILENAME:";
            // 
            // ERROR_LABLE
            // 
            this.ERROR_LABLE.AutoSize = true;
            this.ERROR_LABLE.ForeColor = System.Drawing.Color.Red;
            this.ERROR_LABLE.Location = new System.Drawing.Point(97, 262);
            this.ERROR_LABLE.Name = "ERROR_LABLE";
            this.ERROR_LABLE.Size = new System.Drawing.Size(0, 16);
            this.ERROR_LABLE.TabIndex = 5;
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
            this.Text = "EXCELTOWEBAPICODE";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private MaterialSkin.Controls.MaterialLabel CE_LABLE;
        private MaterialSkin.Controls.MaterialLabel EXCEL_LABLE;
        private MaterialSkin.Controls.MaterialSingleLineTextField EXCEL_TEXT;
        private MaterialSkin.Controls.MaterialSingleLineTextField CS_TEXT;
        private MaterialSkin.Controls.MaterialRaisedButton EXPORT_BUTTON;
        private System.Windows.Forms.Label ERROR_LABLE;
    }
}


namespace CostcoWinForm
{
    partial class FrmCostcoCategories
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.gvCostcoCategories = new System.Windows.Forms.DataGridView();
            this.btnImportCategories = new System.Windows.Forms.Button();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCostcoCategories)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chkAll);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.gvCostcoCategories);
            this.panel1.Controls.Add(this.btnImportCategories);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 525);
            this.panel1.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(899, 490);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // gvCostcoCategories
            // 
            this.gvCostcoCategories.AllowUserToAddRows = false;
            this.gvCostcoCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvCostcoCategories.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCostcoCategories.Location = new System.Drawing.Point(12, 41);
            this.gvCostcoCategories.Name = "gvCostcoCategories";
            this.gvCostcoCategories.Size = new System.Drawing.Size(962, 443);
            this.gvCostcoCategories.TabIndex = 1;
            this.gvCostcoCategories.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvCostcoCategories_CellEndEdit);
            // 
            // btnImportCategories
            // 
            this.btnImportCategories.Location = new System.Drawing.Point(12, 12);
            this.btnImportCategories.Name = "btnImportCategories";
            this.btnImportCategories.Size = new System.Drawing.Size(114, 23);
            this.btnImportCategories.TabIndex = 0;
            this.btnImportCategories.Text = "Import Categories";
            this.btnImportCategories.UseVisualStyleBackColor = true;
            this.btnImportCategories.Click += new System.EventHandler(this.btnImportCategories_Click);
            // 
            // chkAll
            // 
            this.chkAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(844, 16);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 3;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // FrmCostcoCategories
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 525);
            this.Controls.Add(this.panel1);
            this.Name = "FrmCostcoCategories";
            this.Text = "FrmCostcoCategories";
            this.Load += new System.EventHandler(this.FrmCostcoCategories_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCostcoCategories)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvCostcoCategories;
        private System.Windows.Forms.Button btnImportCategories;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkAll;
    }
}
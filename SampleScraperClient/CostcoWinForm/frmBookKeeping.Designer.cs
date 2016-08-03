namespace CostcoWinForm
{
    partial class frmBookKeeping
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
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gvBookKeeping = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gvBookKeeping)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "All",
            "8 - Advertising",
            "9 - Car and truck mileage",
            "10 - Comnissions and fees",
            "11 - Contract labor",
            "12 - Depletion",
            "13 - Depreciation and section 179 expense deduction",
            "14 - Employee benefit programs",
            "15 - Insurance (other than health)",
            "16a - Interest: Mortgage (paid to banks, etc)",
            "16b - Interest: Other",
            "17 - Legal and professional services",
            "18 - Office expense",
            "19 - Pension and profit-sharing plans",
            "20a - Rent of lease: Vehicles, machinery, and equipment",
            "20b - Rent or lease: Other business property",
            "21 - Repairs and maintenance",
            "22 - Supplies",
            "23 - Taxes and licenses",
            "24a - Travel, meals, and entertainment: Travel",
            "24b - Travel, meals, and entertainment: Deductable meals and entertainment",
            "25 - Utilities",
            "26 - Wage (less employment credits)",
            "27 - Other expenses"});
            this.cmbCategory.Location = new System.Drawing.Point(70, 6);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(206, 21);
            this.cmbCategory.TabIndex = 0;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Category:";
            // 
            // gvBookKeeping
            // 
            this.gvBookKeeping.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvBookKeeping.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvBookKeeping.Location = new System.Drawing.Point(12, 33);
            this.gvBookKeeping.Name = "gvBookKeeping";
            this.gvBookKeeping.Size = new System.Drawing.Size(898, 450);
            this.gvBookKeeping.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(835, 489);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(785, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Year:";
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Items.AddRange(new object[] {
            "2016",
            "2017",
            "2018"});
            this.cmbYear.Location = new System.Drawing.Point(823, 6);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(87, 21);
            this.cmbYear.TabIndex = 4;
            // 
            // frmBookKeeping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 524);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbYear);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gvBookKeeping);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCategory);
            this.Name = "frmBookKeeping";
            this.Text = "frmBookKeeping";
            this.Load += new System.EventHandler(this.frmBookKeeping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvBookKeeping)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvBookKeeping;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
    }
}
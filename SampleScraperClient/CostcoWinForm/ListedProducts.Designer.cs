namespace CostcoWinForm
{
    partial class ListedProducts
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.costcoDataSet1 = new CostcoWinForm.CostcoDataSet1();
            this.eBayCurrentListingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.eBay_CurrentListingsTableAdapter = new CostcoWinForm.CostcoDataSet1TableAdapters.eBay_CurrentListingsTableAdapter();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.eBayCategoryIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingDTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayCurrentListingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(94, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Update";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.Image,
            this.eBayCategoryIDDataGridViewTextBoxColumn,
            this.eBayItemNumberDataGridViewTextBoxColumn,
            this.eBayListingPriceDataGridViewTextBoxColumn,
            this.eBayDescriptionDataGridViewTextBoxColumn,
            this.eBayListingDTDataGridViewTextBoxColumn,
            this.eBayUrlDataGridViewTextBoxColumn,
            this.costcoUrlDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.eBayCurrentListingsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 100;
            this.dataGridView1.Size = new System.Drawing.Size(902, 470);
            this.dataGridView1.TabIndex = 2;
            // 
            // costcoDataSet1
            // 
            this.costcoDataSet1.DataSetName = "CostcoDataSet1";
            this.costcoDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // eBayCurrentListingsBindingSource
            // 
            this.eBayCurrentListingsBindingSource.DataMember = "eBay_CurrentListings";
            this.eBayCurrentListingsBindingSource.DataSource = this.costcoDataSet1;
            // 
            // eBay_CurrentListingsTableAdapter
            // 
            this.eBay_CurrentListingsTableAdapter.ClearBeforeFill = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // Image
            // 
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            // 
            // eBayCategoryIDDataGridViewTextBoxColumn
            // 
            this.eBayCategoryIDDataGridViewTextBoxColumn.DataPropertyName = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn.HeaderText = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn.Name = "eBayCategoryIDDataGridViewTextBoxColumn";
            // 
            // eBayItemNumberDataGridViewTextBoxColumn
            // 
            this.eBayItemNumberDataGridViewTextBoxColumn.DataPropertyName = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn.HeaderText = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn.Name = "eBayItemNumberDataGridViewTextBoxColumn";
            // 
            // eBayListingPriceDataGridViewTextBoxColumn
            // 
            this.eBayListingPriceDataGridViewTextBoxColumn.DataPropertyName = "eBayListingPrice";
            this.eBayListingPriceDataGridViewTextBoxColumn.HeaderText = "eBayListingPrice";
            this.eBayListingPriceDataGridViewTextBoxColumn.Name = "eBayListingPriceDataGridViewTextBoxColumn";
            // 
            // eBayDescriptionDataGridViewTextBoxColumn
            // 
            this.eBayDescriptionDataGridViewTextBoxColumn.DataPropertyName = "eBayDescription";
            this.eBayDescriptionDataGridViewTextBoxColumn.HeaderText = "eBayDescription";
            this.eBayDescriptionDataGridViewTextBoxColumn.Name = "eBayDescriptionDataGridViewTextBoxColumn";
            // 
            // eBayListingDTDataGridViewTextBoxColumn
            // 
            this.eBayListingDTDataGridViewTextBoxColumn.DataPropertyName = "eBayListingDT";
            this.eBayListingDTDataGridViewTextBoxColumn.HeaderText = "eBayListingDT";
            this.eBayListingDTDataGridViewTextBoxColumn.Name = "eBayListingDTDataGridViewTextBoxColumn";
            // 
            // eBayUrlDataGridViewTextBoxColumn
            // 
            this.eBayUrlDataGridViewTextBoxColumn.DataPropertyName = "eBayUrl";
            this.eBayUrlDataGridViewTextBoxColumn.HeaderText = "eBayUrl";
            this.eBayUrlDataGridViewTextBoxColumn.Name = "eBayUrlDataGridViewTextBoxColumn";
            // 
            // costcoUrlDataGridViewTextBoxColumn
            // 
            this.costcoUrlDataGridViewTextBoxColumn.DataPropertyName = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn.HeaderText = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn.Name = "costcoUrlDataGridViewTextBoxColumn";
            // 
            // ListedProducts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 524);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ListedProducts";
            this.Text = "ListedProducts";
            this.Load += new System.EventHandler(this.ListedProducts_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayCurrentListingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private CostcoDataSet1 costcoDataSet1;
        private System.Windows.Forms.BindingSource eBayCurrentListingsBindingSource;
        private CostcoDataSet1TableAdapters.eBay_CurrentListingsTableAdapter eBay_CurrentListingsTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayCategoryIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingDTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlDataGridViewTextBoxColumn;
    }
}
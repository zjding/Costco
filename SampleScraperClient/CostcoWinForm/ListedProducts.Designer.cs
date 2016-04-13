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
            this.costcoDataSet3 = new CostcoWinForm.CostcoDataSet3();
            this.eBayCurrentListingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.eBay_CurrentListingsTableAdapter = new CostcoWinForm.CostcoDataSet3TableAdapters.eBay_CurrentListingsTableAdapter();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.eBayCategoryIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingDTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoItemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLinkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet3)).BeginInit();
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
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select,
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Image,
            this.eBayCategoryIDDataGridViewTextBoxColumn,
            this.eBayItemNumberDataGridViewTextBoxColumn,
            this.eBayListingPriceDataGridViewTextBoxColumn,
            this.eBayDescriptionDataGridViewTextBoxColumn,
            this.eBayListingDTDataGridViewTextBoxColumn,
            this.eBayUrlDataGridViewTextBoxColumn,
            this.costcoUrlNumberDataGridViewTextBoxColumn,
            this.costcoItemNumberDataGridViewTextBoxColumn,
            this.costcoUrlDataGridViewTextBoxColumn,
            this.imageLinkDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.eBayCurrentListingsBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(12, 42);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 50;
            this.dataGridView1.Size = new System.Drawing.Size(902, 470);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // costcoDataSet3
            // 
            this.costcoDataSet3.DataSetName = "CostcoDataSet3";
            this.costcoDataSet3.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // eBayCurrentListingsBindingSource
            // 
            this.eBayCurrentListingsBindingSource.DataMember = "eBay_CurrentListings";
            this.eBayCurrentListingsBindingSource.DataSource = this.costcoDataSet3;
            // 
            // eBay_CurrentListingsTableAdapter
            // 
            this.eBay_CurrentListingsTableAdapter.ClearBeforeFill = true;
            // 
            // Select
            // 
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            this.Select.Width = 20;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // Image
            // 
            this.Image.FillWeight = 50F;
            this.Image.HeaderText = "Image";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Image.Name = "Image";
            this.Image.Width = 50;
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
            // costcoUrlNumberDataGridViewTextBoxColumn
            // 
            this.costcoUrlNumberDataGridViewTextBoxColumn.DataPropertyName = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn.HeaderText = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn.Name = "costcoUrlNumberDataGridViewTextBoxColumn";
            // 
            // costcoItemNumberDataGridViewTextBoxColumn
            // 
            this.costcoItemNumberDataGridViewTextBoxColumn.DataPropertyName = "CostcoItemNumber";
            this.costcoItemNumberDataGridViewTextBoxColumn.HeaderText = "CostcoItemNumber";
            this.costcoItemNumberDataGridViewTextBoxColumn.Name = "costcoItemNumberDataGridViewTextBoxColumn";
            this.costcoItemNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // costcoUrlDataGridViewTextBoxColumn
            // 
            this.costcoUrlDataGridViewTextBoxColumn.DataPropertyName = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn.HeaderText = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn.Name = "costcoUrlDataGridViewTextBoxColumn";
            // 
            // imageLinkDataGridViewTextBoxColumn
            // 
            this.imageLinkDataGridViewTextBoxColumn.DataPropertyName = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn.HeaderText = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn.Name = "imageLinkDataGridViewTextBoxColumn";
            this.imageLinkDataGridViewTextBoxColumn.Visible = false;
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
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayCurrentListingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private CostcoDataSet3 costcoDataSet3;
        private System.Windows.Forms.BindingSource eBayCurrentListingsBindingSource;
        private CostcoDataSet3TableAdapters.eBay_CurrentListingsTableAdapter eBay_CurrentListingsTableAdapter;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayCategoryIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingDTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoItemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLinkDataGridViewTextBoxColumn;
    }
}
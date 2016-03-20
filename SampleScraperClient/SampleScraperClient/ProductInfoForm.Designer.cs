namespace SampleScraperClient
{
    partial class ProductInfoForm
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.costcoDataSet = new SampleScraperClient.CostcoDataSet();
            this.productInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.productInfoTableAdapter = new SampleScraperClient.CostcoDataSetTableAdapters.ProductInfoTableAdapter();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shippingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specificationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLinkDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.Image,
            this.nameDataGridViewTextBoxColumn,
            this.itemNumberDataGridViewTextBoxColumn,
            this.priceDataGridViewTextBoxColumn,
            this.shippingDataGridViewTextBoxColumn,
            this.detailsDataGridViewTextBoxColumn,
            this.specificationDataGridViewTextBoxColumn,
            this.imageLinkDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.productInfoBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(921, 482);
            this.dataGridView1.TabIndex = 0;
            // 
            // costcoDataSet
            // 
            this.costcoDataSet.DataSetName = "CostcoDataSet";
            this.costcoDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // productInfoBindingSource
            // 
            this.productInfoBindingSource.DataMember = "ProductInfo";
            this.productInfoBindingSource.DataSource = this.costcoDataSet;
            // 
            // productInfoTableAdapter
            // 
            this.productInfoTableAdapter.ClearBeforeFill = true;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // Image
            // 
            this.Image.DataPropertyName = "ID";
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            // 
            // itemNumberDataGridViewTextBoxColumn
            // 
            this.itemNumberDataGridViewTextBoxColumn.DataPropertyName = "ItemNumber";
            this.itemNumberDataGridViewTextBoxColumn.HeaderText = "ItemNumber";
            this.itemNumberDataGridViewTextBoxColumn.Name = "itemNumberDataGridViewTextBoxColumn";
            // 
            // priceDataGridViewTextBoxColumn
            // 
            this.priceDataGridViewTextBoxColumn.DataPropertyName = "Price";
            this.priceDataGridViewTextBoxColumn.HeaderText = "Price";
            this.priceDataGridViewTextBoxColumn.Name = "priceDataGridViewTextBoxColumn";
            // 
            // shippingDataGridViewTextBoxColumn
            // 
            this.shippingDataGridViewTextBoxColumn.DataPropertyName = "Shipping";
            this.shippingDataGridViewTextBoxColumn.HeaderText = "Shipping";
            this.shippingDataGridViewTextBoxColumn.Name = "shippingDataGridViewTextBoxColumn";
            // 
            // detailsDataGridViewTextBoxColumn
            // 
            this.detailsDataGridViewTextBoxColumn.DataPropertyName = "Details";
            this.detailsDataGridViewTextBoxColumn.HeaderText = "Details";
            this.detailsDataGridViewTextBoxColumn.Name = "detailsDataGridViewTextBoxColumn";
            // 
            // specificationDataGridViewTextBoxColumn
            // 
            this.specificationDataGridViewTextBoxColumn.DataPropertyName = "Specification";
            this.specificationDataGridViewTextBoxColumn.HeaderText = "Specification";
            this.specificationDataGridViewTextBoxColumn.Name = "specificationDataGridViewTextBoxColumn";
            // 
            // imageLinkDataGridViewTextBoxColumn
            // 
            this.imageLinkDataGridViewTextBoxColumn.DataPropertyName = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn.HeaderText = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn.Name = "imageLinkDataGridViewTextBoxColumn";
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            // 
            // ProductInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(921, 482);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ProductInfoForm";
            this.Text = "ProductInfoForm";
            this.Load += new System.EventHandler(this.ProductInfoForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private CostcoDataSet costcoDataSet;
        private System.Windows.Forms.BindingSource productInfoBindingSource;
        private CostcoDataSetTableAdapters.ProductInfoTableAdapter productInfoTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shippingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn detailsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn specificationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLinkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
    }
}
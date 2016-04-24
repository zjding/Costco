﻿namespace CostcoWinForm
{
    partial class Form1
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
            this.btnProductInfo = new System.Windows.Forms.Button();
            this.btnImportCategories = new System.Windows.Forms.Button();
            this.btnImportProducts = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnSubCategories = new System.Windows.Forms.Button();
            this.btnProductText = new System.Windows.Forms.Button();
            this.btnEbayCategory = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.btnExcel = new System.Windows.Forms.Button();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnListedProducts = new System.Windows.Forms.Button();
            this.btnCreatePDF = new System.Windows.Forms.Button();
            this.btnFillWebForm = new System.Windows.Forms.Button();
            this.btnResearch = new System.Windows.Forms.Button();
            this.btnEmailExtract = new System.Windows.Forms.Button();
            this.btnSoldEmailExtract = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnProductInfo
            // 
            this.btnProductInfo.Location = new System.Drawing.Point(32, 481);
            this.btnProductInfo.Name = "btnProductInfo";
            this.btnProductInfo.Size = new System.Drawing.Size(118, 23);
            this.btnProductInfo.TabIndex = 0;
            this.btnProductInfo.Text = "ProductInfo";
            this.btnProductInfo.UseVisualStyleBackColor = true;
            this.btnProductInfo.Click += new System.EventHandler(this.btnProductInfo_Click);
            // 
            // btnImportCategories
            // 
            this.btnImportCategories.Location = new System.Drawing.Point(32, 34);
            this.btnImportCategories.Name = "btnImportCategories";
            this.btnImportCategories.Size = new System.Drawing.Size(118, 23);
            this.btnImportCategories.TabIndex = 1;
            this.btnImportCategories.Text = "Import Categories";
            this.btnImportCategories.UseVisualStyleBackColor = true;
            this.btnImportCategories.Click += new System.EventHandler(this.btnImportCategories_Click);
            // 
            // btnImportProducts
            // 
            this.btnImportProducts.Location = new System.Drawing.Point(32, 63);
            this.btnImportProducts.Name = "btnImportProducts";
            this.btnImportProducts.Size = new System.Drawing.Size(118, 23);
            this.btnImportProducts.TabIndex = 2;
            this.btnImportProducts.Text = "Import Products";
            this.btnImportProducts.UseVisualStyleBackColor = true;
            this.btnImportProducts.Click += new System.EventHandler(this.btnImportProducts_Click);
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(32, 92);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(118, 23);
            this.btnEmail.TabIndex = 5;
            this.btnEmail.Text = "Send Email";
            this.btnEmail.UseVisualStyleBackColor = true;
            this.btnEmail.Click += new System.EventHandler(this.btnEmail_Click);
            // 
            // btnSubCategories
            // 
            this.btnSubCategories.Location = new System.Drawing.Point(32, 121);
            this.btnSubCategories.Name = "btnSubCategories";
            this.btnSubCategories.Size = new System.Drawing.Size(118, 23);
            this.btnSubCategories.TabIndex = 6;
            this.btnSubCategories.Text = "Get SubCategories";
            this.btnSubCategories.UseVisualStyleBackColor = true;
            this.btnSubCategories.Click += new System.EventHandler(this.btnSubCategories_Click);
            // 
            // btnProductText
            // 
            this.btnProductText.Location = new System.Drawing.Point(32, 150);
            this.btnProductText.Name = "btnProductText";
            this.btnProductText.Size = new System.Drawing.Size(118, 23);
            this.btnProductText.TabIndex = 7;
            this.btnProductText.Text = "Import Product Test";
            this.btnProductText.UseVisualStyleBackColor = true;
            this.btnProductText.Click += new System.EventHandler(this.btnProductText_Click);
            // 
            // btnEbayCategory
            // 
            this.btnEbayCategory.Location = new System.Drawing.Point(32, 179);
            this.btnEbayCategory.Name = "btnEbayCategory";
            this.btnEbayCategory.Size = new System.Drawing.Size(118, 23);
            this.btnEbayCategory.TabIndex = 8;
            this.btnEbayCategory.Text = "Get eBay Category";
            this.btnEbayCategory.UseVisualStyleBackColor = true;
            this.btnEbayCategory.Click += new System.EventHandler(this.btnEbayCategory_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.webBrowser1.Location = new System.Drawing.Point(156, 34);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(743, 495);
            this.webBrowser1.TabIndex = 9;
            // 
            // btnExcel
            // 
            this.btnExcel.Location = new System.Drawing.Point(32, 208);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(118, 23);
            this.btnExcel.TabIndex = 10;
            this.btnExcel.Text = "Generate Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(32, 237);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(118, 23);
            this.btnLoad.TabIndex = 11;
            this.btnLoad.Text = "Upload";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // btnListedProducts
            // 
            this.btnListedProducts.Location = new System.Drawing.Point(32, 510);
            this.btnListedProducts.Name = "btnListedProducts";
            this.btnListedProducts.Size = new System.Drawing.Size(118, 23);
            this.btnListedProducts.TabIndex = 12;
            this.btnListedProducts.Text = "Listed Products";
            this.btnListedProducts.UseVisualStyleBackColor = true;
            this.btnListedProducts.Click += new System.EventHandler(this.btnListedProducts_Click);
            // 
            // btnCreatePDF
            // 
            this.btnCreatePDF.Location = new System.Drawing.Point(32, 298);
            this.btnCreatePDF.Name = "btnCreatePDF";
            this.btnCreatePDF.Size = new System.Drawing.Size(118, 23);
            this.btnCreatePDF.TabIndex = 13;
            this.btnCreatePDF.Text = "Create PDF";
            this.btnCreatePDF.UseVisualStyleBackColor = true;
            this.btnCreatePDF.Click += new System.EventHandler(this.btnCreatePDF_Click);
            // 
            // btnFillWebForm
            // 
            this.btnFillWebForm.Location = new System.Drawing.Point(32, 327);
            this.btnFillWebForm.Name = "btnFillWebForm";
            this.btnFillWebForm.Size = new System.Drawing.Size(118, 23);
            this.btnFillWebForm.TabIndex = 14;
            this.btnFillWebForm.Text = "Fill Web Form";
            this.btnFillWebForm.UseVisualStyleBackColor = true;
            this.btnFillWebForm.Click += new System.EventHandler(this.btnFillWebForm_Click);
            // 
            // btnResearch
            // 
            this.btnResearch.Location = new System.Drawing.Point(32, 377);
            this.btnResearch.Name = "btnResearch";
            this.btnResearch.Size = new System.Drawing.Size(118, 23);
            this.btnResearch.TabIndex = 15;
            this.btnResearch.Text = "Product Research";
            this.btnResearch.UseVisualStyleBackColor = true;
            this.btnResearch.Click += new System.EventHandler(this.btnResearch_Click);
            // 
            // btnEmailExtract
            // 
            this.btnEmailExtract.Location = new System.Drawing.Point(32, 406);
            this.btnEmailExtract.Name = "btnEmailExtract";
            this.btnEmailExtract.Size = new System.Drawing.Size(118, 23);
            this.btnEmailExtract.TabIndex = 16;
            this.btnEmailExtract.Text = "Paid Email Extract";
            this.btnEmailExtract.UseVisualStyleBackColor = true;
            this.btnEmailExtract.Click += new System.EventHandler(this.btnEmailExtract_Click);
            // 
            // btnSoldEmailExtract
            // 
            this.btnSoldEmailExtract.Location = new System.Drawing.Point(32, 435);
            this.btnSoldEmailExtract.Name = "btnSoldEmailExtract";
            this.btnSoldEmailExtract.Size = new System.Drawing.Size(118, 23);
            this.btnSoldEmailExtract.TabIndex = 17;
            this.btnSoldEmailExtract.Text = "Sold Email Extract";
            this.btnSoldEmailExtract.UseVisualStyleBackColor = true;
            this.btnSoldEmailExtract.Click += new System.EventHandler(this.btnSoldEmailExtract_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 541);
            this.Controls.Add(this.btnSoldEmailExtract);
            this.Controls.Add(this.btnEmailExtract);
            this.Controls.Add(this.btnResearch);
            this.Controls.Add(this.btnFillWebForm);
            this.Controls.Add(this.btnCreatePDF);
            this.Controls.Add(this.btnListedProducts);
            this.Controls.Add(this.btnLoad);
            this.Controls.Add(this.btnExcel);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnEbayCategory);
            this.Controls.Add(this.btnProductText);
            this.Controls.Add(this.btnSubCategories);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.btnImportProducts);
            this.Controls.Add(this.btnImportCategories);
            this.Controls.Add(this.btnProductInfo);
            this.Name = "Form1";
            this.Text = "Main";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnProductInfo;
        private System.Windows.Forms.Button btnImportCategories;
        private System.Windows.Forms.Button btnImportProducts;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnSubCategories;
        private System.Windows.Forms.Button btnProductText;
        private System.Windows.Forms.Button btnEbayCategory;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnListedProducts;
        private System.Windows.Forms.Button btnCreatePDF;
        private System.Windows.Forms.Button btnFillWebForm;
        private System.Windows.Forms.Button btnResearch;
        private System.Windows.Forms.Button btnEmailExtract;
        private System.Windows.Forms.Button btnSoldEmailExtract;
    }
}


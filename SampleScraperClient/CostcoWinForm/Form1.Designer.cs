namespace CostcoWinForm
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
            this.lblcurrent = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.btnEmail = new System.Windows.Forms.Button();
            this.btnSubCategories = new System.Windows.Forms.Button();
            this.btnProductText = new System.Windows.Forms.Button();
            this.btnEbayCategory = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // btnProductInfo
            // 
            this.btnProductInfo.Location = new System.Drawing.Point(32, 283);
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
            // lblcurrent
            // 
            this.lblcurrent.AutoSize = true;
            this.lblcurrent.Location = new System.Drawing.Point(37, 352);
            this.lblcurrent.Name = "lblcurrent";
            this.lblcurrent.Size = new System.Drawing.Size(13, 13);
            this.lblcurrent.TabIndex = 3;
            this.lblcurrent.Text = "0";
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(92, 352);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(21, 13);
            this.lblTotal.TabIndex = 4;
            this.lblTotal.Text = "/ 0";
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
            this.webBrowser1.Location = new System.Drawing.Point(156, 34);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(456, 353);
            this.webBrowser1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 399);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.btnEbayCategory);
            this.Controls.Add(this.btnProductText);
            this.Controls.Add(this.btnSubCategories);
            this.Controls.Add(this.btnEmail);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.lblcurrent);
            this.Controls.Add(this.btnImportProducts);
            this.Controls.Add(this.btnImportCategories);
            this.Controls.Add(this.btnProductInfo);
            this.Name = "Form1";
            this.Text = "Main";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProductInfo;
        private System.Windows.Forms.Button btnImportCategories;
        private System.Windows.Forms.Button btnImportProducts;
        private System.Windows.Forms.Label lblcurrent;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Button btnEmail;
        private System.Windows.Forms.Button btnSubCategories;
        private System.Windows.Forms.Button btnProductText;
        private System.Windows.Forms.Button btnEbayCategory;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}


namespace CostcoWinForm
{
    partial class eBayFrontEnd
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tpTax = new System.Windows.Forms.TabPage();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.gvTaxExempt = new System.Windows.Forms.DataGridView();
            this.btnSendEmail = new System.Windows.Forms.Button();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.btnGenerateFiles = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gvSummary = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tpSold = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gvEBaySold = new System.Windows.Forms.DataGridView();
            this.paypalTransactionIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paypalPaidDateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paypalPaidEmailPdfDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNumberDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBaySoldDateTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingQualityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBaySoldQualityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayRemainingQualityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBaySoldPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayUrlDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBaySoldEmailPdfDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerIDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerAddress1DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerAddress2DataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerStateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerEmailDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buyerNoteDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlNumberDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoPriceDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoOrderNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoItemNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoItemNumberDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoTrackingNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoShipDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoTaxExemptPdfDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoOrderEmailPdfDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBaySoldTransactionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dsEBaySold = new CostcoWinForm.dsEBaySold();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.tpCurrentListing = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnReloadCurrentListing = new System.Windows.Forms.Button();
            this.gvCurrentListing = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayCategoryIDDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayListingPriceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayDescriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayEndTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoItemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLinkDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SelectListing = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.eBayCurrentListingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dseBayCurrentListings = new CostcoWinForm.dseBayCurrentListings();
            this.btnListingModify = new System.Windows.Forms.Button();
            this.btnListingDelete = new System.Windows.Forms.Button();
            this.tpCostco = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lvCategories = new System.Windows.Forms.ListView();
            this.Select = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Category_8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnCostcoCategory = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button9 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.btnAddPending = new System.Windows.Forms.Button();
            this.btnRefreshProducts = new System.Windows.Forms.Button();
            this.gvProducts = new System.Windows.Forms.DataGridView();
            this.Check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.ImageLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UrlNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shippingDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Discount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specificationDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnCrawl = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpDashboard = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.llEBayOptions = new System.Windows.Forms.LinkLabel();
            this.llEBayPriceDown = new System.Windows.Forms.LinkLabel();
            this.llEBayPriceUp = new System.Windows.Forms.LinkLabel();
            this.label96 = new System.Windows.Forms.Label();
            this.label95 = new System.Windows.Forms.Label();
            this.label93 = new System.Windows.Forms.Label();
            this.label92 = new System.Windows.Forms.Label();
            this.label90 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.label87 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.label82 = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.llEBayDiscontinue = new System.Windows.Forms.LinkLabel();
            this.label88 = new System.Windows.Forms.Label();
            this.label91 = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.label94 = new System.Windows.Forms.Label();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.btnChangeForEBayListings = new System.Windows.Forms.Button();
            this.tpToAdd = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.gvAdd = new System.Windows.Forms.DataGridView();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpload = new System.Windows.Forms.Button();
            this.tpEBayToModify = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.btnToChangeUpdate = new System.Windows.Forms.Button();
            this.btnToChangeDelete = new System.Windows.Forms.Button();
            this.btnToChangeUpload = new System.Windows.Forms.Button();
            this.gvToChange = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameDataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoUrlNumberDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayItemNumberDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayOldListingPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayNewListingPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayReferencePriceDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoOldPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.costcoNewPriceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceChangeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.categoryDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shippingDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.limitDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.discountDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.detailsDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.specificationDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.imageLinkDataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberOfImageDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.urlDataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayCategoryIDDataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionImageWidthDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionImageHeightDataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eBayToChangeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.costcoDataSet6 = new CostcoWinForm.CostcoDataSet6();
            this.tpEbayToDelete = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.gvDelete = new System.Windows.Forms.DataGridView();
            this.ToDeleteSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnToDeleteUpload = new System.Windows.Forms.Button();
            this.tpSaleTax = new System.Windows.Forms.TabPage();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.btnSaleTaxSave = new System.Windows.Forms.Button();
            this.gvSaleTaxHistory = new System.Windows.Forms.DataGridView();
            this.btnGenerateSaleTaxReport = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpSaleTaxTo = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpSaleTaxFrom = new System.Windows.Forms.DateTimePicker();
            this.tpIncomeTax = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnIncomeTaxCalculate = new System.Windows.Forms.Button();
            this.cmbIncomeTaxYear = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.ll31 = new System.Windows.Forms.LinkLabel();
            this.label81 = new System.Windows.Forms.Label();
            this.label49 = new System.Windows.Forms.Label();
            this.ll30 = new System.Windows.Forms.LinkLabel();
            this.label84 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.ll29 = new System.Windows.Forms.LinkLabel();
            this.label80 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.ll28 = new System.Windows.Forms.LinkLabel();
            this.label78 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.ll27 = new System.Windows.Forms.LinkLabel();
            this.label76 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.ll26 = new System.Windows.Forms.LinkLabel();
            this.label74 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.ll25 = new System.Windows.Forms.LinkLabel();
            this.label72 = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.ll24b = new System.Windows.Forms.LinkLabel();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.ll24a = new System.Windows.Forms.LinkLabel();
            this.label67 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.ll23 = new System.Windows.Forms.LinkLabel();
            this.label63 = new System.Windows.Forms.Label();
            this.label62 = new System.Windows.Forms.Label();
            this.ll22 = new System.Windows.Forms.LinkLabel();
            this.label61 = new System.Windows.Forms.Label();
            this.label60 = new System.Windows.Forms.Label();
            this.ll21 = new System.Windows.Forms.LinkLabel();
            this.label59 = new System.Windows.Forms.Label();
            this.label58 = new System.Windows.Forms.Label();
            this.ll20b = new System.Windows.Forms.LinkLabel();
            this.label57 = new System.Windows.Forms.Label();
            this.label56 = new System.Windows.Forms.Label();
            this.ll20a = new System.Windows.Forms.LinkLabel();
            this.label55 = new System.Windows.Forms.Label();
            this.label54 = new System.Windows.Forms.Label();
            this.ll19 = new System.Windows.Forms.LinkLabel();
            this.label53 = new System.Windows.Forms.Label();
            this.label52 = new System.Windows.Forms.Label();
            this.ll18 = new System.Windows.Forms.LinkLabel();
            this.label51 = new System.Windows.Forms.Label();
            this.label50 = new System.Windows.Forms.Label();
            this.ll17 = new System.Windows.Forms.LinkLabel();
            this.label48 = new System.Windows.Forms.Label();
            this.label45 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.ll16a = new System.Windows.Forms.LinkLabel();
            this.label27 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.ll15 = new System.Windows.Forms.LinkLabel();
            this.label29 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.ll14 = new System.Windows.Forms.LinkLabel();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.ll13 = new System.Windows.Forms.LinkLabel();
            this.label33 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.ll12 = new System.Windows.Forms.LinkLabel();
            this.label35 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.ll11 = new System.Windows.Forms.LinkLabel();
            this.label37 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.ll10 = new System.Windows.Forms.LinkLabel();
            this.label39 = new System.Windows.Forms.Label();
            this.label40 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.ll9 = new System.Windows.Forms.LinkLabel();
            this.label42 = new System.Windows.Forms.Label();
            this.ll8 = new System.Windows.Forms.LinkLabel();
            this.label43 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.ll16b = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.ll7 = new System.Windows.Forms.LinkLabel();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.ll6 = new System.Windows.Forms.LinkLabel();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.ll5 = new System.Windows.Forms.LinkLabel();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.ll4 = new System.Windows.Forms.LinkLabel();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.ll3 = new System.Windows.Forms.LinkLabel();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.ll2 = new System.Windows.Forms.LinkLabel();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.ll1c = new System.Windows.Forms.LinkLabel();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ll1b = new System.Windows.Forms.LinkLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.ll1a = new System.Windows.Forms.LinkLabel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.ll66d = new System.Windows.Forms.Label();
            this.ll1 = new System.Windows.Forms.LinkLabel();
            this.tpResearch = new System.Windows.Forms.TabPage();
            this.gvEBayResearch = new System.Windows.Forms.DataGridView();
            this.cmbStore = new System.Windows.Forms.ComboBox();
            this.label66 = new System.Windows.Forms.Label();
            this.btnResearch = new System.Windows.Forms.Button();
            this.tpMaintenance = new System.Windows.Forms.TabPage();
            this.btnImportEBayCatetories = new System.Windows.Forms.Button();
            this.btnEBayItemSpecifics = new System.Windows.Forms.Button();
            this.costcoDataSet4 = new CostcoWinForm.CostcoDataSet4();
            this.eBay_CurrentListingsTableAdapter = new CostcoWinForm.dseBayCurrentListingsTableAdapters.eBay_CurrentListingsTableAdapter();
            this.eBay_ToChangeTableAdapter = new CostcoWinForm.CostcoDataSet6TableAdapters.eBay_ToChangeTableAdapter();
            this.eBay_SoldTransactionsTableAdapter = new CostcoWinForm.dsEBaySoldTableAdapters.eBay_SoldTransactionsTableAdapter();
            this.productInfoTableAdapter1 = new CostcoWinForm.CostcoDataSet4TableAdapters.ProductInfoTableAdapter();
            this.productInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gvChange = new System.Windows.Forms.DataGridView();
            this.ToChangeSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tpTax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvTaxExempt)).BeginInit();
            this.groupBox8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSummary)).BeginInit();
            this.tpSold.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvEBaySold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBaySoldTransactionsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEBaySold)).BeginInit();
            this.tpCurrentListing.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvCurrentListing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayCurrentListingsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dseBayCurrentListings)).BeginInit();
            this.tpCostco.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvProducts)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tpDashboard.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tpToAdd.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvAdd)).BeginInit();
            this.tpEBayToModify.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvToChange)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayToChangeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet6)).BeginInit();
            this.tpEbayToDelete.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDelete)).BeginInit();
            this.tpSaleTax.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSaleTaxHistory)).BeginInit();
            this.tpIncomeTax.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tpResearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvEBayResearch)).BeginInit();
            this.tpMaintenance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChange)).BeginInit();
            this.SuspendLayout();
            // 
            // tpTax
            // 
            this.tpTax.Controls.Add(this.splitContainer4);
            this.tpTax.Location = new System.Drawing.Point(4, 22);
            this.tpTax.Name = "tpTax";
            this.tpTax.Padding = new System.Windows.Forms.Padding(3);
            this.tpTax.Size = new System.Drawing.Size(1244, 634);
            this.tpTax.TabIndex = 4;
            this.tpTax.Text = "Tax Exempt";
            this.tpTax.UseVisualStyleBackColor = true;
            this.tpTax.Enter += new System.EventHandler(this.tpTax_Enter);
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.Location = new System.Drawing.Point(3, 3);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.gvTaxExempt);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.btnSendEmail);
            this.splitContainer4.Panel2.Controls.Add(this.btnCalculate);
            this.splitContainer4.Panel2.Controls.Add(this.dtpFrom);
            this.splitContainer4.Panel2.Controls.Add(this.btnGenerateFiles);
            this.splitContainer4.Panel2.Controls.Add(this.dtpTo);
            this.splitContainer4.Panel2.Controls.Add(this.groupBox8);
            this.splitContainer4.Panel2.Controls.Add(this.label2);
            this.splitContainer4.Panel2.Controls.Add(this.label3);
            this.splitContainer4.Size = new System.Drawing.Size(1238, 628);
            this.splitContainer4.SplitterDistance = 521;
            this.splitContainer4.TabIndex = 0;
            // 
            // gvTaxExempt
            // 
            this.gvTaxExempt.AllowUserToAddRows = false;
            this.gvTaxExempt.AllowUserToDeleteRows = false;
            this.gvTaxExempt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvTaxExempt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvTaxExempt.Location = new System.Drawing.Point(0, 0);
            this.gvTaxExempt.Name = "gvTaxExempt";
            this.gvTaxExempt.Size = new System.Drawing.Size(521, 628);
            this.gvTaxExempt.TabIndex = 0;
            // 
            // btnSendEmail
            // 
            this.btnSendEmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendEmail.Location = new System.Drawing.Point(592, 598);
            this.btnSendEmail.Name = "btnSendEmail";
            this.btnSendEmail.Size = new System.Drawing.Size(112, 23);
            this.btnSendEmail.TabIndex = 5;
            this.btnSendEmail.Text = "Send Email";
            this.btnSendEmail.UseVisualStyleBackColor = true;
            // 
            // btnCalculate
            // 
            this.btnCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCalculate.Location = new System.Drawing.Point(629, 19);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(75, 21);
            this.btnCalculate.TabIndex = 4;
            this.btnCalculate.Text = "Calculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // 
            // dtpFrom
            // 
            this.dtpFrom.Location = new System.Drawing.Point(54, 20);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpFrom.TabIndex = 1;
            // 
            // btnGenerateFiles
            // 
            this.btnGenerateFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerateFiles.Location = new System.Drawing.Point(474, 598);
            this.btnGenerateFiles.Name = "btnGenerateFiles";
            this.btnGenerateFiles.Size = new System.Drawing.Size(112, 23);
            this.btnGenerateFiles.TabIndex = 2;
            this.btnGenerateFiles.Text = "Generate Files";
            this.btnGenerateFiles.UseVisualStyleBackColor = true;
            this.btnGenerateFiles.Click += new System.EventHandler(this.btnGenerateFiles_Click);
            // 
            // dtpTo
            // 
            this.dtpTo.Location = new System.Drawing.Point(309, 20);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(200, 20);
            this.dtpTo.TabIndex = 3;
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.label4);
            this.groupBox8.Controls.Add(this.gvSummary);
            this.groupBox8.Location = new System.Drawing.Point(12, 46);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(692, 546);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Summary";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(317, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Transactions = 20; Total tax = $230.00; Refundable Tax: $200.00";
            // 
            // gvSummary
            // 
            this.gvSummary.AllowUserToAddRows = false;
            this.gvSummary.AllowUserToDeleteRows = false;
            this.gvSummary.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSummary.Location = new System.Drawing.Point(6, 19);
            this.gvSummary.Name = "gvSummary";
            this.gvSummary.ReadOnly = true;
            this.gvSummary.Size = new System.Drawing.Size(680, 504);
            this.gvSummary.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "From:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(280, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "To:";
            // 
            // tpSold
            // 
            this.tpSold.Controls.Add(this.panel1);
            this.tpSold.Location = new System.Drawing.Point(4, 22);
            this.tpSold.Name = "tpSold";
            this.tpSold.Padding = new System.Windows.Forms.Padding(3);
            this.tpSold.Size = new System.Drawing.Size(1244, 634);
            this.tpSold.TabIndex = 3;
            this.tpSold.Text = "eBay Sold";
            this.tpSold.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gvEBaySold);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1238, 628);
            this.panel1.TabIndex = 0;
            // 
            // gvEBaySold
            // 
            this.gvEBaySold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvEBaySold.AutoGenerateColumns = false;
            this.gvEBaySold.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvEBaySold.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.paypalTransactionIDDataGridViewTextBoxColumn,
            this.paypalPaidDateTimeDataGridViewTextBoxColumn,
            this.paypalPaidEmailPdfDataGridViewTextBoxColumn,
            this.eBayItemNumberDataGridViewTextBoxColumn3,
            this.eBaySoldDateTimeDataGridViewTextBoxColumn,
            this.eBayItemNameDataGridViewTextBoxColumn,
            this.eBayListingQualityDataGridViewTextBoxColumn,
            this.eBaySoldQualityDataGridViewTextBoxColumn,
            this.eBayRemainingQualityDataGridViewTextBoxColumn,
            this.eBaySoldPriceDataGridViewTextBoxColumn,
            this.eBayUrlDataGridViewTextBoxColumn2,
            this.eBaySoldEmailPdfDataGridViewTextBoxColumn,
            this.buyerNameDataGridViewTextBoxColumn,
            this.buyerIDDataGridViewTextBoxColumn,
            this.buyerAddress1DataGridViewTextBoxColumn,
            this.buyerAddress2DataGridViewTextBoxColumn,
            this.buyerStateDataGridViewTextBoxColumn,
            this.buyerEmailDataGridViewTextBoxColumn,
            this.buyerNoteDataGridViewTextBoxColumn,
            this.costcoUrlNumberDataGridViewTextBoxColumn3,
            this.costcoUrlDataGridViewTextBoxColumn2,
            this.costcoPriceDataGridViewTextBoxColumn2,
            this.costcoOrderNumberDataGridViewTextBoxColumn,
            this.costcoItemNameDataGridViewTextBoxColumn,
            this.costcoItemNumberDataGridViewTextBoxColumn2,
            this.costcoTrackingNumberDataGridViewTextBoxColumn,
            this.costcoShipDateDataGridViewTextBoxColumn,
            this.costcoTaxExemptPdfDataGridViewTextBoxColumn,
            this.costcoOrderEmailPdfDataGridViewTextBoxColumn});
            this.gvEBaySold.DataSource = this.eBaySoldTransactionsBindingSource;
            this.gvEBaySold.Location = new System.Drawing.Point(3, 30);
            this.gvEBaySold.Name = "gvEBaySold";
            this.gvEBaySold.Size = new System.Drawing.Size(1232, 595);
            this.gvEBaySold.TabIndex = 1;
            // 
            // paypalTransactionIDDataGridViewTextBoxColumn
            // 
            this.paypalTransactionIDDataGridViewTextBoxColumn.DataPropertyName = "PaypalTransactionID";
            this.paypalTransactionIDDataGridViewTextBoxColumn.HeaderText = "PaypalTransactionID";
            this.paypalTransactionIDDataGridViewTextBoxColumn.Name = "paypalTransactionIDDataGridViewTextBoxColumn";
            // 
            // paypalPaidDateTimeDataGridViewTextBoxColumn
            // 
            this.paypalPaidDateTimeDataGridViewTextBoxColumn.DataPropertyName = "PaypalPaidDateTime";
            this.paypalPaidDateTimeDataGridViewTextBoxColumn.HeaderText = "PaypalPaidDateTime";
            this.paypalPaidDateTimeDataGridViewTextBoxColumn.Name = "paypalPaidDateTimeDataGridViewTextBoxColumn";
            // 
            // paypalPaidEmailPdfDataGridViewTextBoxColumn
            // 
            this.paypalPaidEmailPdfDataGridViewTextBoxColumn.DataPropertyName = "PaypalPaidEmailPdf";
            this.paypalPaidEmailPdfDataGridViewTextBoxColumn.HeaderText = "PaypalPaidEmailPdf";
            this.paypalPaidEmailPdfDataGridViewTextBoxColumn.Name = "paypalPaidEmailPdfDataGridViewTextBoxColumn";
            // 
            // eBayItemNumberDataGridViewTextBoxColumn3
            // 
            this.eBayItemNumberDataGridViewTextBoxColumn3.DataPropertyName = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn3.HeaderText = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn3.Name = "eBayItemNumberDataGridViewTextBoxColumn3";
            // 
            // eBaySoldDateTimeDataGridViewTextBoxColumn
            // 
            this.eBaySoldDateTimeDataGridViewTextBoxColumn.DataPropertyName = "eBaySoldDateTime";
            this.eBaySoldDateTimeDataGridViewTextBoxColumn.HeaderText = "eBaySoldDateTime";
            this.eBaySoldDateTimeDataGridViewTextBoxColumn.Name = "eBaySoldDateTimeDataGridViewTextBoxColumn";
            // 
            // eBayItemNameDataGridViewTextBoxColumn
            // 
            this.eBayItemNameDataGridViewTextBoxColumn.DataPropertyName = "eBayItemName";
            this.eBayItemNameDataGridViewTextBoxColumn.HeaderText = "eBayItemName";
            this.eBayItemNameDataGridViewTextBoxColumn.Name = "eBayItemNameDataGridViewTextBoxColumn";
            // 
            // eBayListingQualityDataGridViewTextBoxColumn
            // 
            this.eBayListingQualityDataGridViewTextBoxColumn.DataPropertyName = "eBayListingQuality";
            this.eBayListingQualityDataGridViewTextBoxColumn.HeaderText = "eBayListingQuality";
            this.eBayListingQualityDataGridViewTextBoxColumn.Name = "eBayListingQualityDataGridViewTextBoxColumn";
            // 
            // eBaySoldQualityDataGridViewTextBoxColumn
            // 
            this.eBaySoldQualityDataGridViewTextBoxColumn.DataPropertyName = "eBaySoldQuality";
            this.eBaySoldQualityDataGridViewTextBoxColumn.HeaderText = "eBaySoldQuality";
            this.eBaySoldQualityDataGridViewTextBoxColumn.Name = "eBaySoldQualityDataGridViewTextBoxColumn";
            // 
            // eBayRemainingQualityDataGridViewTextBoxColumn
            // 
            this.eBayRemainingQualityDataGridViewTextBoxColumn.DataPropertyName = "eBayRemainingQuality";
            this.eBayRemainingQualityDataGridViewTextBoxColumn.HeaderText = "eBayRemainingQuality";
            this.eBayRemainingQualityDataGridViewTextBoxColumn.Name = "eBayRemainingQualityDataGridViewTextBoxColumn";
            // 
            // eBaySoldPriceDataGridViewTextBoxColumn
            // 
            this.eBaySoldPriceDataGridViewTextBoxColumn.DataPropertyName = "eBaySoldPrice";
            this.eBaySoldPriceDataGridViewTextBoxColumn.HeaderText = "eBaySoldPrice";
            this.eBaySoldPriceDataGridViewTextBoxColumn.Name = "eBaySoldPriceDataGridViewTextBoxColumn";
            // 
            // eBayUrlDataGridViewTextBoxColumn2
            // 
            this.eBayUrlDataGridViewTextBoxColumn2.DataPropertyName = "eBayUrl";
            this.eBayUrlDataGridViewTextBoxColumn2.HeaderText = "eBayUrl";
            this.eBayUrlDataGridViewTextBoxColumn2.Name = "eBayUrlDataGridViewTextBoxColumn2";
            // 
            // eBaySoldEmailPdfDataGridViewTextBoxColumn
            // 
            this.eBaySoldEmailPdfDataGridViewTextBoxColumn.DataPropertyName = "eBaySoldEmailPdf";
            this.eBaySoldEmailPdfDataGridViewTextBoxColumn.HeaderText = "eBaySoldEmailPdf";
            this.eBaySoldEmailPdfDataGridViewTextBoxColumn.Name = "eBaySoldEmailPdfDataGridViewTextBoxColumn";
            // 
            // buyerNameDataGridViewTextBoxColumn
            // 
            this.buyerNameDataGridViewTextBoxColumn.DataPropertyName = "BuyerName";
            this.buyerNameDataGridViewTextBoxColumn.HeaderText = "BuyerName";
            this.buyerNameDataGridViewTextBoxColumn.Name = "buyerNameDataGridViewTextBoxColumn";
            // 
            // buyerIDDataGridViewTextBoxColumn
            // 
            this.buyerIDDataGridViewTextBoxColumn.DataPropertyName = "BuyerID";
            this.buyerIDDataGridViewTextBoxColumn.HeaderText = "BuyerID";
            this.buyerIDDataGridViewTextBoxColumn.Name = "buyerIDDataGridViewTextBoxColumn";
            // 
            // buyerAddress1DataGridViewTextBoxColumn
            // 
            this.buyerAddress1DataGridViewTextBoxColumn.DataPropertyName = "BuyerAddress1";
            this.buyerAddress1DataGridViewTextBoxColumn.HeaderText = "BuyerAddress1";
            this.buyerAddress1DataGridViewTextBoxColumn.Name = "buyerAddress1DataGridViewTextBoxColumn";
            // 
            // buyerAddress2DataGridViewTextBoxColumn
            // 
            this.buyerAddress2DataGridViewTextBoxColumn.DataPropertyName = "BuyerAddress2";
            this.buyerAddress2DataGridViewTextBoxColumn.HeaderText = "BuyerAddress2";
            this.buyerAddress2DataGridViewTextBoxColumn.Name = "buyerAddress2DataGridViewTextBoxColumn";
            // 
            // buyerStateDataGridViewTextBoxColumn
            // 
            this.buyerStateDataGridViewTextBoxColumn.DataPropertyName = "BuyerState";
            this.buyerStateDataGridViewTextBoxColumn.HeaderText = "BuyerState";
            this.buyerStateDataGridViewTextBoxColumn.Name = "buyerStateDataGridViewTextBoxColumn";
            // 
            // buyerEmailDataGridViewTextBoxColumn
            // 
            this.buyerEmailDataGridViewTextBoxColumn.DataPropertyName = "BuyerEmail";
            this.buyerEmailDataGridViewTextBoxColumn.HeaderText = "BuyerEmail";
            this.buyerEmailDataGridViewTextBoxColumn.Name = "buyerEmailDataGridViewTextBoxColumn";
            // 
            // buyerNoteDataGridViewTextBoxColumn
            // 
            this.buyerNoteDataGridViewTextBoxColumn.DataPropertyName = "BuyerNote";
            this.buyerNoteDataGridViewTextBoxColumn.HeaderText = "BuyerNote";
            this.buyerNoteDataGridViewTextBoxColumn.Name = "buyerNoteDataGridViewTextBoxColumn";
            // 
            // costcoUrlNumberDataGridViewTextBoxColumn3
            // 
            this.costcoUrlNumberDataGridViewTextBoxColumn3.DataPropertyName = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn3.HeaderText = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn3.Name = "costcoUrlNumberDataGridViewTextBoxColumn3";
            // 
            // costcoUrlDataGridViewTextBoxColumn2
            // 
            this.costcoUrlDataGridViewTextBoxColumn2.DataPropertyName = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn2.HeaderText = "CostcoUrl";
            this.costcoUrlDataGridViewTextBoxColumn2.Name = "costcoUrlDataGridViewTextBoxColumn2";
            // 
            // costcoPriceDataGridViewTextBoxColumn2
            // 
            this.costcoPriceDataGridViewTextBoxColumn2.DataPropertyName = "CostcoPrice";
            this.costcoPriceDataGridViewTextBoxColumn2.HeaderText = "CostcoPrice";
            this.costcoPriceDataGridViewTextBoxColumn2.Name = "costcoPriceDataGridViewTextBoxColumn2";
            // 
            // costcoOrderNumberDataGridViewTextBoxColumn
            // 
            this.costcoOrderNumberDataGridViewTextBoxColumn.DataPropertyName = "CostcoOrderNumber";
            this.costcoOrderNumberDataGridViewTextBoxColumn.HeaderText = "CostcoOrderNumber";
            this.costcoOrderNumberDataGridViewTextBoxColumn.Name = "costcoOrderNumberDataGridViewTextBoxColumn";
            // 
            // costcoItemNameDataGridViewTextBoxColumn
            // 
            this.costcoItemNameDataGridViewTextBoxColumn.DataPropertyName = "CostcoItemName";
            this.costcoItemNameDataGridViewTextBoxColumn.HeaderText = "CostcoItemName";
            this.costcoItemNameDataGridViewTextBoxColumn.Name = "costcoItemNameDataGridViewTextBoxColumn";
            // 
            // costcoItemNumberDataGridViewTextBoxColumn2
            // 
            this.costcoItemNumberDataGridViewTextBoxColumn2.DataPropertyName = "CostcoItemNumber";
            this.costcoItemNumberDataGridViewTextBoxColumn2.HeaderText = "CostcoItemNumber";
            this.costcoItemNumberDataGridViewTextBoxColumn2.Name = "costcoItemNumberDataGridViewTextBoxColumn2";
            // 
            // costcoTrackingNumberDataGridViewTextBoxColumn
            // 
            this.costcoTrackingNumberDataGridViewTextBoxColumn.DataPropertyName = "CostcoTrackingNumber";
            this.costcoTrackingNumberDataGridViewTextBoxColumn.HeaderText = "CostcoTrackingNumber";
            this.costcoTrackingNumberDataGridViewTextBoxColumn.Name = "costcoTrackingNumberDataGridViewTextBoxColumn";
            // 
            // costcoShipDateDataGridViewTextBoxColumn
            // 
            this.costcoShipDateDataGridViewTextBoxColumn.DataPropertyName = "CostcoShipDate";
            this.costcoShipDateDataGridViewTextBoxColumn.HeaderText = "CostcoShipDate";
            this.costcoShipDateDataGridViewTextBoxColumn.Name = "costcoShipDateDataGridViewTextBoxColumn";
            // 
            // costcoTaxExemptPdfDataGridViewTextBoxColumn
            // 
            this.costcoTaxExemptPdfDataGridViewTextBoxColumn.DataPropertyName = "CostcoTaxExemptPdf";
            this.costcoTaxExemptPdfDataGridViewTextBoxColumn.HeaderText = "CostcoTaxExemptPdf";
            this.costcoTaxExemptPdfDataGridViewTextBoxColumn.Name = "costcoTaxExemptPdfDataGridViewTextBoxColumn";
            // 
            // costcoOrderEmailPdfDataGridViewTextBoxColumn
            // 
            this.costcoOrderEmailPdfDataGridViewTextBoxColumn.DataPropertyName = "CostcoOrderEmailPdf";
            this.costcoOrderEmailPdfDataGridViewTextBoxColumn.HeaderText = "CostcoOrderEmailPdf";
            this.costcoOrderEmailPdfDataGridViewTextBoxColumn.Name = "costcoOrderEmailPdfDataGridViewTextBoxColumn";
            // 
            // eBaySoldTransactionsBindingSource
            // 
            this.eBaySoldTransactionsBindingSource.DataMember = "eBay_SoldTransactions";
            this.eBaySoldTransactionsBindingSource.DataSource = this.dsEBaySold;
            // 
            // dsEBaySold
            // 
            this.dsEBaySold.DataSetName = "dsEBaySold";
            this.dsEBaySold.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // tpCurrentListing
            // 
            this.tpCurrentListing.Controls.Add(this.panel3);
            this.tpCurrentListing.Location = new System.Drawing.Point(4, 22);
            this.tpCurrentListing.Name = "tpCurrentListing";
            this.tpCurrentListing.Size = new System.Drawing.Size(1244, 634);
            this.tpCurrentListing.TabIndex = 2;
            this.tpCurrentListing.Text = "eBay Current Listing";
            this.tpCurrentListing.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.groupBox6);
            this.panel3.Location = new System.Drawing.Point(7, 6);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1246, 625);
            this.panel3.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.btnReloadCurrentListing);
            this.groupBox6.Controls.Add(this.gvCurrentListing);
            this.groupBox6.Controls.Add(this.btnListingModify);
            this.groupBox6.Controls.Add(this.btnListingDelete);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(1240, 619);
            this.groupBox6.TabIndex = 0;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Current Listing";
            // 
            // btnReloadCurrentListing
            // 
            this.btnReloadCurrentListing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadCurrentListing.Location = new System.Drawing.Point(1159, 590);
            this.btnReloadCurrentListing.Name = "btnReloadCurrentListing";
            this.btnReloadCurrentListing.Size = new System.Drawing.Size(75, 23);
            this.btnReloadCurrentListing.TabIndex = 4;
            this.btnReloadCurrentListing.Text = "Reload";
            this.btnReloadCurrentListing.UseVisualStyleBackColor = true;
            this.btnReloadCurrentListing.Click += new System.EventHandler(this.btnReloadCurrentListing_Click);
            // 
            // gvCurrentListing
            // 
            this.gvCurrentListing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvCurrentListing.AutoGenerateColumns = false;
            this.gvCurrentListing.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvCurrentListing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn2,
            this.nameDataGridViewTextBoxColumn2,
            this.eBayListingNameDataGridViewTextBoxColumn,
            this.eBayCategoryIDDataGridViewTextBoxColumn1,
            this.eBayItemNumberDataGridViewTextBoxColumn,
            this.eBayListingPriceDataGridViewTextBoxColumn1,
            this.eBayDescriptionDataGridViewTextBoxColumn,
            this.eBayEndTimeDataGridViewTextBoxColumn,
            this.eBayUrlDataGridViewTextBoxColumn,
            this.costcoUrlNumberDataGridViewTextBoxColumn,
            this.costcoItemNumberDataGridViewTextBoxColumn,
            this.costcoUrlDataGridViewTextBoxColumn,
            this.costcoPriceDataGridViewTextBoxColumn,
            this.imageLinkDataGridViewTextBoxColumn2,
            this.SelectListing});
            this.gvCurrentListing.DataSource = this.eBayCurrentListingsBindingSource;
            this.gvCurrentListing.Location = new System.Drawing.Point(6, 19);
            this.gvCurrentListing.Name = "gvCurrentListing";
            this.gvCurrentListing.Size = new System.Drawing.Size(1228, 565);
            this.gvCurrentListing.TabIndex = 2;
            this.gvCurrentListing.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvCurrentListing_CellClick);
            // 
            // iDDataGridViewTextBoxColumn2
            // 
            this.iDDataGridViewTextBoxColumn2.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn2.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn2.Name = "iDDataGridViewTextBoxColumn2";
            this.iDDataGridViewTextBoxColumn2.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn2.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn2
            // 
            this.nameDataGridViewTextBoxColumn2.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn2.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn2.Name = "nameDataGridViewTextBoxColumn2";
            this.nameDataGridViewTextBoxColumn2.Visible = false;
            // 
            // eBayListingNameDataGridViewTextBoxColumn
            // 
            this.eBayListingNameDataGridViewTextBoxColumn.DataPropertyName = "eBayListingName";
            this.eBayListingNameDataGridViewTextBoxColumn.HeaderText = "eBayListingName";
            this.eBayListingNameDataGridViewTextBoxColumn.Name = "eBayListingNameDataGridViewTextBoxColumn";
            // 
            // eBayCategoryIDDataGridViewTextBoxColumn1
            // 
            this.eBayCategoryIDDataGridViewTextBoxColumn1.DataPropertyName = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn1.HeaderText = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn1.Name = "eBayCategoryIDDataGridViewTextBoxColumn1";
            // 
            // eBayItemNumberDataGridViewTextBoxColumn
            // 
            this.eBayItemNumberDataGridViewTextBoxColumn.DataPropertyName = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn.HeaderText = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn.Name = "eBayItemNumberDataGridViewTextBoxColumn";
            // 
            // eBayListingPriceDataGridViewTextBoxColumn1
            // 
            this.eBayListingPriceDataGridViewTextBoxColumn1.DataPropertyName = "eBayListingPrice";
            this.eBayListingPriceDataGridViewTextBoxColumn1.HeaderText = "eBayListingPrice";
            this.eBayListingPriceDataGridViewTextBoxColumn1.Name = "eBayListingPriceDataGridViewTextBoxColumn1";
            // 
            // eBayDescriptionDataGridViewTextBoxColumn
            // 
            this.eBayDescriptionDataGridViewTextBoxColumn.DataPropertyName = "eBayDescription";
            this.eBayDescriptionDataGridViewTextBoxColumn.HeaderText = "eBayDescription";
            this.eBayDescriptionDataGridViewTextBoxColumn.Name = "eBayDescriptionDataGridViewTextBoxColumn";
            // 
            // eBayEndTimeDataGridViewTextBoxColumn
            // 
            this.eBayEndTimeDataGridViewTextBoxColumn.DataPropertyName = "eBayEndTime";
            this.eBayEndTimeDataGridViewTextBoxColumn.HeaderText = "eBayEndTime";
            this.eBayEndTimeDataGridViewTextBoxColumn.Name = "eBayEndTimeDataGridViewTextBoxColumn";
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
            this.costcoUrlNumberDataGridViewTextBoxColumn.Visible = false;
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
            // costcoPriceDataGridViewTextBoxColumn
            // 
            this.costcoPriceDataGridViewTextBoxColumn.DataPropertyName = "CostcoPrice";
            this.costcoPriceDataGridViewTextBoxColumn.HeaderText = "CostcoPrice";
            this.costcoPriceDataGridViewTextBoxColumn.Name = "costcoPriceDataGridViewTextBoxColumn";
            // 
            // imageLinkDataGridViewTextBoxColumn2
            // 
            this.imageLinkDataGridViewTextBoxColumn2.DataPropertyName = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn2.HeaderText = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn2.Name = "imageLinkDataGridViewTextBoxColumn2";
            this.imageLinkDataGridViewTextBoxColumn2.Visible = false;
            // 
            // SelectListing
            // 
            this.SelectListing.HeaderText = "Select";
            this.SelectListing.Name = "SelectListing";
            // 
            // eBayCurrentListingsBindingSource
            // 
            this.eBayCurrentListingsBindingSource.DataMember = "eBay_CurrentListings";
            this.eBayCurrentListingsBindingSource.DataSource = this.dseBayCurrentListings;
            // 
            // dseBayCurrentListings
            // 
            this.dseBayCurrentListings.DataSetName = "dseBayCurrentListings";
            this.dseBayCurrentListings.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // btnListingModify
            // 
            this.btnListingModify.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnListingModify.Location = new System.Drawing.Point(87, 590);
            this.btnListingModify.Name = "btnListingModify";
            this.btnListingModify.Size = new System.Drawing.Size(75, 23);
            this.btnListingModify.TabIndex = 1;
            this.btnListingModify.Text = "Modify";
            this.btnListingModify.UseVisualStyleBackColor = true;
            this.btnListingModify.Click += new System.EventHandler(this.btnListingModify_Click);
            // 
            // btnListingDelete
            // 
            this.btnListingDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnListingDelete.Location = new System.Drawing.Point(6, 590);
            this.btnListingDelete.Name = "btnListingDelete";
            this.btnListingDelete.Size = new System.Drawing.Size(75, 23);
            this.btnListingDelete.TabIndex = 0;
            this.btnListingDelete.Text = "Delete";
            this.btnListingDelete.UseVisualStyleBackColor = true;
            this.btnListingDelete.Click += new System.EventHandler(this.btnListingDelete_Click);
            // 
            // tpCostco
            // 
            this.tpCostco.Controls.Add(this.panel2);
            this.tpCostco.Location = new System.Drawing.Point(4, 22);
            this.tpCostco.Name = "tpCostco";
            this.tpCostco.Padding = new System.Windows.Forms.Padding(3);
            this.tpCostco.Size = new System.Drawing.Size(1244, 634);
            this.tpCostco.TabIndex = 0;
            this.tpCostco.Text = "Costco Products";
            this.tpCostco.UseVisualStyleBackColor = true;
            this.tpCostco.Enter += new System.EventHandler(this.tpCostco_Enter);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1238, 628);
            this.panel2.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.btnCostcoCategory);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.webBrowser1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel2.Controls.Add(this.btnCrawl);
            this.splitContainer1.Size = new System.Drawing.Size(1238, 628);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkAll);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.lvCategories);
            this.groupBox1.Location = new System.Drawing.Point(3, 32);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(299, 593);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Categories";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(13, 20);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(37, 17);
            this.chkAll.TabIndex = 7;
            this.chkAll.Text = "All";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(6, 564);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // lvCategories
            // 
            this.lvCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvCategories.CheckBoxes = true;
            this.lvCategories.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Select,
            this.Category_1,
            this.Category_2,
            this.Category_3,
            this.Category_4,
            this.Category_5,
            this.Category_6,
            this.Category_7,
            this.Category_8});
            this.lvCategories.Location = new System.Drawing.Point(6, 45);
            this.lvCategories.Name = "lvCategories";
            this.lvCategories.Size = new System.Drawing.Size(287, 513);
            this.lvCategories.TabIndex = 0;
            this.lvCategories.UseCompatibleStateImageBehavior = false;
            this.lvCategories.View = System.Windows.Forms.View.Details;
            this.lvCategories.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvCategories_ItemChecked);
            // 
            // Select
            // 
            this.Select.Text = "";
            this.Select.Width = 25;
            // 
            // Category_1
            // 
            this.Category_1.Text = "Category1";
            this.Category_1.Width = 100;
            // 
            // Category_2
            // 
            this.Category_2.Text = "Category2";
            this.Category_2.Width = 100;
            // 
            // Category_3
            // 
            this.Category_3.Text = "Category3";
            this.Category_3.Width = 100;
            // 
            // Category_4
            // 
            this.Category_4.Text = "Category4";
            this.Category_4.Width = 100;
            // 
            // Category_5
            // 
            this.Category_5.Text = "Category5";
            this.Category_5.Width = 100;
            // 
            // Category_6
            // 
            this.Category_6.Text = "Category6";
            this.Category_6.Width = 100;
            // 
            // Category_7
            // 
            this.Category_7.Text = "Category7";
            this.Category_7.Width = 100;
            // 
            // Category_8
            // 
            this.Category_8.Text = "Category8";
            this.Category_8.Width = 100;
            // 
            // btnCostcoCategory
            // 
            this.btnCostcoCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCostcoCategory.Location = new System.Drawing.Point(189, 3);
            this.btnCostcoCategory.Name = "btnCostcoCategory";
            this.btnCostcoCategory.Size = new System.Drawing.Size(113, 23);
            this.btnCostcoCategory.TabIndex = 5;
            this.btnCostcoCategory.Text = "Costco Categories";
            this.btnCostcoCategory.UseVisualStyleBackColor = true;
            this.btnCostcoCategory.Click += new System.EventHandler(this.btnCostcoCategory_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(125, 6);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(250, 20);
            this.webBrowser1.TabIndex = 5;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.button9);
            this.groupBox2.Controls.Add(this.button8);
            this.groupBox2.Controls.Add(this.txtFilter);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.btnAddPending);
            this.groupBox2.Controls.Add(this.btnRefreshProducts);
            this.groupBox2.Controls.Add(this.gvProducts);
            this.groupBox2.Location = new System.Drawing.Point(3, 32);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(923, 593);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Products";
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button9.Location = new System.Drawing.Point(211, 564);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(113, 23);
            this.button9.TabIndex = 8;
            this.button9.Text = "Discontinued Items";
            this.button9.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button8.Location = new System.Drawing.Point(122, 564);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(83, 23);
            this.button8.TabIndex = 7;
            this.button8.Text = "New Items";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(43, 19);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(793, 20);
            this.txtFilter.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Filter";
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button3.Location = new System.Drawing.Point(6, 564);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Price Change Items";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // btnAddPending
            // 
            this.btnAddPending.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddPending.Location = new System.Drawing.Point(821, 564);
            this.btnAddPending.Name = "btnAddPending";
            this.btnAddPending.Size = new System.Drawing.Size(96, 23);
            this.btnAddPending.TabIndex = 2;
            this.btnAddPending.Text = "Add to Pending";
            this.btnAddPending.UseVisualStyleBackColor = true;
            this.btnAddPending.Click += new System.EventHandler(this.btnAddPending_Click);
            // 
            // btnRefreshProducts
            // 
            this.btnRefreshProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshProducts.Location = new System.Drawing.Point(842, 17);
            this.btnRefreshProducts.Name = "btnRefreshProducts";
            this.btnRefreshProducts.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshProducts.TabIndex = 1;
            this.btnRefreshProducts.Text = "Refresh ";
            this.btnRefreshProducts.UseVisualStyleBackColor = true;
            this.btnRefreshProducts.Click += new System.EventHandler(this.btnRefreshProducts_Click);
            // 
            // gvProducts
            // 
            this.gvProducts.AllowUserToAddRows = false;
            this.gvProducts.AllowUserToDeleteRows = false;
            this.gvProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Check,
            this.iDDataGridViewTextBoxColumn,
            this.nameDataGridViewTextBoxColumn,
            this.Image,
            this.ImageLink,
            this.UrlNumber,
            this.priceDataGridViewTextBoxColumn,
            this.shippingDataGridViewTextBoxColumn,
            this.Discount,
            this.Limit,
            this.detailsDataGridViewTextBoxColumn,
            this.specificationDataGridViewTextBoxColumn,
            this.urlDataGridViewTextBoxColumn,
            this.itemNumberDataGridViewTextBoxColumn});
            this.gvProducts.Location = new System.Drawing.Point(6, 45);
            this.gvProducts.Name = "gvProducts";
            this.gvProducts.RowTemplate.Height = 100;
            this.gvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvProducts.Size = new System.Drawing.Size(911, 513);
            this.gvProducts.TabIndex = 0;
            this.gvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvProducts_CellClick);
            this.gvProducts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvProducts_CellFormatting);
            this.gvProducts.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.gvProducts_RowsAdded);
            // 
            // Check
            // 
            this.Check.HeaderText = "";
            this.Check.Name = "Check";
            this.Check.Width = 20;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            this.iDDataGridViewTextBoxColumn.Width = 40;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.nameDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.nameDataGridViewTextBoxColumn.Width = 300;
            // 
            // Image
            // 
            this.Image.HeaderText = "Image";
            this.Image.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.Image.Name = "Image";
            this.Image.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Image.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ImageLink
            // 
            this.ImageLink.DataPropertyName = "ImageLink";
            this.ImageLink.HeaderText = "ImageLink";
            this.ImageLink.Name = "ImageLink";
            this.ImageLink.Visible = false;
            // 
            // UrlNumber
            // 
            this.UrlNumber.DataPropertyName = "UrlNumber";
            this.UrlNumber.HeaderText = "UrlNumber";
            this.UrlNumber.Name = "UrlNumber";
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
            // Discount
            // 
            this.Discount.DataPropertyName = "Discount";
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Discount.DefaultCellStyle = dataGridViewCellStyle8;
            this.Discount.HeaderText = "Discount";
            this.Discount.Name = "Discount";
            // 
            // Limit
            // 
            this.Limit.DataPropertyName = "Limit";
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Limit.DefaultCellStyle = dataGridViewCellStyle9;
            this.Limit.HeaderText = "Limit";
            this.Limit.Name = "Limit";
            this.Limit.Width = 80;
            // 
            // detailsDataGridViewTextBoxColumn
            // 
            this.detailsDataGridViewTextBoxColumn.DataPropertyName = "Details";
            this.detailsDataGridViewTextBoxColumn.HeaderText = "Details";
            this.detailsDataGridViewTextBoxColumn.Name = "detailsDataGridViewTextBoxColumn";
            this.detailsDataGridViewTextBoxColumn.Visible = false;
            // 
            // specificationDataGridViewTextBoxColumn
            // 
            this.specificationDataGridViewTextBoxColumn.DataPropertyName = "Specification";
            this.specificationDataGridViewTextBoxColumn.HeaderText = "Specification";
            this.specificationDataGridViewTextBoxColumn.Name = "specificationDataGridViewTextBoxColumn";
            this.specificationDataGridViewTextBoxColumn.Visible = false;
            // 
            // urlDataGridViewTextBoxColumn
            // 
            this.urlDataGridViewTextBoxColumn.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn.Name = "urlDataGridViewTextBoxColumn";
            this.urlDataGridViewTextBoxColumn.Visible = false;
            // 
            // itemNumberDataGridViewTextBoxColumn
            // 
            this.itemNumberDataGridViewTextBoxColumn.DataPropertyName = "ItemNumber";
            this.itemNumberDataGridViewTextBoxColumn.HeaderText = "ItemNumber";
            this.itemNumberDataGridViewTextBoxColumn.Name = "itemNumberDataGridViewTextBoxColumn";
            this.itemNumberDataGridViewTextBoxColumn.Visible = false;
            // 
            // btnCrawl
            // 
            this.btnCrawl.Location = new System.Drawing.Point(3, 3);
            this.btnCrawl.Name = "btnCrawl";
            this.btnCrawl.Size = new System.Drawing.Size(96, 23);
            this.btnCrawl.TabIndex = 4;
            this.btnCrawl.Text = "Start crawling";
            this.btnCrawl.UseVisualStyleBackColor = true;
            this.btnCrawl.Click += new System.EventHandler(this.btnCrawl_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tpDashboard);
            this.tabControl1.Controls.Add(this.tpCostco);
            this.tabControl1.Controls.Add(this.tpToAdd);
            this.tabControl1.Controls.Add(this.tpCurrentListing);
            this.tabControl1.Controls.Add(this.tpEBayToModify);
            this.tabControl1.Controls.Add(this.tpEbayToDelete);
            this.tabControl1.Controls.Add(this.tpSold);
            this.tabControl1.Controls.Add(this.tpTax);
            this.tabControl1.Controls.Add(this.tpSaleTax);
            this.tabControl1.Controls.Add(this.tpIncomeTax);
            this.tabControl1.Controls.Add(this.tpResearch);
            this.tabControl1.Controls.Add(this.tpMaintenance);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1252, 660);
            this.tabControl1.TabIndex = 0;
            // 
            // tpDashboard
            // 
            this.tpDashboard.Controls.Add(this.panel6);
            this.tpDashboard.Location = new System.Drawing.Point(4, 22);
            this.tpDashboard.Name = "tpDashboard";
            this.tpDashboard.Padding = new System.Windows.Forms.Padding(3);
            this.tpDashboard.Size = new System.Drawing.Size(1244, 634);
            this.tpDashboard.TabIndex = 12;
            this.tpDashboard.Text = "Dashboard";
            this.tpDashboard.UseVisualStyleBackColor = true;
            this.tpDashboard.Enter += new System.EventHandler(this.tpDashboard_Enter);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.tableLayoutPanel4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1238, 628);
            this.panel6.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1238, 628);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel5.Controls.Add(this.llEBayOptions, 2, 5);
            this.tableLayoutPanel5.Controls.Add(this.llEBayPriceDown, 2, 4);
            this.tableLayoutPanel5.Controls.Add(this.llEBayPriceUp, 2, 3);
            this.tableLayoutPanel5.Controls.Add(this.label96, 1, 5);
            this.tableLayoutPanel5.Controls.Add(this.label95, 0, 5);
            this.tableLayoutPanel5.Controls.Add(this.label93, 1, 4);
            this.tableLayoutPanel5.Controls.Add(this.label92, 0, 4);
            this.tableLayoutPanel5.Controls.Add(this.label90, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.label89, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.label87, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.label86, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.label82, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.label70, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.llEBayDiscontinue, 2, 2);
            this.tableLayoutPanel5.Controls.Add(this.label88, 1, 7);
            this.tableLayoutPanel5.Controls.Add(this.label91, 1, 8);
            this.tableLayoutPanel5.Controls.Add(this.linkLabel2, 2, 8);
            this.tableLayoutPanel5.Controls.Add(this.label94, 1, 9);
            this.tableLayoutPanel5.Controls.Add(this.linkLabel3, 2, 9);
            this.tableLayoutPanel5.Controls.Add(this.btnChangeForEBayListings, 2, 1);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 11;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(613, 308);
            this.tableLayoutPanel5.TabIndex = 0;
            // 
            // llEBayOptions
            // 
            this.llEBayOptions.AutoSize = true;
            this.llEBayOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llEBayOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llEBayOptions.Location = new System.Drawing.Point(316, 105);
            this.llEBayOptions.Margin = new System.Windows.Forms.Padding(0);
            this.llEBayOptions.Name = "llEBayOptions";
            this.llEBayOptions.Size = new System.Drawing.Size(297, 20);
            this.llEBayOptions.TabIndex = 20;
            this.llEBayOptions.TabStop = true;
            this.llEBayOptions.Text = "0";
            this.llEBayOptions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llEBayOptions.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEBayOptions_LinkClicked);
            // 
            // llEBayPriceDown
            // 
            this.llEBayPriceDown.AutoSize = true;
            this.llEBayPriceDown.BackColor = System.Drawing.Color.WhiteSmoke;
            this.llEBayPriceDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llEBayPriceDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llEBayPriceDown.Location = new System.Drawing.Point(316, 85);
            this.llEBayPriceDown.Margin = new System.Windows.Forms.Padding(0);
            this.llEBayPriceDown.Name = "llEBayPriceDown";
            this.llEBayPriceDown.Size = new System.Drawing.Size(297, 20);
            this.llEBayPriceDown.TabIndex = 19;
            this.llEBayPriceDown.TabStop = true;
            this.llEBayPriceDown.Text = "0";
            this.llEBayPriceDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llEBayPriceDown.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEBayPriceDown_LinkClicked);
            // 
            // llEBayPriceUp
            // 
            this.llEBayPriceUp.AutoSize = true;
            this.llEBayPriceUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llEBayPriceUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llEBayPriceUp.Location = new System.Drawing.Point(316, 65);
            this.llEBayPriceUp.Margin = new System.Windows.Forms.Padding(0);
            this.llEBayPriceUp.Name = "llEBayPriceUp";
            this.llEBayPriceUp.Size = new System.Drawing.Size(297, 20);
            this.llEBayPriceUp.TabIndex = 18;
            this.llEBayPriceUp.TabStop = true;
            this.llEBayPriceUp.Text = "0";
            this.llEBayPriceUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llEBayPriceUp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEBayPriceUp_LinkClicked);
            // 
            // label96
            // 
            this.label96.AutoSize = true;
            this.label96.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label96.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label96.Location = new System.Drawing.Point(20, 105);
            this.label96.Margin = new System.Windows.Forms.Padding(0);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(296, 20);
            this.label96.TabIndex = 13;
            this.label96.Text = "Options changed";
            this.label96.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label95
            // 
            this.label95.AutoSize = true;
            this.label95.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label95.Location = new System.Drawing.Point(3, 105);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(14, 20);
            this.label95.TabIndex = 12;
            this.label95.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label93
            // 
            this.label93.AutoSize = true;
            this.label93.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label93.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label93.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label93.Location = new System.Drawing.Point(20, 85);
            this.label93.Margin = new System.Windows.Forms.Padding(0);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(296, 20);
            this.label93.TabIndex = 10;
            this.label93.Text = "Price down";
            this.label93.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label92.Location = new System.Drawing.Point(3, 85);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(14, 20);
            this.label92.TabIndex = 9;
            this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label90.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label90.Location = new System.Drawing.Point(20, 65);
            this.label90.Margin = new System.Windows.Forms.Padding(0);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(296, 20);
            this.label90.TabIndex = 7;
            this.label90.Text = "Price up";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label89
            // 
            this.label89.AutoSize = true;
            this.label89.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label89.Location = new System.Drawing.Point(3, 65);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(14, 20);
            this.label89.TabIndex = 6;
            this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label87.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label87.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label87.Location = new System.Drawing.Point(20, 45);
            this.label87.Margin = new System.Windows.Forms.Padding(0);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(296, 20);
            this.label87.TabIndex = 4;
            this.label87.Text = "Discoutinue ";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label86
            // 
            this.label86.AutoSize = true;
            this.label86.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label86.Location = new System.Drawing.Point(3, 45);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(14, 20);
            this.label86.TabIndex = 3;
            this.label86.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label82
            // 
            this.label82.AutoSize = true;
            this.label82.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label82.Location = new System.Drawing.Point(3, 20);
            this.label82.Name = "label82";
            this.label82.Size = new System.Drawing.Size(14, 25);
            this.label82.TabIndex = 1;
            this.label82.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.Location = new System.Drawing.Point(20, 20);
            this.label70.Margin = new System.Windows.Forms.Padding(0);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(296, 25);
            this.label70.TabIndex = 0;
            this.label70.Text = "Changes for eBay listings";
            this.label70.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // llEBayDiscontinue
            // 
            this.llEBayDiscontinue.AutoSize = true;
            this.llEBayDiscontinue.BackColor = System.Drawing.Color.WhiteSmoke;
            this.llEBayDiscontinue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.llEBayDiscontinue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.llEBayDiscontinue.Location = new System.Drawing.Point(316, 45);
            this.llEBayDiscontinue.Margin = new System.Windows.Forms.Padding(0);
            this.llEBayDiscontinue.Name = "llEBayDiscontinue";
            this.llEBayDiscontinue.Size = new System.Drawing.Size(297, 20);
            this.llEBayDiscontinue.TabIndex = 14;
            this.llEBayDiscontinue.TabStop = true;
            this.llEBayDiscontinue.Text = "0";
            this.llEBayDiscontinue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.llEBayDiscontinue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llEBayDiscontinue_LinkClicked);
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label88.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label88.Location = new System.Drawing.Point(20, 145);
            this.label88.Margin = new System.Windows.Forms.Padding(0);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(296, 20);
            this.label88.TabIndex = 21;
            this.label88.Text = "Changes for Costco inventory";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label91
            // 
            this.label91.AutoSize = true;
            this.label91.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label91.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label91.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label91.Location = new System.Drawing.Point(20, 165);
            this.label91.Margin = new System.Windows.Forms.Padding(0);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(296, 20);
            this.label91.TabIndex = 22;
            this.label91.Text = "Price down";
            this.label91.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.linkLabel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(316, 165);
            this.linkLabel2.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(297, 20);
            this.linkLabel2.TabIndex = 23;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "0";
            this.linkLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label94.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label94.Location = new System.Drawing.Point(20, 185);
            this.label94.Margin = new System.Windows.Forms.Padding(0);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(296, 20);
            this.label94.TabIndex = 24;
            this.label94.Text = "New products";
            this.label94.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.linkLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.Location = new System.Drawing.Point(316, 185);
            this.linkLabel3.Margin = new System.Windows.Forms.Padding(0);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(297, 20);
            this.linkLabel3.TabIndex = 25;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "0";
            this.linkLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnChangeForEBayListings
            // 
            this.btnChangeForEBayListings.Location = new System.Drawing.Point(316, 20);
            this.btnChangeForEBayListings.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeForEBayListings.Name = "btnChangeForEBayListings";
            this.btnChangeForEBayListings.Size = new System.Drawing.Size(75, 25);
            this.btnChangeForEBayListings.TabIndex = 26;
            this.btnChangeForEBayListings.Text = "Refresh";
            this.btnChangeForEBayListings.UseVisualStyleBackColor = true;
            this.btnChangeForEBayListings.Click += new System.EventHandler(this.btnChangeForEBayListings_Click);
            // 
            // tpToAdd
            // 
            this.tpToAdd.Controls.Add(this.groupBox3);
            this.tpToAdd.Location = new System.Drawing.Point(4, 22);
            this.tpToAdd.Name = "tpToAdd";
            this.tpToAdd.Padding = new System.Windows.Forms.Padding(3);
            this.tpToAdd.Size = new System.Drawing.Size(1244, 634);
            this.tpToAdd.TabIndex = 7;
            this.tpToAdd.Text = "To Add";
            this.tpToAdd.UseVisualStyleBackColor = true;
            this.tpToAdd.Enter += new System.EventHandler(this.tpToAdd_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.gvAdd);
            this.groupBox3.Controls.Add(this.btnSave);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnUpload);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1238, 628);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "To be Added Items";
            // 
            // gvAdd
            // 
            this.gvAdd.AllowUserToAddRows = false;
            this.gvAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvAdd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvAdd.Location = new System.Drawing.Point(6, 39);
            this.gvAdd.Name = "gvAdd";
            this.gvAdd.Size = new System.Drawing.Size(1226, 583);
            this.gvAdd.TabIndex = 6;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(1015, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Update";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click_1);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(934, 10);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 5;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click_1);
            // 
            // btnUpload
            // 
            this.btnUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpload.Location = new System.Drawing.Point(1157, 10);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 2;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // tpEBayToModify
            // 
            this.tpEBayToModify.Controls.Add(this.groupBox4);
            this.tpEBayToModify.Location = new System.Drawing.Point(4, 22);
            this.tpEBayToModify.Name = "tpEBayToModify";
            this.tpEBayToModify.Padding = new System.Windows.Forms.Padding(3);
            this.tpEBayToModify.Size = new System.Drawing.Size(1244, 634);
            this.tpEBayToModify.TabIndex = 8;
            this.tpEBayToModify.Text = "eBay To Modify";
            this.tpEBayToModify.UseVisualStyleBackColor = true;
            this.tpEBayToModify.Enter += new System.EventHandler(this.tpEBayToModify_Enter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.gvChange);
            this.groupBox4.Controls.Add(this.btnToChangeUpdate);
            this.groupBox4.Controls.Add(this.btnToChangeDelete);
            this.groupBox4.Controls.Add(this.btnToChangeUpload);
            this.groupBox4.Controls.Add(this.gvToChange);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(1238, 628);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "To be Modified Items";
            // 
            // btnToChangeUpdate
            // 
            this.btnToChangeUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToChangeUpdate.Location = new System.Drawing.Point(1076, 9);
            this.btnToChangeUpdate.Name = "btnToChangeUpdate";
            this.btnToChangeUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnToChangeUpdate.TabIndex = 6;
            this.btnToChangeUpdate.Text = "Update";
            this.btnToChangeUpdate.UseVisualStyleBackColor = true;
            // 
            // btnToChangeDelete
            // 
            this.btnToChangeDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToChangeDelete.Location = new System.Drawing.Point(995, 9);
            this.btnToChangeDelete.Name = "btnToChangeDelete";
            this.btnToChangeDelete.Size = new System.Drawing.Size(75, 23);
            this.btnToChangeDelete.TabIndex = 6;
            this.btnToChangeDelete.Text = "Delete";
            this.btnToChangeDelete.UseVisualStyleBackColor = true;
            // 
            // btnToChangeUpload
            // 
            this.btnToChangeUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToChangeUpload.Location = new System.Drawing.Point(1157, 9);
            this.btnToChangeUpload.Name = "btnToChangeUpload";
            this.btnToChangeUpload.Size = new System.Drawing.Size(75, 23);
            this.btnToChangeUpload.TabIndex = 3;
            this.btnToChangeUpload.Text = "Upload";
            this.btnToChangeUpload.UseVisualStyleBackColor = true;
            // 
            // gvToChange
            // 
            this.gvToChange.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvToChange.AutoGenerateColumns = false;
            this.gvToChange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvToChange.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn4,
            this.nameDataGridViewTextBoxColumn4,
            this.costcoUrlNumberDataGridViewTextBoxColumn2,
            this.eBayItemNumberDataGridViewTextBoxColumn2,
            this.eBayOldListingPriceDataGridViewTextBoxColumn,
            this.eBayNewListingPriceDataGridViewTextBoxColumn,
            this.eBayReferencePriceDataGridViewTextBoxColumn1,
            this.costcoOldPriceDataGridViewTextBoxColumn,
            this.costcoNewPriceDataGridViewTextBoxColumn,
            this.priceChangeDataGridViewTextBoxColumn,
            this.categoryDataGridViewTextBoxColumn1,
            this.shippingDataGridViewTextBoxColumn2,
            this.limitDataGridViewTextBoxColumn1,
            this.discountDataGridViewTextBoxColumn1,
            this.detailsDataGridViewTextBoxColumn2,
            this.specificationDataGridViewTextBoxColumn2,
            this.imageLinkDataGridViewTextBoxColumn4,
            this.numberOfImageDataGridViewTextBoxColumn1,
            this.urlDataGridViewTextBoxColumn2,
            this.eBayCategoryIDDataGridViewTextBoxColumn3,
            this.descriptionImageWidthDataGridViewTextBoxColumn1,
            this.descriptionImageHeightDataGridViewTextBoxColumn1});
            this.gvToChange.DataSource = this.eBayToChangeBindingSource;
            this.gvToChange.Location = new System.Drawing.Point(6, 344);
            this.gvToChange.Name = "gvToChange";
            this.gvToChange.Size = new System.Drawing.Size(1226, 278);
            this.gvToChange.TabIndex = 0;
            // 
            // iDDataGridViewTextBoxColumn4
            // 
            this.iDDataGridViewTextBoxColumn4.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn4.HeaderText = "ID";
            this.iDDataGridViewTextBoxColumn4.Name = "iDDataGridViewTextBoxColumn4";
            this.iDDataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // nameDataGridViewTextBoxColumn4
            // 
            this.nameDataGridViewTextBoxColumn4.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn4.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn4.Name = "nameDataGridViewTextBoxColumn4";
            // 
            // costcoUrlNumberDataGridViewTextBoxColumn2
            // 
            this.costcoUrlNumberDataGridViewTextBoxColumn2.DataPropertyName = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn2.HeaderText = "CostcoUrlNumber";
            this.costcoUrlNumberDataGridViewTextBoxColumn2.Name = "costcoUrlNumberDataGridViewTextBoxColumn2";
            // 
            // eBayItemNumberDataGridViewTextBoxColumn2
            // 
            this.eBayItemNumberDataGridViewTextBoxColumn2.DataPropertyName = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn2.HeaderText = "eBayItemNumber";
            this.eBayItemNumberDataGridViewTextBoxColumn2.Name = "eBayItemNumberDataGridViewTextBoxColumn2";
            // 
            // eBayOldListingPriceDataGridViewTextBoxColumn
            // 
            this.eBayOldListingPriceDataGridViewTextBoxColumn.DataPropertyName = "eBayOldListingPrice";
            this.eBayOldListingPriceDataGridViewTextBoxColumn.HeaderText = "eBayOldListingPrice";
            this.eBayOldListingPriceDataGridViewTextBoxColumn.Name = "eBayOldListingPriceDataGridViewTextBoxColumn";
            // 
            // eBayNewListingPriceDataGridViewTextBoxColumn
            // 
            this.eBayNewListingPriceDataGridViewTextBoxColumn.DataPropertyName = "eBayNewListingPrice";
            this.eBayNewListingPriceDataGridViewTextBoxColumn.HeaderText = "eBayNewListingPrice";
            this.eBayNewListingPriceDataGridViewTextBoxColumn.Name = "eBayNewListingPriceDataGridViewTextBoxColumn";
            // 
            // eBayReferencePriceDataGridViewTextBoxColumn1
            // 
            this.eBayReferencePriceDataGridViewTextBoxColumn1.DataPropertyName = "eBayReferencePrice";
            this.eBayReferencePriceDataGridViewTextBoxColumn1.HeaderText = "eBayReferencePrice";
            this.eBayReferencePriceDataGridViewTextBoxColumn1.Name = "eBayReferencePriceDataGridViewTextBoxColumn1";
            // 
            // costcoOldPriceDataGridViewTextBoxColumn
            // 
            this.costcoOldPriceDataGridViewTextBoxColumn.DataPropertyName = "CostcoOldPrice";
            this.costcoOldPriceDataGridViewTextBoxColumn.HeaderText = "CostcoOldPrice";
            this.costcoOldPriceDataGridViewTextBoxColumn.Name = "costcoOldPriceDataGridViewTextBoxColumn";
            // 
            // costcoNewPriceDataGridViewTextBoxColumn
            // 
            this.costcoNewPriceDataGridViewTextBoxColumn.DataPropertyName = "CostcoNewPrice";
            this.costcoNewPriceDataGridViewTextBoxColumn.HeaderText = "CostcoNewPrice";
            this.costcoNewPriceDataGridViewTextBoxColumn.Name = "costcoNewPriceDataGridViewTextBoxColumn";
            // 
            // priceChangeDataGridViewTextBoxColumn
            // 
            this.priceChangeDataGridViewTextBoxColumn.DataPropertyName = "PriceChange";
            this.priceChangeDataGridViewTextBoxColumn.HeaderText = "PriceChange";
            this.priceChangeDataGridViewTextBoxColumn.Name = "priceChangeDataGridViewTextBoxColumn";
            // 
            // categoryDataGridViewTextBoxColumn1
            // 
            this.categoryDataGridViewTextBoxColumn1.DataPropertyName = "Category";
            this.categoryDataGridViewTextBoxColumn1.HeaderText = "Category";
            this.categoryDataGridViewTextBoxColumn1.Name = "categoryDataGridViewTextBoxColumn1";
            // 
            // shippingDataGridViewTextBoxColumn2
            // 
            this.shippingDataGridViewTextBoxColumn2.DataPropertyName = "Shipping";
            this.shippingDataGridViewTextBoxColumn2.HeaderText = "Shipping";
            this.shippingDataGridViewTextBoxColumn2.Name = "shippingDataGridViewTextBoxColumn2";
            // 
            // limitDataGridViewTextBoxColumn1
            // 
            this.limitDataGridViewTextBoxColumn1.DataPropertyName = "Limit";
            this.limitDataGridViewTextBoxColumn1.HeaderText = "Limit";
            this.limitDataGridViewTextBoxColumn1.Name = "limitDataGridViewTextBoxColumn1";
            // 
            // discountDataGridViewTextBoxColumn1
            // 
            this.discountDataGridViewTextBoxColumn1.DataPropertyName = "Discount";
            this.discountDataGridViewTextBoxColumn1.HeaderText = "Discount";
            this.discountDataGridViewTextBoxColumn1.Name = "discountDataGridViewTextBoxColumn1";
            // 
            // detailsDataGridViewTextBoxColumn2
            // 
            this.detailsDataGridViewTextBoxColumn2.DataPropertyName = "Details";
            this.detailsDataGridViewTextBoxColumn2.HeaderText = "Details";
            this.detailsDataGridViewTextBoxColumn2.Name = "detailsDataGridViewTextBoxColumn2";
            // 
            // specificationDataGridViewTextBoxColumn2
            // 
            this.specificationDataGridViewTextBoxColumn2.DataPropertyName = "Specification";
            this.specificationDataGridViewTextBoxColumn2.HeaderText = "Specification";
            this.specificationDataGridViewTextBoxColumn2.Name = "specificationDataGridViewTextBoxColumn2";
            // 
            // imageLinkDataGridViewTextBoxColumn4
            // 
            this.imageLinkDataGridViewTextBoxColumn4.DataPropertyName = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn4.HeaderText = "ImageLink";
            this.imageLinkDataGridViewTextBoxColumn4.Name = "imageLinkDataGridViewTextBoxColumn4";
            // 
            // numberOfImageDataGridViewTextBoxColumn1
            // 
            this.numberOfImageDataGridViewTextBoxColumn1.DataPropertyName = "NumberOfImage";
            this.numberOfImageDataGridViewTextBoxColumn1.HeaderText = "NumberOfImage";
            this.numberOfImageDataGridViewTextBoxColumn1.Name = "numberOfImageDataGridViewTextBoxColumn1";
            // 
            // urlDataGridViewTextBoxColumn2
            // 
            this.urlDataGridViewTextBoxColumn2.DataPropertyName = "Url";
            this.urlDataGridViewTextBoxColumn2.HeaderText = "Url";
            this.urlDataGridViewTextBoxColumn2.Name = "urlDataGridViewTextBoxColumn2";
            // 
            // eBayCategoryIDDataGridViewTextBoxColumn3
            // 
            this.eBayCategoryIDDataGridViewTextBoxColumn3.DataPropertyName = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn3.HeaderText = "eBayCategoryID";
            this.eBayCategoryIDDataGridViewTextBoxColumn3.Name = "eBayCategoryIDDataGridViewTextBoxColumn3";
            // 
            // descriptionImageWidthDataGridViewTextBoxColumn1
            // 
            this.descriptionImageWidthDataGridViewTextBoxColumn1.DataPropertyName = "DescriptionImageWidth";
            this.descriptionImageWidthDataGridViewTextBoxColumn1.HeaderText = "DescriptionImageWidth";
            this.descriptionImageWidthDataGridViewTextBoxColumn1.Name = "descriptionImageWidthDataGridViewTextBoxColumn1";
            // 
            // descriptionImageHeightDataGridViewTextBoxColumn1
            // 
            this.descriptionImageHeightDataGridViewTextBoxColumn1.DataPropertyName = "DescriptionImageHeight";
            this.descriptionImageHeightDataGridViewTextBoxColumn1.HeaderText = "DescriptionImageHeight";
            this.descriptionImageHeightDataGridViewTextBoxColumn1.Name = "descriptionImageHeightDataGridViewTextBoxColumn1";
            // 
            // eBayToChangeBindingSource
            // 
            this.eBayToChangeBindingSource.DataMember = "eBay_ToChange";
            this.eBayToChangeBindingSource.DataSource = this.costcoDataSet6;
            // 
            // costcoDataSet6
            // 
            this.costcoDataSet6.DataSetName = "CostcoDataSet6";
            this.costcoDataSet6.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tpEbayToDelete
            // 
            this.tpEbayToDelete.Controls.Add(this.groupBox5);
            this.tpEbayToDelete.Location = new System.Drawing.Point(4, 22);
            this.tpEbayToDelete.Name = "tpEbayToDelete";
            this.tpEbayToDelete.Padding = new System.Windows.Forms.Padding(3);
            this.tpEbayToDelete.Size = new System.Drawing.Size(1244, 634);
            this.tpEbayToDelete.TabIndex = 9;
            this.tpEbayToDelete.Text = "eBay To Delete";
            this.tpEbayToDelete.UseVisualStyleBackColor = true;
            this.tpEbayToDelete.Enter += new System.EventHandler(this.tpEbayToDelete_Enter);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.gvDelete);
            this.groupBox5.Controls.Add(this.btnToDeleteUpload);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(1238, 628);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "To be Deleted Items";
            // 
            // gvDelete
            // 
            this.gvDelete.AllowUserToAddRows = false;
            this.gvDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvDelete.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDelete.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ToDeleteSelect});
            this.gvDelete.Location = new System.Drawing.Point(6, 39);
            this.gvDelete.Name = "gvDelete";
            this.gvDelete.Size = new System.Drawing.Size(1226, 583);
            this.gvDelete.TabIndex = 4;
            this.gvDelete.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvDelete_CellValueChanged);
            // 
            // ToDeleteSelect
            // 
            this.ToDeleteSelect.HeaderText = "Select";
            this.ToDeleteSelect.Name = "ToDeleteSelect";
            this.ToDeleteSelect.Width = 20;
            // 
            // btnToDeleteUpload
            // 
            this.btnToDeleteUpload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToDeleteUpload.Location = new System.Drawing.Point(1157, 10);
            this.btnToDeleteUpload.Name = "btnToDeleteUpload";
            this.btnToDeleteUpload.Size = new System.Drawing.Size(75, 23);
            this.btnToDeleteUpload.TabIndex = 3;
            this.btnToDeleteUpload.Text = "Upload";
            this.btnToDeleteUpload.UseVisualStyleBackColor = true;
            this.btnToDeleteUpload.Click += new System.EventHandler(this.btnToDeleteUpload_Click);
            // 
            // tpSaleTax
            // 
            this.tpSaleTax.Controls.Add(this.panel4);
            this.tpSaleTax.Location = new System.Drawing.Point(4, 22);
            this.tpSaleTax.Name = "tpSaleTax";
            this.tpSaleTax.Padding = new System.Windows.Forms.Padding(3);
            this.tpSaleTax.Size = new System.Drawing.Size(1244, 634);
            this.tpSaleTax.TabIndex = 5;
            this.tpSaleTax.Text = "Sale Tax";
            this.tpSaleTax.UseVisualStyleBackColor = true;
            this.tpSaleTax.Enter += new System.EventHandler(this.tpSaleTax_Enter);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox7);
            this.panel4.Controls.Add(this.btnGenerateSaleTaxReport);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.dtpSaleTaxTo);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.dtpSaleTaxFrom);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1238, 628);
            this.panel4.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox7.Controls.Add(this.btnSaleTaxSave);
            this.groupBox7.Controls.Add(this.gvSaleTaxHistory);
            this.groupBox7.Location = new System.Drawing.Point(3, 35);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(1232, 590);
            this.groupBox7.TabIndex = 15;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Sale Tax History";
            // 
            // btnSaleTaxSave
            // 
            this.btnSaleTaxSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaleTaxSave.Location = new System.Drawing.Point(1148, 562);
            this.btnSaleTaxSave.Name = "btnSaleTaxSave";
            this.btnSaleTaxSave.Size = new System.Drawing.Size(78, 23);
            this.btnSaleTaxSave.TabIndex = 16;
            this.btnSaleTaxSave.Text = "Save";
            this.btnSaleTaxSave.UseVisualStyleBackColor = true;
            this.btnSaleTaxSave.Click += new System.EventHandler(this.btnSaleTaxSave_Click);
            // 
            // gvSaleTaxHistory
            // 
            this.gvSaleTaxHistory.AllowUserToAddRows = false;
            this.gvSaleTaxHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvSaleTaxHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvSaleTaxHistory.Location = new System.Drawing.Point(6, 19);
            this.gvSaleTaxHistory.Name = "gvSaleTaxHistory";
            this.gvSaleTaxHistory.Size = new System.Drawing.Size(1220, 537);
            this.gvSaleTaxHistory.TabIndex = 0;
            // 
            // btnGenerateSaleTaxReport
            // 
            this.btnGenerateSaleTaxReport.Location = new System.Drawing.Point(545, 8);
            this.btnGenerateSaleTaxReport.Name = "btnGenerateSaleTaxReport";
            this.btnGenerateSaleTaxReport.Size = new System.Drawing.Size(144, 23);
            this.btnGenerateSaleTaxReport.TabIndex = 14;
            this.btnGenerateSaleTaxReport.Text = "Generate Sale Tax Report";
            this.btnGenerateSaleTaxReport.UseVisualStyleBackColor = true;
            this.btnGenerateSaleTaxReport.Click += new System.EventHandler(this.btnGenerateSaleTaxReport_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(276, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "To:";
            // 
            // dtpSaleTaxTo
            // 
            this.dtpSaleTaxTo.Location = new System.Drawing.Point(303, 9);
            this.dtpSaleTaxTo.Name = "dtpSaleTaxTo";
            this.dtpSaleTaxTo.Size = new System.Drawing.Size(200, 20);
            this.dtpSaleTaxTo.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 13);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "From:";
            // 
            // dtpSaleTaxFrom
            // 
            this.dtpSaleTaxFrom.Location = new System.Drawing.Point(47, 9);
            this.dtpSaleTaxFrom.Name = "dtpSaleTaxFrom";
            this.dtpSaleTaxFrom.Size = new System.Drawing.Size(200, 20);
            this.dtpSaleTaxFrom.TabIndex = 10;
            // 
            // tpIncomeTax
            // 
            this.tpIncomeTax.Controls.Add(this.panel5);
            this.tpIncomeTax.Location = new System.Drawing.Point(4, 22);
            this.tpIncomeTax.Name = "tpIncomeTax";
            this.tpIncomeTax.Padding = new System.Windows.Forms.Padding(3);
            this.tpIncomeTax.Size = new System.Drawing.Size(1244, 634);
            this.tpIncomeTax.TabIndex = 6;
            this.tpIncomeTax.Text = "Income Tax";
            this.tpIncomeTax.UseVisualStyleBackColor = true;
            this.tpIncomeTax.Enter += new System.EventHandler(this.tpIncomeTax_Enter);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnIncomeTaxCalculate);
            this.panel5.Controls.Add(this.cmbIncomeTaxYear);
            this.panel5.Controls.Add(this.tableLayoutPanel1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1238, 628);
            this.panel5.TabIndex = 0;
            // 
            // btnIncomeTaxCalculate
            // 
            this.btnIncomeTaxCalculate.Location = new System.Drawing.Point(141, 4);
            this.btnIncomeTaxCalculate.Name = "btnIncomeTaxCalculate";
            this.btnIncomeTaxCalculate.Size = new System.Drawing.Size(75, 21);
            this.btnIncomeTaxCalculate.TabIndex = 2;
            this.btnIncomeTaxCalculate.Text = "Calculate";
            this.btnIncomeTaxCalculate.UseVisualStyleBackColor = true;
            this.btnIncomeTaxCalculate.Click += new System.EventHandler(this.btnIncomeTaxCalculate_Click);
            // 
            // cmbIncomeTaxYear
            // 
            this.cmbIncomeTaxYear.FormattingEnabled = true;
            this.cmbIncomeTaxYear.Items.AddRange(new object[] {
            "2016",
            "2017",
            "2018",
            "2019",
            "2020"});
            this.cmbIncomeTaxYear.Location = new System.Drawing.Point(3, 4);
            this.cmbIncomeTaxYear.Name = "cmbIncomeTaxYear";
            this.cmbIncomeTaxYear.Size = new System.Drawing.Size(132, 21);
            this.cmbIncomeTaxYear.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label8, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label7, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1232, 566);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel3.Controls.Add(this.ll31, 2, 26);
            this.tableLayoutPanel3.Controls.Add(this.label81, 1, 26);
            this.tableLayoutPanel3.Controls.Add(this.label49, 0, 26);
            this.tableLayoutPanel3.Controls.Add(this.ll30, 2, 25);
            this.tableLayoutPanel3.Controls.Add(this.label84, 1, 25);
            this.tableLayoutPanel3.Controls.Add(this.label83, 0, 25);
            this.tableLayoutPanel3.Controls.Add(this.ll29, 2, 24);
            this.tableLayoutPanel3.Controls.Add(this.label80, 1, 24);
            this.tableLayoutPanel3.Controls.Add(this.label79, 0, 24);
            this.tableLayoutPanel3.Controls.Add(this.ll28, 2, 23);
            this.tableLayoutPanel3.Controls.Add(this.label78, 1, 23);
            this.tableLayoutPanel3.Controls.Add(this.label77, 0, 23);
            this.tableLayoutPanel3.Controls.Add(this.ll27, 2, 22);
            this.tableLayoutPanel3.Controls.Add(this.label76, 1, 22);
            this.tableLayoutPanel3.Controls.Add(this.label75, 0, 22);
            this.tableLayoutPanel3.Controls.Add(this.ll26, 2, 21);
            this.tableLayoutPanel3.Controls.Add(this.label74, 1, 21);
            this.tableLayoutPanel3.Controls.Add(this.label73, 0, 21);
            this.tableLayoutPanel3.Controls.Add(this.ll25, 2, 20);
            this.tableLayoutPanel3.Controls.Add(this.label72, 1, 20);
            this.tableLayoutPanel3.Controls.Add(this.label71, 0, 20);
            this.tableLayoutPanel3.Controls.Add(this.ll24b, 2, 19);
            this.tableLayoutPanel3.Controls.Add(this.label69, 1, 19);
            this.tableLayoutPanel3.Controls.Add(this.label68, 0, 19);
            this.tableLayoutPanel3.Controls.Add(this.ll24a, 2, 18);
            this.tableLayoutPanel3.Controls.Add(this.label67, 1, 18);
            this.tableLayoutPanel3.Controls.Add(this.label65, 0, 18);
            this.tableLayoutPanel3.Controls.Add(this.ll23, 2, 17);
            this.tableLayoutPanel3.Controls.Add(this.label63, 1, 17);
            this.tableLayoutPanel3.Controls.Add(this.label62, 0, 17);
            this.tableLayoutPanel3.Controls.Add(this.ll22, 2, 16);
            this.tableLayoutPanel3.Controls.Add(this.label61, 1, 16);
            this.tableLayoutPanel3.Controls.Add(this.label60, 0, 16);
            this.tableLayoutPanel3.Controls.Add(this.ll21, 2, 15);
            this.tableLayoutPanel3.Controls.Add(this.label59, 1, 15);
            this.tableLayoutPanel3.Controls.Add(this.label58, 0, 15);
            this.tableLayoutPanel3.Controls.Add(this.ll20b, 2, 14);
            this.tableLayoutPanel3.Controls.Add(this.label57, 1, 14);
            this.tableLayoutPanel3.Controls.Add(this.label56, 0, 14);
            this.tableLayoutPanel3.Controls.Add(this.ll20a, 2, 13);
            this.tableLayoutPanel3.Controls.Add(this.label55, 1, 13);
            this.tableLayoutPanel3.Controls.Add(this.label54, 0, 13);
            this.tableLayoutPanel3.Controls.Add(this.ll19, 2, 12);
            this.tableLayoutPanel3.Controls.Add(this.label53, 1, 12);
            this.tableLayoutPanel3.Controls.Add(this.label52, 0, 12);
            this.tableLayoutPanel3.Controls.Add(this.ll18, 2, 11);
            this.tableLayoutPanel3.Controls.Add(this.label51, 1, 11);
            this.tableLayoutPanel3.Controls.Add(this.label50, 0, 11);
            this.tableLayoutPanel3.Controls.Add(this.ll17, 2, 10);
            this.tableLayoutPanel3.Controls.Add(this.label48, 1, 10);
            this.tableLayoutPanel3.Controls.Add(this.label45, 0, 9);
            this.tableLayoutPanel3.Controls.Add(this.label47, 0, 10);
            this.tableLayoutPanel3.Controls.Add(this.ll16a, 2, 8);
            this.tableLayoutPanel3.Controls.Add(this.label27, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.label28, 0, 8);
            this.tableLayoutPanel3.Controls.Add(this.ll15, 2, 7);
            this.tableLayoutPanel3.Controls.Add(this.label29, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.label30, 0, 7);
            this.tableLayoutPanel3.Controls.Add(this.ll14, 2, 6);
            this.tableLayoutPanel3.Controls.Add(this.label31, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.label32, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.ll13, 2, 5);
            this.tableLayoutPanel3.Controls.Add(this.label33, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.label34, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.ll12, 2, 4);
            this.tableLayoutPanel3.Controls.Add(this.label35, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label36, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.ll11, 2, 3);
            this.tableLayoutPanel3.Controls.Add(this.label37, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.label38, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.ll10, 2, 2);
            this.tableLayoutPanel3.Controls.Add(this.label39, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.label40, 0, 2);
            this.tableLayoutPanel3.Controls.Add(this.label41, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.ll9, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.label42, 1, 1);
            this.tableLayoutPanel3.Controls.Add(this.ll8, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.label43, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label44, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label46, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.ll16b, 2, 9);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(616, 20);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 28;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(616, 546);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // ll31
            // 
            this.ll31.AutoSize = true;
            this.ll31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ll31.Location = new System.Drawing.Point(440, 520);
            this.ll31.Margin = new System.Windows.Forms.Padding(0);
            this.ll31.Name = "ll31";
            this.ll31.Size = new System.Drawing.Size(176, 20);
            this.ll31.TabIndex = 89;
            this.ll31.TabStop = true;
            this.ll31.Text = "0.00";
            this.ll31.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label81.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label81.Location = new System.Drawing.Point(30, 520);
            this.label81.Margin = new System.Windows.Forms.Padding(0);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(410, 20);
            this.label81.TabIndex = 88;
            this.label81.Text = "Net profit or (loss). Subtract line 30 from line 29";
            this.label81.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label49.Location = new System.Drawing.Point(0, 520);
            this.label49.Margin = new System.Windows.Forms.Padding(0);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(30, 20);
            this.label49.TabIndex = 87;
            this.label49.Text = "31";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll30
            // 
            this.ll30.AutoSize = true;
            this.ll30.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll30.Location = new System.Drawing.Point(440, 500);
            this.ll30.Margin = new System.Windows.Forms.Padding(0);
            this.ll30.Name = "ll30";
            this.ll30.Size = new System.Drawing.Size(176, 20);
            this.ll30.TabIndex = 86;
            this.ll30.TabStop = true;
            this.ll30.Text = "0.00";
            this.ll30.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll30.TextChanged += new System.EventHandler(this.ll30_TextChanged);
            // 
            // label84
            // 
            this.label84.AutoSize = true;
            this.label84.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label84.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label84.Location = new System.Drawing.Point(30, 500);
            this.label84.Margin = new System.Windows.Forms.Padding(0);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(410, 20);
            this.label84.TabIndex = 85;
            this.label84.Text = "Expreses fro business use of your home. Attach Form 8829";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label83
            // 
            this.label83.AutoSize = true;
            this.label83.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label83.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label83.Location = new System.Drawing.Point(0, 500);
            this.label83.Margin = new System.Windows.Forms.Padding(0);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(30, 20);
            this.label83.TabIndex = 84;
            this.label83.Text = "30";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll29
            // 
            this.ll29.AutoSize = true;
            this.ll29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll29.Location = new System.Drawing.Point(440, 480);
            this.ll29.Margin = new System.Windows.Forms.Padding(0);
            this.ll29.Name = "ll29";
            this.ll29.Size = new System.Drawing.Size(176, 20);
            this.ll29.TabIndex = 80;
            this.ll29.TabStop = true;
            this.ll29.Text = "0.00";
            this.ll29.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll29.TextChanged += new System.EventHandler(this.ll29_TextChanged);
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label80.Location = new System.Drawing.Point(30, 480);
            this.label80.Margin = new System.Windows.Forms.Padding(0);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(410, 20);
            this.label80.TabIndex = 79;
            this.label80.Text = "Tentative profile or (loss). Subtract line 28 from line 7";
            this.label80.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label79.Location = new System.Drawing.Point(0, 480);
            this.label79.Margin = new System.Windows.Forms.Padding(0);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(30, 20);
            this.label79.TabIndex = 78;
            this.label79.Text = "29";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll28
            // 
            this.ll28.AutoSize = true;
            this.ll28.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll28.Location = new System.Drawing.Point(440, 460);
            this.ll28.Margin = new System.Windows.Forms.Padding(0);
            this.ll28.Name = "ll28";
            this.ll28.Size = new System.Drawing.Size(176, 20);
            this.ll28.TabIndex = 77;
            this.ll28.TabStop = true;
            this.ll28.Text = "0.00";
            this.ll28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll28.TextChanged += new System.EventHandler(this.ll28_TextChanged);
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label78.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label78.Location = new System.Drawing.Point(30, 460);
            this.label78.Margin = new System.Windows.Forms.Padding(0);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(410, 20);
            this.label78.TabIndex = 76;
            this.label78.Text = "Total expense before expense for business use of home. Add lines 8 through 27";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label77.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label77.Location = new System.Drawing.Point(0, 460);
            this.label77.Margin = new System.Windows.Forms.Padding(0);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(30, 20);
            this.label77.TabIndex = 75;
            this.label77.Text = "28";
            this.label77.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll27
            // 
            this.ll27.AutoSize = true;
            this.ll27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll27.Location = new System.Drawing.Point(440, 440);
            this.ll27.Margin = new System.Windows.Forms.Padding(0);
            this.ll27.Name = "ll27";
            this.ll27.Size = new System.Drawing.Size(176, 20);
            this.ll27.TabIndex = 74;
            this.ll27.TabStop = true;
            this.ll27.Text = "0.00";
            this.ll27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll27.TextChanged += new System.EventHandler(this.ll27_TextChanged);
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label76.Location = new System.Drawing.Point(30, 440);
            this.label76.Margin = new System.Windows.Forms.Padding(0);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(410, 20);
            this.label76.TabIndex = 73;
            this.label76.Text = "Other expenses";
            this.label76.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label75.Location = new System.Drawing.Point(0, 440);
            this.label75.Margin = new System.Windows.Forms.Padding(0);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(30, 20);
            this.label75.TabIndex = 72;
            this.label75.Text = "27";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll26
            // 
            this.ll26.AutoSize = true;
            this.ll26.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll26.Location = new System.Drawing.Point(440, 420);
            this.ll26.Margin = new System.Windows.Forms.Padding(0);
            this.ll26.Name = "ll26";
            this.ll26.Size = new System.Drawing.Size(176, 20);
            this.ll26.TabIndex = 71;
            this.ll26.TabStop = true;
            this.ll26.Text = "0.00";
            this.ll26.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll26.TextChanged += new System.EventHandler(this.ll26_TextChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label74.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label74.Location = new System.Drawing.Point(30, 420);
            this.label74.Margin = new System.Windows.Forms.Padding(0);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(410, 20);
            this.label74.TabIndex = 70;
            this.label74.Text = "Wage (less employment credits)";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label73.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label73.Location = new System.Drawing.Point(0, 420);
            this.label73.Margin = new System.Windows.Forms.Padding(0);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(30, 20);
            this.label73.TabIndex = 69;
            this.label73.Text = "26";
            this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll25
            // 
            this.ll25.AutoSize = true;
            this.ll25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll25.Location = new System.Drawing.Point(440, 400);
            this.ll25.Margin = new System.Windows.Forms.Padding(0);
            this.ll25.Name = "ll25";
            this.ll25.Size = new System.Drawing.Size(176, 20);
            this.ll25.TabIndex = 68;
            this.ll25.TabStop = true;
            this.ll25.Text = "0.00";
            this.ll25.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll25.TextChanged += new System.EventHandler(this.ll25_TextChanged);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label72.Location = new System.Drawing.Point(30, 400);
            this.label72.Margin = new System.Windows.Forms.Padding(0);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(410, 20);
            this.label72.TabIndex = 67;
            this.label72.Text = "Utilities";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label71.Location = new System.Drawing.Point(0, 400);
            this.label71.Margin = new System.Windows.Forms.Padding(0);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(30, 20);
            this.label71.TabIndex = 66;
            this.label71.Text = "25";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll24b
            // 
            this.ll24b.AutoSize = true;
            this.ll24b.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll24b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll24b.Location = new System.Drawing.Point(440, 380);
            this.ll24b.Margin = new System.Windows.Forms.Padding(0);
            this.ll24b.Name = "ll24b";
            this.ll24b.Size = new System.Drawing.Size(176, 20);
            this.ll24b.TabIndex = 65;
            this.ll24b.TabStop = true;
            this.ll24b.Text = "0.00";
            this.ll24b.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll24b.TextChanged += new System.EventHandler(this.ll24b_TextChanged);
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label69.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label69.Location = new System.Drawing.Point(30, 380);
            this.label69.Margin = new System.Windows.Forms.Padding(0);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(410, 20);
            this.label69.TabIndex = 64;
            this.label69.Text = "Travel, meals, and entertainment: Deductible meals and entertainment";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label68.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label68.Location = new System.Drawing.Point(0, 380);
            this.label68.Margin = new System.Windows.Forms.Padding(0);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(30, 20);
            this.label68.TabIndex = 63;
            this.label68.Text = "24b";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll24a
            // 
            this.ll24a.AutoSize = true;
            this.ll24a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll24a.Location = new System.Drawing.Point(440, 360);
            this.ll24a.Margin = new System.Windows.Forms.Padding(0);
            this.ll24a.Name = "ll24a";
            this.ll24a.Size = new System.Drawing.Size(176, 20);
            this.ll24a.TabIndex = 62;
            this.ll24a.TabStop = true;
            this.ll24a.Text = "0.00";
            this.ll24a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll24a.TextChanged += new System.EventHandler(this.ll24a_TextChanged);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label67.Location = new System.Drawing.Point(30, 360);
            this.label67.Margin = new System.Windows.Forms.Padding(0);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(410, 20);
            this.label67.TabIndex = 61;
            this.label67.Text = "Travel, meals, and entertainment: Travel";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label65.Location = new System.Drawing.Point(0, 360);
            this.label65.Margin = new System.Windows.Forms.Padding(0);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(30, 20);
            this.label65.TabIndex = 60;
            this.label65.Text = "24a";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll23
            // 
            this.ll23.AutoSize = true;
            this.ll23.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll23.Location = new System.Drawing.Point(440, 340);
            this.ll23.Margin = new System.Windows.Forms.Padding(0);
            this.ll23.Name = "ll23";
            this.ll23.Size = new System.Drawing.Size(176, 20);
            this.ll23.TabIndex = 59;
            this.ll23.TabStop = true;
            this.ll23.Text = "0.00";
            this.ll23.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll23.TextChanged += new System.EventHandler(this.ll23_TextChanged);
            // 
            // label63
            // 
            this.label63.AutoSize = true;
            this.label63.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label63.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label63.Location = new System.Drawing.Point(30, 340);
            this.label63.Margin = new System.Windows.Forms.Padding(0);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(410, 20);
            this.label63.TabIndex = 58;
            this.label63.Text = "Taxes and licenses";
            this.label63.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label62
            // 
            this.label62.AutoSize = true;
            this.label62.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label62.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label62.Location = new System.Drawing.Point(0, 340);
            this.label62.Margin = new System.Windows.Forms.Padding(0);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(30, 20);
            this.label62.TabIndex = 57;
            this.label62.Text = "23";
            this.label62.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll22
            // 
            this.ll22.AutoSize = true;
            this.ll22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll22.Location = new System.Drawing.Point(440, 320);
            this.ll22.Margin = new System.Windows.Forms.Padding(0);
            this.ll22.Name = "ll22";
            this.ll22.Size = new System.Drawing.Size(176, 20);
            this.ll22.TabIndex = 55;
            this.ll22.TabStop = true;
            this.ll22.Text = "0.00";
            this.ll22.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll22.TextChanged += new System.EventHandler(this.ll22_TextChanged);
            // 
            // label61
            // 
            this.label61.AutoSize = true;
            this.label61.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label61.Location = new System.Drawing.Point(30, 320);
            this.label61.Margin = new System.Windows.Forms.Padding(0);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(410, 20);
            this.label61.TabIndex = 54;
            this.label61.Text = "Supplies";
            this.label61.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label60
            // 
            this.label60.AutoSize = true;
            this.label60.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label60.Location = new System.Drawing.Point(0, 320);
            this.label60.Margin = new System.Windows.Forms.Padding(0);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(30, 20);
            this.label60.TabIndex = 53;
            this.label60.Text = "22";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll21
            // 
            this.ll21.AutoSize = true;
            this.ll21.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll21.Location = new System.Drawing.Point(440, 300);
            this.ll21.Margin = new System.Windows.Forms.Padding(0);
            this.ll21.Name = "ll21";
            this.ll21.Size = new System.Drawing.Size(176, 20);
            this.ll21.TabIndex = 52;
            this.ll21.TabStop = true;
            this.ll21.Text = "0.00";
            this.ll21.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll21.TextChanged += new System.EventHandler(this.ll21_TextChanged);
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label59.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label59.Location = new System.Drawing.Point(30, 300);
            this.label59.Margin = new System.Windows.Forms.Padding(0);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(410, 20);
            this.label59.TabIndex = 51;
            this.label59.Text = "Repairs and maintenance";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label58
            // 
            this.label58.AutoSize = true;
            this.label58.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label58.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label58.Location = new System.Drawing.Point(0, 300);
            this.label58.Margin = new System.Windows.Forms.Padding(0);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(30, 20);
            this.label58.TabIndex = 50;
            this.label58.Text = "21";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll20b
            // 
            this.ll20b.AutoSize = true;
            this.ll20b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll20b.Location = new System.Drawing.Point(440, 280);
            this.ll20b.Margin = new System.Windows.Forms.Padding(0);
            this.ll20b.Name = "ll20b";
            this.ll20b.Size = new System.Drawing.Size(176, 20);
            this.ll20b.TabIndex = 49;
            this.ll20b.TabStop = true;
            this.ll20b.Text = "0.00";
            this.ll20b.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll20b.TextChanged += new System.EventHandler(this.ll20b_TextChanged);
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label57.Location = new System.Drawing.Point(30, 280);
            this.label57.Margin = new System.Windows.Forms.Padding(0);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(410, 20);
            this.label57.TabIndex = 48;
            this.label57.Text = "Rent or lease: Other business property";
            this.label57.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label56.Location = new System.Drawing.Point(0, 280);
            this.label56.Margin = new System.Windows.Forms.Padding(0);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(30, 20);
            this.label56.TabIndex = 47;
            this.label56.Text = "20b";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll20a
            // 
            this.ll20a.AutoSize = true;
            this.ll20a.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll20a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll20a.Location = new System.Drawing.Point(440, 260);
            this.ll20a.Margin = new System.Windows.Forms.Padding(0);
            this.ll20a.Name = "ll20a";
            this.ll20a.Size = new System.Drawing.Size(176, 20);
            this.ll20a.TabIndex = 46;
            this.ll20a.TabStop = true;
            this.ll20a.Text = "0.00";
            this.ll20a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll20a.TextChanged += new System.EventHandler(this.ll20a_TextChanged);
            // 
            // label55
            // 
            this.label55.AutoSize = true;
            this.label55.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label55.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label55.Location = new System.Drawing.Point(30, 260);
            this.label55.Margin = new System.Windows.Forms.Padding(0);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(410, 20);
            this.label55.TabIndex = 45;
            this.label55.Text = "Rent of lease: Vehicles, machinery, and equipment";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label54.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label54.Location = new System.Drawing.Point(0, 260);
            this.label54.Margin = new System.Windows.Forms.Padding(0);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(30, 20);
            this.label54.TabIndex = 44;
            this.label54.Text = "20a ";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll19
            // 
            this.ll19.AutoSize = true;
            this.ll19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll19.Location = new System.Drawing.Point(440, 240);
            this.ll19.Margin = new System.Windows.Forms.Padding(0);
            this.ll19.Name = "ll19";
            this.ll19.Size = new System.Drawing.Size(176, 20);
            this.ll19.TabIndex = 43;
            this.ll19.TabStop = true;
            this.ll19.Text = "0.00";
            this.ll19.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll19.TextChanged += new System.EventHandler(this.ll19_TextChanged);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label53.Location = new System.Drawing.Point(30, 240);
            this.label53.Margin = new System.Windows.Forms.Padding(0);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(410, 20);
            this.label53.TabIndex = 42;
            this.label53.Text = "Pension and profilt-sharing plans";
            this.label53.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label52.Location = new System.Drawing.Point(0, 240);
            this.label52.Margin = new System.Windows.Forms.Padding(0);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(30, 20);
            this.label52.TabIndex = 41;
            this.label52.Text = "19";
            this.label52.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll18
            // 
            this.ll18.AutoSize = true;
            this.ll18.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll18.Location = new System.Drawing.Point(440, 220);
            this.ll18.Margin = new System.Windows.Forms.Padding(0);
            this.ll18.Name = "ll18";
            this.ll18.Size = new System.Drawing.Size(176, 20);
            this.ll18.TabIndex = 40;
            this.ll18.TabStop = true;
            this.ll18.Text = "0.00";
            this.ll18.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll18.TextChanged += new System.EventHandler(this.ll18_TextChanged);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label51.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label51.Location = new System.Drawing.Point(30, 220);
            this.label51.Margin = new System.Windows.Forms.Padding(0);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(410, 20);
            this.label51.TabIndex = 39;
            this.label51.Text = "Office expense";
            this.label51.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label50.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label50.Location = new System.Drawing.Point(0, 220);
            this.label50.Margin = new System.Windows.Forms.Padding(0);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(30, 20);
            this.label50.TabIndex = 37;
            this.label50.Text = "18";
            this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll17
            // 
            this.ll17.AutoSize = true;
            this.ll17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll17.Location = new System.Drawing.Point(440, 200);
            this.ll17.Margin = new System.Windows.Forms.Padding(0);
            this.ll17.Name = "ll17";
            this.ll17.Size = new System.Drawing.Size(176, 20);
            this.ll17.TabIndex = 35;
            this.ll17.TabStop = true;
            this.ll17.Text = "0.00";
            this.ll17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll17.TextChanged += new System.EventHandler(this.ll17_TextChanged);
            // 
            // label48
            // 
            this.label48.AutoSize = true;
            this.label48.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label48.Location = new System.Drawing.Point(30, 200);
            this.label48.Margin = new System.Windows.Forms.Padding(0);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(410, 20);
            this.label48.TabIndex = 34;
            this.label48.Text = "Legal and professional services";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label45.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label45.Location = new System.Drawing.Point(0, 180);
            this.label45.Margin = new System.Windows.Forms.Padding(0);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(30, 20);
            this.label45.TabIndex = 33;
            this.label45.Text = "16b";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label47.Location = new System.Drawing.Point(0, 200);
            this.label47.Margin = new System.Windows.Forms.Padding(0);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(30, 20);
            this.label47.TabIndex = 32;
            this.label47.Text = "17";
            this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll16a
            // 
            this.ll16a.AutoSize = true;
            this.ll16a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll16a.Location = new System.Drawing.Point(440, 160);
            this.ll16a.Margin = new System.Windows.Forms.Padding(0);
            this.ll16a.Name = "ll16a";
            this.ll16a.Size = new System.Drawing.Size(176, 20);
            this.ll16a.TabIndex = 27;
            this.ll16a.TabStop = true;
            this.ll16a.Text = "0.00";
            this.ll16a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll16a.TextChanged += new System.EventHandler(this.ll16a_TextChanged);
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label27.Location = new System.Drawing.Point(30, 160);
            this.label27.Margin = new System.Windows.Forms.Padding(0);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(410, 20);
            this.label27.TabIndex = 26;
            this.label27.Text = "Interest: Mortgage (paid to banks, etc)";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label28.Location = new System.Drawing.Point(0, 160);
            this.label28.Margin = new System.Windows.Forms.Padding(0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(30, 20);
            this.label28.TabIndex = 25;
            this.label28.Text = "16a";
            this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll15
            // 
            this.ll15.AutoSize = true;
            this.ll15.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll15.Location = new System.Drawing.Point(440, 140);
            this.ll15.Margin = new System.Windows.Forms.Padding(0);
            this.ll15.Name = "ll15";
            this.ll15.Size = new System.Drawing.Size(176, 20);
            this.ll15.TabIndex = 24;
            this.ll15.TabStop = true;
            this.ll15.Text = "0.00";
            this.ll15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll15.TextChanged += new System.EventHandler(this.ll15_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label29.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label29.Location = new System.Drawing.Point(30, 140);
            this.label29.Margin = new System.Windows.Forms.Padding(0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(410, 20);
            this.label29.TabIndex = 23;
            this.label29.Text = "Insurance (other than health)";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label30.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label30.Location = new System.Drawing.Point(0, 140);
            this.label30.Margin = new System.Windows.Forms.Padding(0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(30, 20);
            this.label30.TabIndex = 22;
            this.label30.Text = "15";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll14
            // 
            this.ll14.AutoSize = true;
            this.ll14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll14.Location = new System.Drawing.Point(440, 120);
            this.ll14.Margin = new System.Windows.Forms.Padding(0);
            this.ll14.Name = "ll14";
            this.ll14.Size = new System.Drawing.Size(176, 20);
            this.ll14.TabIndex = 21;
            this.ll14.TabStop = true;
            this.ll14.Text = "0.00";
            this.ll14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll14.TextChanged += new System.EventHandler(this.ll14_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label31.Location = new System.Drawing.Point(30, 120);
            this.label31.Margin = new System.Windows.Forms.Padding(0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(410, 20);
            this.label31.TabIndex = 20;
            this.label31.Text = "Employee benefit programs";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label32.Location = new System.Drawing.Point(0, 120);
            this.label32.Margin = new System.Windows.Forms.Padding(0);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(30, 20);
            this.label32.TabIndex = 19;
            this.label32.Text = "14";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll13
            // 
            this.ll13.AutoSize = true;
            this.ll13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll13.Location = new System.Drawing.Point(440, 100);
            this.ll13.Margin = new System.Windows.Forms.Padding(0);
            this.ll13.Name = "ll13";
            this.ll13.Size = new System.Drawing.Size(176, 20);
            this.ll13.TabIndex = 18;
            this.ll13.TabStop = true;
            this.ll13.Text = "0.00";
            this.ll13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll13.TextChanged += new System.EventHandler(this.ll13_TextChanged);
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label33.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label33.Location = new System.Drawing.Point(30, 100);
            this.label33.Margin = new System.Windows.Forms.Padding(0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(410, 20);
            this.label33.TabIndex = 17;
            this.label33.Text = "Depreciation and section 179 expense deduction";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label34.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label34.Location = new System.Drawing.Point(0, 100);
            this.label34.Margin = new System.Windows.Forms.Padding(0);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(30, 20);
            this.label34.TabIndex = 16;
            this.label34.Text = "13";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll12
            // 
            this.ll12.AutoSize = true;
            this.ll12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll12.Location = new System.Drawing.Point(440, 80);
            this.ll12.Margin = new System.Windows.Forms.Padding(0);
            this.ll12.Name = "ll12";
            this.ll12.Size = new System.Drawing.Size(176, 20);
            this.ll12.TabIndex = 15;
            this.ll12.TabStop = true;
            this.ll12.Text = "0.00";
            this.ll12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll12.TextChanged += new System.EventHandler(this.ll12_TextChanged);
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label35.Location = new System.Drawing.Point(30, 80);
            this.label35.Margin = new System.Windows.Forms.Padding(0);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(410, 20);
            this.label35.TabIndex = 14;
            this.label35.Text = "Depletion";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label36.Location = new System.Drawing.Point(0, 80);
            this.label36.Margin = new System.Windows.Forms.Padding(0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(30, 20);
            this.label36.TabIndex = 13;
            this.label36.Text = "12";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll11
            // 
            this.ll11.AutoSize = true;
            this.ll11.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll11.Location = new System.Drawing.Point(440, 60);
            this.ll11.Margin = new System.Windows.Forms.Padding(0);
            this.ll11.Name = "ll11";
            this.ll11.Size = new System.Drawing.Size(176, 20);
            this.ll11.TabIndex = 12;
            this.ll11.TabStop = true;
            this.ll11.Text = "0.00";
            this.ll11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll11.TextChanged += new System.EventHandler(this.ll11_TextChanged);
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label37.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label37.Location = new System.Drawing.Point(30, 60);
            this.label37.Margin = new System.Windows.Forms.Padding(0);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(410, 20);
            this.label37.TabIndex = 11;
            this.label37.Text = "Contract labor";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label38.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label38.Location = new System.Drawing.Point(0, 60);
            this.label38.Margin = new System.Windows.Forms.Padding(0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(30, 20);
            this.label38.TabIndex = 10;
            this.label38.Text = "11";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll10
            // 
            this.ll10.AutoSize = true;
            this.ll10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll10.Location = new System.Drawing.Point(440, 40);
            this.ll10.Margin = new System.Windows.Forms.Padding(0);
            this.ll10.Name = "ll10";
            this.ll10.Size = new System.Drawing.Size(176, 20);
            this.ll10.TabIndex = 9;
            this.ll10.TabStop = true;
            this.ll10.Text = "0.00";
            this.ll10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll10.TextChanged += new System.EventHandler(this.ll10_TextChanged);
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label39.Location = new System.Drawing.Point(30, 40);
            this.label39.Margin = new System.Windows.Forms.Padding(0);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(410, 20);
            this.label39.TabIndex = 8;
            this.label39.Text = "Commissions and fees";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label40.Location = new System.Drawing.Point(0, 40);
            this.label40.Margin = new System.Windows.Forms.Padding(0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(30, 20);
            this.label40.TabIndex = 7;
            this.label40.Text = "10";
            this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label41.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label41.Location = new System.Drawing.Point(0, 20);
            this.label41.Margin = new System.Windows.Forms.Padding(0);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(30, 20);
            this.label41.TabIndex = 6;
            this.label41.Text = "9";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll9
            // 
            this.ll9.AutoSize = true;
            this.ll9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll9.Location = new System.Drawing.Point(440, 20);
            this.ll9.Margin = new System.Windows.Forms.Padding(0);
            this.ll9.Name = "ll9";
            this.ll9.Size = new System.Drawing.Size(176, 20);
            this.ll9.TabIndex = 5;
            this.ll9.TabStop = true;
            this.ll9.Text = "0.00";
            this.ll9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll9.TextChanged += new System.EventHandler(this.ll9_TextChanged);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label42.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label42.Location = new System.Drawing.Point(30, 20);
            this.label42.Margin = new System.Windows.Forms.Padding(0);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(410, 20);
            this.label42.TabIndex = 4;
            this.label42.Text = "Car and truck mileage";
            this.label42.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll8
            // 
            this.ll8.AutoSize = true;
            this.ll8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll8.Location = new System.Drawing.Point(440, 0);
            this.ll8.Margin = new System.Windows.Forms.Padding(0);
            this.ll8.Name = "ll8";
            this.ll8.Size = new System.Drawing.Size(176, 20);
            this.ll8.TabIndex = 0;
            this.ll8.TabStop = true;
            this.ll8.Text = "0.00";
            this.ll8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll8_LinkClicked);
            this.ll8.TextChanged += new System.EventHandler(this.ll8_TextChanged);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label43.Location = new System.Drawing.Point(0, 0);
            this.label43.Margin = new System.Windows.Forms.Padding(0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(30, 20);
            this.label43.TabIndex = 1;
            this.label43.Text = "8";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label44.Location = new System.Drawing.Point(30, 0);
            this.label44.Margin = new System.Windows.Forms.Padding(0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(410, 20);
            this.label44.TabIndex = 2;
            this.label44.Text = "Advertising";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label46.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label46.Location = new System.Drawing.Point(30, 180);
            this.label46.Margin = new System.Windows.Forms.Padding(0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(410, 20);
            this.label46.TabIndex = 29;
            this.label46.Text = "Interest: Other";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll16b
            // 
            this.ll16b.AutoSize = true;
            this.ll16b.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll16b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll16b.Location = new System.Drawing.Point(440, 180);
            this.ll16b.Margin = new System.Windows.Forms.Padding(0);
            this.ll16b.Name = "ll16b";
            this.ll16b.Size = new System.Drawing.Size(176, 20);
            this.ll16b.TabIndex = 31;
            this.ll16b.TabStop = true;
            this.ll16b.Text = "0.00";
            this.ll16b.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll16b.TextChanged += new System.EventHandler(this.ll16b_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(616, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(616, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "Expense";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(0, 0);
            this.label7.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(615, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "Income";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.Controls.Add(this.ll7, 2, 9);
            this.tableLayoutPanel2.Controls.Add(this.label26, 1, 9);
            this.tableLayoutPanel2.Controls.Add(this.label25, 0, 9);
            this.tableLayoutPanel2.Controls.Add(this.ll6, 2, 8);
            this.tableLayoutPanel2.Controls.Add(this.label24, 1, 8);
            this.tableLayoutPanel2.Controls.Add(this.label23, 0, 8);
            this.tableLayoutPanel2.Controls.Add(this.ll5, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.label22, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.ll4, 2, 6);
            this.tableLayoutPanel2.Controls.Add(this.label20, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label19, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.ll3, 2, 5);
            this.tableLayoutPanel2.Controls.Add(this.label18, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label17, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.ll2, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.label16, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label15, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.ll1c, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.label14, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.ll1b, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.label12, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.ll1a, 2, 1);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label10, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label64, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ll66d, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.ll1, 2, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 20);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 21;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(616, 546);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // ll7
            // 
            this.ll7.AutoSize = true;
            this.ll7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ll7.Location = new System.Drawing.Point(440, 180);
            this.ll7.Margin = new System.Windows.Forms.Padding(0);
            this.ll7.Name = "ll7";
            this.ll7.Size = new System.Drawing.Size(176, 20);
            this.ll7.TabIndex = 27;
            this.ll7.TabStop = true;
            this.ll7.Text = "0.00";
            this.ll7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll7.TextChanged += new System.EventHandler(this.ll7_TextChanged);
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label26.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(30, 180);
            this.label26.Margin = new System.Windows.Forms.Padding(0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(410, 20);
            this.label26.TabIndex = 26;
            this.label26.Text = "Gross income. Add line 5 and 6";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label25.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label25.Location = new System.Drawing.Point(0, 180);
            this.label25.Margin = new System.Windows.Forms.Padding(0);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(30, 20);
            this.label25.TabIndex = 25;
            this.label25.Text = "7";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll6
            // 
            this.ll6.AutoSize = true;
            this.ll6.BackColor = System.Drawing.Color.Transparent;
            this.ll6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll6.Location = new System.Drawing.Point(440, 160);
            this.ll6.Margin = new System.Windows.Forms.Padding(0);
            this.ll6.Name = "ll6";
            this.ll6.Size = new System.Drawing.Size(176, 20);
            this.ll6.TabIndex = 24;
            this.ll6.TabStop = true;
            this.ll6.Text = "0.00";
            this.ll6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll6.TextChanged += new System.EventHandler(this.ll6_TextChanged);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label24.Location = new System.Drawing.Point(30, 160);
            this.label24.Margin = new System.Windows.Forms.Padding(0);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(410, 20);
            this.label24.TabIndex = 23;
            this.label24.Text = "Other income, including federal and state gasoline or fuel tax credit or refund";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label23.Location = new System.Drawing.Point(0, 160);
            this.label23.Margin = new System.Windows.Forms.Padding(0);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(30, 20);
            this.label23.TabIndex = 22;
            this.label23.Text = "6";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll5
            // 
            this.ll5.AutoSize = true;
            this.ll5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll5.Location = new System.Drawing.Point(440, 140);
            this.ll5.Margin = new System.Windows.Forms.Padding(0);
            this.ll5.Name = "ll5";
            this.ll5.Size = new System.Drawing.Size(176, 20);
            this.ll5.TabIndex = 21;
            this.ll5.TabStop = true;
            this.ll5.Text = "0.00";
            this.ll5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll5.TextChanged += new System.EventHandler(this.ll5_TextChanged);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label22.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label22.Location = new System.Drawing.Point(30, 140);
            this.label22.Margin = new System.Windows.Forms.Padding(0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(410, 20);
            this.label22.TabIndex = 20;
            this.label22.Text = "Gross Profit (Subtract line 4 from line 3)";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label21.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label21.Location = new System.Drawing.Point(0, 140);
            this.label21.Margin = new System.Windows.Forms.Padding(0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(30, 20);
            this.label21.TabIndex = 19;
            this.label21.Text = "5";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll4
            // 
            this.ll4.AutoSize = true;
            this.ll4.BackColor = System.Drawing.Color.Transparent;
            this.ll4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll4.Location = new System.Drawing.Point(440, 120);
            this.ll4.Margin = new System.Windows.Forms.Padding(0);
            this.ll4.Name = "ll4";
            this.ll4.Size = new System.Drawing.Size(176, 20);
            this.ll4.TabIndex = 18;
            this.ll4.TabStop = true;
            this.ll4.Text = "0.00";
            this.ll4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll4.TextChanged += new System.EventHandler(this.ll4_TextChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label20.Location = new System.Drawing.Point(30, 120);
            this.label20.Margin = new System.Windows.Forms.Padding(0);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(410, 20);
            this.label20.TabIndex = 17;
            this.label20.Text = "Cost of goods sold";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.BackColor = System.Drawing.Color.Transparent;
            this.label19.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label19.Location = new System.Drawing.Point(0, 120);
            this.label19.Margin = new System.Windows.Forms.Padding(0);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(30, 20);
            this.label19.TabIndex = 16;
            this.label19.Text = "4";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll3
            // 
            this.ll3.AutoSize = true;
            this.ll3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll3.Location = new System.Drawing.Point(440, 100);
            this.ll3.Margin = new System.Windows.Forms.Padding(0);
            this.ll3.Name = "ll3";
            this.ll3.Size = new System.Drawing.Size(176, 20);
            this.ll3.TabIndex = 15;
            this.ll3.TabStop = true;
            this.ll3.Text = "0.00";
            this.ll3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll3.TextChanged += new System.EventHandler(this.ll3_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label18.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label18.Location = new System.Drawing.Point(30, 100);
            this.label18.Margin = new System.Windows.Forms.Padding(0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(410, 20);
            this.label18.TabIndex = 14;
            this.label18.Text = "Subtract line 2 from line 1";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label17.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label17.Location = new System.Drawing.Point(0, 100);
            this.label17.Margin = new System.Windows.Forms.Padding(0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(30, 20);
            this.label17.TabIndex = 13;
            this.label17.Text = "3";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll2
            // 
            this.ll2.AutoSize = true;
            this.ll2.BackColor = System.Drawing.Color.Transparent;
            this.ll2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll2.Location = new System.Drawing.Point(440, 80);
            this.ll2.Margin = new System.Windows.Forms.Padding(0);
            this.ll2.Name = "ll2";
            this.ll2.Size = new System.Drawing.Size(176, 20);
            this.ll2.TabIndex = 12;
            this.ll2.TabStop = true;
            this.ll2.Text = "0.00";
            this.ll2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll2.TextChanged += new System.EventHandler(this.ll2_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label16.Location = new System.Drawing.Point(30, 80);
            this.label16.Margin = new System.Windows.Forms.Padding(0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(410, 20);
            this.label16.TabIndex = 11;
            this.label16.Text = "Returns and allowances";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label15.Location = new System.Drawing.Point(0, 80);
            this.label15.Margin = new System.Windows.Forms.Padding(0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(30, 20);
            this.label15.TabIndex = 10;
            this.label15.Text = "2";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll1c
            // 
            this.ll1c.AutoSize = true;
            this.ll1c.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll1c.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll1c.Location = new System.Drawing.Point(440, 60);
            this.ll1c.Margin = new System.Windows.Forms.Padding(0);
            this.ll1c.Name = "ll1c";
            this.ll1c.Size = new System.Drawing.Size(176, 20);
            this.ll1c.TabIndex = 9;
            this.ll1c.TabStop = true;
            this.ll1c.Text = "0.00";
            this.ll1c.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll1c.TextChanged += new System.EventHandler(this.ll1c_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label14.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label14.Location = new System.Drawing.Point(30, 60);
            this.label14.Margin = new System.Windows.Forms.Padding(0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(410, 20);
            this.label14.TabIndex = 8;
            this.label14.Text = "    Tip Income";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label13.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label13.Location = new System.Drawing.Point(0, 60);
            this.label13.Margin = new System.Windows.Forms.Padding(0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(30, 20);
            this.label13.TabIndex = 7;
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label11.Location = new System.Drawing.Point(0, 40);
            this.label11.Margin = new System.Windows.Forms.Padding(0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 20);
            this.label11.TabIndex = 6;
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll1b
            // 
            this.ll1b.AutoSize = true;
            this.ll1b.BackColor = System.Drawing.Color.Transparent;
            this.ll1b.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll1b.Location = new System.Drawing.Point(440, 40);
            this.ll1b.Margin = new System.Windows.Forms.Padding(0);
            this.ll1b.Name = "ll1b";
            this.ll1b.Size = new System.Drawing.Size(176, 20);
            this.ll1b.TabIndex = 5;
            this.ll1b.TabStop = true;
            this.ll1b.Text = "0.00";
            this.ll1b.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll1b.TextChanged += new System.EventHandler(this.ll1b_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.Location = new System.Drawing.Point(30, 40);
            this.label12.Margin = new System.Windows.Forms.Padding(0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(410, 20);
            this.label12.TabIndex = 4;
            this.label12.Text = "    Sales Tax Collected";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll1a
            // 
            this.ll1a.AutoSize = true;
            this.ll1a.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ll1a.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll1a.Location = new System.Drawing.Point(440, 20);
            this.ll1a.Margin = new System.Windows.Forms.Padding(0);
            this.ll1a.Name = "ll1a";
            this.ll1a.Size = new System.Drawing.Size(176, 20);
            this.ll1a.TabIndex = 0;
            this.ll1a.TabStop = true;
            this.ll1a.Text = "0.00";
            this.ll1a.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll1a.TextChanged += new System.EventHandler(this.ll1a_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(0, 20);
            this.label9.Margin = new System.Windows.Forms.Padding(0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 20);
            this.label9.TabIndex = 1;
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(30, 20);
            this.label10.Margin = new System.Windows.Forms.Padding(0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(410, 20);
            this.label10.TabIndex = 2;
            this.label10.Text = "    Sales";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label64
            // 
            this.label64.AutoSize = true;
            this.label64.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label64.Location = new System.Drawing.Point(0, 0);
            this.label64.Margin = new System.Windows.Forms.Padding(0);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(30, 20);
            this.label64.TabIndex = 28;
            this.label64.Text = "1";
            this.label64.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll66d
            // 
            this.ll66d.AutoSize = true;
            this.ll66d.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll66d.Location = new System.Drawing.Point(30, 0);
            this.ll66d.Margin = new System.Windows.Forms.Padding(0);
            this.ll66d.Name = "ll66d";
            this.ll66d.Size = new System.Drawing.Size(410, 20);
            this.ll66d.TabIndex = 29;
            this.ll66d.Text = "Gross receipts or sales";
            this.ll66d.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ll1
            // 
            this.ll1.AutoSize = true;
            this.ll1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ll1.Location = new System.Drawing.Point(440, 0);
            this.ll1.Margin = new System.Windows.Forms.Padding(0);
            this.ll1.Name = "ll1";
            this.ll1.Size = new System.Drawing.Size(176, 20);
            this.ll1.TabIndex = 30;
            this.ll1.TabStop = true;
            this.ll1.Text = "0.00";
            this.ll1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ll1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ll1_LinkClicked);
            this.ll1.TextChanged += new System.EventHandler(this.ll1_TextChanged);
            // 
            // tpResearch
            // 
            this.tpResearch.Controls.Add(this.gvEBayResearch);
            this.tpResearch.Controls.Add(this.cmbStore);
            this.tpResearch.Controls.Add(this.label66);
            this.tpResearch.Controls.Add(this.btnResearch);
            this.tpResearch.Location = new System.Drawing.Point(4, 22);
            this.tpResearch.Name = "tpResearch";
            this.tpResearch.Padding = new System.Windows.Forms.Padding(3);
            this.tpResearch.Size = new System.Drawing.Size(1244, 634);
            this.tpResearch.TabIndex = 10;
            this.tpResearch.Text = "Research";
            this.tpResearch.UseVisualStyleBackColor = true;
            this.tpResearch.Enter += new System.EventHandler(this.tpResearch_Enter);
            // 
            // gvEBayResearch
            // 
            this.gvEBayResearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gvEBayResearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvEBayResearch.Location = new System.Drawing.Point(6, 35);
            this.gvEBayResearch.Name = "gvEBayResearch";
            this.gvEBayResearch.Size = new System.Drawing.Size(1232, 593);
            this.gvEBayResearch.TabIndex = 4;
            this.gvEBayResearch.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gvEBayResearch_CellClick);
            this.gvEBayResearch.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvEBayResearch_CellFormatting);
            // 
            // cmbStore
            // 
            this.cmbStore.FormattingEnabled = true;
            this.cmbStore.Location = new System.Drawing.Point(78, 8);
            this.cmbStore.Name = "cmbStore";
            this.cmbStore.Size = new System.Drawing.Size(267, 21);
            this.cmbStore.TabIndex = 3;
            this.cmbStore.SelectedIndexChanged += new System.EventHandler(this.cmbStore_SelectedIndexChanged);
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(3, 11);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(66, 13);
            this.label66.TabIndex = 2;
            this.label66.Text = "Store Name:";
            // 
            // btnResearch
            // 
            this.btnResearch.Location = new System.Drawing.Point(351, 6);
            this.btnResearch.Name = "btnResearch";
            this.btnResearch.Size = new System.Drawing.Size(75, 23);
            this.btnResearch.TabIndex = 0;
            this.btnResearch.Text = "Research";
            this.btnResearch.UseVisualStyleBackColor = true;
            this.btnResearch.Click += new System.EventHandler(this.btnResearch_Click);
            // 
            // tpMaintenance
            // 
            this.tpMaintenance.Controls.Add(this.btnImportEBayCatetories);
            this.tpMaintenance.Controls.Add(this.btnEBayItemSpecifics);
            this.tpMaintenance.Location = new System.Drawing.Point(4, 22);
            this.tpMaintenance.Name = "tpMaintenance";
            this.tpMaintenance.Padding = new System.Windows.Forms.Padding(3);
            this.tpMaintenance.Size = new System.Drawing.Size(1244, 634);
            this.tpMaintenance.TabIndex = 11;
            this.tpMaintenance.Text = "Maintenance";
            this.tpMaintenance.UseVisualStyleBackColor = true;
            // 
            // btnImportEBayCatetories
            // 
            this.btnImportEBayCatetories.Location = new System.Drawing.Point(6, 6);
            this.btnImportEBayCatetories.Name = "btnImportEBayCatetories";
            this.btnImportEBayCatetories.Size = new System.Drawing.Size(165, 23);
            this.btnImportEBayCatetories.TabIndex = 1;
            this.btnImportEBayCatetories.Text = "Import eBay Categories";
            this.btnImportEBayCatetories.UseVisualStyleBackColor = true;
            this.btnImportEBayCatetories.Click += new System.EventHandler(this.btnImportEBayCatetories_Click);
            // 
            // btnEBayItemSpecifics
            // 
            this.btnEBayItemSpecifics.Location = new System.Drawing.Point(6, 35);
            this.btnEBayItemSpecifics.Name = "btnEBayItemSpecifics";
            this.btnEBayItemSpecifics.Size = new System.Drawing.Size(165, 23);
            this.btnEBayItemSpecifics.TabIndex = 0;
            this.btnEBayItemSpecifics.Text = "Import eBay Item Specifics";
            this.btnEBayItemSpecifics.UseVisualStyleBackColor = true;
            this.btnEBayItemSpecifics.Click += new System.EventHandler(this.btnEBayItemSpecifics_Click);
            // 
            // costcoDataSet4
            // 
            this.costcoDataSet4.DataSetName = "CostcoDataSet4";
            this.costcoDataSet4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // eBay_CurrentListingsTableAdapter
            // 
            this.eBay_CurrentListingsTableAdapter.ClearBeforeFill = true;
            // 
            // eBay_ToChangeTableAdapter
            // 
            this.eBay_ToChangeTableAdapter.ClearBeforeFill = true;
            // 
            // eBay_SoldTransactionsTableAdapter
            // 
            this.eBay_SoldTransactionsTableAdapter.ClearBeforeFill = true;
            // 
            // productInfoTableAdapter1
            // 
            this.productInfoTableAdapter1.ClearBeforeFill = true;
            // 
            // productInfoBindingSource
            // 
            this.productInfoBindingSource.DataMember = "ProductInfo";
            this.productInfoBindingSource.DataSource = this.costcoDataSet4;
            // 
            // gvChange
            // 
            this.gvChange.AllowUserToAddRows = false;
            this.gvChange.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvChange.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ToChangeSelect});
            this.gvChange.Location = new System.Drawing.Point(6, 38);
            this.gvChange.Name = "gvChange";
            this.gvChange.Size = new System.Drawing.Size(1226, 300);
            this.gvChange.TabIndex = 7;
            // 
            // ToChangeSelect
            // 
            this.ToChangeSelect.HeaderText = "Select";
            this.ToChangeSelect.Name = "ToChangeSelect";
            this.ToChangeSelect.Width = 20;
            // 
            // eBayFrontEnd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1276, 684);
            this.Controls.Add(this.tabControl1);
            this.Name = "eBayFrontEnd";
            this.Text = "eBayFrontEnd";
            this.Load += new System.EventHandler(this.eBayFrontEnd_Load);
            this.tpTax.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvTaxExempt)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvSummary)).EndInit();
            this.tpSold.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvEBaySold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBaySoldTransactionsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsEBaySold)).EndInit();
            this.tpCurrentListing.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvCurrentListing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayCurrentListingsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dseBayCurrentListings)).EndInit();
            this.tpCostco.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvProducts)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tpDashboard.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.tpToAdd.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvAdd)).EndInit();
            this.tpEBayToModify.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvToChange)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eBayToChangeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet6)).EndInit();
            this.tpEbayToDelete.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvDelete)).EndInit();
            this.tpSaleTax.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gvSaleTaxHistory)).EndInit();
            this.tpIncomeTax.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tpResearch.ResumeLayout(false);
            this.tpResearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvEBayResearch)).EndInit();
            this.tpMaintenance.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.costcoDataSet4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.productInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvChange)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private CostcoDataSet4 costcoDataSet4;
        private dseBayCurrentListings dseBayCurrentListings;
        private System.Windows.Forms.BindingSource eBayCurrentListingsBindingSource;
        private dseBayCurrentListingsTableAdapters.eBay_CurrentListingsTableAdapter eBay_CurrentListingsTableAdapter;
        private CostcoDataSet6 costcoDataSet6;
        private System.Windows.Forms.BindingSource eBayToChangeBindingSource;
        private CostcoDataSet6TableAdapters.eBay_ToChangeTableAdapter eBay_ToChangeTableAdapter;
        private dsEBaySold dsEBaySold;
        private System.Windows.Forms.BindingSource eBaySoldTransactionsBindingSource;
        private dsEBaySoldTableAdapters.eBay_SoldTransactionsTableAdapter eBay_SoldTransactionsTableAdapter;
        private System.Windows.Forms.TabPage tpTax;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.DataGridView gvTaxExempt;
        private System.Windows.Forms.Button btnSendEmail;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Button btnGenerateFiles;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView gvSummary;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabPage tpSold;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView gvEBaySold;
        private System.Windows.Forms.DataGridViewTextBoxColumn paypalTransactionIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paypalPaidDateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn paypalPaidEmailPdfDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNumberDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBaySoldDateTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingQualityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBaySoldQualityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayRemainingQualityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBaySoldPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayUrlDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBaySoldEmailPdfDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerIDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerAddress1DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerAddress2DataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerStateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerEmailDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn buyerNoteDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlNumberDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoPriceDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoOrderNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoItemNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoItemNumberDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoTrackingNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoShipDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoTaxExemptPdfDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoOrderEmailPdfDataGridViewTextBoxColumn;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TabPage tpCurrentListing;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnReloadCurrentListing;
        private System.Windows.Forms.DataGridView gvCurrentListing;
        private System.Windows.Forms.DataGridViewCheckBoxColumn SelectListing;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayCategoryIDDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayListingPriceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayDescriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayEndTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoItemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLinkDataGridViewTextBoxColumn2;
        private System.Windows.Forms.Button btnListingModify;
        private System.Windows.Forms.Button btnListingDelete;
        private System.Windows.Forms.TabPage tpCostco;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView lvCategories;
        private System.Windows.Forms.ColumnHeader Select;
        private System.Windows.Forms.ColumnHeader Category_1;
        private System.Windows.Forms.ColumnHeader Category_2;
        private System.Windows.Forms.ColumnHeader Category_3;
        private System.Windows.Forms.ColumnHeader Category_4;
        private System.Windows.Forms.ColumnHeader Category_5;
        private System.Windows.Forms.ColumnHeader Category_6;
        private System.Windows.Forms.ColumnHeader Category_7;
        private System.Windows.Forms.ColumnHeader Category_8;
        private System.Windows.Forms.Button btnCostcoCategory;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btnAddPending;
        private System.Windows.Forms.Button btnRefreshProducts;
        private System.Windows.Forms.DataGridView gvProducts;
        private System.Windows.Forms.Button btnCrawl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpSaleTax;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.DataGridView gvSaleTaxHistory;
        private System.Windows.Forms.Button btnGenerateSaleTaxReport;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpSaleTaxTo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpSaleTaxFrom;
        private System.Windows.Forms.Button btnSaleTaxSave;
        private System.Windows.Forms.TabPage tpIncomeTax;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.ComboBox cmbIncomeTaxYear;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.LinkLabel ll1a;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.LinkLabel ll1b;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.LinkLabel ll1c;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.LinkLabel ll6;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.LinkLabel ll5;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.LinkLabel ll4;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.LinkLabel ll3;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.LinkLabel ll2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.LinkLabel ll7;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.LinkLabel ll16a;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.LinkLabel ll15;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.LinkLabel ll14;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.LinkLabel ll13;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.LinkLabel ll12;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.LinkLabel ll11;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.LinkLabel ll10;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.LinkLabel ll9;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.LinkLabel ll8;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.LinkLabel ll20b;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.LinkLabel ll20a;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.LinkLabel ll19;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.LinkLabel ll18;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.LinkLabel ll17;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.LinkLabel ll16b;
        private System.Windows.Forms.LinkLabel ll22;
        private System.Windows.Forms.Label label61;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.LinkLabel ll21;
        private System.Windows.Forms.LinkLabel ll23;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.LinkLabel ll26;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.LinkLabel ll25;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.LinkLabel ll24b;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.LinkLabel ll24a;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.LinkLabel ll29;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.LinkLabel ll28;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.LinkLabel ll27;
        private System.Windows.Forms.LinkLabel ll31;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.LinkLabel ll30;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label ll66d;
        private System.Windows.Forms.LinkLabel ll1;
        private System.Windows.Forms.Button btnIncomeTaxCalculate;
        private System.Windows.Forms.TabPage tpToAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.TabPage tpEBayToModify;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnToChangeUpdate;
        private System.Windows.Forms.Button btnToChangeDelete;
        private System.Windows.Forms.Button btnToChangeUpload;
        private System.Windows.Forms.DataGridView gvToChange;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoUrlNumberDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayItemNumberDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayOldListingPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayNewListingPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayReferencePriceDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoOldPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn costcoNewPriceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceChangeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn categoryDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn shippingDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn limitDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn discountDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn detailsDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn specificationDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLinkDataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberOfImageDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn eBayCategoryIDDataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionImageWidthDataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionImageHeightDataGridViewTextBoxColumn1;
        private System.Windows.Forms.TabPage tpEbayToDelete;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btnToDeleteUpload;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabPage tpResearch;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Button btnResearch;
        private System.Windows.Forms.DataGridView gvEBayResearch;
        private System.Windows.Forms.ComboBox cmbStore;
        private System.Windows.Forms.TabPage tpMaintenance;
        private System.Windows.Forms.Button btnEBayItemSpecifics;
        private System.Windows.Forms.DataGridView gvAdd;
        private System.Windows.Forms.Button btnImportEBayCatetories;
        private CostcoDataSet4TableAdapters.ProductInfoTableAdapter productInfoTableAdapter1;
        private System.Windows.Forms.BindingSource productInfoBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Check;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewLinkColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn imageLinkDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn UrlNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shippingDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Discount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Limit;
        private System.Windows.Forms.DataGridViewTextBoxColumn detailsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn specificationDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn urlDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageLink;
        private System.Windows.Forms.TabPage tpDashboard;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.LinkLabel llEBayOptions;
        private System.Windows.Forms.LinkLabel llEBayPriceDown;
        private System.Windows.Forms.LinkLabel llEBayPriceUp;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Label label92;
        private System.Windows.Forms.Label label90;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Label label82;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.LinkLabel llEBayDiscontinue;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label label94;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.Button btnChangeForEBayListings;
        private System.Windows.Forms.DataGridView gvDelete;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ToDeleteSelect;
        private System.Windows.Forms.DataGridView gvChange;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ToChangeSelect;
    }
}
﻿/*
Deployment script for CostcoDatabase_1

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "CostcoDatabase_1"
:setvar DefaultFilePrefix "CostcoDatabase_1"
:setvar DefaultDataPath "C:\Users\Jason Ding\AppData\Local\Microsoft\VisualStudio\SSDT\SampleScraperClient"
:setvar DefaultLogPath "C:\Users\Jason Ding\AppData\Local\Microsoft\VisualStudio\SSDT\SampleScraperClient"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS OFF,
                ANSI_PADDING OFF,
                ANSI_WARNINGS OFF,
                QUOTED_IDENTIFIER OFF,
                ANSI_NULL_DEFAULT OFF 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ALLOW_SNAPSHOT_ISOLATION ON;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET READ_COMMITTED_SNAPSHOT ON 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
USE [$(DatabaseName)];


GO
PRINT N'Creating [dbo].[_ProductInfo_Filtered]...';


GO
CREATE TABLE [dbo].[_ProductInfo_Filtered] (
    [ID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [UrlNumber]     NVARCHAR (MAX) NULL,
    [ItemNumber]    NVARCHAR (50)  NULL,
    [Category]      NVARCHAR (MAX) NULL,
    [Price]         MONEY          NULL,
    [Shipping]      SMALLMONEY     NULL,
    [Discount]      NVARCHAR (MAX) NOT NULL,
    [Details]       NVARCHAR (MAX) NULL,
    [Specification] NVARCHAR (MAX) NULL,
    [ImageLink]     NVARCHAR (MAX) NULL,
    [Url]           NVARCHAR (MAX) NULL
);


GO
PRINT N'Creating [dbo].[_Staging_ProductInfo_Filtered]...';


GO
CREATE TABLE [dbo].[_Staging_ProductInfo_Filtered] (
    [Name]          NVARCHAR (MAX) NULL,
    [UrlNumber]     NVARCHAR (MAX) NULL,
    [ItemNumber]    NVARCHAR (50)  NULL,
    [Category]      NVARCHAR (MAX) NULL,
    [Price]         MONEY          NULL,
    [Shipping]      SMALLMONEY     NULL,
    [Discount]      NVARCHAR (MAX) NULL,
    [Details]       NVARCHAR (MAX) NULL,
    [Specification] NVARCHAR (MAX) NULL,
    [ImageLink]     NVARCHAR (MAX) NULL,
    [Url]           NVARCHAR (MAX) NULL
);


GO
PRINT N'Creating [dbo].[Archieve]...';


GO
CREATE TABLE [dbo].[Archieve] (
    [ID]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (200)  NULL,
    [UrlNumber]     NVARCHAR (200)  NULL,
    [ItemNumber]    NVARCHAR (50)   NULL,
    [Category]      NVARCHAR (200)  NULL,
    [Price]         MONEY           NULL,
    [Shipping]      SMALLMONEY      NULL,
    [Limit]         INT             NULL,
    [Discount]      NVARCHAR (500)  NULL,
    [Details]       NVARCHAR (4000) NULL,
    [Specification] NVARCHAR (4000) NULL,
    [Url]           NVARCHAR (500)  NULL,
    [Options]       NVARCHAR (2000) NULL,
    [ImageLink]     NVARCHAR (2000) NULL,
    [ImageOptions]  NVARCHAR (4000) NULL,
    [NumberOfImage] INT             NULL,
    [ImportedDT]    DATETIME        NULL,
    [Thumb]         NVARCHAR (200)  NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[BookKeeping]...';


GO
CREATE TABLE [dbo].[BookKeeping] (
    [ID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Date]         DATE            NULL,
    [Name]         NVARCHAR (200)  NULL,
    [CategoryCode] NVARCHAR (10)   NULL,
    [Amount]       DECIMAL (18, 2) NULL,
    [Note]         NVARCHAR (200)  NULL,
    [Receipt]      NVARCHAR (500)  NULL,
    [Expense]      BIT             NULL,
    CONSTRAINT [PK_eBay_BookKeeping] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[Costco_Categories]...';


GO
CREATE TABLE [dbo].[Costco_Categories] (
    [ID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Category1] NVARCHAR (100) NULL,
    [Category2] NVARCHAR (100) NULL,
    [Category3] NVARCHAR (100) NULL,
    [Category4] NVARCHAR (100) NULL,
    [Category5] NVARCHAR (100) NULL,
    [Category6] NVARCHAR (100) NULL,
    [Category7] NVARCHAR (100) NULL,
    [Category8] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Costco_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[Costco_Departments]...';


GO
CREATE TABLE [dbo].[Costco_Departments] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [DepartmentName] NVARCHAR (100) NULL,
    [CategoryName]   NVARCHAR (100) NULL,
    [CategoryUrl]    NVARCHAR (500) NULL,
    [bInclude]       BIT            NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[Costco_eBay_Categories]...';


GO
CREATE TABLE [dbo].[Costco_eBay_Categories] (
    [ID]                   BIGINT         IDENTITY (1, 1) NOT NULL,
    [Category1]            NVARCHAR (100) NULL,
    [Category2]            NVARCHAR (100) NULL,
    [Category3]            NVARCHAR (100) NULL,
    [Category4]            NVARCHAR (100) NULL,
    [Category5]            NVARCHAR (100) NULL,
    [Category6]            NVARCHAR (100) NULL,
    [Category7]            NVARCHAR (100) NULL,
    [Category8]            NVARCHAR (100) NULL,
    [eBay_Category_Number] NVARCHAR (20)  NULL,
    [Template_Name]        NVARCHAR (100) NULL,
    [Specifics]            NVARCHAR (500) NULL,
    CONSTRAINT [PK_Costco_eBay_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[CostcoInventoryChange_Discontinue]...';


GO
CREATE TABLE [dbo].[CostcoInventoryChange_Discontinue] (
    [ID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (200) NULL,
    [CostcoUrl] NVARCHAR (500) NULL,
    [UrlNumber] NVARCHAR (200) NULL,
    [Thumb]     NVARCHAR (200) NULL
);


GO
PRINT N'Creating [dbo].[CostcoInventoryChange_New]...';


GO
CREATE TABLE [dbo].[CostcoInventoryChange_New] (
    [ID]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (200)  NULL,
    [UrlNumber]     NVARCHAR (200)  NULL,
    [ItemNumber]    NVARCHAR (50)   NULL,
    [Category]      NVARCHAR (200)  NULL,
    [Price]         MONEY           NULL,
    [Shipping]      SMALLMONEY      NULL,
    [Limit]         INT             NULL,
    [Discount]      NVARCHAR (500)  NULL,
    [Details]       NVARCHAR (4000) NULL,
    [Specification] NVARCHAR (4000) NULL,
    [CostcoUrl]     NVARCHAR (500)  NULL,
    [Options]       NVARCHAR (2000) NULL,
    [Thumb]         NVARCHAR (200)  NULL,
    [ImageLink]     NVARCHAR (2000) NULL,
    [ImageOptions]  NVARCHAR (4000) NULL,
    [NumberOfImage] INT             NULL,
    [InsertTime]    DATETIME        NULL,
    CONSTRAINT [PK_CostcoInventoryChange_New] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[CostcoInventoryChange_PriceDown]...';


GO
CREATE TABLE [dbo].[CostcoInventoryChange_PriceDown] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200)  NULL,
    [CostcoUrl]      NVARCHAR (500)  NULL,
    [UrlNumber]      NVARCHAR (200)  NULL,
    [CostcoOldPrice] MONEY           NULL,
    [CostcoNewPrice] MONEY           NULL,
    [ImageLink]      NVARCHAR (2000) NULL,
    [Thumb]          NVARCHAR (200)  NULL
);


GO
PRINT N'Creating [dbo].[CostcoInventoryChange_PriceUp]...';


GO
CREATE TABLE [dbo].[CostcoInventoryChange_PriceUp] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200)  NULL,
    [CostcoUrl]      NVARCHAR (500)  NULL,
    [UrlNumber]      NVARCHAR (200)  NULL,
    [CostcoOldPrice] MONEY           NULL,
    [CostcoNewPrice] MONEY           NULL,
    [ImageLink]      NVARCHAR (2000) NULL,
    [Thumb]          NVARCHAR (200)  NULL
);


GO
PRINT N'Creating [dbo].[DatabaseToUse]...';


GO
CREATE TABLE [dbo].[DatabaseToUse] (
    [ApplicationName]  NVARCHAR (100) NULL,
    [ConnectionString] NVARCHAR (500) NULL,
    [bUse]             BIT            NULL
);


GO
PRINT N'Creating [dbo].[Dev_CategoryUrlArray]...';


GO
CREATE TABLE [dbo].[Dev_CategoryUrlArray] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Dev_CategoryUrlArray_Staging]...';


GO
CREATE TABLE [dbo].[Dev_CategoryUrlArray_Staging] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Dev_ProductListPages]...';


GO
CREATE TABLE [dbo].[Dev_ProductListPages] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Dev_ProductListPages_Staging]...';


GO
CREATE TABLE [dbo].[Dev_ProductListPages_Staging] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Dev_ProductUrlArray]...';


GO
CREATE TABLE [dbo].[Dev_ProductUrlArray] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Dev_ProductUrlArray_Staging]...';


GO
CREATE TABLE [dbo].[Dev_ProductUrlArray_Staging] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[eBay_Categories]...';


GO
CREATE TABLE [dbo].[eBay_Categories] (
    [CategoryId]  NVARCHAR (255) NULL,
    [Category]    NVARCHAR (255) NULL,
    [F2]          NVARCHAR (255) NULL,
    [F3]          NVARCHAR (255) NULL,
    [F4]          NVARCHAR (255) NULL,
    [F5]          NVARCHAR (255) NULL,
    [F6]          NVARCHAR (255) NULL,
    [F7]          NVARCHAR (255) NULL,
    [F8]          NVARCHAR (255) NULL,
    [CategoryId1] NVARCHAR (255) NULL,
    [ParentId]    NVARCHAR (255) NULL,
    [Specifics]   NVARCHAR (255) NULL
);


GO
PRINT N'Creating [dbo].[eBay_CurrentListings]...';


GO
CREATE TABLE [dbo].[eBay_CurrentListings] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (200)  NULL,
    [eBayListingName]    NVARCHAR (200)  NULL,
    [eBayCategoryID]     NVARCHAR (50)   NULL,
    [eBayItemNumber]     NVARCHAR (50)   NULL,
    [eBayListingPrice]   MONEY           NULL,
    [eBayReferencePrice] MONEY           NULL,
    [eBayDescription]    NVARCHAR (MAX)  NULL,
    [eBayEndTime]        DATETIME        NULL,
    [eBayUrl]            NVARCHAR (500)  NULL,
    [eBayReferenceUrl]   NVARCHAR (500)  NULL,
    [CostcoUrlNumber]    NVARCHAR (50)   NULL,
    [CostcoItemNumber]   NVARCHAR (50)   NULL,
    [CostcoUrl]          NVARCHAR (500)  NULL,
    [CostcoPrice]        MONEY           NULL,
    [CostcoOptions]      NVARCHAR (2000) NULL,
    [Thumb]              NVARCHAR (200)  NULL,
    [ImageLink]          NVARCHAR (2000) NULL,
    [DeleteDT]           DATETIME        NULL,
    [InsertDT]           DATETIME        NULL,
    [PendingChange]      INT             NULL,
    [ImageOptions]       NVARCHAR (4000) NULL,
    CONSTRAINT [PK_eBay_CurrentListings] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBay_ProductsResearch]...';


GO
CREATE TABLE [dbo].[eBay_ProductsResearch] (
    [Name]           NVARCHAR (200) NULL,
    [eBayUrl]        NVARCHAR (500) NULL,
    [eBayPrice]      NVARCHAR (50)  NULL,
    [eBaySoldNumber] INT            NULL,
    [productUrl]     NVARCHAR (500) NULL,
    [productPrice]   MONEY          NULL,
    [eBayUserId]     NVARCHAR (200) NULL
);


GO
PRINT N'Creating [dbo].[eBay_SoldTransactions]...';


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER OFF;


GO
CREATE TABLE [dbo].[eBay_SoldTransactions] (
    [PaypalTransactionID]  NVARCHAR (30)  NULL,
    [PaypalPaidDateTime]   DATETIME       NULL,
    [PaypalPaidEmailPdf]   NVARCHAR (200) NULL,
    [eBayItemNumber]       NVARCHAR (50)  NULL,
    [eBaySoldDateTime]     DATETIME       NULL,
    [eBayItemName]         NVARCHAR (200) NULL,
    [eBayListingQuality]   INT            NULL,
    [eBaySoldQuality]      INT            NULL,
    [eBayRemainingQuality] INT            NULL,
    [eBaySoldVariation]    NVARCHAR (50)  NULL,
    [eBaySoldPrice]        MONEY          NULL,
    [eBaySaleTax]          MONEY          NULL,
    [eBayUrl]              NVARCHAR (500) NULL,
    [eBaySoldEmailPdf]     NVARCHAR (200) NULL,
    [BuyerName]            NVARCHAR (100) NULL,
    [BuyerID]              NVARCHAR (100) NULL,
    [BuyerAddress1]        NVARCHAR (200) NULL,
    [BuyerAddress2]        NVARCHAR (100) NULL,
    [BuyerCity]            NVARCHAR (50)  NULL,
    [BuyerState]           NVARCHAR (20)  NULL,
    [BuyerZip]             NVARCHAR (10)  NULL,
    [BuyerEmail]           NVARCHAR (100) NULL,
    [BuyerNote]            NVARCHAR (500) NULL,
    [CostcoUrlNumber]      NVARCHAR (50)  NULL,
    [CostcoUrl]            NVARCHAR (500) NULL,
    [CostcoPrice]          MONEY          NULL,
    [CostcoTax]            MONEY          NULL,
    [CostcoOrderNumber]    NVARCHAR (20)  NULL,
    [CostcoOrderDate]      DATE           NULL,
    [CostcoItemName]       NVARCHAR (200) NULL,
    [CostcoItemNumber]     NVARCHAR (20)  NULL,
    [CostcoOrderEmailPdf]  NVARCHAR (200) NULL,
    [CostcoTrackingNumber] NVARCHAR (30)  NULL,
    [CostcoShipDate]       NCHAR (10)     NULL,
    [CostcoShipEmailPdf]   NVARCHAR (200) NULL,
    [CostcoTaxExemptPdf]   NVARCHAR (200) NULL
);


GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;


GO
PRINT N'Creating [dbo].[eBay_ToAdd]...';


GO
CREATE TABLE [dbo].[eBay_ToAdd] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (200)  NULL,
    [eBayName]               NVARCHAR (80)   NULL,
    [UrlNumber]              NVARCHAR (200)  NULL,
    [ItemNumber]             NVARCHAR (50)   NULL,
    [Category]               NVARCHAR (200)  NULL,
    [Price]                  MONEY           NULL,
    [Shipping]               SMALLMONEY      NULL,
    [Limit]                  INT             NULL,
    [Discount]               NVARCHAR (500)  NULL,
    [Details]                NVARCHAR (4000) NULL,
    [Options]                NVARCHAR (2000) NULL,
    [Specification]          NVARCHAR (4000) NULL,
    [Url]                    NVARCHAR (500)  NULL,
    [Thumb]                  NVARCHAR (200)  NULL,
    [ImageLink]              NVARCHAR (2000) NULL,
    [NumberOfImage]          INT             NULL,
    [ImageOptions]           NVARCHAR (4000) NULL,
    [eBayCategoryID]         NVARCHAR (50)   NULL,
    [eBayReferencePrice]     MONEY           NULL,
    [eBayListingPrice]       MONEY           NULL,
    [eBayReferenceUrl]       NVARCHAR (500)  NULL,
    [DescriptionImageWidth]  INT             NULL,
    [DescriptionImageHeight] INT             NULL,
    [TemplateName]           NVARCHAR (100)  NULL,
    [Specifics]              NVARCHAR (500)  NULL,
    [InsertTime]             DATETIME        NULL,
    [DeleteTime]             DATETIME        NULL,
    CONSTRAINT [PK_eBayToAdd] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBay_ToChange]...';


GO
CREATE TABLE [dbo].[eBay_ToChange] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (200)  NULL,
    [CostcoUrlNumber]        NVARCHAR (50)   NULL,
    [eBayItemNumber]         NVARCHAR (50)   NULL,
    [eBayOldListingPrice]    MONEY           NULL,
    [eBayNewListingPrice]    MONEY           NULL,
    [eBayReferencePrice]     MONEY           NULL,
    [CostcoOldPrice]         MONEY           NULL,
    [CostcoNewPrice]         MONEY           NULL,
    [PriceChange]            NVARCHAR (10)   NULL,
    [Category]               NVARCHAR (200)  NULL,
    [Shipping]               SMALLMONEY      NULL,
    [Limit]                  INT             NULL,
    [Discount]               NVARCHAR (500)  NULL,
    [Details]                NVARCHAR (4000) NULL,
    [Specification]          NVARCHAR (4000) NULL,
    [Thumb]                  NVARCHAR (200)  NULL,
    [ImageLink]              NVARCHAR (2000) NULL,
    [NumberOfImage]          INT             NULL,
    [Url]                    NVARCHAR (500)  NULL,
    [eBayCategoryID]         NVARCHAR (50)   NULL,
    [DescriptionImageWidth]  INT             NULL,
    [DescriptionImageHeight] INT             NULL,
    [OldOptions]             NVARCHAR (2000) NULL,
    [NewOptions]             NVARCHAR (2000) NULL,
    [NewImageOptions]        NVARCHAR (4000) NULL,
    [InsertTime]             DATETIME        NULL,
    [DeleteTime]             DATETIME        NULL,
    CONSTRAINT [PK_eBay_ToChange] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBay_ToRemove]...';


GO
CREATE TABLE [dbo].[eBay_ToRemove] (
    [ID]               BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (200)  NULL,
    [eBayListingName]  NVARCHAR (200)  NULL,
    [eBayCategoryID]   NVARCHAR (50)   NULL,
    [eBayItemNumber]   NVARCHAR (50)   NULL,
    [eBayListingPrice] MONEY           NULL,
    [eBayDescription]  NVARCHAR (MAX)  NULL,
    [eBayEndTime]      DATETIME        NULL,
    [eBayUrl]          NVARCHAR (500)  NULL,
    [CostcoUrlNumber]  NVARCHAR (50)   NULL,
    [CostcoItemNumber] NVARCHAR (50)   NULL,
    [CostcoUrl]        NVARCHAR (500)  NULL,
    [CostcoPrice]      MONEY           NULL,
    [Thumb]            NVARCHAR (200)  NULL,
    [ImageLink]        NVARCHAR (2000) NULL,
    [InsertTime]       DATETIME        NULL,
    [DeleteTime]       DATETIME        NULL,
    CONSTRAINT [PK_eBay_ToRemove] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBayListingChange_Discontinue]...';


GO
CREATE TABLE [dbo].[eBayListingChange_Discontinue] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200)  NULL,
    [CostcoUrl]      NVARCHAR (200)  NULL,
    [UrlNumber]      NVARCHAR (200)  NULL,
    [eBayItemNumber] NVARCHAR (50)   NULL,
    [ImageLink]      NVARCHAR (2000) NULL,
    [Thumb]          NVARCHAR (200)  NULL,
    CONSTRAINT [PK_eBayListingChange_Discontinue] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBayListingChange_OptionChange]...';


GO
CREATE TABLE [dbo].[eBayListingChange_OptionChange] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (200)  NULL,
    [CostcoUrl]             NVARCHAR (200)  NULL,
    [UrlNumber]             NVARCHAR (200)  NULL,
    [eBayItemNumber]        NVARCHAR (50)   NULL,
    [CostcoOldOptions]      NVARCHAR (2000) NULL,
    [CostcoNewOptions]      NVARCHAR (2000) NULL,
    [CostcoNewImageOptions] NVARCHAR (4000) NULL,
    [ImageLink]             NVARCHAR (2000) NULL,
    [Thumb]                 NVARCHAR (200)  NULL
);


GO
PRINT N'Creating [dbo].[eBayListingChange_PriceDown]...';


GO
CREATE TABLE [dbo].[eBayListingChange_PriceDown] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (200)  NULL,
    [CostcoUrl]           NVARCHAR (200)  NULL,
    [UrlNumber]           NVARCHAR (200)  NULL,
    [eBayItemNumber]      NVARCHAR (50)   NULL,
    [CostcoOldPrice]      MONEY           NULL,
    [CostcoNewPrice]      MONEY           NULL,
    [eBayListingOldPrice] MONEY           NULL,
    [eBayListingNewPrice] MONEY           NULL,
    [ImageLink]           NVARCHAR (2000) NULL,
    [Thumb]               NVARCHAR (200)  NULL,
    CONSTRAINT [PK_eBayListingChange_PriceDown] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[eBayListingChange_PriceUp]...';


GO
CREATE TABLE [dbo].[eBayListingChange_PriceUp] (
    [ID]                  BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (200)  NULL,
    [CostcoUrl]           NVARCHAR (200)  NULL,
    [UrlNumber]           NVARCHAR (200)  NULL,
    [eBayItemNumber]      NVARCHAR (50)   NULL,
    [CostcoOldPrice]      MONEY           NULL,
    [CostcoNewPrice]      MONEY           NULL,
    [eBayListingOldPrice] MONEY           NULL,
    [eBayListingNewPrice] MONEY           NULL,
    [ImageLink]           NVARCHAR (2000) NULL,
    [Thumb]               NVARCHAR (200)  NULL,
    CONSTRAINT [PK_eBayListingChange_PriceUp] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[Import_Errors]...';


GO
CREATE TABLE [dbo].[Import_Errors] (
    [UrlNumber] NVARCHAR (50)  NULL,
    [Url]       NVARCHAR (500) NULL,
    [Exception] NVARCHAR (MAX) NULL
);


GO
PRINT N'Creating [dbo].[Import_Skips]...';


GO
CREATE TABLE [dbo].[Import_Skips] (
    [UrlNumber] NVARCHAR (50)  NULL,
    [Url]       NVARCHAR (500) NULL,
    [SkipPoint] NVARCHAR (50)  NULL
);


GO
PRINT N'Creating [dbo].[IncomeTax]...';


GO
CREATE TABLE [dbo].[IncomeTax] (
    [ID]           BIGINT         NULL,
    [Name]         NVARCHAR (200) NULL,
    [Amount]       MONEY          NULL,
    [DateTime]     DATETIME       NULL,
    [Income]       BIT            NULL,
    [CategoryCode] NVARCHAR (10)  NULL,
    [Receipt]      NVARCHAR (300) NULL,
    [Note]         NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[IncomeTaxCategory]...';


GO
CREATE TABLE [dbo].[IncomeTaxCategory] (
    [ID]   INT            NULL,
    [Code] NVARCHAR (10)  NULL,
    [Name] NVARCHAR (100) NULL
);


GO
PRINT N'Creating [dbo].[ProductInfo]...';


GO
CREATE TABLE [dbo].[ProductInfo] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200)  NULL,
    [UrlNumber]      NVARCHAR (200)  NULL,
    [ItemNumber]     NVARCHAR (50)   NULL,
    [Category]       NVARCHAR (200)  NULL,
    [Price]          MONEY           NULL,
    [Shipping]       SMALLMONEY      NULL,
    [Limit]          INT             NULL,
    [Discount]       NVARCHAR (500)  NULL,
    [Details]        NVARCHAR (4000) NULL,
    [Specification]  NVARCHAR (4000) NULL,
    [Url]            NVARCHAR (500)  NULL,
    [Options]        NVARCHAR (2000) NULL,
    [Thumb]          NVARCHAR (200)  NULL,
    [ImageLink]      NVARCHAR (2000) NULL,
    [ImageOptions]   NVARCHAR (4000) NULL,
    [NumberOfImage]  INT             NULL,
    [ImportedDT]     DATETIME        NULL,
    [eBayCategoryID] NVARCHAR (50)   NULL,
    CONSTRAINT [PK_ProductInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[ProductList]...';


GO
CREATE TABLE [dbo].[ProductList] (
    [Url] NVARCHAR (500) NULL
);


GO
PRINT N'Creating [dbo].[Raw_ProductInfo]...';


GO
CREATE TABLE [dbo].[Raw_ProductInfo] (
    [Name]          NVARCHAR (200)  NULL,
    [UrlNumber]     NVARCHAR (200)  NULL,
    [ItemNumber]    NVARCHAR (50)   NULL,
    [Category]      NVARCHAR (200)  NULL,
    [Price]         MONEY           NULL,
    [Shipping]      SMALLMONEY      NULL,
    [Limit]         INT             NULL,
    [Discount]      NVARCHAR (500)  NULL,
    [Details]       NVARCHAR (4000) NULL,
    [Specification] NVARCHAR (4000) NULL,
    [Url]           NVARCHAR (500)  NULL,
    [Options]       NVARCHAR (2000) NULL,
    [Thumb]         NVARCHAR (200)  NULL,
    [ImageLink]     NVARCHAR (2000) NULL,
    [ImageOptions]  NVARCHAR (4000) NULL,
    [NumberOfImage] INT             NULL
);


GO
PRINT N'Creating [dbo].[SaleTax]...';


GO
CREATE TABLE [dbo].[SaleTax] (
    [ReportID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [FromDate]             DATE            NULL,
    [ToDate]               DATE            NULL,
    [NumberOfTransactions] INT             NULL,
    [StateSaleTax]         NUMERIC (18, 2) NULL,
    [CitySaleTax]          NUMERIC (18, 2) NULL,
    [CountySaleTax]        NUMERIC (18, 2) NULL,
    [StateTaxSubmitted]    BIT             NULL,
    [CityTaxSubmitted]     BIT             NULL,
    [CountyTaxSubmitted]   BIT             NULL,
    CONSTRAINT [PK_SaleTax] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);


GO
PRINT N'Creating [dbo].[Staging_ProductInfo]...';


GO
CREATE TABLE [dbo].[Staging_ProductInfo] (
    [Name]          NVARCHAR (200)  NULL,
    [UrlNumber]     NVARCHAR (200)  NULL,
    [ItemNumber]    NVARCHAR (50)   NULL,
    [Category]      NVARCHAR (200)  NULL,
    [Price]         MONEY           NULL,
    [Shipping]      SMALLMONEY      NULL,
    [Limit]         INT             NULL,
    [Discount]      NVARCHAR (500)  NULL,
    [Details]       NVARCHAR (4000) NULL,
    [Specification] NVARCHAR (4000) NULL,
    [Url]           NVARCHAR (500)  NULL,
    [Options]       NVARCHAR (2000) NULL,
    [Thumb]         NVARCHAR (200)  NULL,
    [ImageLink]     NVARCHAR (2000) NULL,
    [ImageOptions]  NVARCHAR (4000) NULL,
    [NumberOfImage] INT             NULL
);


GO
PRINT N'Creating [dbo].[Tasks]...';


GO
CREATE TABLE [dbo].[Tasks] (
    [ID]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [ItemNumber]      NVARCHAR (50)  NULL,
    [Price]           MONEY          NULL,
    [VariationNumber] INT            NULL,
    [TaskName]        NVARCHAR (500) NULL,
    [Completed]       BIT            NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating [dbo].[TaxExemption]...';


GO
CREATE TABLE [dbo].[TaxExemption] (
    [FromDate]      DATE           NULL,
    [ToDate]        DATE           NULL,
    [TotalSell]     MONEY          NULL,
    [TotalCost]     MONEY          NULL,
    [TotalTax]      MONEY          NULL,
    [RefundableTax] MONEY          NULL,
    [TotalRevenue]  MONEY          NULL,
    [Report]        NVARCHAR (100) NULL,
    [Sent]          BIT            NULL,
    [Refund]        BIT            NULL,
    [ActualRefund]  MONEY          NULL
);


GO
PRINT N'Update complete.';


GO
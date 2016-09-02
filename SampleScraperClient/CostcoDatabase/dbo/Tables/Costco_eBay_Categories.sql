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






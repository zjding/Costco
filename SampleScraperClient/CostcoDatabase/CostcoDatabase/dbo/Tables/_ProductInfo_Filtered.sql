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


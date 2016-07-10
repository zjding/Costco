CREATE TABLE [dbo].[Staging_ProductInfo] (
    [Name]          NVARCHAR (MAX) NULL,
    [UrlNumber]     NVARCHAR (MAX) NULL,
    [ItemNumber]    NVARCHAR (50)  NULL,
    [Category]      NVARCHAR (MAX) NULL,
    [Price]         MONEY          NULL,
    [Shipping]      SMALLMONEY     NULL,
    [Limit]         INT            NULL,
    [Discount]      NVARCHAR (MAX) NULL,
    [Details]       NVARCHAR (MAX) NULL,
    [Specification] NVARCHAR (MAX) NULL,
    [ImageLink]     NVARCHAR (MAX) NULL,
    [Url]           NVARCHAR (MAX) NULL,
    [NumberOfImage] INT            NULL
);


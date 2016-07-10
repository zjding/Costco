CREATE TABLE [dbo].[ProductInfo] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (MAX) NULL,
    [UrlNumber]      NVARCHAR (MAX) NULL,
    [ItemNumber]     NVARCHAR (50)  NULL,
    [Category]       NVARCHAR (MAX) NULL,
    [Price]          MONEY          NULL,
    [Shipping]       SMALLMONEY     NULL,
    [Limit]          INT            NULL,
    [Discount]       NVARCHAR (MAX) NULL,
    [Details]        NVARCHAR (MAX) NULL,
    [Specification]  NVARCHAR (MAX) NULL,
    [ImageLink]      NVARCHAR (MAX) NULL,
    [Url]            NVARCHAR (MAX) NULL,
    [ImportedDT]     DATETIME       NULL,
    [eBayCategoryID] NVARCHAR (50)  NULL,
    [NumberOfImage]  INT            NULL,
    CONSTRAINT [PK_ProductInfo] PRIMARY KEY CLUSTERED ([ID] ASC)
);


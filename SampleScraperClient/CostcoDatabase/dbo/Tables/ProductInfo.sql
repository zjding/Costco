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








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








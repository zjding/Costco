CREATE TABLE [dbo].[CostcoInventoryChange_PriceUp] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200) NULL,
    [CostcoUrl]      NVARCHAR (200) NULL,
    [UrlNumber]      NVARCHAR (200) NULL,
    [CostcoOldPrice] MONEY          NULL,
    [CostcoNewPrice] MONEY          NULL
);


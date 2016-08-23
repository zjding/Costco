CREATE TABLE [dbo].[eBayListingChange_PriceDown] (
    [ID]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                NVARCHAR (200) NULL,
    [CostcoUrl]           NVARCHAR (200) NULL,
    [UrlNumber]           NVARCHAR (200) NULL,
    [eBayItemNumber]      NVARCHAR (50)  NULL,
    [CostcoOldPrice]      MONEY          NULL,
    [CostcoNewPrice]      MONEY          NULL,
    [eBayListingOldPrice] MONEY          NULL,
    [eBayListingNewPrice] MONEY          NULL
);


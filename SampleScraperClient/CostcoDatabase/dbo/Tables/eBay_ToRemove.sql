CREATE TABLE [dbo].[eBay_ToRemove] (
    [ID]               BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (MAX) NULL,
    [eBayListingName]  NVARCHAR (80)  NULL,
    [eBayCategoryID]   NVARCHAR (MAX) NULL,
    [eBayItemNumber]   NVARCHAR (MAX) NULL,
    [eBayListingPrice] MONEY          NULL,
    [eBayDescription]  NVARCHAR (MAX) NULL,
    [eBayEndTime]      DATETIME       NULL,
    [eBayUrl]          NVARCHAR (MAX) NULL,
    [CostcoUrlNumber]  NVARCHAR (MAX) NULL,
    [CostcoItemNumber] NVARCHAR (50)  NULL,
    [CostcoUrl]        NVARCHAR (MAX) NULL,
    [CostcoPrice]      MONEY          NULL,
    [ImageLink]        NVARCHAR (MAX) NULL
);


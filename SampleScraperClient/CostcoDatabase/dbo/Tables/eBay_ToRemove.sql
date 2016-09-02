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
    [ImageLink]        NVARCHAR (2000) NULL,
    [InsertTime]       DATETIME        NULL,
    [DeleteTime]       DATETIME        NULL
);






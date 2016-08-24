CREATE TABLE [dbo].[eBay_ToChange] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (MAX)  NULL,
    [CostcoUrlNumber]        NVARCHAR (MAX)  NULL,
    [eBayItemNumber]         NVARCHAR (50)   NULL,
    [eBayOldListingPrice]    MONEY           NULL,
    [eBayNewListingPrice]    MONEY           NULL,
    [eBayReferencePrice]     MONEY           NULL,
    [CostcoOldPrice]         MONEY           NULL,
    [CostcoNewPrice]         MONEY           NULL,
    [PriceChange]            NVARCHAR (10)   NULL,
    [Category]               NVARCHAR (MAX)  NULL,
    [Shipping]               SMALLMONEY      NULL,
    [Limit]                  INT             NULL,
    [Discount]               NVARCHAR (MAX)  NULL,
    [Details]                NVARCHAR (MAX)  NULL,
    [Specification]          NVARCHAR (MAX)  NULL,
    [ImageLink]              NVARCHAR (MAX)  NULL,
    [NumberOfImage]          INT             NULL,
    [Url]                    NVARCHAR (MAX)  NULL,
    [eBayCategoryID]         NVARCHAR (50)   NULL,
    [DescriptionImageWidth]  INT             NULL,
    [DescriptionImageHeight] INT             NULL,
    [OldOptions]             NVARCHAR (2000) NULL,
    [NewOptions]             NVARCHAR (2000) NULL
);




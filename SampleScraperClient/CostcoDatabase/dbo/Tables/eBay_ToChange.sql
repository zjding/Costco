CREATE TABLE [dbo].[eBay_ToChange] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (200)  NULL,
    [CostcoUrlNumber]        NVARCHAR (50)   NULL,
    [eBayItemNumber]         NVARCHAR (50)   NULL,
    [eBayOldListingPrice]    MONEY           NULL,
    [eBayNewListingPrice]    MONEY           NULL,
    [eBayReferencePrice]     MONEY           NULL,
    [CostcoOldPrice]         MONEY           NULL,
    [CostcoNewPrice]         MONEY           NULL,
    [PriceChange]            NVARCHAR (10)   NULL,
    [Category]               NVARCHAR (200)  NULL,
    [Shipping]               SMALLMONEY      NULL,
    [Limit]                  INT             NULL,
    [Discount]               NVARCHAR (500)  NULL,
    [Details]                NVARCHAR (4000) NULL,
    [Specification]          NVARCHAR (4000) NULL,
    [Thumb]                  NVARCHAR (200)  NULL,
    [ImageLink]              NVARCHAR (2000) NULL,
    [NumberOfImage]          INT             NULL,
    [Url]                    NVARCHAR (500)  NULL,
    [eBayCategoryID]         NVARCHAR (50)   NULL,
    [DescriptionImageWidth]  INT             NULL,
    [DescriptionImageHeight] INT             NULL,
    [OldOptions]             NVARCHAR (2000) NULL,
    [NewOptions]             NVARCHAR (2000) NULL,
    [NewImageOptions]        NVARCHAR (4000) NULL,
    [InsertTime]             DATETIME        NULL,
    [DeleteTime]             DATETIME        NULL,
    CONSTRAINT [PK_eBay_ToChange] PRIMARY KEY CLUSTERED ([ID] ASC)
);












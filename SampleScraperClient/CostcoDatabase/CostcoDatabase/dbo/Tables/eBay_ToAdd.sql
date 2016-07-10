CREATE TABLE [dbo].[eBay_ToAdd] (
    [ID]                     BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (MAX) NULL,
    [UrlNumber]              NVARCHAR (MAX) NULL,
    [ItemNumber]             NVARCHAR (50)  NULL,
    [Category]               NVARCHAR (MAX) NULL,
    [Price]                  MONEY          NULL,
    [Shipping]               SMALLMONEY     NULL,
    [Limit]                  INT            NULL,
    [Discount]               NVARCHAR (MAX) NULL,
    [Details]                NVARCHAR (MAX) NULL,
    [Specification]          NVARCHAR (MAX) NULL,
    [ImageLink]              NVARCHAR (MAX) NULL,
    [NumberOfImage]          INT            NULL,
    [Url]                    NVARCHAR (MAX) NULL,
    [eBayCategoryID]         NVARCHAR (50)  NULL,
    [eBayReferencePrice]     MONEY          NULL,
    [eBayListingPrice]       MONEY          NULL,
    [DescriptionImageWidth]  INT            NULL,
    [DescriptionImageHeight] INT            NULL,
    CONSTRAINT [PK_eBayToAdd] PRIMARY KEY CLUSTERED ([ID] ASC)
);


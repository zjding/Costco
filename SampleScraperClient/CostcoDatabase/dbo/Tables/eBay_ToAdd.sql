﻿CREATE TABLE [dbo].[eBay_ToAdd] (
    [ID]                     BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                   NVARCHAR (200)  NULL,
    [eBayName]               NVARCHAR (80)   NULL,
    [UrlNumber]              NVARCHAR (200)  NULL,
    [ItemNumber]             NVARCHAR (50)   NULL,
    [Category]               NVARCHAR (200)  NULL,
    [Price]                  MONEY           NULL,
    [Shipping]               SMALLMONEY      NULL,
    [Limit]                  INT             NULL,
    [Discount]               NVARCHAR (500)  NULL,
    [Details]                NVARCHAR (MAX)  NULL,
    [Options]                NVARCHAR (2000) NULL,
    [Specification]          NVARCHAR (4000) NULL,
    [Url]                    NVARCHAR (500)  NULL,
    [Thumb]                  NVARCHAR (200)  NULL,
    [ImageLink]              NVARCHAR (4000) NULL,
    [NumberOfImage]          INT             NULL,
    [ImageOptions]           NVARCHAR (4000) NULL,
    [eBayCategoryID]         NVARCHAR (50)   NULL,
    [eBayReferencePrice]     MONEY           NULL,
    [eBayListingPrice]       MONEY           NULL,
    [eBayReferenceUrl]       NVARCHAR (2000) NULL,
    [DescriptionImageWidth]  INT             NULL,
    [DescriptionImageHeight] INT             NULL,
    [ReviewImageWidth]       INT             NULL,
    [ReviewImageHeight]      INT             NULL,
    [RatingImageWidth]       INT             NULL,
    [RatingImageHeight]      INT             NULL,
    [Feature]                NVARCHAR (500)  NULL,
    [TemplateName]           NVARCHAR (100)  NULL,
    [Specifics]              NVARCHAR (500)  NULL,
    [InsertTime]             DATETIME        NULL,
    [DeleteTime]             DATETIME        NULL,
    [eBaySoldNumber]         INT             NULL,
    CONSTRAINT [PK_eBayToAdd] PRIMARY KEY CLUSTERED ([ID] ASC)
);
















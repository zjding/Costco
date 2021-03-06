﻿CREATE TABLE [dbo].[eBay_CurrentListings] (
    [ID]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]               NVARCHAR (200)  NULL,
    [eBayListingName]    NVARCHAR (200)  NULL,
    [eBayCategoryID]     NVARCHAR (50)   NULL,
    [eBayItemNumber]     NVARCHAR (50)   NULL,
    [eBayListingPrice]   MONEY           NULL,
    [eBayReferencePrice] MONEY           NULL,
    [eBayDescription]    NVARCHAR (MAX)  NULL,
    [eBayEndTime]        DATETIME        NULL,
    [eBayUrl]            NVARCHAR (500)  NULL,
    [eBayReferenceUrl]   NVARCHAR (500)  NULL,
    [CostcoUrlNumber]    NVARCHAR (50)   NULL,
    [CostcoItemNumber]   NVARCHAR (50)   NULL,
    [CostcoUrl]          NVARCHAR (500)  NULL,
    [CostcoPrice]        MONEY           NULL,
    [CostcoOptions]      NVARCHAR (2000) NULL,
    [Thumb]              NVARCHAR (200)  NULL,
    [ImageLink]          NVARCHAR (2000) NULL,
    [DeleteDT]           DATETIME        NULL,
    [InsertDT]           DATETIME        NULL,
    [PendingChange]      INT             NULL,
    [ImageOptions]       NVARCHAR (4000) NULL,
    CONSTRAINT [PK_eBay_CurrentListings] PRIMARY KEY CLUSTERED ([ID] ASC)
);












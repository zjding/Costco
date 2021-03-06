﻿--USE [Costco]
--GO

--/****** Object: Table [dbo].[eBay_SoldTransactions] Script Date: 7/11/2016 8:35:59 PM ******/
--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

CREATE TABLE [dbo].[eBay_SoldTransactions] (
    [PaypalTransactionID]  NVARCHAR (30)  NULL,
    [PaypalPaidDateTime]   DATETIME       NULL,
    [PaypalPaidEmailPdf]   NVARCHAR (200) NULL,
    [eBayItemNumber]       NVARCHAR (50)  NULL,
    [eBaySoldDateTime]     DATETIME       NULL,
    [eBayItemName]         NVARCHAR (200) NULL,
    [eBayListingQuality]   INT            NULL,
    [eBaySoldQuality]      INT            NULL,
    [eBayRemainingQuality] INT            NULL,
    [eBaySoldVariation]    NVARCHAR (50)  NULL,
    [eBaySoldPrice]        MONEY          NULL,
    [eBaySaleTax]          MONEY          NULL,
    [eBayUrl]              NVARCHAR (500) NULL,
    [eBaySoldEmailPdf]     NVARCHAR (200) NULL,
    [BuyerName]            NVARCHAR (100) NULL,
    [BuyerID]              NVARCHAR (100) NULL,
    [BuyerAddress1]        NVARCHAR (200) NULL,
    [BuyerAddress2]        NVARCHAR (100) NULL,
    [BuyerCity]            NVARCHAR (50)  NULL,
    [BuyerState]           NVARCHAR (20)  NULL,
    [BuyerZip]             NVARCHAR (10)  NULL,
    [BuyerEmail]           NVARCHAR (100) NULL,
    [BuyerNote]            NVARCHAR (500) NULL,
    [CostcoUrlNumber]      NVARCHAR (50)  NULL,
    [CostcoUrl]            NVARCHAR (500) NULL,
    [CostcoPrice]          MONEY          NULL,
    [CostcoTax]            MONEY          NULL,
    [CostcoOrderNumber]    NVARCHAR (20)  NULL,
    [CostcoOrderDate]      DATE           NULL,
    [CostcoItemName]       NVARCHAR (200) NULL,
    [CostcoItemNumber]     NVARCHAR (20)  NULL,
    [CostcoOrderEmailPdf]  NVARCHAR (200) NULL,
    [CostcoTrackingNumber] NVARCHAR (30)  NULL,
    [CostcoShipDate]       NCHAR (10)     NULL,
    [CostcoShipEmailPdf]   NVARCHAR (200) NULL,
    [CostcoTaxExemptPdf]   NVARCHAR (200) NULL,
    [CostcoOrderStatus]    NVARCHAR (20)  NULL
);













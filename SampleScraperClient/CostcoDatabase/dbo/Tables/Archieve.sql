﻿CREATE TABLE [dbo].[Archieve] (
    [ID]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (200)  NULL,
    [UrlNumber]     NVARCHAR (200)  NULL,
    [ItemNumber]    NVARCHAR (50)   NULL,
    [Category]      NVARCHAR (200)  NULL,
    [Price]         MONEY           NULL,
    [Shipping]      SMALLMONEY      NULL,
    [Limit]         INT             NULL,
    [Discount]      NVARCHAR (500)  NULL,
    [Details]       NVARCHAR (4000) NULL,
    [Specification] NVARCHAR (4000) NULL,
    [ImageLink]     NVARCHAR (4000) NULL,
    [Url]           NVARCHAR (500)  NULL,
    [ImportedDT]    DATETIME        NULL,
    [NumberOfImage] INT             NULL,
    [Options]       NVARCHAR (2000) NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([ID] ASC)
);




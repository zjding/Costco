CREATE TABLE [dbo].[Archieve] (
    [ID]            BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]          NVARCHAR (MAX) NULL,
    [UrlNumber]     NVARCHAR (MAX) NULL,
    [ItemNumber]    NVARCHAR (50)  NULL,
    [Category]      NVARCHAR (MAX) NULL,
    [Price]         MONEY          NULL,
    [Shipping]      SMALLMONEY     NULL,
    [Discount]      NVARCHAR (MAX) NULL,
    [Details]       NVARCHAR (MAX) NULL,
    [Specification] NVARCHAR (MAX) NULL,
    [ImageLink]     NVARCHAR (MAX) NULL,
    [Url]           NVARCHAR (MAX) NULL,
    [ImportedDT]    DATETIME       NULL,
    CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED ([ID] ASC)
);


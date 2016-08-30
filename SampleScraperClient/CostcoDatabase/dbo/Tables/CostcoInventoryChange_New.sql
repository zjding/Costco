CREATE TABLE [dbo].[CostcoInventoryChange_New] (
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
    [CostcoUrl]     NVARCHAR (500)  NULL,
    [Options]       NVARCHAR (2000) NULL,
    [ImageLink]     NVARCHAR (500)  NULL,
    [ImageOptions]  NVARCHAR (4000) NULL,
    [NumberOfImage] INT             NULL,
    [InsertTime]    DATETIME        NULL,
    CONSTRAINT [PK_CostcoInventoryChange_New] PRIMARY KEY CLUSTERED ([ID] ASC)
);




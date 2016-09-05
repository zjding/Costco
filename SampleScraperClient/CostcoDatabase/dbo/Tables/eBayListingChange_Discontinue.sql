CREATE TABLE [dbo].[eBayListingChange_Discontinue] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]           NVARCHAR (200) NULL,
    [CostcoUrl]      NVARCHAR (200) NULL,
    [UrlNumber]      NVARCHAR (200) NULL,
    [eBayItemNumber] NVARCHAR (50)  NULL,
    [ImageLink]      NVARCHAR (500) NULL,
    CONSTRAINT [PK_eBayListingChange_Discontinue] PRIMARY KEY CLUSTERED ([ID] ASC)
);






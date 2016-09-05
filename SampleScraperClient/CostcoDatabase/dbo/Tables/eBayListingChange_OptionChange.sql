CREATE TABLE [dbo].[eBayListingChange_OptionChange] (
    [ID]                    BIGINT          IDENTITY (1, 1) NOT NULL,
    [Name]                  NVARCHAR (200)  NULL,
    [CostcoUrl]             NVARCHAR (200)  NULL,
    [UrlNumber]             NVARCHAR (200)  NULL,
    [eBayItemNumber]        NVARCHAR (50)   NULL,
    [CostcoOldOptions]      NVARCHAR (2000) NULL,
    [CostcoNewOptions]      NVARCHAR (2000) NULL,
    [CostcoNewImageOptions] NVARCHAR (4000) NULL,
    [ImageLink]             NVARCHAR (500)  NULL
);






CREATE TABLE [dbo].[Costco_Categories] (
    [ID]        BIGINT         IDENTITY (1, 1) NOT NULL,
    [Category1] NVARCHAR (100) NULL,
    [Category2] NVARCHAR (100) NULL,
    [Category3] NVARCHAR (100) NULL,
    [Category4] NVARCHAR (100) NULL,
    [Category5] NVARCHAR (100) NULL,
    [Category6] NVARCHAR (100) NULL,
    [Category7] NVARCHAR (100) NULL,
    [Category8] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Costco_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);




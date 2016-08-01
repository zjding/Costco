CREATE TABLE [dbo].[IncomeTax] (
    [ID]           BIGINT         NULL,
    [Name]         NVARCHAR (200) NULL,
    [Amount]       MONEY          NULL,
    [DateTime]     DATETIME       NULL,
    [Income]       BIT            NULL,
    [CategoryCode] NVARCHAR (10)  NULL,
    [Receipt]      NVARCHAR (300) NULL,
    [Note]         NVARCHAR (500) NULL
);


CREATE TABLE [dbo].[BookKeeping] (
    [ID]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [Date]         DATE            NULL,
    [Name]         NVARCHAR (200)  NULL,
    [CategoryCode] NVARCHAR (10)   NULL,
    [Amount]       DECIMAL (18, 2) NULL,
    [Note]         NVARCHAR (200)  NULL,
    [Receipt]      NVARCHAR (500)  NULL,
    [Expense]      BIT             NULL,
    CONSTRAINT [PK_eBay_BookKeeping] PRIMARY KEY CLUSTERED ([ID] ASC)
);


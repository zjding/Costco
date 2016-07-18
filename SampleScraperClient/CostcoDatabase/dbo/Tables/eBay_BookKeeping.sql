CREATE TABLE [dbo].[eBay_BookKeeping] (
    [TransactionDate] DATE           NULL,
    [Amount]          DECIMAL (18)   NULL,
    [Note]            NVARCHAR (200) NULL,
    [Receipt]         NVARCHAR (500) NULL,
    [Gain]            BIT            NULL
);


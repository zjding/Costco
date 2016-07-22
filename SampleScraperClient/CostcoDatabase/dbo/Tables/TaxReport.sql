CREATE TABLE [dbo].[TaxReport] (
    [FromDate]      DATE           NULL,
    [ToDate]        DATE           NULL,
    [TotalSell]     MONEY          NULL,
    [TotalCost]     MONEY          NULL,
    [TotalTax]      MONEY          NULL,
    [RefundableTax] MONEY          NULL,
    [TotalRevenue]  MONEY          NULL,
    [Report]        NVARCHAR (100) NULL,
    [Sent]          BIT            NULL,
    [Refund]        BIT            NULL,
    [ActualRefund]  MONEY          NULL
);


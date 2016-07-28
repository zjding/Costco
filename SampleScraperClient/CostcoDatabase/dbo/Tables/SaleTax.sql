CREATE TABLE [dbo].[SaleTax] (
    [ReportID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [FromDate]             DATE            NULL,
    [ToDate]               DATE            NULL,
    [NumberOfTransactions] INT             NULL,
    [StateSaleTax]         NUMERIC (18, 2) NULL,
    [CitySaleTax]          NUMERIC (18, 2) NULL,
    [CountySaleTax]        NUMERIC (18, 2) NULL,
    [StateTaxSubmitted]    BIT             NULL,
    [CityTaxSubmitted]     BIT             NULL,
    [CountyTaxSubmitted]   BIT             NULL,
    CONSTRAINT [PK_SaleTax] PRIMARY KEY CLUSTERED ([ReportID] ASC)
);


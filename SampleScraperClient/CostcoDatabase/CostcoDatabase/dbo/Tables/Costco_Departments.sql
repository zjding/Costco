CREATE TABLE [dbo].[Costco_Departments] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [DepartmentName] NVARCHAR (MAX) NULL,
    [CategoryName]   NVARCHAR (MAX) NULL,
    [CategoryUrl]    NVARCHAR (MAX) NULL,
    [bInclude]       BIT            NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);


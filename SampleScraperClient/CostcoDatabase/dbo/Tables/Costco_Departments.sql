CREATE TABLE [dbo].[Costco_Departments] (
    [ID]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [DepartmentName] NVARCHAR (100) NULL,
    [CategoryName]   NVARCHAR (100) NULL,
    [CategoryUrl]    NVARCHAR (500) NULL,
    [bInclude]       BIT            NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED ([ID] ASC)
);




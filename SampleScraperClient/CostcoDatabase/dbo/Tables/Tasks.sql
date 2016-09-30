CREATE TABLE [dbo].[Tasks] (
    [ID]              BIGINT        IDENTITY (1, 1) NOT NULL,
    [ItemNumber]      NVARCHAR (50) NULL,
    [Price]           MONEY         NULL,
    [VariationNumber] INT           NULL,
    [TaskName]        NVARCHAR (50) NULL,
    [Completed]       BIT           NULL,
    CONSTRAINT [PK_Tasks] PRIMARY KEY CLUSTERED ([ID] ASC)
);


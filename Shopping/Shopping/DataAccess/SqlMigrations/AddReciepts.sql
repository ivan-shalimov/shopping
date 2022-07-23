BEGIN TRANSACTION;
GO

CREATE TABLE [ReceiptItems] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [ReceiptId] uniqueidentifier NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Amount] decimal(18,3) NOT NULL,
    CONSTRAINT [PK_ReceiptItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Receipts] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Total] decimal(18,3) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Receipts] PRIMARY KEY ([Id])
);
GO

INSERT INTO [dbo].[Receipts]([Id],[Description],[Total],[CreatedOn]) SELECT NEWID(), ' - ' AS [Description], SUM([Amount] * [Price]) AS Total, DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created])) AS [CreatedOn] FROM [dbo].[Purchases] GROUP BY DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created]));
GO

INSERT INTO [dbo].[ReceiptItems]([Id], [ReceiptId],[ProductId],[Price],[Amount]) SELECT [Id],[Id] AS [ReceiptId],[ProductId],[Amount],[Price] FROM [dbo].[Purchases];
GO

UPDATE RI SET RI.ReceiptId = R.Id FROM [dbo].[ReceiptItems] RI INNER JOIN [dbo].[Purchases] P ON RI.Id = P.Id INNER JOIN [dbo].[Receipts] R ON DATEFROMPARTS (DATEPART(yy, [Created]), DATEPART(mm, [Created]), DATEPART(dd, [Created])) = R.CreatedOn;
GO

CREATE INDEX [IX_ReceiptItems_ProductId] ON [ReceiptItems] ([ProductId]);
GO

CREATE INDEX [IX_ReceiptItems_ReceiptId] ON [ReceiptItems] ([ReceiptId]);
GO

DROP TABLE [Purchases];
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220212111444_AddReciepts', N'6.0.1');
GO

COMMIT;
GO


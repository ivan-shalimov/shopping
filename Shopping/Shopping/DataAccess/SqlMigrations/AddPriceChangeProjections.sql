BEGIN TRANSACTION;
GO

CREATE TABLE [PriceChangeProjections] (
    [Id] uniqueidentifier NOT NULL,
    [ProductId] uniqueidentifier NOT NULL,
    [ProductName] nvarchar(max) NULL,
    [ProductKindName] nvarchar(max) NULL,
    [Shop] nvarchar(450) NULL,
    [PreviousPrice] decimal(18,2) NOT NULL,
    [LastPrice] decimal(18,2) NOT NULL,
    [ChangePercent] decimal(18,2) NOT NULL,
    [ChangedDate] datetime2 NOT NULL,
    [Version] rowversion NULL,
    CONSTRAINT [PK_PriceChangeProjections] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_PriceChangeProjections_ProductId] ON [PriceChangeProjections] ([ProductId]);
GO

CREATE UNIQUE INDEX [IX_PriceChangeProjections_ProductId_Shop] ON [PriceChangeProjections] ([ProductId], [Shop]) WHERE [Shop] IS NOT NULL;
GO

CREATE INDEX [IX_PriceChangeProjections_Shop] ON [PriceChangeProjections] ([Shop]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240210095625_AddPriceChangeProjections', N'7.0.5');
GO

COMMIT;
GO


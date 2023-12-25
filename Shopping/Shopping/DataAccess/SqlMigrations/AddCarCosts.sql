BEGIN TRANSACTION;
GO

CREATE TABLE [CarCosts] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Amount] decimal(18,3) NOT NULL,
    [Date] datetime2 NOT NULL,
    CONSTRAINT [PK_CarCosts] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231224192404_AddCarCosts', N'7.0.5');
GO

COMMIT;
GO


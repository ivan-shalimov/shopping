BEGIN TRANSACTION;
GO

ALTER TABLE [ProductKinds] ADD [IsHiden] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [ProductKinds] ADD [IsMain] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

CREATE TABLE [Shops] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Shops] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230312085728_AddShopTableAndExtendKindWithFlags', N'7.0.3');
GO

COMMIT;
GO


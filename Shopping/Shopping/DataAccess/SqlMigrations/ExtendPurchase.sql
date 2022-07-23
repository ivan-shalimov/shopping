BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Purchases]') AND [c].[name] = N'Cost');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Purchases] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Purchases] DROP COLUMN [Cost];
GO

ALTER TABLE [Purchases] ADD [Amount] decimal(18,3) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [Purchases] ADD [Price] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220206101227_ExtendPurchase', N'6.0.1');
GO

COMMIT;
GO


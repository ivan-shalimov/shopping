BEGIN TRANSACTION;
GO

ALTER TABLE [Products] ADD [Hidden] bit NOT NULL DEFAULT CAST(0 AS bit);
GO

ALTER TABLE [Products] ADD [Type] nvarchar(max) NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230325174726_AddTypeAndHiddenFlagForProducts', N'7.0.4');
GO

COMMIT;
GO


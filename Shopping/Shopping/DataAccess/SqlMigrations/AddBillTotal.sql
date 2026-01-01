BEGIN TRANSACTION;
ALTER TABLE [Bills] ADD [Total] int NOT NULL DEFAULT 0;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260101101144_AddBillTotal', N'10.0.1');

COMMIT;
GO


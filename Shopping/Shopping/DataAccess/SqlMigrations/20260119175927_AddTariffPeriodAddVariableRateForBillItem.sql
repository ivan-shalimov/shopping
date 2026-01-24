BEGIN TRANSACTION;
ALTER TABLE [BillItems] ADD [Rate] decimal(18,5) NOT NULL DEFAULT 0.0;

GO -- Update existing BillItems with the Tariff Rate
UPDATE BI SET BI.Rate = T.Rate FROM BillItems BI INNER JOIN Tariffs T ON BI.TariffId = T.Id;

CREATE TABLE [TariffPeriods] (
    [Id] uniqueidentifier NOT NULL,
    [TariffId] uniqueidentifier NOT NULL,
    [Rate] decimal(18,5) NOT NULL,
    [StartOn] datetime2 NOT NULL,
    [EndOn] datetime2 NULL,
    CONSTRAINT [PK_TariffPeriods] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TariffPeriods_Tariffs_TariffId] FOREIGN KEY ([TariffId]) REFERENCES [Tariffs] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_TariffPeriods_TariffId] ON [TariffPeriods] ([TariffId]);

GO -- Migrate existing Tariff data to TariffPeriods
INSERT INTO TariffPeriods (Id, TariffId, Rate, StartOn, EndOn) SELECT NEWID(), Id, Rate, StartOn, EndOn FROM Tariffs;

DECLARE @var nvarchar(max);
SELECT @var = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tariffs]') AND [c].[name] = N'EndOn');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [Tariffs] DROP CONSTRAINT ' + @var + ';');
ALTER TABLE [Tariffs] DROP COLUMN [EndOn];

DECLARE @var1 nvarchar(max);
SELECT @var1 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tariffs]') AND [c].[name] = N'Rate');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Tariffs] DROP CONSTRAINT ' + @var1 + ';');
ALTER TABLE [Tariffs] DROP COLUMN [Rate];

DECLARE @var2 nvarchar(max);
SELECT @var2 = QUOTENAME([d].[name])
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tariffs]') AND [c].[name] = N'StartOn');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Tariffs] DROP CONSTRAINT ' + @var2 + ';');
ALTER TABLE [Tariffs] DROP COLUMN [StartOn];

ALTER TABLE [Tariffs] ADD [RateType] int NOT NULL DEFAULT 0;

GO -- Set existing Tariffs to Constant Rate (1)
UPDATE Tariffs SET RateType = 1;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260119175927_AddTariffPeriodAddVariableRateForBillItem', N'10.0.1');

COMMIT;
GO


BEGIN TRANSACTION;
GO

ALTER TABLE [Purchases] ADD [ProductId] uniqueidentifier NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000';
GO

CREATE TABLE [ProductKinds] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ProductKinds] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Products] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(max) NOT NULL,
    [ProductKindId] uniqueidentifier NOT NULL DEFAULT '272f0bd7-3896-41fe-8c96-b1772519d306',
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id])
);
GO

INSERT INTO[dbo].[ProductKinds] ([Id],[Name]) VALUES('272f0bd7-3896-41fe-8c96-b1772519d306', 'undefined');
GO

INSERT INTO [dbo].[Products] (Id, [Name]) SELECT NEWID() as Id, [Name] FROM [Purchases] group by [Name];
GO

UPDATE P SET P.[ProductId] = PR.Id FROM [Purchases] P INNER JOIN [Products] PR ON P.[Name] = PR.[Name];
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Purchases]') AND [c].[name] = N'Name');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Purchases] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Purchases] DROP COLUMN [Name];
GO

CREATE INDEX [IX_Purchases_ProductId] ON [Purchases] ([ProductId]);
GO

CREATE INDEX [IX_Products_ProductKindId] ON [Products] ([ProductKindId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220212080818_AddProducts', N'6.0.1');
GO

COMMIT;
GO


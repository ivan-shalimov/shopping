BEGIN TRANSACTION;
GO

CREATE TABLE [BillItems] (
    [Id] uniqueidentifier NOT NULL,
    [BillId] uniqueidentifier NOT NULL,
    [TariffId] uniqueidentifier NOT NULL,
    [PreviousValue] int NOT NULL,
    [Quantity] int NOT NULL,
    CONSTRAINT [PK_BillItems] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Bills] (
    [Id] uniqueidentifier NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [CreatedOn] datetime2 NOT NULL,
    CONSTRAINT [PK_Bills] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Tariffs] (
    [Id] uniqueidentifier NOT NULL,
    [GroupName] nvarchar(max) NOT NULL,
    [Description] nvarchar(max) NOT NULL,
    [Rate] decimal(18,5) NOT NULL,
    [Quantifiable] bit NOT NULL,
    [StartOn] datetime2 NOT NULL,
    [EndOn] datetime2 NULL,
    CONSTRAINT [PK_Tariffs] PRIMARY KEY ([Id])
);
GO

CREATE INDEX [IX_BillItems_BillId] ON [BillItems] ([BillId]);
GO

CREATE INDEX [IX_BillItems_TariffId] ON [BillItems] ([TariffId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240907175829_AddBillsInfrastructure', N'8.0.3');
GO

COMMIT;
GO


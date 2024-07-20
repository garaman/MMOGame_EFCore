IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Guild] (
    [GuildId] int NOT NULL IDENTITY,
    [GuildName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Guild] PRIMARY KEY ([GuildId])
);
GO

CREATE TABLE [Player] (
    [PlayerId] int NOT NULL IDENTITY,
    [Name] nvarchar(20) NOT NULL,
    [GuildId] int NOT NULL,
    CONSTRAINT [PK_Player] PRIMARY KEY ([PlayerId]),
    CONSTRAINT [FK_Player_Guild_GuildId] FOREIGN KEY ([GuildId]) REFERENCES [Guild] ([GuildId]) ON DELETE CASCADE
);
GO

CREATE TABLE [Item] (
    [ItemId] int NOT NULL IDENTITY,
    [SoftDeleted] bit NOT NULL,
    [TemplateId] int NOT NULL,
    [CreateTime] datetime2 NOT NULL,
    [OwnerId] int NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY ([ItemId]),
    CONSTRAINT [FK_Item_Player_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Player] ([PlayerId]) ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_Item_OwnerId] ON [Item] ([OwnerId]);
GO

CREATE UNIQUE INDEX [Index_Person_Name] ON [Player] ([Name]);
GO

CREATE INDEX [IX_Player_GuildId] ON [Player] ([GuildId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240720094727_HelloMigration', N'8.0.7');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

ALTER TABLE [Item] ADD [ItemGrade] int NOT NULL DEFAULT 0;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240720094828_ItemGrade', N'8.0.7');
GO

COMMIT;
GO


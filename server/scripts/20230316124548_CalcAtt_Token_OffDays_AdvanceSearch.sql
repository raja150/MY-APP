BEGIN TRANSACTION;
GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'ModifiedBy');
select @var0
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var0 + '];');
--ALTER TABLE [Users] ALTER COLUMN [ModifiedBy] nvarchar(16) NULL;
GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Users]') AND [c].[name] = N'CreatedBy');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Users] DROP CONSTRAINT [' + @var1 + '];');
--ALTER TABLE [Users] ALTER COLUMN [CreatedBy] nvarchar(16) NULL;
GO

ALTER TABLE [Org_WorkType] ADD [CalculateAtt] bit NOT NULL DEFAULT CAST(1 AS bit);
GO

ALTER TABLE [LM_AttendanceSum] ADD [NoOfLeaves] decimal(18,2) NOT NULL DEFAULT 0.0;
GO

ALTER TABLE [LM_AttendanceSum] ADD [OffDays] decimal(4,2) NOT NULL DEFAULT 0.0;
GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[_AppForms]') AND [c].[name] = N'Name');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [_AppForms] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [_AppForms] ALTER COLUMN [Name] nvarchar(50) NOT NULL;
GO

DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[_AppForms]') AND [c].[name] = N'JSON');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [_AppForms] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [_AppForms] ALTER COLUMN [JSON] nvarchar(max) NULL;
GO

DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[_AppForms]') AND [c].[name] = N'Header');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [_AppForms] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [_AppForms] ALTER COLUMN [Header] nvarchar(50) NOT NULL;
GO

DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[_AppForms]') AND [c].[name] = N'DisplayName');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [_AppForms] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [_AppForms] ALTER COLUMN [DisplayName] nvarchar(100) NOT NULL;
GO

ALTER TABLE [_AppForms] ADD [SearchJSON] nvarchar(max) NULL;
GO

CREATE TABLE [_Tokens] (
    [ID] uniqueidentifier NOT NULL,
    [Key] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    [AddedAt] datetime2 NOT NULL,
    [ModifiedAt] datetime2 NULL,
    [CreatedBy] nvarchar(16) NULL,
    [ModifiedBy] nvarchar(16) NULL,
    CONSTRAINT [PK__Tokens] PRIMARY KEY ([ID]),
    CONSTRAINT [FK__Tokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([ID]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX__Tokens_UserId] ON [_Tokens] ([UserId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230316124548_CalcAtt_Token_OffDays_AdvanceSearch', N'6.0.14');
GO

COMMIT;
GO


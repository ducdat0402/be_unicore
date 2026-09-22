-- Migrate announcement scopes to PUBLIC | DEPARTMENT | CLASS | STUDENT
-- Run on existing databases (SSDT recreate CHECK alone may not update live DB).

IF EXISTS (
    SELECT 1 FROM sys.check_constraints
    WHERE name = N'CK_announcements_scope_type' AND parent_object_id = OBJECT_ID(N'dbo.announcements')
)
BEGIN
    ALTER TABLE [dbo].[announcements] DROP CONSTRAINT [CK_announcements_scope_type];
END
GO

UPDATE [dbo].[announcements] SET [scope_type] = N'PUBLIC' WHERE [scope_type] = N'ALL';
UPDATE [dbo].[announcements] SET [scope_type] = N'DEPARTMENT' WHERE [scope_type] = N'MAJOR';
-- Legacy COHORT rows: map to CLASS only if you set a valid class id manually; otherwise set PUBLIC.
UPDATE [dbo].[announcements] SET [scope_type] = N'PUBLIC', [scope_value] = NULL WHERE [scope_type] = N'COHORT';
GO

ALTER TABLE [dbo].[announcements] WITH CHECK
ADD CONSTRAINT [CK_announcements_scope_type]
CHECK ([scope_type] IN ('PUBLIC', 'DEPARTMENT', 'CLASS', 'STUDENT'));
GO

IF OBJECT_ID(N'[dbo].[announcement_targets]', N'U') IS NOT NULL
BEGIN
    DROP TABLE [dbo].[announcement_targets];
END
GO

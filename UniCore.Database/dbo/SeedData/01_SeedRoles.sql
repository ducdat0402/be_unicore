PRINT 'Seeding Roles...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [name] = 'Admin')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-admin-001', 'ROL-00001', 'Admin', 'System Administrator with full permissions', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [name] = 'Lecturer')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-lecturer-002', 'ROL-00002', 'Lecturer', 'Academic instructor and course supervisor', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [name] = 'Student')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-student-003', 'ROL-00003', 'Student', 'Enrolled academic student', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[roles] WHERE [name] = 'Staff')
BEGIN
    INSERT INTO [dbo].[roles] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('role-staff-004', 'ROL-00004', 'Staff', 'Department academic staff', 1, 0);
END;

GO

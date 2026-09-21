PRINT 'Seeding School Classes...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [name] = 'SE1701')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('cls-se1701', 'CLS-00001', 'SE1701', 'Software Engineering Class Cohort 17 - Room 302', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [name] = 'SE1702')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('cls-se1702', 'CLS-00002', 'SE1702', 'Software Engineering Class Cohort 17 - Room 304', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[classes] WHERE [name] = 'IT1801')
BEGIN
    INSERT INTO [dbo].[classes] ([id], [code], [name], [description], [is_active], [is_deleted])
    VALUES ('cls-it1801', 'CLS-00003', 'IT1801', 'Information Technology Class Cohort 18 - Room 401', 1, 0);
END;

GO

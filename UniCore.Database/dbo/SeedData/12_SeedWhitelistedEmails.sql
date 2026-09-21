PRINT 'Seeding Whitelisted Emails...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [email] = 'student1@unicore.edu.vn')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std1-email', 'WLE-00001', 'student1@unicore.edu.vn', 1, 'usr-student-001', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[whitelisted_emails] WHERE [email] = 'student2@unicore.edu.vn')
BEGIN
    INSERT INTO [dbo].[whitelisted_emails] ([id], [code], [email], [is_confirmed], [student_id], [is_active], [is_deleted])
    VALUES ('wle-std2-email', 'WLE-00002', 'student2@unicore.edu.vn', 1, 'usr-student-002', 1, 0);
END;

GO

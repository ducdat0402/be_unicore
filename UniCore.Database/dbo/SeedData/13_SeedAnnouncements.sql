PRINT 'Seeding Announcements...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [title] = 'Welcome to Fall Semester 2026')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-welcome-001', 'ANC-00001', 'Welcome to Fall Semester 2026', 'Semester opening announcement', 'Welcome all students and faculty to Fall Semester 2026. Please check your course schedules.', 'NORMAL', 'PUBLISHED', 'ALL', 0, sysutcdatetime(), 'usr-admin-001');
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[announcements] WHERE [title] = 'Midterm Exam Schedule Notification')
BEGIN
    INSERT INTO [dbo].[announcements] ([id], [code], [title], [description], [content], [type], [status], [scope_type], [require_acknowledgement], [publish_date], [created_by])
    VALUES ('anc-exam-002', 'ANC-00002', 'Midterm Exam Schedule Notification', 'Important notice regarding midterm exams', 'Midterm examinations will take place from Oct 15 to Oct 22. Acknowledge this notice immediately.', 'IMPORTANT', 'PUBLISHED', 'COHORT', 1, sysutcdatetime(), 'usr-admin-001');
END;

GO

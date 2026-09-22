PRINT 'Seeding Announcement Student Read Receipts...';

-- PUBLIC announcements do not snapshot recipients; only CLASS/DEPARTMENT/STUDENT do.
IF NOT EXISTS (SELECT 1 FROM [dbo].[announcement_students] WHERE [announcement_id] = 'anc-exam-002' AND [student_id] = 'usr-student-001')
BEGIN
    INSERT INTO [dbo].[announcement_students] ([id], [announcement_id], [student_id], [viewed_at], [acknowledged_at])
    VALUES ('ast-std1-anc2', 'anc-exam-002', 'usr-student-001', sysutcdatetime(), sysutcdatetime());
END;

GO

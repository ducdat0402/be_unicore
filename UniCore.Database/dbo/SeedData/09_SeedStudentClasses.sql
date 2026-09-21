PRINT 'Seeding Student Classes...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [class_id] = 'cls-se1701' AND [student_id] = 'usr-student-001')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student1-se1701', 'SLC-00001', 'cls-se1701', 'usr-student-001', '2026-09-01', '2026-12-31', 8.50, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[student_classes] WHERE [class_id] = 'cls-se1701' AND [student_id] = 'usr-student-002')
BEGIN
    INSERT INTO [dbo].[student_classes] ([id], [code], [class_id], [student_id], [start_date], [end_date], [final_score], [supervisor], [status], [is_active], [is_deleted])
    VALUES ('slc-student2-se1701', 'SLC-00002', 'cls-se1701', 'usr-student-002', '2026-09-01', '2026-12-31', 9.20, 'Dr. Nguyen Vinh', 'ACTIVE', 1, 0);
END;

GO

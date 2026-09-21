PRINT 'Seeding Course Students...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[course_students] WHERE [course_id] = 'crs-dbms-002' AND [student_class_id] = 'slc-student1-se1701')
BEGIN
    INSERT INTO [dbo].[course_students] ([id], [code], [course_id], [student_class_id], [start_date], [end_date], [weight], [final_score], [status], [is_active], [is_deleted])
    VALUES ('cst-std1-dbms', 'CST-00001', 'crs-dbms-002', 'slc-student1-se1701', '2026-09-01', '2026-12-31', 3, 8.80, 'ENROLLED', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[course_students] WHERE [course_id] = 'crs-dsa-001' AND [student_class_id] = 'slc-student1-se1701')
BEGIN
    INSERT INTO [dbo].[course_students] ([id], [code], [course_id], [student_class_id], [start_date], [end_date], [weight], [final_score], [status], [is_active], [is_deleted])
    VALUES ('cst-std1-dsa', 'CST-00002', 'crs-dsa-001', 'slc-student1-se1701', '2026-09-01', '2026-12-31', 4, 9.50, 'ENROLLED', 1, 0);
END;

GO

PRINT 'Seeding Schedules...';

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [student_course_id] = 'cst-std1-dbms' AND [time_slot] = '08:00-10:15')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-dbms-tue', 'SCH-00001', 'cst-std1-dbms', '08:00-10:15', '2026-09-22 08:00:00', 1, 0);
END;

IF NOT EXISTS (SELECT 1 FROM [dbo].[schedules] WHERE [student_course_id] = 'cst-std1-dsa' AND [time_slot] = '13:30-15:45')
BEGIN
    INSERT INTO [dbo].[schedules] ([id], [code], [student_course_id], [time_slot], [day_occur], [is_active], [is_deleted])
    VALUES ('sch-dsa-thu', 'SCH-00002', 'cst-std1-dsa', '13:30-15:45', '2026-09-24 13:30:00', 1, 0);
END;

GO

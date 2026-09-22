/*
Post-Deployment Script UniCore
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.
 Use SQLCMD syntax to include a file in the post-deployment script.
 Example:      :r .\filepath.file
--------------------------------------------------------------------------------------
*/

PRINT 'Executing Post-Deployment Seed Scripts...';

:r .\SeedData\01_SeedRoles.sql
:r .\SeedData\02_SeedUsers.sql
:r .\SeedData\03_SeedPermissions.sql
:r .\SeedData\06_SeedDepartments.sql
:r .\SeedData\07_SeedCourses.sql
:r .\SeedData\08_SeedClasses.sql
:r .\SeedData\09_SeedStudentClasses.sql
:r .\SeedData\10_SeedCourseStudents.sql
:r .\SeedData\11_SeedSchedules.sql
:r .\SeedData\12_SeedWhitelistedEmails.sql
:r .\SeedData\16_MigrateAnnouncementScopes.sql
:r .\SeedData\13_SeedAnnouncements.sql
:r .\SeedData\14_SeedAnnouncementStudents.sql

PRINT 'Post-Deployment Seed Data complete.';
GO
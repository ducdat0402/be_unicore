CREATE TABLE [dbo].[schedules] (
    [id]                VARCHAR (50)  CONSTRAINT [DF_schedules_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [code]              VARCHAR (50)  NULL,
    [student_course_id] VARCHAR (50)  NOT NULL,
    [time_slot]         VARCHAR (20)  NOT NULL,
    [day_occur]         SMALLDATETIME NOT NULL,
    [is_active]         BIT           DEFAULT ((1)) NOT NULL,
    [is_deleted]        BIT           DEFAULT ((0)) NOT NULL,
    [created_at]        DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [updated_at]        DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    [created_by]        VARCHAR (50)  NULL,
    [updated_by]        VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_schedules_course_students] FOREIGN KEY ([student_course_id]) REFERENCES [dbo].[course_students] ([id]) ON DELETE CASCADE,
    CONSTRAINT [FK_schedules_created_by] FOREIGN KEY ([created_by]) REFERENCES [dbo].[users] ([id]),
    CONSTRAINT [FK_schedules_updated_by] FOREIGN KEY ([updated_by]) REFERENCES [dbo].[users] ([id])
);

CREATE TABLE [dbo].[announcement_email_whitelists] (
    [id]         VARCHAR (50)  CONSTRAINT [DF_announcement_email_whitelists_id] DEFAULT ([dbo].[fn_GenerateUUIDv7]()) NOT NULL,
    [type]       VARCHAR (20)  NOT NULL,
    [value]      VARCHAR (255) NOT NULL,
    [status]     VARCHAR (20)  DEFAULT ('ACTIVE') NOT NULL,
    [created_at] DATETIME2 (7) DEFAULT (sysutcdatetime()) NOT NULL,
    [updated_at] DATETIME2 (7) NULL,
    [created_by] VARCHAR (50)  NULL,
    [updated_by] VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [UQ_announcement_email_whitelists] UNIQUE ([type], [value]),
    CONSTRAINT [CK_announcement_email_whitelists_type] CHECK ([type] IN ('EMAIL', 'DOMAIN')),
    CONSTRAINT [CK_announcement_email_whitelists_status] CHECK ([status] IN ('ACTIVE', 'INACTIVE'))
);
GO

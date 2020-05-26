CREATE TABLE [dbo].[AuditLogs](
	[AuditLogId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Action] [varchar](50) NOT NULL,
	[Description] [nvarchar](300) NOT NULL,
	[IpAddress] [nvarchar](100) NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL);



USE [DyadAppDev]
GO

/****** Object:  Table [dbo].[ChatMessages]    Script Date: 5/20/2020 9:08:36 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ChatMessages](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiverId] [int] NOT NULL,
 CONSTRAINT [PK_Messenges] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD  CONSTRAINT [FK_MessengesReceiver_Users] FOREIGN KEY([ReceiverId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[ChatMessages] CHECK CONSTRAINT [FK_MessengesReceiver_Users]
GO

ALTER TABLE [dbo].[ChatMessages]  WITH CHECK ADD  CONSTRAINT [FK_MessengesSender_Users] FOREIGN KEY([SenderId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[ChatMessages] CHECK CONSTRAINT [FK_MessengesSender_Users]
GO



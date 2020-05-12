USE [DyadAppDev]
GO

/****** Object:  Table [dbo].[Matches]    Script Date: 5/12/2020 10:10:15 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Matches](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PrimaryUserID] [int] NOT NULL,
	[SecondaryUserID] [int] NOT NULL,
	[MatchedDate] [datetime] NULL,
 CONSTRAINT [PK_Matched] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matched_Users] FOREIGN KEY([SecondaryUserID])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matched_Users]
GO

ALTER TABLE [dbo].[Matches]  WITH CHECK ADD  CONSTRAINT [FK_Matched_UsersPrime] FOREIGN KEY([PrimaryUserID])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Matches] CHECK CONSTRAINT [FK_Matched_UsersPrime]
GO



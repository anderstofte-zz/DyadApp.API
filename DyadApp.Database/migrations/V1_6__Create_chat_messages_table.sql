CREATE TABLE [dbo].[ChatMessages](
	[ChatMessageId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[MatchId] [int] NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[SenderId] [int] NOT NULL,
	[ReceiverId] [int] NOT NULL,
	[IsRead] [bit] NOT NULL,
	[Modified] [datetime2](7) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	FOREIGN KEY (MatchId) REFERENCES Matches(MatchId));



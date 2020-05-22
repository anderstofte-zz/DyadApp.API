CREATE TABLE [dbo].[AwaitingMatches](   
      [AwaitingMatchId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [UserId] [int] NOT NULL,
	  [IsMatched] [bit] NOT NULL,
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL,
	  FOREIGN KEY (UserId) REFERENCES Users(UserId));



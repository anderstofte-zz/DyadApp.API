CREATE TABLE [dbo].[UserMatches](   
      [UserMatchId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [UserId] [int] NOT NULL,
	  [MatchId] [int] NOT NULL,
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL,
	  FOREIGN KEY (UserId) REFERENCES Users(UserId),
	  FOREIGN KEY (MatchId) REFERENCES Matches(MatchId));



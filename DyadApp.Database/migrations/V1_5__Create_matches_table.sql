CREATE TABLE [dbo].[Matches](   
      [MatchId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [PrimaryUserId] [int] NOT NULL,
	  [SecondaryUserId] [int] NOT NULL,
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL,
	  FOREIGN KEY (PrimaryUserId) REFERENCES Users(UserId),
	  FOREIGN KEY (SecondaryUserId) REFERENCES Users(UserId));
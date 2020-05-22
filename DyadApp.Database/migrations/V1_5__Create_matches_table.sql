CREATE TABLE [dbo].[Matches](   
      [MatchId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL);
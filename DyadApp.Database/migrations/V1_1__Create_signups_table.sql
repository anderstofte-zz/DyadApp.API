CREATE TABLE [dbo].[Signups](   
      [SignupId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [UserId] [int] NOT NULL,
	  [Token] [nvarchar](200) NOT NULL,
	  [ExpirationDate] [datetime2](7) NOT NULL,
	  [AcceptDate] [datetime2](7),
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL,
	  FOREIGN KEY (UserId) REFERENCES Users(UserId),
	  CONSTRAINT UC_Signups_Token UNIQUE ([Token]));
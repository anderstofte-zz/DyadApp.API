CREATE TABLE [dbo].[Users](   
      [UserId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
      [Name] [nvarchar](100) NOT NULL,
      [Email] [nvarchar](200) NOT NULL,
	  [Password] [nvarchar](100),
	  [DateOfBirth] [Date],
	  [ProfileImage] [varbinary](max) NOT NULL,
      [Modified] [datetime2](7) NOT NULL,
      [ModifiedBy] [int] NOT NULL,
      [Created] [datetime2](7) NOT NULL,
      [CreatedBy] [int] NOT NULL);



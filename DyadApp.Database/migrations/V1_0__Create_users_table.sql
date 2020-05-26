CREATE TABLE [dbo].[Users](   
	[UserId] [int] IDENTITY(1,1) PRIMARY KEY NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](200) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Salt] [varchar](100) NOT NULL,
	[DateOfBirth] [date],
	[ProfileImage] [varbinary](max) NOT NULL,
	[Verified] bit NOT NULL,
	[Active] bit NOT NULL,
	[Modified] [datetime2](7) NOT NULL,
	[ModifiedBy] [int] NOT NULL,
	[Created] [datetime2](7) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	CONSTRAINT UC_Users_Email UNIQUE ([Email]));

 
 
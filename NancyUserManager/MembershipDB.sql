/* Tables for Nancy User Manager App */


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
DROP TABLE [dbo].[Users]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Users](
	[Guid] [uniqueidentifier] NOT NULL,
	[Email] [varchar](320) NOT NULL,
	[FirstName] [nchar](50) NOT NULL,
	[LastName] [nchar](50) NOT NULL,
	[Hash] [char](60) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[LastUpdated] [datetime] NOT NULL,
	[LastUpdatedBy] [nchar](50) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


insert into users (Guid,Email,FirstName,LastName,Hash,CreateDate,Lastupdated,LastUpdatedBy)
values('4F63519E-C271-41C4-BE97-9BEEC98F3B1E','admin@admin.com','admin','user','$2a$10$/ZXTwmPLKqaw5ac5imwVb.WdaXArXZDS8sfhW0x0GJPbJQjZMWGq.',Getdate(),getdate(),'admin@admin.com')


select * from users
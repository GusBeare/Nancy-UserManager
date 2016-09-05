
/* drop both the tables if they exist */
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
	DROP TABLE [dbo].[UserRoles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
	DROP TABLE [dbo].[Roles]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
	DROP TABLE [dbo].[Users]
GO


CREATE TABLE [dbo].[Users](
	[Guid] [uniqueidentifier] NOT NULL,
	[Email] [varchar](320) NOT NULL,
	[FirstName] [nchar](50) NOT NULL,
	[LastName] [nchar](50) NOT NULL,
	[Hash] [char](60) NOT NULL,
	[FailedLogins][int] NULL,
	[LastFailedLoginDate] [datetime2] NULL,
	[LastFailedLoginIPAddress][VARCHAR](50) NULL,
	[LastSuccessfulLoginDate] [datetime2] NULL,
	[LastSuccessfulLoginIPAddress][VARCHAR](50) NULL,
	[CreatedDate] [datetime] NOT NULL,
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

-- create admin user
insert into users (Guid,Email,FirstName,LastName,Hash,CreatedDate,Lastupdated,LastUpdatedBy)
values('4F63519E-C271-41C4-BE97-9BEEC98F3B1E','admin@admin.com','admin','user','$2a$10$/ZXTwmPLKqaw5ac5imwVb.WdaXArXZDS8sfhW0x0GJPbJQjZMWGq.',Getdate(),getdate(),'admin@admin.com')

-- create viewer user
insert into users (Guid,Email,FirstName,LastName,Hash,CreatedDate,Lastupdated,LastUpdatedBy)
values('B049F476-9AF5-467E-84BC-3CDAF3A542E1','viewer@viewer.com','viewer','user','$2a$10$Tnz6ywr1zZXpyT.WZGOr0uzfO/EQ0gLnnMhO0653WKXPqBqQ7OjUK',Getdate(),getdate(),'admin@admin.com')

-- create editor user
insert into users (Guid,Email,FirstName,LastName,Hash,CreatedDate,Lastupdated,LastUpdatedBy)
values('D9CC9DEE-FF51-42F2-A5BE-331902D6F54C','editor@editor.com','editor','user','$2a$10$lsd/H0ESP3EP9c7d75Z.S.mo74oHHYZuoY1el1OI3RehatAuNL0aO',Getdate(),getdate(),'admin@admin.com')


/* create user roles */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserRoles](
	[UserGuid] [uniqueidentifier] NOT NULL,
	[RoleGuid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK__UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserGuid] ASC,
	[RoleGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


/* create roles */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[RoleGuid] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleGuid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

/* unique index on role guid plus name */
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND name = N'RoleGuid_RoleName_Unique')
CREATE UNIQUE NONCLUSTERED INDEX [RoleGuid_RoleName_Unique] ON [dbo].[Roles]
(
	[RoleGuid] ASC,
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

/* insert some roles */
insert into roles (RoleGuid,RoleName,Description)
values ('95BB5C68-30A7-4555-AA1D-64DFFD16AECC','Admin','Can do any admin task')
go

insert into roles (RoleGuid,RoleName,Description)
values ('77F7D3AC-847C-44F1-B547-6E1B45B90345','Editor','Can do some data management')
go

insert into roles (RoleGuid,RoleName,Description)
values ('80394D70-53A9-41FE-851E-0CE5BE976149','Viewer','Can only view data')
go

/* Insert User Roles */
/* the roles system is very simple; users only have one role, if they have no role they are the same as public users and 
can only view the home page */

/* admin user role */
insert into UserRoles (UserGuid,RoleGuid)
values ('4F63519E-C271-41C4-BE97-9BEEC98F3B1E','95BB5C68-30A7-4555-AA1D-64DFFD16AECC')
go

/* editor user role */
insert into UserRoles (UserGuid,RoleGuid)
values ('D9CC9DEE-FF51-42F2-A5BE-331902D6F54C','77F7D3AC-847C-44F1-B547-6E1B45B90345')
go

/* viewer user role */
insert into UserRoles (UserGuid,RoleGuid)
values ('B049F476-9AF5-467E-84BC-3CDAF3A542E1','80394D70-53A9-41FE-851E-0CE5BE976149')
go


/* now add the foreign keys */
alter table UserRoles
add constraint UR_UserID_FK FOREIGN KEY ([UserGuid]) references Users([Guid])
go

alter table UserRoles
add constraint UR_RoleGuid_FK FOREIGN KEY ([RoleGuid]) references Roles([RoleGuid])
go

--select * from users
--select * from roles
--select * from userRoles
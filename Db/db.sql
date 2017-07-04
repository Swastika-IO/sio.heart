USE [tts]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoleClaims]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoleClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](450) NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Name] [nvarchar](256) NULL,
	[NormalizedName] [nvarchar](256) NULL,
 CONSTRAINT [PK_AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserClaims]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[LoginProvider] [nvarchar](450) NOT NULL,
	[ProviderKey] [nvarchar](450) NOT NULL,
	[ProviderDisplayName] [nvarchar](max) NULL,
	[UserId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](450) NOT NULL,
	[RoleId] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](450) NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[ConcurrencyStamp] [nvarchar](max) NULL,
	[Email] [nvarchar](256) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[LockoutEnd] [datetimeoffset](7) NULL,
	[NormalizedEmail] [nvarchar](256) NULL,
	[NormalizedUserName] [nvarchar](256) NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[UserName] [nvarchar](256) NULL,
	[Avatar] [nvarchar](max) NULL,
	[CountryId] [int] NOT NULL,
	[Culture] [nvarchar](max) NULL,
	[DOB] [datetime2](7) NULL,
	[Gender] [nvarchar](max) NULL,
	[IsActived] [bit] NOT NULL,
	[JoinDate] [datetime2](7) NOT NULL,
	[LastModified] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](max) NULL,
	[NickName] [nvarchar](max) NULL,
	[RegisterType] [nvarchar](max) NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AspNetUserTokens]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserTokens](
	[UserId] [nvarchar](450) NOT NULL,
	[LoginProvider] [nvarchar](450) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Value] [nvarchar](max) NULL,
 CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Article]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Article](
	[Id] [int] NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
	[Menus] [nvarchar](50) NULL,
	[Image] [nvarchar](250) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Views] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[Title] [nvarchar](4000) NULL,
	[BriefContent] [nvarchar](max) NULL,
	[FullContent] [nvarchar](max) NULL,
	[SEName] [nvarchar](4000) NULL,
	[SEOTitle] [nvarchar](4000) NULL,
	[SEODescription] [nvarchar](4000) NULL,
	[SEOKeywords] [nvarchar](4000) NULL,
	[Source] [nvarchar](max) NULL,
	[Position] [int] NULL,
	[MenuId] [int] NULL,
	[Index] [int] NULL,
	[IsVisible] [bit] NOT NULL,
	[Hot] [bit] NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_TTS_Article] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Banner]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Banner](
	[Id] [nvarchar](128) NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
	[Url] [nvarchar](250) NOT NULL,
	[Alias] [nvarchar](250) NULL,
	[Image] [nvarchar](250) NOT NULL,
	[CreatedDate] [datetime] NULL,
	[IsPublished] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](450) NULL,
 CONSTRAINT [PK_TTS_Banner] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Comment]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Comment](
	[CommentId] [uniqueidentifier] NOT NULL,
	[ArticleId] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](250) NULL,
	[EditedBy] [nvarchar](250) NULL,
	[FullName] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Content] [nvarchar](max) NULL,
	[IsView] [bit] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Copy]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Copy](
	[Culture] [nvarchar](10) NOT NULL,
	[Keyword] [nvarchar](250) NOT NULL,
	[Value] [nvarchar](max) NULL,
	[Note] [nvarchar](250) NULL,
 CONSTRAINT [PK_TTX_Copy] PRIMARY KEY CLUSTERED 
(
	[Culture] ASC,
	[Keyword] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Culture]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Culture](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
	[LCID] [nvarchar](50) NULL,
	[Alias] [nvarchar](150) NULL,
	[FullName] [nvarchar](150) NULL,
	[Description] [nvarchar](250) NULL,
	[Icon] [nvarchar](50) NULL,
 CONSTRAINT [PK_TTS_Culture] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Menu]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Menu](
	[MenuId] [int] NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
	[Icon] [nvarchar](50) NULL,
	[Title] [nvarchar](4000) NULL,
	[Description] [nvarchar](max) NULL,
	[Views] [int] NULL,
	[Position] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
	[SEName] [nvarchar](4000) NULL,
	[SEOTitle] [nvarchar](4000) NULL,
	[SEODescription] [nvarchar](4000) NULL,
	[SEOKeywords] [nvarchar](4000) NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[Level] [int] NULL,
	[ParentMenuId] [int] NULL,
 CONSTRAINT [PK_TTS_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC,
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Menu_Article]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Menu_Article](
	[ArticleId] [int] NOT NULL,
	[MenuId] [int] NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_TTS_Menu_Article] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC,
	[MenuId] ASC,
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Menu_Menu]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Menu_Menu](
	[MenuId] [int] NOT NULL,
	[ParentId] [int] NOT NULL,
	[Specificulture] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_TTS_Menu_Menu] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC,
	[ParentId] ASC,
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[TTS_Parameter]    Script Date: 7/4/2017 2:18:37 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TTS_Parameter](
	[Name] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20161206204752_Initial', N'1.1.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20170626141230_aaa', N'1.1.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20170626142431_AddFirstName', N'1.1.2')
INSERT [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion]) VALUES (N'20170626144300_AddPolicy', N'1.1.2')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] ON 

INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (1, N'Add User', N'Add User', N'745e62c4-6fc7-4ff5-9e25-32c742f2580e')
INSERT [dbo].[AspNetUserClaims] ([Id], [ClaimType], [ClaimValue], [UserId]) VALUES (2, N'Edit User', N'Edit User', N'745e62c4-6fc7-4ff5-9e25-32c742f2580e')
SET IDENTITY_INSERT [dbo].[AspNetUserClaims] OFF
INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [Email], [EmailConfirmed], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName], [Avatar], [CountryId], [Culture], [DOB], [Gender], [IsActived], [JoinDate], [LastModified], [ModifiedBy], [NickName], [RegisterType], [FirstName], [LastName]) VALUES (N'745e62c4-6fc7-4ff5-9e25-32c742f2580e', 0, N'72e3d222-a10d-4fd4-8572-f2b4e29e3732', N'nhathoang989@gmail.com', 0, 1, NULL, N'NHATHOANG989@GMAIL.COM', N'NHATHOANG989@GMAIL.COM', N'AQAAAAEAACcQAAAAEOqxAEE6l/JhxNj/LL1pyx3I+ILvvasYMn3ywyf3Q6f5gBpE+WVzb1zqdWSfEaLcsg==', NULL, 0, N'350eea6d-81ca-4e3a-bea3-9f46b31cd795', 0, N'nhathoang989@gmail.com', NULL, 0, NULL, NULL, NULL, 0, CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), CAST(N'0001-01-01T00:00:00.0000000' AS DateTime2), NULL, N'tinku', NULL, N'Hoang', N'Nguyen')
SET IDENTITY_INSERT [dbo].[TTS_Culture] ON 

INSERT [dbo].[TTS_Culture] ([Id], [Specificulture], [LCID], [Alias], [FullName], [Description], [Icon]) VALUES (1, N'en-us', N'1033      ', N'English   ', N'English Us', N'Tiếng Anh', N'flag-icon-us')
INSERT [dbo].[TTS_Culture] ([Id], [Specificulture], [LCID], [Alias], [FullName], [Description], [Icon]) VALUES (2, N'vi-vn', NULL, N'Vietnamese', N'Vietnamese', N'Tiếng Việt', N'flag-icon-vn')
INSERT [dbo].[TTS_Culture] ([Id], [Specificulture], [LCID], [Alias], [FullName], [Description], [Icon]) VALUES (3, N'zh-TW', N'1028', N'Traditional Chinese', N'繁體中文', N'Tiếng Hoa', N'flag-icon-cn')
SET IDENTITY_INSERT [dbo].[TTS_Culture] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_TTS_Culture]    Script Date: 7/4/2017 2:18:37 PM ******/
ALTER TABLE [dbo].[TTS_Culture] ADD  CONSTRAINT [IX_TTS_Culture] UNIQUE NONCLUSTERED 
(
	[Specificulture] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [CountryId]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ((0)) FOR [IsActived]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('0001-01-01T00:00:00.000') FOR [JoinDate]
GO
ALTER TABLE [dbo].[AspNetUsers] ADD  DEFAULT ('0001-01-01T00:00:00.000') FOR [LastModified]
GO
ALTER TABLE [dbo].[TTS_Article] ADD  CONSTRAINT [DF_Articles_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TTS_Article] ADD  CONSTRAINT [DF_Articles_IsVisible]  DEFAULT ((1)) FOR [IsVisible]
GO
ALTER TABLE [dbo].[TTS_Article] ADD  CONSTRAINT [DF_Articles_Hot]  DEFAULT ((0)) FOR [Hot]
GO
ALTER TABLE [dbo].[TTS_Article] ADD  CONSTRAINT [DF_Articles_loai]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[TTS_Banner] ADD  CONSTRAINT [DF_TTS_Banner_CultureCode]  DEFAULT (N'en') FOR [Specificulture]
GO
ALTER TABLE [dbo].[TTS_Banner] ADD  CONSTRAINT [DF_TTS_Banner_Url]  DEFAULT (N'#') FOR [Url]
GO
ALTER TABLE [dbo].[TTS_Banner] ADD  CONSTRAINT [DF_TTS_Banner_IsPublished]  DEFAULT ((0)) FOR [IsPublished]
GO
ALTER TABLE [dbo].[TTS_Banner] ADD  CONSTRAINT [DF_TTS_Banner_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
ALTER TABLE [dbo].[TTS_Menu] ADD  CONSTRAINT [DF_Menus_Level]  DEFAULT ((0)) FOR [Level]
GO
ALTER TABLE [dbo].[TTS_Menu] ADD  CONSTRAINT [DF_Menus_ParentMenuId]  DEFAULT ((0)) FOR [ParentMenuId]
GO
ALTER TABLE [dbo].[AspNetRoleClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetRoleClaims] CHECK CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserClaims] CHECK CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[TTS_Article]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Article_TTS_Culture] FOREIGN KEY([Specificulture])
REFERENCES [dbo].[TTS_Culture] ([Specificulture])
GO
ALTER TABLE [dbo].[TTS_Article] CHECK CONSTRAINT [FK_TTS_Article_TTS_Culture]
GO
ALTER TABLE [dbo].[TTS_Banner]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Banner_TTS_Culture] FOREIGN KEY([Specificulture])
REFERENCES [dbo].[TTS_Culture] ([Specificulture])
GO
ALTER TABLE [dbo].[TTS_Banner] CHECK CONSTRAINT [FK_TTS_Banner_TTS_Culture]
GO
ALTER TABLE [dbo].[TTS_Menu]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Menu_TTS_Culture] FOREIGN KEY([Specificulture])
REFERENCES [dbo].[TTS_Culture] ([Specificulture])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TTS_Menu] CHECK CONSTRAINT [FK_TTS_Menu_TTS_Culture]
GO
ALTER TABLE [dbo].[TTS_Menu_Article]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Menu_Article_TTS_Article1] FOREIGN KEY([ArticleId], [Specificulture])
REFERENCES [dbo].[TTS_Article] ([Id], [Specificulture])
GO
ALTER TABLE [dbo].[TTS_Menu_Article] CHECK CONSTRAINT [FK_TTS_Menu_Article_TTS_Article1]
GO
ALTER TABLE [dbo].[TTS_Menu_Article]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Menu_Article_TTS_Menu] FOREIGN KEY([MenuId], [Specificulture])
REFERENCES [dbo].[TTS_Menu] ([MenuId], [Specificulture])
GO
ALTER TABLE [dbo].[TTS_Menu_Article] CHECK CONSTRAINT [FK_TTS_Menu_Article_TTS_Menu]
GO
ALTER TABLE [dbo].[TTS_Menu_Menu]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Menu_Menu_TTS_Menu] FOREIGN KEY([MenuId], [Specificulture])
REFERENCES [dbo].[TTS_Menu] ([MenuId], [Specificulture])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TTS_Menu_Menu] CHECK CONSTRAINT [FK_TTS_Menu_Menu_TTS_Menu]
GO
ALTER TABLE [dbo].[TTS_Menu_Menu]  WITH CHECK ADD  CONSTRAINT [FK_TTS_Menu_Menu_TTS_Menu1] FOREIGN KEY([ParentId], [Specificulture])
REFERENCES [dbo].[TTS_Menu] ([MenuId], [Specificulture])
GO
ALTER TABLE [dbo].[TTS_Menu_Menu] CHECK CONSTRAINT [FK_TTS_Menu_Menu_TTS_Menu1]
GO

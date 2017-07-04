/****** Object:  Table [dbo].[Constants]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Constants](
	[Name] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Constants] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parameters]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameters](
	[Name] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
 CONSTRAINT [PK_Parameters] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Menus]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Menus](
	[MenuId] [int] NOT NULL,
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
	[ParentIDSets] [varchar](50) NULL,
 CONSTRAINT [PK_Menus] PRIMARY KEY CLUSTERED 
(
	[MenuId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TeaBanners]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeaBanners](
	[TeaBannerId] [int] NOT NULL,
	[Page] [nvarchar](256) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
	[Pos] [int] NULL,
	[Description] [nvarchar](max) NULL,
	[priority] [int] NULL,
	[IsVisible] [bit] NOT NULL,
	[Name] [nvarchar](256) NULL,
	[Link] [nvarchar](256) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Banners] PRIMARY KEY CLUSTERED 
(
	[TeaBannerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TeaArticles]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TeaArticles](
	[ArticleId] [int] NOT NULL,
	[Menus] [nvarchar](50) NULL,
	[Image] [nvarchar](250) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Views] [int] NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](250) NOT NULL,
	[Title] [nvarchar](4000) NULL,
	[Company] [nvarchar](4000) NULL,
	[CompanyPosition] [nvarchar](4000) NULL,
	[Said] [nvarchar](max) NULL,
	[Tags] [nvarchar](max) NULL,
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
	[loai] [int] NOT NULL,
	[Name] [nvarchar](4000) NULL,
 CONSTRAINT [PK_TeaArticles] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventId] [int] NOT NULL,
	[Menus] [nvarchar](50) NULL,
	[Image] [nvarchar](250) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Views] [int] NULL,
	[FromDate] [datetime] NOT NULL,
	[ToDate] [datetime] NOT NULL,
	[Place] [nvarchar](4000) NULL,
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
	[loai] [int] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[EventId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Articles]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Articles](
	[ArticleId] [int] NOT NULL,
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
	[loai] [int] NOT NULL,
 CONSTRAINT [PK_Articles] PRIMARY KEY CLUSTERED 
(
	[ArticleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 11/19/2012 09:48:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
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
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Default [DF_Articles_IsDeleted]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Articles_IsVisible]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_IsVisible]  DEFAULT ((1)) FOR [IsVisible]
GO
/****** Object:  Default [DF_Articles_Hot]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_Hot]  DEFAULT ((0)) FOR [Hot]
GO
/****** Object:  Default [DF_Articles_loai]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Articles] ADD  CONSTRAINT [DF_Articles_loai]  DEFAULT ((0)) FOR [loai]
GO
/****** Object:  Default [DF_Events_IsDeleted]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Events_IsVisible]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_IsVisible]  DEFAULT ((1)) FOR [IsVisible]
GO
/****** Object:  Default [DF_Events_Hot]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_Hot]  DEFAULT ((0)) FOR [Hot]
GO
/****** Object:  Default [DF_Events_loai]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_loai]  DEFAULT ((0)) FOR [loai]
GO
/****** Object:  Default [DF_Menus_Level]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_Level]  DEFAULT ((0)) FOR [Level]
GO
/****** Object:  Default [DF_Menus_ParentMenuId]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Menus] ADD  CONSTRAINT [DF_Menus_ParentMenuId]  DEFAULT ((0)) FOR [ParentMenuId]
GO
/****** Object:  Default [DF_TeaArticles_IsDeleted]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaArticles] ADD  CONSTRAINT [DF_TeaArticles_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_TeaArticles_IsVisible]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaArticles] ADD  CONSTRAINT [DF_TeaArticles_IsVisible]  DEFAULT ((1)) FOR [IsVisible]
GO
/****** Object:  Default [DF_TeaArticles_Hot]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaArticles] ADD  CONSTRAINT [DF_TeaArticles_Hot]  DEFAULT ((0)) FOR [Hot]
GO
/****** Object:  Default [DF_TeaArticles_loai]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaArticles] ADD  CONSTRAINT [DF_TeaArticles_loai]  DEFAULT ((0)) FOR [loai]
GO
/****** Object:  Default [DF_Banners_IsVisible]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaBanners] ADD  CONSTRAINT [DF_Banners_IsVisible]  DEFAULT ((1)) FOR [IsVisible]
GO
/****** Object:  Default [DF_TeaBanners_IsDeleted]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaBanners] ADD  CONSTRAINT [DF_TeaBanners_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  ForeignKey [FK_Article_Menu]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Articles]  WITH CHECK ADD  CONSTRAINT [FK_Article_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([MenuId])
GO
ALTER TABLE [dbo].[Articles] CHECK CONSTRAINT [FK_Article_Menu]
GO
/****** Object:  ForeignKey [FK_Comment_Article]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comment_Article] FOREIGN KEY([ArticleId])
REFERENCES [dbo].[Articles] ([ArticleId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comment_Article]
GO
/****** Object:  ForeignKey [FK_Event_Menu]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Event_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([MenuId])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Event_Menu]
GO
/****** Object:  ForeignKey [FK_TeaArticle_Menu]    Script Date: 11/19/2012 09:48:13 ******/
ALTER TABLE [dbo].[TeaArticles]  WITH CHECK ADD  CONSTRAINT [FK_TeaArticle_Menu] FOREIGN KEY([MenuId])
REFERENCES [dbo].[Menus] ([MenuId])
GO
ALTER TABLE [dbo].[TeaArticles] CHECK CONSTRAINT [FK_TeaArticle_Menu]
GO

USE [Jobs]
GO

/****** Object:  Table [dbo].[Job]    Script Date: 21-10-2022 01:49:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Job](
	[JobId] [bigint] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[LocId] [bigint] NOT NULL,
	[DeptId] [bigint] NOT NULL,
	[ClosingDate] [datetime] NOT NULL,
	[PostDate] [datetime] NOT NULL,
	[JobCode] [nvarchar](100) NULL,
 CONSTRAINT [PK_Job] PRIMARY KEY CLUSTERED 
(
	[JobId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Dept] FOREIGN KEY([DeptId])
REFERENCES [dbo].[Department] ([DeptId])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Dept]
GO

ALTER TABLE [dbo].[Job]  WITH CHECK ADD  CONSTRAINT [FK_Job_Loc] FOREIGN KEY([LocId])
REFERENCES [dbo].[Location] ([LocId])
GO

ALTER TABLE [dbo].[Job] CHECK CONSTRAINT [FK_Job_Loc]
GO



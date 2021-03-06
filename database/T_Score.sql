USE [ZongCe]
GO
/****** Object:  Table [dbo].[T_Score]    Script Date: 2017/9/11 15:32:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[T_Score](
	[ID] [varchar](50) NOT NULL,
	[EvaID] [varchar](50) NOT NULL,
	[s1] [float] NOT NULL,
	[s2] [float] NOT NULL,
	[s3] [float] NOT NULL,
	[s4] [float] NOT NULL,
 CONSTRAINT [PK_T_Score] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[EvaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

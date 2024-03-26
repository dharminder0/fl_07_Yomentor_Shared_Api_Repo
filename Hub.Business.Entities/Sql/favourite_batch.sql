/****** Object:  Table [dbo].[favourite_batch]    Script Date: 26-03-2024 13:14:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[favourite_batch](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[userId] [int] NULL,
	[IsFavourite] [bit] NULL,
	[CreatedDate] [datetime] NULL,
	[EntityTypeId] [int] NULL,
	[EntityType] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



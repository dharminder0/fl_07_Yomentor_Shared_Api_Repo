/****** Object:  Table [dbo].[announcements]    Script Date: 26-03-2024 4.09.53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[announcements](
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [BatchId] INT NULL,
    [TeacherId] INT NULL,
    [Announcement] NVARCHAR(MAX) NULL,
    [CreatedOn] DATETIME NULL,
    [UpdatedOn] DATETIME NULL
);


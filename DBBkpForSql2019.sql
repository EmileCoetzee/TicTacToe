USE [master]
GO
/****** Object:  Database [TicTacToe]    Script Date: 2022/02/02 20:20:54 ******/
CREATE DATABASE [TicTacToe]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TicTacToe', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\TicTacToe.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'TicTacToe_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\TicTacToe_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [TicTacToe] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TicTacToe].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TicTacToe] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TicTacToe] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TicTacToe] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TicTacToe] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TicTacToe] SET ARITHABORT OFF 
GO
ALTER DATABASE [TicTacToe] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TicTacToe] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TicTacToe] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TicTacToe] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TicTacToe] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TicTacToe] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TicTacToe] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TicTacToe] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TicTacToe] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TicTacToe] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TicTacToe] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TicTacToe] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TicTacToe] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TicTacToe] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TicTacToe] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TicTacToe] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TicTacToe] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TicTacToe] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TicTacToe] SET  MULTI_USER 
GO
ALTER DATABASE [TicTacToe] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TicTacToe] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TicTacToe] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TicTacToe] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TicTacToe] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TicTacToe] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TicTacToe] SET QUERY_STORE = OFF
GO
USE [TicTacToe]
GO
/****** Object:  Table [dbo].[Games]    Script Date: 2022/02/02 20:20:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Games](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Player1Id] [int] NULL,
	[Player2Id] [int] NULL,
	[Player1Points] [int] NULL,
	[Player2Points] [int] NULL,
	[HighestRoundCompleted] [int] NULL,
	[Date] [datetime] NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Moves]    Script Date: 2022/02/02 20:20:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Moves](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GamesId] [int] NULL,
	[B1] [int] NULL,
	[B2] [int] NULL,
	[B3] [int] NULL,
	[B4] [int] NULL,
	[B5] [int] NULL,
	[B6] [int] NULL,
	[B7] [int] NULL,
	[B8] [int] NULL,
	[B9] [int] NULL,
 CONSTRAINT [PK_Move] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Players]    Script Date: 2022/02/02 20:20:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Players](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NULL,
	[TotalPoints] [int] NULL,
 CONSTRAINT [PK_Player] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Games] ON 

INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (1032, 1012, 1013, 32, 20, 3, CAST(N'2022-01-02T20:49:45.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2026, 2002, 2003, 13, 2, 1, CAST(N'2022-02-02T19:22:27.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2027, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:23:38.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2028, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:23:49.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2029, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:24:17.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2030, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:24:43.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (2031, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:25:06.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (3026, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:30:04.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (3027, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:30:27.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (3028, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:30:37.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (3029, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:30:56.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (3030, 2002, 2003, 13, 2, 1, CAST(N'2022-02-02T19:31:16.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (4026, 2002, 2003, 4, 14, 1, CAST(N'2022-02-02T19:32:41.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (4027, 2002, 2003, 18, 17, 2, CAST(N'2022-02-02T19:34:51.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (4028, 2002, 2003, 5, 4, 1, CAST(N'2022-02-02T19:36:05.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (4029, 2002, 2003, 15, 4, 1, CAST(N'2022-02-02T19:36:12.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (4030, 2002, 2003, 5, 4, 1, CAST(N'2022-02-02T19:36:22.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5026, 2002, 2003, 0, 0, 0, CAST(N'2022-02-02T19:40:00.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5027, 3002, 3003, 15, 4, 1, CAST(N'2022-02-02T19:40:21.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5028, 3002, 3003, 32, 9, 3, CAST(N'2022-02-02T19:49:16.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5029, 1013, 3004, 43, 10, 3, CAST(N'2022-02-02T20:12:18.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5030, 3005, 3006, 31, 8, 3, CAST(N'2022-02-02T20:14:01.000' AS DateTime))
INSERT [dbo].[Games] ([Id], [Player1Id], [Player2Id], [Player1Points], [Player2Points], [HighestRoundCompleted], [Date]) VALUES (5031, 3007, 3008, 0, 0, 0, CAST(N'2022-02-02T20:15:07.000' AS DateTime))
SET IDENTITY_INSERT [dbo].[Games] OFF
GO
SET IDENTITY_INSERT [dbo].[Moves] ON 

INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2002, 2026, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2003, 2027, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2004, 2028, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2005, 2029, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2006, 2030, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (2007, 2031, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (3002, 3026, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (3003, 3027, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (3004, 3028, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (3005, 3029, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (3006, 3030, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (4002, 4026, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (4003, 4027, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (4004, 4028, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (4005, 4029, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (4006, 4030, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (5002, 5026, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (5003, 5027, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Moves] ([Id], [GamesId], [B1], [B2], [B3], [B4], [B5], [B6], [B7], [B8], [B9]) VALUES (5007, 5031, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Moves] OFF
GO
SET IDENTITY_INSERT [dbo].[Players] ON 

INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (1012, N'Emile', 32)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (1013, N'Peter', 63)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (2002, N'Courtney', 0)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (2003, N'Jess', 0)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3002, N'Goose', 416)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3003, N'Chicken', 117)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3004, N'Jackson', 10)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3005, N'Bart', 31)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3006, N'Simpson', 8)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3007, N'Jesse', 0)
INSERT [dbo].[Players] ([Id], [Name], [TotalPoints]) VALUES (3008, N'James', 0)
SET IDENTITY_INSERT [dbo].[Players] OFF
GO
ALTER TABLE [dbo].[Games] ADD  CONSTRAINT [DF_Game_Player1Points]  DEFAULT ((0)) FOR [Player1Points]
GO
ALTER TABLE [dbo].[Games] ADD  CONSTRAINT [DF_Game_Player2Points]  DEFAULT ((0)) FOR [Player2Points]
GO
ALTER TABLE [dbo].[Games] ADD  CONSTRAINT [DF_Game_HighestRoundCompleted]  DEFAULT ((0)) FOR [HighestRoundCompleted]
GO
ALTER TABLE [dbo].[Players] ADD  CONSTRAINT [DF_Player_TotalPoints]  DEFAULT ((0)) FOR [TotalPoints]
GO
ALTER TABLE [dbo].[Games]  WITH CHECK ADD  CONSTRAINT [FK_Game_Player] FOREIGN KEY([Player1Id])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[Games] CHECK CONSTRAINT [FK_Game_Player]
GO
ALTER TABLE [dbo].[Games]  WITH CHECK ADD  CONSTRAINT [FK_Game_Player1] FOREIGN KEY([Player2Id])
REFERENCES [dbo].[Players] ([Id])
GO
ALTER TABLE [dbo].[Games] CHECK CONSTRAINT [FK_Game_Player1]
GO
ALTER TABLE [dbo].[Moves]  WITH CHECK ADD  CONSTRAINT [FK_Move_Games] FOREIGN KEY([GamesId])
REFERENCES [dbo].[Games] ([Id])
GO
ALTER TABLE [dbo].[Moves] CHECK CONSTRAINT [FK_Move_Games]
GO
USE [master]
GO
ALTER DATABASE [TicTacToe] SET  READ_WRITE 
GO

USE [master]
GO
/****** Object:  Database [karmajew_optionchain]    Script Date: 16-02-2025 09:33:33 PM ******/
CREATE DATABASE [karmajew_optionchain]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'karmajew_optionchain', FILENAME = N'E:\Program Files\MSSQL\DATA\karmajew_optionchain.mdf' , SIZE = 1182784KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'karmajew_optionchain_log', FILENAME = N'E:\Program Files\MSSQL\DATA\karmajew_optionchain_log.ldf' , SIZE = 52416KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [karmajew_optionchain] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [karmajew_optionchain].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [karmajew_optionchain] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET ARITHABORT OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [karmajew_optionchain] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [karmajew_optionchain] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [karmajew_optionchain] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET  ENABLE_BROKER 
GO
ALTER DATABASE [karmajew_optionchain] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [karmajew_optionchain] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [karmajew_optionchain] SET  MULTI_USER 
GO
ALTER DATABASE [karmajew_optionchain] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [karmajew_optionchain] SET DB_CHAINING OFF 
GO
ALTER DATABASE [karmajew_optionchain] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [karmajew_optionchain] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
USE [karmajew_optionchain]
GO
/****** Object:  User [karmajew_sa]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE USER [karmajew_sa] FOR LOGIN [karmajew_sa] WITH DEFAULT_SCHEMA=[karmajew_sa]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [karmajew_sa]
GO
ALTER ROLE [db_backupoperator] ADD MEMBER [karmajew_sa]
GO
ALTER ROLE [db_datareader] ADD MEMBER [karmajew_sa]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [karmajew_sa]
GO
/****** Object:  Schema [karmajew_sa]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE SCHEMA [karmajew_sa]
GO
/****** Object:  Table [karmajew_sa].[__EFMigrationsHistory]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[Advance]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[Advance](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Declines] [nvarchar](max) NULL,
	[Advances] [nvarchar](max) NULL,
	[Unchanged] [nvarchar](max) NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_Advance] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[AllOptionData]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[AllOptionData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Identifier] [nvarchar](max) NULL,
	[AskPrice] [float] NOT NULL,
	[AskQty] [int] NOT NULL,
	[BidPrice] [float] NOT NULL,
	[BidQty] [int] NOT NULL,
	[Change] [float] NOT NULL,
	[ChangeInOpenInterest] [float] NOT NULL,
	[ExpiryDate] [nvarchar](max) NULL,
	[ImpliedVolatility] [float] NOT NULL,
	[LastPrice] [float] NOT NULL,
	[OpenInterest] [float] NOT NULL,
	[PChange] [float] NOT NULL,
	[PChangeInOpenInterest] [float] NOT NULL,
	[StrikePrice] [float] NOT NULL,
	[TotalBuyQuantity] [int] NOT NULL,
	[TotalSellQuantity] [int] NOT NULL,
	[TotalTradedVolume] [int] NOT NULL,
	[Underlying] [nvarchar](max) NULL,
	[UnderlyingValue] [float] NOT NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_AllOptionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[BankExpiryOptionData]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[BankExpiryOptionData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Identifier] [nvarchar](max) NULL,
	[AskPrice] [float] NOT NULL,
	[AskQty] [int] NOT NULL,
	[BidPrice] [float] NOT NULL,
	[BidQty] [int] NOT NULL,
	[Change] [float] NOT NULL,
	[ChangeInOpenInterest] [float] NOT NULL,
	[ExpiryDate] [nvarchar](max) NULL,
	[ImpliedVolatility] [float] NOT NULL,
	[LastPrice] [float] NOT NULL,
	[OpenInterest] [float] NOT NULL,
	[PChange] [float] NOT NULL,
	[PChangeInOpenInterest] [float] NOT NULL,
	[StrikePrice] [float] NOT NULL,
	[TotalBuyQuantity] [int] NOT NULL,
	[TotalSellQuantity] [int] NOT NULL,
	[TotalTradedVolume] [int] NOT NULL,
	[Underlying] [nvarchar](max) NULL,
	[UnderlyingValue] [float] NOT NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_BankExpiryOptionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[BankOptionData]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[BankOptionData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Identifier] [nvarchar](max) NULL,
	[AskPrice] [float] NOT NULL,
	[AskQty] [int] NOT NULL,
	[BidPrice] [float] NOT NULL,
	[BidQty] [int] NOT NULL,
	[Change] [float] NOT NULL,
	[ChangeInOpenInterest] [float] NOT NULL,
	[ExpiryDate] [nvarchar](max) NULL,
	[ImpliedVolatility] [float] NOT NULL,
	[LastPrice] [float] NOT NULL,
	[OpenInterest] [float] NOT NULL,
	[PChange] [float] NOT NULL,
	[PChangeInOpenInterest] [float] NOT NULL,
	[StrikePrice] [float] NOT NULL,
	[TotalBuyQuantity] [int] NOT NULL,
	[TotalSellQuantity] [int] NOT NULL,
	[TotalTradedVolume] [int] NOT NULL,
	[Underlying] [nvarchar](max) NULL,
	[UnderlyingValue] [float] NOT NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_BankOptionData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[BankSummary]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[BankSummary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TotOICE] [float] NOT NULL,
	[TotOIPE] [float] NOT NULL,
	[TotVolCE] [float] NOT NULL,
	[TotVolPE] [float] NOT NULL,
	[CEPEOIDiff] [float] NOT NULL,
	[CEPEVolDiff] [float] NOT NULL,
	[CEPEOIPrevDiff] [float] NOT NULL,
	[CEPEVolPrevDiff] [float] NOT NULL,
	[Time] [time](7) NULL,
	[EntryDate] [datetime2](7) NULL,
 CONSTRAINT [PK_BankSummary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[BroderMarkets]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[BroderMarkets](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Key] [nvarchar](max) NULL,
	[Index] [nvarchar](max) NULL,
	[IndexSymbol] [nvarchar](max) NULL,
	[Last] [decimal](18, 2) NOT NULL,
	[Variation] [decimal](18, 2) NOT NULL,
	[PercentChange] [decimal](18, 2) NOT NULL,
	[Open] [decimal](18, 2) NOT NULL,
	[High] [decimal](18, 2) NOT NULL,
	[Low] [decimal](18, 2) NOT NULL,
	[PreviousClose] [decimal](18, 2) NOT NULL,
	[YearHigh] [decimal](18, 2) NOT NULL,
	[YearLow] [decimal](18, 2) NOT NULL,
	[IndicativeClose] [decimal](18, 2) NOT NULL,
	[PE] [nvarchar](10) NULL,
	[PB] [nvarchar](10) NULL,
	[DY] [nvarchar](10) NULL,
	[Declines] [nvarchar](max) NULL,
	[Advances] [nvarchar](max) NULL,
	[Unchanged] [nvarchar](max) NULL,
	[Date365dAgo] [nvarchar](max) NULL,
	[Chart365dPath] [nvarchar](max) NULL,
	[Date30dAgo] [nvarchar](max) NULL,
	[PerChange30d] [decimal](18, 2) NULL,
	[Chart30dPath] [nvarchar](max) NULL,
	[PreviousDay] [decimal](18, 2) NULL,
	[OneWeekAgo] [decimal](18, 2) NULL,
	[OneMonthAgo] [decimal](18, 2) NULL,
	[OneYearAgo] [decimal](18, 2) NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_BroderMarkets] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[CurrentExpiryOptionDaata]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[CurrentExpiryOptionDaata](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Identifier] [nvarchar](max) NULL,
	[AskPrice] [float] NOT NULL,
	[AskQty] [int] NOT NULL,
	[BidPrice] [float] NOT NULL,
	[BidQty] [int] NOT NULL,
	[Change] [float] NOT NULL,
	[ChangeInOpenInterest] [float] NOT NULL,
	[ExpiryDate] [nvarchar](max) NULL,
	[ImpliedVolatility] [float] NOT NULL,
	[LastPrice] [float] NOT NULL,
	[OpenInterest] [float] NOT NULL,
	[PChange] [float] NOT NULL,
	[PChangeInOpenInterest] [float] NOT NULL,
	[StrikePrice] [float] NOT NULL,
	[TotalBuyQuantity] [int] NOT NULL,
	[TotalSellQuantity] [int] NOT NULL,
	[TotalTradedVolume] [int] NOT NULL,
	[Underlying] [nvarchar](max) NULL,
	[UnderlyingValue] [float] NOT NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_CurrentExpiryOptionDaata] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[RFactors]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[RFactors](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [nvarchar](450) NULL,
	[DayHigh] [float] NOT NULL,
	[DayLow] [float] NOT NULL,
	[Price] [float] NOT NULL,
	[RFactor] [float] NOT NULL,
	[Time] [time](7) NULL,
	[EntryDate] [datetime2](7) NULL,
 CONSTRAINT [PK_RFactors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[SameOpenLowHigh]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[SameOpenLowHigh](
	[Id] [bigint] NOT NULL,
	[Symbol] [nvarchar](max) NULL,
	[PChange] [float] NULL,
	[LastPrice] [float] NULL,
	[Change] [float] NULL,
	[DayHigh] [float] NULL,
	[DayLow] [float] NULL,
	[TFactor] [float] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[Sectors]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[Sectors](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [nvarchar](max) NULL,
	[MappingName] [nvarchar](max) NULL,
	[Industry] [nvarchar](max) NULL,
 CONSTRAINT [PK_Sectors] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[Sessions]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[Sessions](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Cookie] [nvarchar](max) NULL,
	[UpdatedDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Sessions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[StockData]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[StockData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Priority] [int] NOT NULL,
	[Symbol] [nvarchar](450) NULL,
	[Identifier] [nvarchar](max) NULL,
	[Series] [nvarchar](max) NULL,
	[Open] [float] NOT NULL,
	[DayHigh] [float] NOT NULL,
	[DayLow] [float] NOT NULL,
	[LastPrice] [float] NOT NULL,
	[PreviousClose] [float] NOT NULL,
	[Change] [float] NOT NULL,
	[PChange] [float] NOT NULL,
	[TotalTradedVolume] [bigint] NOT NULL,
	[StockIndClosePrice] [float] NOT NULL,
	[TotalTradedValue] [float] NOT NULL,
	[LastUpdateTime] [nvarchar](max) NULL,
	[YearHigh] [float] NOT NULL,
	[Ffmc] [float] NOT NULL,
	[YearLow] [float] NOT NULL,
	[NearWKH] [float] NOT NULL,
	[NearWKL] [float] NOT NULL,
	[Date365dAgo] [nvarchar](max) NULL,
	[Chart365dPath] [nvarchar](max) NULL,
	[Date30dAgo] [nvarchar](max) NULL,
	[Chart30dPath] [nvarchar](max) NULL,
	[ChartTodayPath] [nvarchar](max) NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_StockData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[StockMetaData]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[StockMetaData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Symbol] [nvarchar](max) NULL,
	[CompanyName] [nvarchar](max) NULL,
	[Industry] [nvarchar](max) NULL,
	[IsFNOSec] [bit] NOT NULL,
	[IsCASec] [bit] NOT NULL,
	[IsSLBSec] [bit] NOT NULL,
	[IsDebtSec] [bit] NOT NULL,
	[IsSuspended] [bit] NOT NULL,
	[IsETFSec] [bit] NOT NULL,
	[IsDelisted] [bit] NOT NULL,
	[Isin] [nvarchar](max) NULL,
	[SlbIsin] [nvarchar](max) NULL,
	[ListingDate] [datetime2](7) NOT NULL,
	[IsMunicipalBond] [bit] NOT NULL,
	[EntryDate] [datetime2](7) NULL,
	[Time] [time](7) NULL,
 CONSTRAINT [PK_StockMetaData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[Summary]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[Summary](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TotOICE] [float] NOT NULL,
	[TotOIPE] [float] NOT NULL,
	[TotVolCE] [float] NOT NULL,
	[TotVolPE] [float] NOT NULL,
	[CEPEOIDiff] [float] NOT NULL,
	[CEPEVolDiff] [float] NOT NULL,
	[CEPEOIPrevDiff] [float] NOT NULL,
	[CEPEVolPrevDiff] [float] NOT NULL,
	[Time] [time](7) NULL,
	[EntryDate] [datetime2](7) NULL,
 CONSTRAINT [PK_Summary] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [karmajew_sa].[Users]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [karmajew_sa].[Users](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[GivenName] [nvarchar](max) NULL,
	[FamilyName] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NOT NULL,
	[ProfileImgeUrl] [nvarchar](max) NULL,
	[VerifiedEmail] [bit] NOT NULL,
	[LastUpdated] [datetime2](7) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_OptionDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_OptionDataIndex_EntryDate] ON [karmajew_sa].[AllOptionData]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BankExpiryOptionDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_BankExpiryOptionDataIndex_EntryDate] ON [karmajew_sa].[BankExpiryOptionData]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BankOptionDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_BankOptionDataIndex_EntryDate] ON [karmajew_sa].[BankOptionData]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_BroderMarketsIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_BroderMarketsIndex_EntryDate] ON [karmajew_sa].[BroderMarkets]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FilteredOptionDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_FilteredOptionDataIndex_EntryDate] ON [karmajew_sa].[CurrentExpiryOptionDaata]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_RFactorIndex_Symbol_EntryDate_Time]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_RFactorIndex_Symbol_EntryDate_Time] ON [karmajew_sa].[RFactors]
(
	[Symbol] ASC,
	[EntryDate] ASC,
	[Time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockDataIndex_EntryDate] ON [karmajew_sa].[StockData]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_StockDataIndex_Symbol_EntryDate_Time]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockDataIndex_Symbol_EntryDate_Time] ON [karmajew_sa].[StockData]
(
	[Symbol] ASC,
	[EntryDate] ASC,
	[Time] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_StockMetaDataIndex_EntryDate]    Script Date: 16-02-2025 09:33:34 PM ******/
CREATE NONCLUSTERED INDEX [IX_StockMetaDataIndex_EntryDate] ON [karmajew_sa].[StockMetaData]
(
	[EntryDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [karmajew_sa].[GetOpenLowHighStock]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [karmajew_sa].[GetOpenLowHighStock] 
	-- Add the parameters for the stored procedure here
	@currentDate VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @CurrentTime TIME;

	SELECT TOP 1 @CurrentTime = Time FROM StockData WHERE entrydate=@currentDate;
	PRINT @CurrentTime
	
    -- Insert statements for procedure here
	SELECT * FROM 
	(
		SELECT CONVERT(VARCHAR(5), sd.[Time], 108) Time, 'Positive' AS 'Type', sd.Id, sd.Symbol, LastPrice, PChange, Change, sd.DayHigh, sd.DayLow, [Open], 
		ROUND(CAST(ISNULL((SELECT TOP 1 RFactor FROM RFactors WHERE Symbol=sd.Symbol AND Entrydate=@currentDate),0.0) AS FLOAT),2) AS 'TFactor' 
		FROM stockdata sd
		WHERE sd.entrydate=@currentDate 
			AND [Open] = sd.DayLow 
			AND LastPrice > 500 
			AND PChange > 1 
			AND CONVERT(VARCHAR(5), sd.[Time], 108) LIKE CONVERT(VARCHAR(5), @CurrentTime, 108) + '%'

		UNION ALL

		SELECT CONVERT(VARCHAR(5), sd.[Time], 108) Time, 'Negetive' AS 'Type', sd.Id, sd.Symbol, LastPrice, PChange, Change, sd.DayHigh, sd.DayLow, [Open], 
		ROUND(CAST(ISNULL((SELECT TOP 1 RFactor FROM RFactors WHERE Symbol=sd.Symbol AND Entrydate=@currentDate),0.0) AS FLOAT),2) AS 'TFactor' 
		FROM stockdata sd
		WHERE sd.entrydate=@currentDate 
			AND [Open] = sd.DayHigh 
			AND LastPrice > 500 
			AND PChange < -1 
			AND CONVERT(VARCHAR(5), sd.[Time], 108) LIKE CONVERT(VARCHAR(5), @CurrentTime, 108) + '%'
		
	)T
	
	ORDER BY pChange DESC

END
GO
/****** Object:  StoredProcedure [karmajew_sa].[UpdateRelativeFactor]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [karmajew_sa].[UpdateRelativeFactor]
AS
BEGIN
    SET NOCOUNT ON;

    -- Temporary table to store the calculated relative factors
    CREATE TABLE #TempRFactor (
        Symbol NVARCHAR(50),
        DayHigh FLOAT,
        DayLow FLOAT,
        Price FLOAT,
        EntryDate DATE,
        Time TIME,
        RFactor FLOAT
    );

    -- Get the current date (ignoring time)
    DECLARE @CurrentDate DATE = CAST(GETDATE() AS DATE);

    -- Calculate the Relative Factor for each stock
    INSERT INTO #TempRFactor (Symbol, DayHigh, DayLow, Price, EntryDate, Time, RFactor)
    SELECT
        SD.Symbol,
        MAX(Last20Days.DayHigh) AS DayHigh,
        MIN(Last20Days.DayLow) AS DayLow,
        SD.LastPrice 'Price',
        CAST(SD.EntryDate AS DATE) AS EntryDate,
        SD.Time,
        (((MAX(Last20Days.DayHigh) - MIN(Last20Days.DayLow)) / SD.LastPrice) * 10) AS RFactor
    FROM
        StockData SD
    INNER JOIN (
        -- Fetch the last 20 days of data for each stock (excluding today)
        SELECT 
            Symbol,
            DayHigh,
            DayLow,
            EntryDate
        FROM StockData
        WHERE EntryDate < @CurrentDate
          AND EntryDate >= DATEADD(DAY, -31, @CurrentDate)
    ) AS Last20Days
    ON SD.Symbol = Last20Days.Symbol
    WHERE SD.EntryDate = @CurrentDate -- Only process current day's data
    GROUP BY SD.Symbol, SD.EntryDate, SD.Time, SD.LastPrice;

    -- Update the RFactorTable with the calculated Relative Factors
    MERGE INTO RFactors AS Target
    USING #TempRFactor AS Source
    ON Target.Symbol = Source.Symbol
       AND Target.EntryDate = Source.EntryDate
       AND Target.Time = Source.Time
    WHEN MATCHED THEN
        UPDATE SET 
            Target.DayHigh = Source.DayHigh,
            Target.DayLow = Source.DayLow,
            Target.Price = Source.Price,
            Target.RFactor = Source.RFactor
    WHEN NOT MATCHED THEN
        INSERT (Symbol, DayHigh, DayLow, Price, EntryDate, Time, RFactor)
        VALUES (Source.Symbol, Source.DayHigh, Source.DayLow, Source.Price, Source.EntryDate, Source.Time, Source.RFactor);

    -- Clean up temporary table
    DROP TABLE #TempRFactor;

    SET NOCOUNT OFF;
END

GO
/****** Object:  StoredProcedure [karmajew_sa].[WeeklyMarketUpdate]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [karmajew_sa].[WeeklyMarketUpdate]
AS
BEGIN
    WITH EODEntries AS (
        SELECT 
            IndexSymbol,
            Last,
            EntryDate,
            Time,
			PercentChange,
            DATEADD(DAY, -DATEPART(WEEKDAY, EntryDate) + 2, CAST(EntryDate AS DATE)) AS WeekStartDate, -- Gets Monday
            ROW_NUMBER() OVER (
                PARTITION BY IndexSymbol, CAST(EntryDate AS DATE) ORDER BY Time DESC
            ) AS RowNum
        FROM karmajew_sa.BroderMarkets WHERE [Key]='SECTORAL INDICES'
    )
    SELECT 
        IndexSymbol as [Name],
        WeekStartDate,
        DATEADD(DAY, 4, WeekStartDate) AS WeekEndDate, -- Gets Friday
        AVG(Last) AS WeeklyAverage,
		SUM(PercentChange) AS PChange
    FROM EODEntries
    WHERE RowNum = 1 --AND IndexSymbol='NIFTY REALTY'-- Only taking the last record of each day
    GROUP BY IndexSymbol, WeekStartDate
	Order by WeekStartDate desc
END

-- EXEC WeeklyMarketUpdate

/*
NIFTY IT
NIFTY PVT BANK
NIFTY BANK
NIFTY FIN SERVICE
NIFTY CONSR DURBL
NIFTY PHARMA
NIFTY HEALTHCARE
NIFTY AUTO
NIFTY MEDIA
NIFTY FMCG
NIFTY MS IT TELCM
NIFTY CONSUMPTION
NIFTY PSU BANK
NIFTY REALTY
NIFTY INFRA
NIFTY OIL AND GAS
NIFTY PSE
NIFTY METAL
NIFTY CPSE

*/



GO
/****** Object:  StoredProcedure [karmajew_sa].[WeeklyStockUpdate]    Script Date: 16-02-2025 09:33:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [karmajew_sa].[WeeklyStockUpdate]
	@CurrentDate VARCHAR(50) = '2025-02-10',
	@weekEndDate VARCHAR(50) = '2025-02-14'
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT 
		L.Id,
		T.Symbol, 
		T.Change,
		L.LastPrice, 
		T.PChange, 
		K.TFactor,
		L.DayHigh, L.DayLow, L.[Open], L.[Time]
	FROM 
		(SELECT Symbol, SUM(PChange) AS PChange, SUM(Change) Change 
		 FROM stockdata 
		 WHERE id IN 
			 (SELECT MAX(id) 
			  FROM stockdata 
			  WHERE entrydate >= @CurrentDate AND EntryDate <=@weekEndDate
			  GROUP BY Symbol, entrydate)
		GROUP BY Symbol) T
	JOIN 
		(SELECT Symbol, AVG(RFactor) * 10 AS TFactor 
		 FROM RFactors 
		 WHERE id IN 
			 (SELECT MAX(id) 
			  FROM RFactors 
			  WHERE Entrydate >= @CurrentDate AND EntryDate <= @weekEndDate
			  GROUP BY Symbol, EntryDate)
		 GROUP BY Symbol) K
	ON T.Symbol = K.Symbol
	JOIN
		(
      SELECT Id, Symbol, LastPrice, DayHigh, DayLow, [Open], CAST([Time] as VARCHAR(50)) as [Time]     
	  FROM stockdata
      WHERE id IN 
        (
          SELECT MAX(id) 
          FROM stockdata
          WHERE entrydate >= @CurrentDate AND EntryDate <= @weekEndDate
          GROUP BY Symbol
        )
    ) AS L
	ON T.Symbol = L.Symbol

	ORDER BY T.PChange;

END
GO
USE [master]
GO
ALTER DATABASE [karmajew_optionchain] SET  READ_WRITE 
GO

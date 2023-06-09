USE [master]
GO
/****** Object:  Database [TourBooking]    Script Date: 3/13/2023 7:40:33 PM ******/
CREATE DATABASE [TourBooking]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TourBooking_Data', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.PHUOCLT\MSSQL\DATA\TourBooking.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TourBooking_Log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.PHUOCLT\MSSQL\DATA\TourBooking.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [TourBooking] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TourBooking].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TourBooking] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TourBooking] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TourBooking] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TourBooking] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TourBooking] SET ARITHABORT OFF 
GO
ALTER DATABASE [TourBooking] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [TourBooking] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TourBooking] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TourBooking] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TourBooking] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TourBooking] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TourBooking] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TourBooking] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TourBooking] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TourBooking] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TourBooking] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TourBooking] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TourBooking] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TourBooking] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TourBooking] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TourBooking] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TourBooking] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TourBooking] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TourBooking] SET  MULTI_USER 
GO
ALTER DATABASE [TourBooking] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TourBooking] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TourBooking] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TourBooking] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [TourBooking] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [TourBooking] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [TourBooking] SET QUERY_STORE = ON
GO
ALTER DATABASE [TourBooking] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [TourBooking]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[first_name] [varchar](255) NOT NULL,
	[phone] [varchar](255) NOT NULL,
	[address] [varchar](255) NOT NULL,
	[last_name] [varchar](255) NOT NULL,
	[city] [varchar](255) NULL,
	[province] [varchar](255) NULL,
	[district] [varchar](255) NULL,
	[role] [int] NOT NULL,
	[avatar] [nvarchar](max) NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK__Account__3213E83F779F57B2] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Booking]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tour_id] [int] NOT NULL,
	[customer_id] [int] NOT NULL,
	[booking_date] [date] NOT NULL,
	[num_adults] [int] NOT NULL,
	[num_children] [int] NOT NULL,
	[num_infants] [int] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Destination]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Destination](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[region] [varchar](255) NOT NULL,
	[description] [text] NOT NULL,
	[status] [int] NOT NULL,
 CONSTRAINT [PK__Destinat__3213E83F7A890D0F] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DestinationImage]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DestinationImage](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[destination_id] [int] NOT NULL,
	[image] [nvarchar](max) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[booking_id] [int] NOT NULL,
	[payment_method] [varchar](255) NOT NULL,
	[payment_date] [date] NOT NULL,
	[payment_amount] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Role]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Role](
	[accountID] [int] NOT NULL,
	[role] [varchar](255) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tour]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tour](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tour_name] [varchar](255) NOT NULL,
	[tour_duration] [int] NOT NULL,
	[tour_capacity] [int] NOT NULL,
	[status] [int] NOT NULL,
	[tour_guide_id] [int] NULL,
 CONSTRAINT [PK__Tour__3213E83F771A814A] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourDetail]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourDetail](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tour_id] [int] NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[departure] [varchar](255) NOT NULL,
	[destination_id] [int] NOT NULL,
	[expired_date] [date] NOT NULL,
	[transportation_id] [int] NOT NULL,
	[tour_description] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourGuide]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourGuide](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tour_id] [int] NOT NULL,
	[tour_guide_name] [varchar](255) NOT NULL,
	[tour_guide_age] [int] NOT NULL,
	[tour_guide_phone] [varchar](255) NOT NULL,
	[tour_guide_email] [varchar](255) NOT NULL,
	[tour_guide_language_spoken] [varchar](255) NOT NULL,
	[tour_guide_ava] [nvarchar](max) NOT NULL,
	[tour_guide_bio] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TourPrice]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TourPrice](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[tour_id] [int] NOT NULL,
	[price_adults] [decimal](10, 2) NOT NULL,
	[price_children] [decimal](10, 2) NOT NULL,
	[price_infants] [decimal](10, 2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Transportation]    Script Date: 3/13/2023 7:40:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transportation](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[transportation_type] [varchar](255) NOT NULL,
	[transportation_description] [text] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF_Account_role]  DEFAULT ((2)) FOR [role]
GO
ALTER TABLE [dbo].[Account] ADD  CONSTRAINT [DF__Account__status__18EBB532]  DEFAULT ((1)) FOR [status]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK__Booking__tour_id__5BE2A6F2] FOREIGN KEY([tour_id])
REFERENCES [dbo].[Tour] ([id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK__Booking__tour_id__5BE2A6F2]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Account] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Account] ([id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Account]
GO
ALTER TABLE [dbo].[DestinationImage]  WITH CHECK ADD  CONSTRAINT [FK__Destinati__desti__5DCAEF64] FOREIGN KEY([destination_id])
REFERENCES [dbo].[Destination] ([id])
GO
ALTER TABLE [dbo].[DestinationImage] CHECK CONSTRAINT [FK__Destinati__desti__5DCAEF64]
GO
ALTER TABLE [dbo].[Payment]  WITH CHECK ADD FOREIGN KEY([booking_id])
REFERENCES [dbo].[Booking] ([id])
GO
ALTER TABLE [dbo].[TourDetail]  WITH CHECK ADD  CONSTRAINT [FK__TourDetai__desti__619B8048] FOREIGN KEY([destination_id])
REFERENCES [dbo].[Destination] ([id])
GO
ALTER TABLE [dbo].[TourDetail] CHECK CONSTRAINT [FK__TourDetai__desti__619B8048]
GO
ALTER TABLE [dbo].[TourDetail]  WITH CHECK ADD  CONSTRAINT [FK__TourDetai__tour___60A75C0F] FOREIGN KEY([tour_id])
REFERENCES [dbo].[Tour] ([id])
GO
ALTER TABLE [dbo].[TourDetail] CHECK CONSTRAINT [FK__TourDetai__tour___60A75C0F]
GO
ALTER TABLE [dbo].[TourDetail]  WITH CHECK ADD FOREIGN KEY([transportation_id])
REFERENCES [dbo].[Transportation] ([id])
GO
ALTER TABLE [dbo].[TourGuide]  WITH CHECK ADD  CONSTRAINT [FK__TourGuide__tour___6383C8BA] FOREIGN KEY([tour_id])
REFERENCES [dbo].[Tour] ([id])
GO
ALTER TABLE [dbo].[TourGuide] CHECK CONSTRAINT [FK__TourGuide__tour___6383C8BA]
GO
ALTER TABLE [dbo].[TourPrice]  WITH CHECK ADD  CONSTRAINT [FK__TourPrice__tour___6477ECF3] FOREIGN KEY([tour_id])
REFERENCES [dbo].[Tour] ([id])
GO
ALTER TABLE [dbo].[TourPrice] CHECK CONSTRAINT [FK__TourPrice__tour___6477ECF3]
GO
USE [master]
GO
ALTER DATABASE [TourBooking] SET  READ_WRITE 
GO

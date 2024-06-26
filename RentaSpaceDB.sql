USE [master]
GO
/****** Object:  Database [RentaSpaceDB]    Script Date: 2024-04-11 10:00:38 PM ******/
CREATE DATABASE [RentaSpaceDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'RentaSpaceDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RentaSpaceDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'RentaSpaceDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\RentaSpaceDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [RentaSpaceDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RentaSpaceDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RentaSpaceDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [RentaSpaceDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [RentaSpaceDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET  ENABLE_BROKER 
GO
ALTER DATABASE [RentaSpaceDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [RentaSpaceDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET RECOVERY FULL 
GO
ALTER DATABASE [RentaSpaceDB] SET  MULTI_USER 
GO
ALTER DATABASE [RentaSpaceDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [RentaSpaceDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [RentaSpaceDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [RentaSpaceDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [RentaSpaceDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [RentaSpaceDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'RentaSpaceDB', N'ON'
GO
ALTER DATABASE [RentaSpaceDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [RentaSpaceDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [RentaSpaceDB]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 2024-04-11 10:00:38 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Admins](
	[AdminID] [int] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AdminID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Apartments]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Apartments](
	[ApartmentID] [nvarchar](5) NOT NULL,
	[Bedrooms] [int] NOT NULL,
	[Bathrooms] [int] NOT NULL,
	[FloorArea] [decimal](5, 2) NOT NULL,
	[StatusID] [nchar](2) NOT NULL,
	[Price] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK__Apartmen__CBDF5744844CC0F2] PRIMARY KEY CLUSTERED 
(
	[ApartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Appointments]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Appointments](
	[AppointmentID] [int] IDENTITY(1000000,1) NOT NULL,
	[ManagerID] [int] NOT NULL,
	[TenantID] [nvarchar](5) NOT NULL,
	[ScheduleID] [int] NOT NULL,
	[AppointmentDate] [date] NOT NULL,
	[StatusID] [nchar](2) NOT NULL,
	[ApartmentID] [nvarchar](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityID] [int] IDENTITY(10,1) NOT NULL,
	[CityName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CityID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1000,1) NOT NULL,
	[JobID] [int] NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](10) NOT NULL,
	[Salary] [decimal](10, 2) NOT NULL,
	[StatusID] [nchar](2) NOT NULL,
	[SupervisorID] [int] NULL,
 CONSTRAINT [PK__Employee__7AD04FF14942793E] PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Jobs]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jobs](
	[JobID] [int] IDENTITY(500,1) NOT NULL,
	[JobTitle] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[JobID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Managers]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Managers](
	[ManagerID] [int] NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[CityID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ManagerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Messages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[Sender] [nvarchar](50) NOT NULL,
	[Receiver] [nvarchar](50) NOT NULL,
	[Message] [nvarchar](250) NOT NULL,
	[Subject] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Messages__C87C037CDCB956B4] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Properties]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Properties](
	[PropertyID] [nvarchar](5) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[CityID] [int] NOT NULL,
	[YearEstablished] [nvarchar](4) NOT NULL,
	[TotalUnits] [int] NOT NULL,
	[AvailableUnits] [int] NOT NULL,
	[StatusID] [nchar](2) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyApartments]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyApartments](
	[PropertyID] [nvarchar](5) NOT NULL,
	[ApartmentID] [nvarchar](5) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC,
	[ApartmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyManagers]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyManagers](
	[PropertyID] [nvarchar](5) NOT NULL,
	[ManagerID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PropertyID] ASC,
	[ManagerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Schedules]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Schedules](
	[ScheduleID] [int] IDENTITY(200,1) NOT NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[ScheduleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusID] [nchar](2) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tenants]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tenants](
	[TenantID] [nvarchar](5) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[TenantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserAccounts]    Script Date: 2024-04-11 10:00:38 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserAccounts](
	[UserAccount_Id] [int] IDENTITY(10000,1) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[UserType] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__UserAcco__A9D10535CD230647] PRIMARY KEY CLUSTERED 
(
	[UserAccount_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [dbo].[Admins] ([AdminID], [Email]) VALUES (1000, N'bernadette@mail.com')
GO
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A1029', 1, 1, CAST(48.50 AS Decimal(5, 2)), N'A2', CAST(2195.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A1142', 1, 1, CAST(46.08 AS Decimal(5, 2)), N'A1', CAST(2145.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A2731', 2, 2, CAST(102.19 AS Decimal(5, 2)), N'A1', CAST(2580.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A3213', 2, 1, CAST(87.05 AS Decimal(5, 2)), N'A1', CAST(1800.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A3321', 2, 1, CAST(116.50 AS Decimal(5, 2)), N'A1', CAST(2470.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A3930', 2, 1, CAST(80.00 AS Decimal(5, 2)), N'A2', CAST(1830.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A4183', 3, 2, CAST(132.10 AS Decimal(5, 2)), N'A1', CAST(2930.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A4726', 2, 1, CAST(91.88 AS Decimal(5, 2)), N'A1', CAST(3600.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A4827', 1, 1, CAST(74.32 AS Decimal(5, 2)), N'A1', CAST(1670.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A7245', 2, 2, CAST(104.23 AS Decimal(5, 2)), N'A1', CAST(4195.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A8219', 2, 1, CAST(123.00 AS Decimal(5, 2)), N'A1', CAST(2920.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A8506', 2, 1, CAST(87.20 AS Decimal(5, 2)), N'A1', CAST(1800.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A9131', 3, 2, CAST(120.77 AS Decimal(5, 2)), N'A2', CAST(3095.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A9686', 1, 1, CAST(82.04 AS Decimal(5, 2)), N'A1', CAST(1595.00 AS Decimal(10, 2)))
INSERT [dbo].[Apartments] ([ApartmentID], [Bedrooms], [Bathrooms], [FloorArea], [StatusID], [Price]) VALUES (N'A9985', 1, 1, CAST(56.02 AS Decimal(5, 2)), N'A1', CAST(1440.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [dbo].[Appointments] ON 

INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001015, 1015, N'T5876', 212, CAST(N'2024-04-25' AS Date), N'S2', N'A4726')
INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001016, 1015, N'T9966', 210, CAST(N'2024-04-30' AS Date), N'S1', N'A2731')
INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001017, 1016, N'T9966', 214, CAST(N'2024-04-29' AS Date), N'S2', N'A9985')
INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001018, 1016, N'T9966', 213, CAST(N'2024-05-10' AS Date), N'S2', N'A3321')
INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001019, 1016, N'T5876', 213, CAST(N'2024-04-29' AS Date), N'S1', N'A4183')
INSERT [dbo].[Appointments] ([AppointmentID], [ManagerID], [TenantID], [ScheduleID], [AppointmentDate], [StatusID], [ApartmentID]) VALUES (1001020, 1015, N'T5876', 211, CAST(N'2024-04-30' AS Date), N'S1', N'A7245')
SET IDENTITY_INSERT [dbo].[Appointments] OFF
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 

INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (11, N'Laval')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (13, N'Longueuil ')
INSERT [dbo].[Cities] ([CityID], [CityName]) VALUES (10, N'Montreal')
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1000, 500, N'Bernadette', N'Fernando', N'bernadette@mail.com', N'5141234567', CAST(100000.00 AS Decimal(10, 2)), N'E1', NULL)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1003, 501, N'Padme', N'Amidala', N'padme@mail.com', N'5553456789', CAST(70000.00 AS Decimal(10, 2)), N'E1', 1000)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1012, 501, N'Anakin', N'Skywalker', N'anakin@mail.com', N'5557654321', CAST(75000.00 AS Decimal(10, 2)), N'E1', 1000)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1013, 503, N'Finn', N'Dameron', N'finn.dameron@resistance.com', N'5556543210', CAST(50000.00 AS Decimal(10, 2)), N'E1', 1003)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1014, 503, N'Poe', N'Organa', N'poe.organa@resistance.org', N'5555678901', CAST(52000.00 AS Decimal(10, 2)), N'E1', 1012)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1015, 502, N'Leia', N'Organa - Solo', N'leia@mail.com', N'5143333333', CAST(60000.00 AS Decimal(10, 2)), N'E1', 1012)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1016, 502, N'Han', N'Solo', N'han.solo@mail.com', N'5141239999', CAST(70500.00 AS Decimal(10, 2)), N'E1', 1003)
INSERT [dbo].[Employees] ([EmployeeID], [JobID], [FirstName], [LastName], [Email], [Phone], [Salary], [StatusID], [SupervisorID]) VALUES (1017, 502, N'Rey ', N'Skywalker', N'rey_sky@mail.com', N'5443789000', CAST(97000.00 AS Decimal(10, 2)), N'E2', 1003)
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Jobs] ON 

INSERT [dbo].[Jobs] ([JobID], [JobTitle]) VALUES (500, N'Admin')
INSERT [dbo].[Jobs] ([JobID], [JobTitle]) VALUES (503, N'Maintenance')
INSERT [dbo].[Jobs] ([JobID], [JobTitle]) VALUES (502, N'Manager')
INSERT [dbo].[Jobs] ([JobID], [JobTitle]) VALUES (501, N'Supervisor')
SET IDENTITY_INSERT [dbo].[Jobs] OFF
GO
INSERT [dbo].[Managers] ([ManagerID], [Email], [CityID]) VALUES (1015, N'leia@mail.com', 10)
INSERT [dbo].[Managers] ([ManagerID], [Email], [CityID]) VALUES (1016, N'han.solo@mail.com', 11)
INSERT [dbo].[Managers] ([ManagerID], [Email], [CityID]) VALUES (1017, N'rey_sky@mail.com', 10)
GO
SET IDENTITY_INSERT [dbo].[Messages] ON 

INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1024, N'1015', N'T5876', N'Hi! I have a new listing you could be interested in.', N'New Listing!')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1025, N'T5876', N'1015', N'Sure! Can''t wait to see it.', N'New Listing Reply')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1026, N'T9966', N'1015', N'Hi! I am Monica. I am interested to see one of your listings.', N'Apartment Inquiry')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1027, N'T9966', N'1016', N'Hi! Do you have available listings?', N'Monica Geller - Question')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1028, N'1016', N'T5876', N'Hi! I have a new listing you could be interested in.', N'New Listing!')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1029, N'1016', N'T9966', N'Yes! I will set an appointment with you.', N'New Listing!')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1030, N'1016', N'1000', N'Hi Admin! I would like to report an incident.', N'Property Incident Report')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1031, N'1016', N'1000', N'Hi Bernadette, I have a report.', N'Property Incident Report')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1032, N'1016', N'T5876', N'Chandler, I booked an appointment with you!', N'New Listing!')
INSERT [dbo].[Messages] ([MessageID], [Sender], [Receiver], [Message], [Subject]) VALUES (1033, N'T5876', N'1015', N'Hi, are you available on May 01 2024?', N'Appointment Query')
SET IDENTITY_INSERT [dbo].[Messages] OFF
GO
INSERT [dbo].[Properties] ([PropertyID], [Address], [CityID], [YearEstablished], [TotalUnits], [AvailableUnits], [StatusID]) VALUES (N'P2092', N'235 Young street', 10, N'2023', 2, 2, N'P2')
INSERT [dbo].[Properties] ([PropertyID], [Address], [CityID], [YearEstablished], [TotalUnits], [AvailableUnits], [StatusID]) VALUES (N'P4513', N'3105 Promenade du Quartier-Saint-Martin', 11, N'2021', 2, 1, N'P1')
INSERT [dbo].[Properties] ([PropertyID], [Address], [CityID], [YearEstablished], [TotalUnits], [AvailableUnits], [StatusID]) VALUES (N'P5109', N'4555 Bonavista Avenue', 10, N'2019', 1, 1, N'P2')
INSERT [dbo].[Properties] ([PropertyID], [Address], [CityID], [YearEstablished], [TotalUnits], [AvailableUnits], [StatusID]) VALUES (N'P5922', N'2255 Daniel-Johnson boulevard', 11, N'2020', 4, 4, N'P2')
INSERT [dbo].[Properties] ([PropertyID], [Address], [CityID], [YearEstablished], [TotalUnits], [AvailableUnits], [StatusID]) VALUES (N'P6900', N'3460 Peel Street', 10, N'2022', 4, 3, N'P2')
GO
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P2092', N'A2731')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P2092', N'A4827')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P4513', N'A3930')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P4513', N'A9985')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P5109', N'A3213')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P5922', N'A3321')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P5922', N'A4183')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P5922', N'A8219')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P5922', N'A9686')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P6900', N'A1029')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P6900', N'A1142')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P6900', N'A4726')
INSERT [dbo].[PropertyApartments] ([PropertyID], [ApartmentID]) VALUES (N'P6900', N'A7245')
GO
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P2092', 1015)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P2092', 1017)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P4513', 1016)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P5109', 1015)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P5109', 1017)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P5922', 1016)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P6900', 1015)
INSERT [dbo].[PropertyManagers] ([PropertyID], [ManagerID]) VALUES (N'P6900', 1017)
GO
SET IDENTITY_INSERT [dbo].[Schedules] ON 

INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (200, CAST(N'08:00:00' AS Time), CAST(N'09:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (209, CAST(N'09:00:00' AS Time), CAST(N'10:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (210, CAST(N'10:00:00' AS Time), CAST(N'11:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (211, CAST(N'11:00:00' AS Time), CAST(N'12:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (212, CAST(N'13:00:00' AS Time), CAST(N'14:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (213, CAST(N'14:00:00' AS Time), CAST(N'15:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (214, CAST(N'15:00:00' AS Time), CAST(N'16:00:00' AS Time))
INSERT [dbo].[Schedules] ([ScheduleID], [StartTime], [EndTime]) VALUES (215, CAST(N'16:00:00' AS Time), CAST(N'17:00:00' AS Time))
SET IDENTITY_INSERT [dbo].[Schedules] OFF
GO
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'A1', N'AVAILABLE')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'A2', N'OCCUPIED')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'E1', N'ACTIVE')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'E2', N'INACTIVE')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'P1', N'FULL')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'P2', N'OPEN')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'S1', N'PENDING CONFIRMATION')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'S2', N'CONFIRMED')
INSERT [dbo].[Status] ([StatusID], [Description]) VALUES (N'S3', N'CANCELLED')
GO
INSERT [dbo].[Tenants] ([TenantID], [FirstName], [LastName], [Email], [Phone]) VALUES (N'T5876', N'Chanandler', N'Bing', N'chandler.bing@hotmail.com', N'1234567877')
INSERT [dbo].[Tenants] ([TenantID], [FirstName], [LastName], [Email], [Phone]) VALUES (N'T9966', N'Monica', N'Geller - Bing', N'monica@mail.com', N'5141231235')
GO
SET IDENTITY_INSERT [dbo].[UserAccounts] ON 

INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10001, N'bernadette@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10022, N'padme@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10023, N'anakin@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10024, N'leia@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10025, N'han.solo@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10026, N'chandler.bing@hotmail.com', N'P@ssw0rd', N'Tenant')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10027, N'monica@mail.com', N'P@ssw0rd', N'Tenant')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10028, N'rey_sky@mail.com', N'P@ssw0rd', N'Employee')
INSERT [dbo].[UserAccounts] ([UserAccount_Id], [Email], [Password], [UserType]) VALUES (10029, N'rachel@mail.com', N'P@ssw0rd', N'Tenant')
SET IDENTITY_INSERT [dbo].[UserAccounts] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Cities__886159E5C099DEE4]    Script Date: 2024-04-11 10:00:39 PM ******/
ALTER TABLE [dbo].[Cities] ADD UNIQUE NONCLUSTERED 
(
	[CityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Jobs__44C68B9F5AB489C9]    Script Date: 2024-04-11 10:00:39 PM ******/
ALTER TABLE [dbo].[Jobs] ADD UNIQUE NONCLUSTERED 
(
	[JobTitle] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__tmp_ms_x__A9D10534658E18A3]    Script Date: 2024-04-11 10:00:39 PM ******/
ALTER TABLE [dbo].[UserAccounts] ADD UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Admins]  WITH CHECK ADD  CONSTRAINT [FK__Admins__AdminID__59063A47] FOREIGN KEY([AdminID])
REFERENCES [dbo].[Employees] ([EmployeeID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Admins] CHECK CONSTRAINT [FK__Admins__AdminID__59063A47]
GO
ALTER TABLE [dbo].[Admins]  WITH CHECK ADD  CONSTRAINT [FK__Admins__Email__59FA5E80] FOREIGN KEY([Email])
REFERENCES [dbo].[UserAccounts] ([Email])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Admins] CHECK CONSTRAINT [FK__Admins__Email__59FA5E80]
GO
ALTER TABLE [dbo].[Apartments]  WITH CHECK ADD  CONSTRAINT [FK__Apartment__Statu__619B8048] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Apartments] CHECK CONSTRAINT [FK__Apartment__Statu__619B8048]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD  CONSTRAINT [FK__Appointme__Apart__6AEFE058] FOREIGN KEY([ApartmentID])
REFERENCES [dbo].[Apartments] ([ApartmentID])
GO
ALTER TABLE [dbo].[Appointments] CHECK CONSTRAINT [FK__Appointme__Apart__6AEFE058]
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Managers] ([ManagerID])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([ScheduleID])
REFERENCES [dbo].[Schedules] ([ScheduleID])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
GO
ALTER TABLE [dbo].[Appointments]  WITH CHECK ADD FOREIGN KEY([TenantID])
REFERENCES [dbo].[Tenants] ([TenantID])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK__Employees__JobID__4D94879B] FOREIGN KEY([JobID])
REFERENCES [dbo].[Jobs] ([JobID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK__Employees__JobID__4D94879B]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK__Employees__Statu__4E88ABD4] FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK__Employees__Statu__4E88ABD4]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [FK__Employees__Super__4F7CD00D] FOREIGN KEY([SupervisorID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [FK__Employees__Super__4F7CD00D]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
ON UPDATE CASCADE
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK__Managers__Email__5535A963] FOREIGN KEY([Email])
REFERENCES [dbo].[UserAccounts] ([Email])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK__Managers__Email__5535A963]
GO
ALTER TABLE [dbo].[Managers]  WITH CHECK ADD  CONSTRAINT [FK__Managers__Manage__5441852A] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Employees] ([EmployeeID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[Managers] CHECK CONSTRAINT [FK__Managers__Manage__5441852A]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD FOREIGN KEY([CityID])
REFERENCES [dbo].[Cities] ([CityID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD FOREIGN KEY([StatusID])
REFERENCES [dbo].[Status] ([StatusID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[PropertyApartments]  WITH CHECK ADD  CONSTRAINT [FK__PropertyA__Apart__74AE54BC] FOREIGN KEY([ApartmentID])
REFERENCES [dbo].[Apartments] ([ApartmentID])
GO
ALTER TABLE [dbo].[PropertyApartments] CHECK CONSTRAINT [FK__PropertyA__Apart__74AE54BC]
GO
ALTER TABLE [dbo].[PropertyApartments]  WITH CHECK ADD FOREIGN KEY([PropertyID])
REFERENCES [dbo].[Properties] ([PropertyID])
GO
ALTER TABLE [dbo].[PropertyManagers]  WITH CHECK ADD FOREIGN KEY([ManagerID])
REFERENCES [dbo].[Managers] ([ManagerID])
GO
ALTER TABLE [dbo].[PropertyManagers]  WITH CHECK ADD FOREIGN KEY([PropertyID])
REFERENCES [dbo].[Properties] ([PropertyID])
GO
ALTER TABLE [dbo].[Tenants]  WITH CHECK ADD  CONSTRAINT [FK__Tenants__Email__3C69FB99] FOREIGN KEY([Email])
REFERENCES [dbo].[UserAccounts] ([Email])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Tenants] CHECK CONSTRAINT [FK__Tenants__Email__3C69FB99]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [CK__Employees__Salar__5070F446] CHECK  (([Salary]>=(0)))
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [CK__Employees__Salar__5070F446]
GO
ALTER TABLE [dbo].[Properties]  WITH CHECK ADD CHECK  (([TotalUnits]>=[AvailableUnits]))
GO
USE [master]
GO
ALTER DATABASE [RentaSpaceDB] SET  READ_WRITE 
GO

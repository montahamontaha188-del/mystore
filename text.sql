
CREATE DATABASE [StoreDb];
GO
USE [StoreDb];
GO


CREATE TABLE [dbo].[Products](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [Name] [nvarchar](150) NOT NULL,
    [Price] [float] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Category] [int] NOT NULL
);

CREATE TABLE [dbo].[Customers](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [Name] [nvarchar](100) NOT NULL,
    [PhoneNumber] [nvarchar](20) NOT NULL
);

CREATE TABLE [dbo].[Discounts](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [Code] [nvarchar](50) NOT NULL UNIQUE,
    [Percentage] [float] NOT NULL,
    [IsActive] [bit] NOT NULL
);

CREATE TABLE [dbo].[Orders](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [CustomerId] [int] NOT NULL,
    [OrderDate] [datetime] NOT NULL,
    [AppliedDiscountCode] [nvarchar](50) NULL,
    [DiscountPercentage] [float] NOT NULL,
    CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerId]) REFERENCES [dbo].[Customers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[OrderItems](
    [Id] [int] IDENTITY(1,1) PRIMARY KEY,
    [OrderId] [int] NOT NULL,
    [ProductId] [int] NOT NULL,
    [Quantity] [int] NOT NULL,
    [Price] [float] NULL,
    CONSTRAINT [FK_OrderItems_Orders] FOREIGN KEY([OrderId]) REFERENCES [dbo].[Orders] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_OrderItems_Products] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Products] ([Id])
);
GO
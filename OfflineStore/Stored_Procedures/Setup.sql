USE [SalesDB]
GO

/****** Object: Table [dbo].[Sales] Script Date: 24-Sep-20 1:42:25 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sales] (
    [region]         NVARCHAR (MAX)  NOT NULL,
    [country]        NVARCHAR (MAX)  NOT NULL,
    [item_type]      NVARCHAR (MAX)  NOT NULL,
    [sales_channel]  NVARCHAR (MAX)  NOT NULL,
    [order_priority] NVARCHAR (3)    NOT NULL,
    [order_date]     DATE            NOT NULL,
    [order_id]       BIGINT          NOT NULL,
    [ship_date]      DATETIME        NOT NULL,
    [units_sold]     BIGINT          NOT NULL,
    [unit_price]     NUMERIC (18, 4) NOT NULL,
    [unit_cost]      NUMERIC (18, 4) NOT NULL,
    [total_revenue]  NUMERIC (18, 4) NOT NULL,
    [total_cost]     NUMERIC (18, 4) NOT NULL,
    [total_profit]   NUMERIC (18, 4) NOT NULL
);

GO

CREATE PROCEDURE spTop5ProfitableItemTypes
@DATE1 DATE,
@DATE2 DATE
AS

BEGIN
	SELECT TOP 5 item_type AS ITEM, SUM(total_profit) AS PROFIT FROM SALES WHERE order_date BETWEEN @DATE1 AND @DATE2 GROUP BY item_type ORDER BY PROFIT DESC
END

GO

CREATE PROCEDURE spTotalProfitMade
@DATE1 DATE,
@DATE2 DATE
AS
BEGIN
	SELECT SUM(total_profit) AS TOTALPROFIT FROM sales 
	WHERE order_date BETWEEN @DATE1 AND @DATE2
END

GO

CREATE PROCEDURE spGetTop1000SalesRecords
AS
BEGIN
	SELECT TOP 1000 * FROM SALES;
END

GO


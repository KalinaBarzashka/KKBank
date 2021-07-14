--Add Data

USE [KKBank]
GO

INSERT INTO [17118069].[AccountTypes] ([AccountTypeName], [CreatedOn_17118069], [IsDeleted_17118069])
VALUES ('Checking Account', '2021-05-28', 0)
GO

INSERT INTO [17118069].[Currency] ([CurrencyName], [CurrencyAbbreviation], [CreatedOn_17118069], [IsDeleted_17118069])
VALUES ('Bulgarian lev', 'BGN', '2021-05-28', 0), ('Euro', 'EUR', '2021-05-28', 0), ('United States dollar', 'USD', '2021-05-28', 0)
GO

INSERT INTO [17118069].[RequestTypes] ([Name], [CreatedOn_17118069], [IsDeleted_17118069])
VALUES ('Add Checking Account', '2021-05-28', 0), ('Delete Checking Account', '2021-05-28', 0)
GO

INSERT INTO [17118069].[PaymentOrderStatus] ([StatusName], [CreatedOn_17118069], [IsDeleted_17118069])
VALUES ('Operation executed', '2021-05-28', '0'), ('Cancelled By User', '2021-05-28', '0'), ('Sending Problem', '2021-05-28', '0')
GO

INSERT INTO [17118069].[AccountRequestStatus] ([Name], [CreatedOn_17118069], [IsDeleted_17118069])
VALUES ('Awaiting Approval', '2021-05-28',0), ('Approved', '2021-05-28', 0), ('Closed by Client', '2021-05-28', 0), ('Closed by Bank', '2021-05-28', 0)
GO

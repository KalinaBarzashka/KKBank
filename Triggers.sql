--1. AccountRequests Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertAccReqSetCreatedOnDate
ON [17118069].[AccountRequests]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[AccountRequests] 
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE AccountRequests.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateAccReqModifiedOnDate
ON [17118069].[AccountRequests]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[AccountRequests]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE AccountRequests.Id = inserted.Id
	
	
END

GO
--2. Accounts Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertAccCreatedOnDatePlusIBAN
ON [17118069].[Accounts]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[Accounts]
	SET CreatedOn_17118069 = GETDATE(), IBAN = 'BG14KKBB930014' + SUBSTRING(FORMAT(Accounts.Id, 'd08'), 1, 8)
	FROM inserted
	WHERE Accounts.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateAccModifiedOnDate
ON [17118069].[Accounts]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[Accounts]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE Accounts.Id = inserted.Id
END

--BG -> Country code, xx -> ckech number, KKBB -> Bank code, BBAN (0-9, 26 главни букви от A-Z); in BG IBAN is fixed length of 22 symbols

--BG
--07 - контролно число
--STSA - идентификатор на банката
--9300 - идентификатор на БАЕ
--00 - идентификатор на вида сметка
--23587454

GO
--3. AccountTypes Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertAccTypesCreatedOnDate
ON [17118069].[AccountTypes]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[AccountTypes]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE AccountTypes.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateAccTypesModifiedOnDate
ON [17118069].[AccountTypes]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[AccountTypes]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE AccountTypes.Id = inserted.Id
END

GO
--4. Currency Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertCurrencyCreatedOnDate
ON [17118069].[Currency]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[Currency]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE Currency.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateCurrencyModifiedOnDate
ON [17118069].[Currency]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[Currency]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE Currency.Id = inserted.Id
END

GO
--5. PaymentOrders Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertPaymentOrdersCreatedOnDate
ON [17118069].[PaymentOrders]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[PaymentOrders]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE PaymentOrders.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdatePaymentOrdersModifiedOnDate
ON [17118069].[PaymentOrders]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[PaymentOrders]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE PaymentOrders.Id = inserted.Id
END

GO
--6. PaymentOrderStatus Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertPaymentOrderStatusCreatedOnDate
ON [17118069].[PaymentOrderStatus]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[PaymentOrderStatus]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE PaymentOrderStatus.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdatePaymentOrderStatusModifiedOnDate
ON [17118069].[PaymentOrderStatus]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[PaymentOrderStatus]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE PaymentOrderStatus.Id = inserted.Id
END

GO
--7. RequestTypes Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertRequestTypesCreatedOnDate
ON [17118069].[RequestTypes]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[RequestTypes]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE RequestTypes.Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateRequestTypesModifiedOnDate
ON [17118069].[RequestTypes]
FOR UPDATE
AS
BEGIN
	IF EXISTS (SELECT * FROM DELETED)
		UPDATE [17118069].[RequestTypes]
		SET ModifiedOn_17118069 = GETDATE()
		FROM inserted
		WHERE RequestTypes.Id = inserted.Id
END

GO
--8. Acc Request Status Table TRIGGERS
CREATE OR ALTER TRIGGER [17118069].OnInsertStatusCreatedOnDate
ON [17118069].[AccountRequestStatus]
FOR INSERT
AS
BEGIN
	UPDATE [17118069].[AccountRequestStatus]
	SET CreatedOn_17118069 = GETDATE()
	FROM inserted
	WHERE [AccountRequestStatus].Id = inserted.Id
END

GO

CREATE OR ALTER TRIGGER [17118069].OnUpdateStatusModifiedOnDate
ON [17118069].[AccountRequestStatus]
FOR UPDATE
AS
BEGIN
	IF EXISTS(SELECT * FROM INSERTED) AND EXISTS(SELECT * FROM DELETED) --UPDATE
		BEGIN
			UPDATE [17118069].[AccountRequestStatus]
			SET ModifiedOn_17118069 = GETDATE()
			FROM inserted
			WHERE [AccountRequestStatus].Id = inserted.Id;

			INSERT INTO [17118069].[log_17118069] ([TableName], [Operation], [Message], [Date])
			SELECT 'AccountRequestStatus', 'UPDATE', CONCAT('Updated account request status: Old status: ', deleted.[Name], ', New status: ', inserted.[Name]), GETDATE()
			FROM inserted, deleted
		END
END

GO

CREATE OR ALTER TRIGGER [17118069].OnInsertStatusModifiedOnDate
ON [17118069].[AccountRequestStatus]
FOR INSERT
AS
BEGIN
	IF EXISTS(SELECT * FROM INSERTED) --INSERT
		BEGIN
			INSERT INTO [17118069].[log_17118069] ([TableName], [Operation], [Message], [Date])
			SELECT 'AccountRequestStatus', 'INSERT', CONCAT('Inserted new account request status: ', inserted.[Name]), GETDATE()
			FROM inserted
		END
END

GO

CREATE OR ALTER TRIGGER [17118069].OnDeleteStatusModifiedOnDate
ON [17118069].[AccountRequestStatus]
FOR DELETE
AS
BEGIN
	IF EXISTS(SELECT * FROM DELETED) --DELETE
		BEGIN
			INSERT INTO [17118069].[log_17118069] ([TableName], [Operation], [Message], [Date])
			SELECT 'AccountRequestStatus', 'DELETE', CONCAT('Deleted account request status: ', deleted.[Name]), GETDATE()
			FROM deleted
		END
END

GO
﻿-- =============================================
-- After insert files
-- =============================================

--ALTER TRIGGER [SynologenOpqFiles_AfterInsert] 
CREATE TRIGGER [SynologenOpqFiles_AfterInsert] 
ON [dbo].[SynologenOpqFiles]
AFTER INSERT
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE	@id INT,
			@ndeId INT,
			@order INT,
			@contextInfo VARBINARY (128)
			
	SELECT	@id = Id,
			@ndeId = NdeId
	FROM	INSERTED
	
	SET @order = 1
	
	SELECT	@order = MAX ([Order]) + 1
	FROM	dbo.SynologenOpqFiles
	WHERE	NdeId = @ndeId
			
	SET @contextInfo = CAST ('DontUpdateFile' + SPACE (128) AS VARBINARY (128))  
	SET CONTEXT_INFO @contextInfo	

	UPDATE	dbo.SynologenOpqFiles
	SET		[Order] = ISNULL (@order, 1)
	WHERE	Id = @id
	
	SET @contextInfo = CAST ('UpdateClear' + SPACE (128) AS VARBINARY (128))  
	SET CONTEXT_INFO @contextInfo		
END
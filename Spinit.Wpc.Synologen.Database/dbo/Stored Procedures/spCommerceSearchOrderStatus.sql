﻿CREATE PROCEDURE spCommerceSearchOrderStatus
					@type INT,
					@id INT,
					@status INT OUTPUT
AS
BEGIN
	IF @type = 0
		BEGIN
			SELECT	cId,
					cName
			FROM	tblCommerceOrderStatus
			ORDER BY cName ASC
		END
		
	IF @type = 1
		BEGIN
			SELECT	cId,
					cName
			FROM	tblCommerceOrderStatus
			WHERE	cId = @id
			ORDER BY cName ASC
		END
						
	SET @status = @@ERROR
			
END

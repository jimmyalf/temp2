﻿-- =============================================
-- Instead-of update document
-- =============================================

CREATE TRIGGER [SynologenOpqNodes_InsteadOfInsert] 
ON [dbo].[SynologenOpqNodes]
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON
	
	DECLARE	@parent INT,
			@order INT
			
	SELECT	@parent = Parent
	FROM	INSERTED
	
	SET @order = 1
	
	IF @parent IS NULL
		BEGIN
			SELECT	@order = MAX ([Order]) + 1
			FROM	dbo.SynologenOpqNodes
			WHERE	Parent IS NULL
		END
	ELSE
		BEGIN
			SELECT	@order = MAX ([Order]) + 1
			FROM	dbo.SynologenOpqNodes
			WHERE	Parent = @parent
		END
			
	INSERT INTO dbo.SynologenOpqNodes (
		Parent, [Order], [Name], IsActive, CreatedById, CreatedByName, CreatedDate,
		ChangedById, ChangedByName, ChangedDate, ApprovedById, ApprovedByName, ApprovedDate,
		LockedById, LockedByName, LockedDate)
	SELECT
		Parent,
		@order,
		[Name],
		IsActive,
		CreatedById,
		CreatedByName,
		CreatedDate,
		ChangedById,
		ChangedByName,
		ChangedDate,
		ApprovedById,
		ApprovedByName,
		ApprovedDate,
		LockedById,
		LockedByName,
		LockedDate
	FROM INSERTED
END
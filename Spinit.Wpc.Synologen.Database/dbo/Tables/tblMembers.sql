﻿CREATE TABLE [dbo].[tblMembers] (
    [cId]      INT            IDENTITY (1, 1) NOT NULL,
    [cOrgName] NVARCHAR (255) NOT NULL,
    [cActive]  BIT            NOT NULL,
    CONSTRAINT [PK_tblMembers] PRIMARY KEY CLUSTERED ([cId] ASC)
);


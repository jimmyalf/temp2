﻿CREATE TABLE [dbo].[tblCommerceProductFileCategory] (
    [cId]   INT           IDENTITY (1, 1) NOT NULL,
    [cName] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_tblCommerceProductFileCategory] PRIMARY KEY CLUSTERED ([cId] ASC)
);


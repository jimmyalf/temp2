﻿CREATE TABLE [dbo].[tblBaseLog] (
    [cId]           INT             IDENTITY (1, 1) NOT NULL,
    [cLgTpeId]      INT             NOT NULL,
    [cLocId]        INT             NULL,
    [cCmpId]        INT             NULL,
    [cBackground]   BIT             NOT NULL,
    [cAdmin]        BIT             NOT NULL,
    [cHash]         INT             NOT NULL,
    [cCount]        INT             NOT NULL,
    [cException]    NVARCHAR (500)  NULL,
    [cMessage]      NVARCHAR (2000) NULL,
    [cIpAddress]    VARCHAR (15)    NULL,
    [cUserAgent]    NVARCHAR (64)   NULL,
    [cHttpReferrer] NVARCHAR (256)  NULL,
    [cCreatedBy]    NVARCHAR (100)  NULL,
    [cCreatedDate]  DATETIME        CONSTRAINT [DF_tblBaseLog_DateCreated] DEFAULT (getdate()) NULL,
    [cChangedBy]    NVARCHAR (100)  NULL,
    [cChangedDate]  DATETIME        NULL,
    [cClearedBy]    NVARCHAR (100)  NULL,
    [cClearedDate]  DATETIME        NULL,
    CONSTRAINT [PK_tblBaseLog] PRIMARY KEY CLUSTERED ([cId] ASC),
    CONSTRAINT [FK_tblBaseLog_tblBaseComponents] FOREIGN KEY ([cCmpId]) REFERENCES [dbo].[tblBaseComponents] ([cId]),
    CONSTRAINT [FK_tblBaseLog_tblBaseLocations] FOREIGN KEY ([cLocId]) REFERENCES [dbo].[tblBaseLocations] ([cId]),
    CONSTRAINT [FK_tblBaseLog_tblBaseLogTypes] FOREIGN KEY ([cLgTpeId]) REFERENCES [dbo].[tblBaseLogTypes] ([cId])
);

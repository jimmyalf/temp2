﻿CREATE TABLE [dbo].[tblBaseLocations] (
    [cId]                   INT            IDENTITY (1, 1) NOT NULL,
    [cName]                 NVARCHAR (256) NOT NULL,
    [cDescription]          NVARCHAR (256) NULL,
    [cAllowCrossPublishing] BIT            NULL,
    [cInfoName]             NVARCHAR (256) NULL,
    [cInfoAdress]           NVARCHAR (256) NULL,
    [cInfoVisitAdress]      NVARCHAR (256) NULL,
    [cInfoZipCode]          NVARCHAR (256) NULL,
    [cInfoCity]             NVARCHAR (256) NULL,
    [cInfoPhone]            NVARCHAR (256) NULL,
    [cInfoFax]              NVARCHAR (256) NULL,
    [cInfoEmail]            NVARCHAR (256) NULL,
    [cInfoCopyRightInfo]    NVARCHAR (256) NULL,
    [cInfoWebMaster]        NVARCHAR (256) NULL,
    [cAlias1]               NVARCHAR (256) NULL,
    [cAlias2]               NVARCHAR (256) NULL,
    [cAlias3]               NVARCHAR (256) NULL,
    [cPublishPath]          NVARCHAR (256) NULL,
    [cRelativePath]         NVARCHAR (256) NULL,
    [cSitePath]             NVARCHAR (255) NULL,
    [cPublishActive]        BIT            NULL,
    [cFtpPublishActive]     BIT            NULL,
    [cFtpPassive]           BIT            NULL,
    [cFtpUserName]          NVARCHAR (256) NULL,
    [cFtpPassword]          NVARCHAR (256) NULL,
    [cFtpSite]              NVARCHAR (256) NULL,
    [cExtranet]             BIT            CONSTRAINT [DF_tblBaseLocations_cExtranet] DEFAULT (0) NULL,
    [cDocType]              INT            NOT NULL,
    [cDocSubType]           INT            NOT NULL,
    [cFrontType]            INT            CONSTRAINT [DF_tblBaseLocations_cFrontType] DEFAULT (2) NOT NULL,
    CONSTRAINT [PK_tblBaseLocation] PRIMARY KEY CLUSTERED ([cId] ASC)
);


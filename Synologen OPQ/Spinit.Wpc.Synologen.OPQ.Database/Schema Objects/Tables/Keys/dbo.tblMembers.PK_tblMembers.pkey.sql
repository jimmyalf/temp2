﻿ALTER TABLE [dbo].[tblMembers]
    ADD CONSTRAINT [PK_tblMembers] PRIMARY KEY CLUSTERED ([cId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


﻿ALTER TABLE [dbo].[tblMemberUserConnection]
    ADD CONSTRAINT [PK_tblMemberUserConnection] PRIMARY KEY CLUSTERED ([cMemberId] ASC, [cUserId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


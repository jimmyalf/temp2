﻿ALTER TABLE [dbo].[tblForumUsers]
    ADD CONSTRAINT [IX_forums_Users_2] UNIQUE NONCLUSTERED ([UserName] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);


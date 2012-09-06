﻿using System.Collections.Generic;
using System.Linq;
using Spinit.Data.SqlClient.SqlBuilder;
using Synologen.Maintenance.UpgradeWpc2012.Domain.Model.Entities;

namespace Synologen.Maintenance.UpgradeWpc2012.Persistence.Queries
{
	public class ContentEntitiesMatchingSearchQuery : PersistenceBase
	{
		private readonly string _query;

		public ContentEntitiesMatchingSearchQuery(string query)
		{
			_query = query;
		}

		public IList<ContentEntity> Execute()
		{
			var query = QueryBuilder
				.Build(@"
					SELECT 
						tblContPage.cId,
						tblContPage.cContent,
						tblContTree.cLocId,
						cUrl = dbo.sfContGetFileUrlDownString(tblContTree.cId),
						tblContPage.cName
					FROM tblContPage 
					LEFT OUTER JOIN tblContTree ON tblContTree.cPgeId = tblContPage.cId")
				.Where("cContent LIKE @Match")
				.AddParameters(new { Match = '%' + EscapeSqlString(_query) + '%' });

			return Query(query, ContentEntity.Parse).ToList();
		}		 
	}
}
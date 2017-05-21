﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.Dapper.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.Dapper.QueryRepositories
{
    public class PriorityQueryRepository : BaseRepository, IPriorityQueryRepository
    {
        public PriorityQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<IList<PriorityDto>> All()
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * FROM Priorities";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<PriorityDto>(query);
                return result.ToList();
            }
        }
    }
}

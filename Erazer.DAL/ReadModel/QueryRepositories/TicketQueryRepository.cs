﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Erazer.DAL.ReadModel.Base;
using Erazer.Services.Queries.DTOs;
using Erazer.Services.Queries.Repositories;
using Microsoft.Extensions.Configuration;

namespace Erazer.DAL.ReadModel.QueryRepositories
{
    public class TicketQueryRepository : BaseRepository, ITicketQueryRepository
    {
        public TicketQueryRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<TicketDto> Find(string id)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"SELECT * FROM TicketIV WITH (NOEXPAND)
                                       WHERE Id = @Id";

                dbConnection.Open();
                var result = await dbConnection.QueryAsync<TicketDto>(query, new {Id = id});
                return result.FirstOrDefault();
            }
        }

        public Task<TicketListDto> All()
        {
            throw new NotImplementedException();
        }

        public async Task Update(TicketDto ticket)
        {
            using (var dbConnection = Connection)
            {
                const string query = @"UPDATE[dbo].[Tickets] SET Title = @Title, Description = @Description, PriorityId = @PriorityId, StatusId = @StatusId WHERE Id = @Id";

                dbConnection.Open();
                var result = await dbConnection.ExecuteAsync(query, ticket);
            }
        }
    }
}
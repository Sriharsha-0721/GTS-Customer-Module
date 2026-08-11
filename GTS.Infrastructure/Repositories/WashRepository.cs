using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class WashRepository : IWashRepository
    {
        private readonly IConfiguration _config;

        public WashRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<WashDetails?> GetWashDetails(int custId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryFirstOrDefaultAsync<WashDetails>(
                "GetWashDetails",
                new
                {
                    CustId = custId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetGarmentTypes()
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<string>(
                "GetWashGarmentTypes",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveWash(WashDetails dto)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters p = new();

            p.Add("@CustId", dto.CustId);
            p.Add("@WashCom", dto.WashCom);
            p.Add("@Formula", dto.Formula);

            p.Add(
                "@Result",
                dbType: DbType.Int32,
                direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "SaveWashDetails",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@Result");
        }
    }
}
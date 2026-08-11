using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class PackoutRepository : IPackoutRepository
    {
        private readonly IConfiguration _config;

        public PackoutRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<PackoutDetails>> GetPackoutDetails(int custId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<PackoutDetails>(
                "GetPackoutRestrictionData",
                new
                {
                    CustID = custId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<string>> GetPackoutItems(int custId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<string>(
                "GetPackoutItems",
                new
                {
                    CustID = custId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SavePackout(CreatePackout dto)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters p = new();

            p.Add("@CustId", dto.CustId);
            p.Add("@PackoutRestrict", dto.PkoutRestrict);
            p.Add("@ItemType", dto.Item);
            p.Add("@Color", dto.Color);
            p.Add("@Size", dto.Size);
            p.Add("@UpdtUser", dto.UpdtUser);
            p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "SavePackoutRestrictData",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@Result");
        }

        public async Task<int> DeletePackout(int pkoutRestrictId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters p = new();

            p.Add("@PackoutRestrictId", pkoutRestrictId);

            p.Add(
                "@Result",
                dbType: DbType.Int32,
                direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "DeletePackoutRestrictionDetails",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@Result");
        }
    }
}
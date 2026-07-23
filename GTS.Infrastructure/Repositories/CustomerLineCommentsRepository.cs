using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class CustomerLineCommentsRepository
        : ICustomerLineCommentsRepository
    {
        private readonly IConfiguration _config;

        public CustomerLineCommentsRepository(
            IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<CustWearerItem>>
            GetCustWearItems(int custId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<CustWearerItem>(
                "cRep_GetCustWearerItems",
                new
                {
                    CustId = custId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int>
            InsertCustLineCmts(CustLineCmts custLineCmts)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters param = new();

            param.Add("CustId", custLineCmts.CustId);
            param.Add("WearItemId", custLineCmts.WearItemId);
            param.Add("Descr", custLineCmts.Descr);

            param.Add(
                "RetVal",
                dbType: DbType.Int32,
                direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "cRep_InsCustLineCmts",
                param,
                commandType: CommandType.StoredProcedure);

            return param.Get<int>("RetVal");
        }
    }
}
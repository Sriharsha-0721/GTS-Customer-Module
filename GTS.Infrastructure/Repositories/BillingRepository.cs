using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class BillingRepository : IBillingRepository
    {
        private readonly IConfiguration _config;

        public BillingRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<BillingChargeDetails>> GetBillingCharges(int custId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<BillingChargeDetails>(
                "prcGETBillingChargesDtls",
                new
                {
                    CustID = custId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<BillingData>> GetBillingData()
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.QueryAsync<BillingData>(
                "GetBillingData",
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> SaveBillingCharge(CreateBillingCharge dto)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters p = new();

            p.Add("@BillingChargesId", dto.ChargeTypeId);
            p.Add("@custID", dto.CustId);
            p.Add("@Charges", dto.Charges);
            p.Add("@UpdtUser", dto.UpdtUser);
            p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "prcSaveBillingCharges",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@Result");
        }

        public async Task<int> DeleteBillingCharge(int billingChargesId)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            DynamicParameters p = new();

            p.Add("@BillingChargesId", billingChargesId);
            p.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await con.ExecuteAsync(
                "prcDeleteBillingCharges",
                p,
                commandType: CommandType.StoredProcedure);

            return p.Get<int>("@Result");
        }
    }
}
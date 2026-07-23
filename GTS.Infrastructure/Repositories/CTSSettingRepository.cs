using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTS.Infrastructure.Repositories
{
    public class CTSSettingRepository
        : ICTSSettingRepository
    {
        private readonly
            IConfiguration _config;

        public CTSSettingRepository(
            IConfiguration config)
        {
            _config = config;
        }

        public async Task<IEnumerable<CTSSettingDetails>> GetCTSSettings(int custNbr)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            var result = (await con.QueryAsync<CTSSettingDetails>(
                "AdmCsWr_GetCTSSettings",
                new
                {
                    CustNbr = custNbr
                },
                commandType: System.Data.CommandType.StoredProcedure)).ToList();

            var first = result.FirstOrDefault();

            if (first != null)
            {
                Console.WriteLine("===== REPOSITORY =====");
                Console.WriteLine($"Issue = {first.PrintIssueStatusFlag}");
                Console.WriteLine($"Born  = {first.PrintBornonDateFlag}");
                Console.WriteLine($"NOG   = {first.NOGFlag}");
                Console.WriteLine($"Label = {first.LabelHeader}");
            }

            return result;
        }

        public async Task<int>
            SaveCTSSetting(
                CreateCTSSetting dto)
        {
            using SqlConnection con =
                new(
                    _config
                        .GetConnectionString(
                            "DefaultConnection"));

            return await
                con.QuerySingleAsync<int>(
                    "AdmCsWr_SaveCTSSettings",
                    dto,
                    commandType:
                    System.Data.CommandType
                        .StoredProcedure);
        }
    }
}
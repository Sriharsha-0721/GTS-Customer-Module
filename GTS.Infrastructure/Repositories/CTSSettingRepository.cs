using Dapper;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories;

public class CTSSettingRepository : ICTSSettingRepository
{
    private readonly IConfiguration _config;

    public CTSSettingRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IEnumerable<CTSSettingDetails>> GetCTSSettings(int custNbr)
    {
        using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        return await con.QueryAsync<CTSSettingDetails>(
            "dbo.AdmCsWr_GetCTSSettings",
            new { CustNbr = custNbr },
            commandType: CommandType.StoredProcedure);
    }

    public async Task<int> SaveCTSSetting(CreateCTSSetting entity)
    {
        using var con = new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        var parameters = new DynamicParameters();
        parameters.Add("@MarketCenter", entity.MC, DbType.Int16);
        parameters.Add("@CustNbr", entity.CustNbr, DbType.Int32);
        parameters.Add("@PrintIssueStatusFlag", entity.PrintIssueStatusFlag, DbType.Boolean);
        parameters.Add("@PrintBornonDateFlag", entity.PrintBornonDateFlag, DbType.Boolean);
        parameters.Add("@NOGFlag", entity.NOGFlag, DbType.Boolean);
        parameters.Add("@LabelHeader", entity.LabelHeader ?? string.Empty, DbType.String);

        return await con.ExecuteAsync(
            "dbo.AdmCsWr_SaveCTSSettings",
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}
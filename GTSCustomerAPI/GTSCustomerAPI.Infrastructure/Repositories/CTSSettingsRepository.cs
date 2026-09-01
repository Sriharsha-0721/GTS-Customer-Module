using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class CTSSettingsRepository : ICTSSettingsRepository
{
    private readonly string _connectionString;

    public CTSSettingsRepository(IConfiguration configuration)
    {
        _connectionString = configuration
            .GetConnectionString("DefaultConnection")!;
    }

    // GET by CustId — reads from Customers table
    public async Task<CTSSettings?> GetByCustIdAsync(int custId)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "AdmCsWr_GetCTSSettings", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustId", custId);

        await con.OpenAsync();
        using var r = await cmd.ExecuteReaderAsync();

        if (!await r.ReadAsync()) return null;

        return new CTSSettings
        {
            CustId = r.GetInt32(r.GetOrdinal("CustId")),
            CustNbr = r.GetInt32(r.GetOrdinal("CustNbr")),
            MarketCenter = r.GetInt16(r.GetOrdinal("MarketCenter")),
            Name = r.IsDBNull(r.GetOrdinal("Name"))
                        ? null : r.GetString(r.GetOrdinal("Name")),
            Route = r.GetInt32(r.GetOrdinal("Route")),
            GID = r.IsDBNull(r.GetOrdinal("GID"))
                        ? null : r.GetString(r.GetOrdinal("GID")),
            PrintIssueStatusFlag = r.IsDBNull(
                r.GetOrdinal("PrintIssueStatusFlag"))
                ? null
                : r.GetInt16(r.GetOrdinal("PrintIssueStatusFlag")),
            PrintBornonDateFlag = r.IsDBNull(
                r.GetOrdinal("PrintBornonDateFlag"))
                ? null
                : r.GetInt16(r.GetOrdinal("PrintBornonDateFlag")),
            NOGFlag = r.IsDBNull(r.GetOrdinal("NOGFlag"))
                        ? null
                        : r.GetBoolean(r.GetOrdinal("NOGFlag")),
            LabelHeader = r.IsDBNull(r.GetOrdinal("LabelHeader"))
                          ? null
                          : r.GetString(r.GetOrdinal("LabelHeader"))
        };
    }

    // SAVE — updates Customers table
    public async Task<bool> SaveAsync(CTSSettings s)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand(
            "AdmCsWr_SaveCTSSettings", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustId",
            s.CustId);
        cmd.Parameters.AddWithValue("@PrintIssueStatus",
            (object?)s.PrintIssueStatusFlag ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@PrintBornOnDate",
            (object?)s.PrintBornonDateFlag ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@NOGFlag",
            (object?)s.NOGFlag ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@LabelHeader",
            (object?)s.LabelHeader ?? DBNull.Value);

        await con.OpenAsync();
        var rows = Convert.ToInt32(
            await cmd.ExecuteScalarAsync());
        return rows > 0;
    }
}
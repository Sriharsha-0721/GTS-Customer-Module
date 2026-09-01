using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class DosageRepository : IDosageRepository
{
    private readonly string _cs;

    public DosageRepository(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;
    }

    // GetCustProfileDosage(@CustID)
    public async Task<CustomerDosage?> GetDosageAsync(
        int custId)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "GetCustProfileDosage", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustID", custId);

        await con.OpenAsync();
        using var r = await cmd.ExecuteReaderAsync();
        if (!await r.ReadAsync()) return null;

        return new CustomerDosage
        {
            CustId = custId,
            Dosage = r.IsDBNull(
                r.GetOrdinal("Dosage"))
                ? null
                : r.GetBoolean(
                    r.GetOrdinal("Dosage"))
        };
    }

    // AddUpdateCustProfileDosage
    public async Task UpdateDosageAsync(
        int custId, bool? dosage)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "AddUpdateCustProfileDosage", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustID", custId);
        cmd.Parameters.AddWithValue("@Dosage",
            (object?)dosage ?? DBNull.Value);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
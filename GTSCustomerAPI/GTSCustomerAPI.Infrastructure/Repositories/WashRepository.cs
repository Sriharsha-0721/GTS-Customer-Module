using System;
using System.Collections.Generic;
using System.Text;

using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class WashRepository : IWashRepository
{
    private readonly string _cs;

    public WashRepository(
        IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;
    }


    // =====================================================
    // GARMENT TYPES
    // SP: GetGarmentTypes
    // =====================================================

    public async Task<IEnumerable<GarmentType>>
        GetGarmentTypesAsync()
    {
        var list =
            new List<GarmentType>();

        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "GetGarmentTypes",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        await con.OpenAsync();

        using var reader =
            await cmd.ExecuteReaderAsync();


        while (await reader.ReadAsync())
        {
            list.Add(
     new GarmentType
     {
         ItemDesc = GetStr(reader, "ItemDesc")
     }
 );
        }

        return list;
    }


    // =====================================================
    // GET FORMULA
    // SP: prcGETGarmentFromulaDtls
    // =====================================================

    public async Task<string>
        GetFormulaAsync(
            int marketCenter,
            int custId)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "prcGETGarmentFromulaDtls",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        cmd.Parameters.AddWithValue(
            "@MC",
            marketCenter);

        cmd.Parameters.AddWithValue(
            "@CustID",
            custId);


        await con.OpenAsync();

        using var reader =
            await cmd.ExecuteReaderAsync();


        if (await reader.ReadAsync())
        {
            return GetStr(
                reader,
                "Formula");
        }

        return "";
    }


    // =====================================================
    // SAVE FORMULA
    // SP: SaveGarmentFormula
    // =====================================================

    public async Task<int>
        SaveFormulaAsync(
            int marketCenter,
            int custId,
            string formulaString)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "SaveGarmentFormula",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        cmd.Parameters.AddWithValue(
            "@MC",
            marketCenter);

        cmd.Parameters.AddWithValue(
            "@CustID",
            custId);

        cmd.Parameters.AddWithValue(
            "@FormulaString",
            formulaString ?? "");


        var resultParam =
            new SqlParameter
            {
                ParameterName =
                    "@Result",

                SqlDbType =
                    SqlDbType.Int,

                Direction =
                    ParameterDirection.Output
            };


        cmd.Parameters.Add(
            resultParam);


        await con.OpenAsync();

        await cmd.ExecuteNonQueryAsync();


        return resultParam.Value == DBNull.Value
            ? 0
            : Convert.ToInt32(
                resultParam.Value);
    }


    // =====================================================
    // HELPER
    // =====================================================

    private static string GetStr(
        SqlDataReader reader,
        string column)
    {
        try
        {
            int index =
                reader.GetOrdinal(
                    column);

            if (reader.IsDBNull(index))
                return "";

            return Convert.ToString(
                reader.GetValue(index))
                ?? "";
        }
        catch
        {
            return "";
        }
    }
}
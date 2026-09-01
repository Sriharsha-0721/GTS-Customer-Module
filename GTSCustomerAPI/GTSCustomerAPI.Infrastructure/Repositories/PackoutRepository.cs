using System;
using System.Collections.Generic;
using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class PackoutRepository : IPackoutRepository
{
    private readonly string _cs;

    public PackoutRepository(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString("DefaultConnection")!;
    }

    // =====================================================
    // GET EXISTING PACKOUT ROWS
    // SP: GetPackoutRestrictionData
    // =====================================================

    public async Task<IEnumerable<PackoutDetails>>
        GetPackoutByCustIdAsync(int custId)
    {
        var list = new List<PackoutDetails>();

        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "GetPackoutRestrictionData",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID",
            custId);

        await con.OpenAsync();

        using var reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(
                new PackoutDetails
                {
                    PkoutRestrictId =
                        GetInt(
                            reader,
                            "PkoutRestrictId"),

                    PkoutRestrict =
                        GetStr(
                            reader,
                            "PkoutRestrict"),

                    Item =
                        GetStr(
                            reader,
                            "Item"),

                    Color =
                        GetStr(
                            reader,
                            "Color"),

                    Size =
                        GetStr(
                            reader,
                            "Size")
                }
            );
        }

        return list;
    }


    // =====================================================
    // GET ITEM DROPDOWN
    // SP: GetPackoutItems
    // Source: GarmInServ.ItemCode
    // =====================================================

    public async Task<IEnumerable<string>>
        GetPackoutItemsAsync(int custId)
    {
        var list =
            new List<string>();

        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "GetPackoutItems",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID",
            custId);

        await con.OpenAsync();

        using var reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var item =
                GetStr(
                    reader,
                    "Item");

            if (!string.IsNullOrWhiteSpace(item))
            {
                list.Add(
                    item.Trim());
            }
        }

        return list;
    }


    // =====================================================
    // SAVE PACKOUT
    // SP: SavePackoutRestrictData
    // =====================================================

    public async Task<int> SavePackoutAsync(
        PackoutSave data)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "SavePackoutRestrictData",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        cmd.Parameters.AddWithValue(
            "@CustID",
            data.CustId);

        cmd.Parameters.AddWithValue(
            "@PackoutRestrict",
            data.PackoutRestrict ?? "");

        cmd.Parameters.AddWithValue(
            "@ItemType",
            data.ItemType ?? "");

        cmd.Parameters.AddWithValue(
            "@Color",
            data.Color ?? "");

        cmd.Parameters.AddWithValue(
            "@Size",
            data.Size ?? "");

        cmd.Parameters.AddWithValue(
            "@UserId",
            data.UserId);


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
    // DELETE PACKOUT
    // SP: DeletePackoutRestrictionDetails
    // =====================================================

    public async Task<int> DeletePackoutAsync(
        int restrictId)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "DeletePackoutRestrictionDetails",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        cmd.Parameters.AddWithValue(
            "@PackoutRestrictId",
            restrictId);


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
    // HELPERS
    // =====================================================

    private static string GetStr(
        SqlDataReader reader,
        string column)
    {
        try
        {
            int index =
                reader.GetOrdinal(column);

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


    private static int GetInt(
        SqlDataReader reader,
        string column)
    {
        try
        {
            int index =
                reader.GetOrdinal(column);

            if (reader.IsDBNull(index))
                return 0;

            return Convert.ToInt32(
                reader.GetValue(index));
        }
        catch
        {
            return 0;
        }
    }
}
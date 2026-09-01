using System;
using System.Collections.Generic;
using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class BillingRepository : IBillingRepository
{
    private readonly string _cs;

    public BillingRepository(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;
    }


    // =====================================================
    // GET BILLING CHARGES BY CUSTOMER
    // Stored Procedure:
    // prcGETBillingChargesDtls
   

    public async Task<IEnumerable<ChargeDetails>>
        GetBillingbyCustIdAsync(int custId)
    {
        var list =
            new List<ChargeDetails>();

        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "prcGETBillingChargesDtls",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID",
            custId);


        await con.OpenAsync();

        using var r =
            await cmd.ExecuteReaderAsync();


        while (await r.ReadAsync())
        {
            list.Add(
                new ChargeDetails
                {
                    // IMPORTANT:
                    // Actual BillingCharges row ID
                    BillingChargesId =
                        GetInt(
                            r,
                            "BillingChargesId"),

                    ChargeType =
                        GetStr(
                            r,
                            "ChargeType"),

                    Charges =
                        GetStr(
                            r,
                            "Charges")
                }
            );
        }


        return list;
    }


    // =====================================================
    // GET BILLING DATA FOR DROPDOWN
    // Stored Procedure:
    // GetBillingData
    

    public async Task<IEnumerable<BillingData>>
        GetBillingDataAsync()
    {
        var list =
            new List<BillingData>();

        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "GetBillingData",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        await con.OpenAsync();

        using var r =
            await cmd.ExecuteReaderAsync();


        while (await r.ReadAsync())
        {
            list.Add(
                new BillingData
                {
                    BillingDataId =
                        GetInt(
                            r,
                            "BillingDataId"),

                    ChargeType =
                        GetStr(
                            r,
                            "ChargeType")
                }
            );
        }


        return list;
    }


    // =====================================================
    // SAVE BILLING CHARGE
    //
    // Stored Procedure:
    // prcSaveBillingCharges
    

    public async Task<int> SaveBillingAsync(
        BillingCharge charge)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "prcSaveBillingCharges",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        // BillingDataId selected from dropdown
        cmd.Parameters.AddWithValue(
            "@ChargeTypeId",
            charge.ChargeTypeId);


        cmd.Parameters.AddWithValue(
            "@custID",
            charge.CustId);


        cmd.Parameters.AddWithValue(
            "@Charges",
            charge.Charges);


        cmd.Parameters.AddWithValue(
            "@UpdtUser",
            (object?)charge.UpdatedUser
            ?? DBNull.Value);


        // OUTPUT RESULT

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
    // DELETE BILLING CHARGE
    

    public async Task<int> DeleteBillingAsync(
        int billingChargeId)
    {
        using var con =
            new SqlConnection(_cs);

        using var cmd =
            new SqlCommand(
                "prcDeleteBillingCharges",
                con);

        cmd.CommandType =
            CommandType.StoredProcedure;


        cmd.Parameters.AddWithValue(
            "@BillingChargesId",
            billingChargeId);


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
        SqlDataReader r,
        string col)
    {
        try
        {
            int i =
                r.GetOrdinal(col);

            return r.IsDBNull(i)
                ? ""
                : Convert.ToString(
                    r.GetValue(i))
                  ?? "";
        }
        catch
        {
            return "";
        }
    }


    private static int GetInt(
        SqlDataReader r,
        string col)
    {
        try
        {
            int i =
                r.GetOrdinal(col);

            return r.IsDBNull(i)
                ? 0
                : Convert.ToInt32(
                    r.GetValue(i));
        }
        catch
        {
            return 0;
        }
    }
}
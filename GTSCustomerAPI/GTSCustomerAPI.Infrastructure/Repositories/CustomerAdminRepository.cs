using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class CustomerAdminRepository
    : ICustomerAdminRepository
{
    private readonly string _cs;

    public CustomerAdminRepository(
        IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;  
    }

    public async Task<CustomerAdmin?>
        GetByCustIdAsync(int custId)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "AdmCsWr_GetCustomer", con);
        cmd.CommandType =
            CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID", custId);

        var ossParam = new SqlParameter(
            "@OSSFlag", SqlDbType.Bit)
        {
            Direction =
                ParameterDirection.Output
        };
        var stParam = new SqlParameter(
            "@STFlag", SqlDbType.Bit)
        {
            Direction =
                ParameterDirection.Output
        };
        cmd.Parameters.Add(ossParam);
        cmd.Parameters.Add(stParam);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();

        bool oss = ossParam.Value != DBNull.Value
            && Convert.ToBoolean(ossParam.Value);
        bool st = stParam.Value != DBNull.Value
            && Convert.ToBoolean(stParam.Value);

        return new CustomerAdmin
        {
            CustId = custId,
            OSSFlag = oss,
            STFlag = st
        };
    }

    public async Task<bool> SaveAsync(
        int custId, bool ossFlag, bool stFlag)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
    "AdmCsWr_SaveCustomer", con);
        cmd.CommandType =
            CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@CustID", custId);
        cmd.Parameters.AddWithValue(
            "@OSSFlag", ossFlag);
        cmd.Parameters.AddWithValue(
            "@STFlag", stFlag);

        await con.OpenAsync();

        // SP returns SELECT @@ROWCOUNT
        var rows = await cmd.ExecuteScalarAsync();
        return Convert.ToInt32(rows) > 0;
    }
}
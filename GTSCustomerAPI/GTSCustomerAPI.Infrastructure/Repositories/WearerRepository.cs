using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class WearerRepository : IWearerRepository
{
    private readonly string _cs;

    public WearerRepository(IConfiguration cfg)
    {
        _cs = cfg.GetConnectionString(
            "DefaultConnection")!;
    }

    public async Task<IEnumerable<NextWearer>>
        GetAllWearers(int custId)
    {
        return await RunSp(
            "AdmCsWr_GetAllWearers",
            cmd => cmd.Parameters
                .AddWithValue("@CustID", custId),
            custId);
    }

    public async Task<IEnumerable<NextWearer>>
        GetNextWearer(int custId, int? wrrId)
    {
        return await RunSp(
            "AdmCsWr_GetNextWearer",
            cmd =>
            {
                cmd.Parameters.AddWithValue(
                    "@CustID", custId);
                cmd.Parameters.AddWithValue(
                    "@LastWrID",
                    (object?)wrrId ?? DBNull.Value);
            },
            custId);
    }

    public async Task<IEnumerable<NextWearer>>
        GetPrevWearer(int custId, int? wrrId)
    {
        return await RunSp(
            "AdmCsWr_GetPrevWearer",
            cmd =>
            {
                cmd.Parameters.AddWithValue(
                    "@CustID", custId);
                cmd.Parameters.AddWithValue(
                    "@FirstWrID",
                    (object?)wrrId ?? DBNull.Value);
            },
            custId);
    }

    public async Task<IEnumerable<NextWearer>>
        GetWearer(int custId, string wrNbr)
    {
        return await RunSp(
            "AdmCsWr_GetWearer",
            cmd =>
            {
                cmd.Parameters.AddWithValue(
                    "@CustID", custId);
                cmd.Parameters.AddWithValue(
                    "@WearNbr", wrNbr);
            },
            custId);
    }

    private async Task<List<NextWearer>> RunSp(
        string sp,
        Action<SqlCommand> addParams,
        int custId)
    {
        var list = new List<NextWearer>();
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(sp, con);
        cmd.CommandType = CommandType.StoredProcedure;
        addParams(cmd);
        await con.OpenAsync();
        using var r = await cmd.ExecuteReaderAsync();
        while (await r.ReadAsync())
            list.Add(MapRow(r, custId));
        return list;
    }

    public async Task<int> SaveWearer(Wearer w)
    {
        using var con = new SqlConnection(_cs);
        using var cmd = new SqlCommand(
            "AdmCsWr_SaveWearer", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue(
            "@WearerID", w.WearerId);
        cmd.Parameters.AddWithValue(
            "@Locker",
            (object?)w.Locker ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@LockRm",
            (object?)w.LockRm ?? DBNull.Value);
        cmd.Parameters.AddWithValue(
            "@Sex", w.Sex);

        var ret = new SqlParameter(
            "@ret", SqlDbType.Int)
        {
            Direction = ParameterDirection.ReturnValue
        };
        cmd.Parameters.Add(ret);

        await con.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
        return Convert.ToInt32(ret.Value);
    }

    // ── MAPPER ──────────────────────────────────
    // SP columns: WearerId, CustId, CustNbr,
    //             WearNbr, FirstName, LastName,
    //             Locker, LockRm, Sex(int 0=F/1=M)
    private static NextWearer MapRow(
        SqlDataReader r, int custId)
    {
        // Print all columns for debugging
        for (int i = 0; i < r.FieldCount; i++)
        {
            Console.WriteLine(
                $"Col[{i}]: {r.GetName(i)} = " +
                $"{(r.IsDBNull(i) ? "NULL" : r.GetValue(i))}");
        }

        return new NextWearer
        {
            WearerId = GetInt(r, "WearerId"),
            CustId = custId,
            CustNbr = GetInt(r, "CustNbr"),
            WearNbr = GetAny(r, "WearNbr"),
            FirstName = GetAny(r, "FirstName"),
            LastName = GetAny(r, "LastName"),
            Locker = GetAny(r, "Locker"),
            LockRm = GetAny(r, "LockRm"),
            SexInt = GetInt(r, "Sex")
        };
    }

    // Reads ANY column type as string
    private static string? GetAny(
        SqlDataReader r, string col)
    {
        try
        {
            int i = r.GetOrdinal(col);
            if (r.IsDBNull(i)) return null;
            var val = r.GetValue(i)
                        ?.ToString()
                        ?.Trim();
            return string.IsNullOrEmpty(val)
                   ? null : val;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"GetAny({col}) error: {ex.Message}");
            return null;
        }
    }

    private static int GetInt(
        SqlDataReader r, string col)
    {
        try
        {
            int i = r.GetOrdinal(col);
            if (r.IsDBNull(i)) return 0;
            return Convert.ToInt32(r.GetValue(i));
        }
        catch { return 0; }
    }
}
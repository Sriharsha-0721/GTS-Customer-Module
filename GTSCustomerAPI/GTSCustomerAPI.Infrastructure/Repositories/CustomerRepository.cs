using System.Data;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace GTSCustomerAPI.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(IConfiguration configuration)
    {
        _connectionString = configuration
            .GetConnectionString("DefaultConnection")!;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var customers = new List<Customer>();

        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetAllCustomers", con);
        cmd.CommandType = CommandType.StoredProcedure;

        await con.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
            customers.Add(MapReaderToCustomer(reader));

        return customers;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_GetCustomerById", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustId", id);

        await con.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
            return MapReaderToCustomer(reader);

        return null;
    }

    public async Task<Customer> CreateAsync(Customer c)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_CreateCustomer", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@MarketCenter", c.MarketCenter);
        cmd.Parameters.AddWithValue("@CustNbr", c.CustNbr);
        cmd.Parameters.AddWithValue("@Account", (object?)c.Account ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Dept", (object?)c.Dept ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Name", (object?)c.Name ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Route", c.Route);
        cmd.Parameters.AddWithValue("@GarmInServ", (object?)c.GarmInServ ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@GID", c.GID);
        cmd.Parameters.AddWithValue("@StopDt", (object?)c.StopDt ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@MonSeq", c.MonSeq);
        cmd.Parameters.AddWithValue("@TueSeq", c.TueSeq);
        cmd.Parameters.AddWithValue("@WedSeq", c.WedSeq);
        cmd.Parameters.AddWithValue("@ThuSeq", c.ThuSeq);
        cmd.Parameters.AddWithValue("@FriSeq", c.FriSeq);
        cmd.Parameters.AddWithValue("@SatSeq", c.SatSeq);
        cmd.Parameters.AddWithValue("@SunSeq", c.SunSeq);
        cmd.Parameters.AddWithValue("@Addr1", (object?)c.Addr1 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Addr2", (object?)c.Addr2 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@City", (object?)c.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@State", (object?)c.State ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Zip", (object?)c.Zip ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Phone", (object?)c.Phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Fax", (object?)c.Fax ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Freq", (object?)c.Freq ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@OSSFlag", c.OSSFlag);
        cmd.Parameters.AddWithValue("@PONumber", (object?)c.PONumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillName", (object?)c.BillName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillAddr", (object?)c.BillAddr ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillCity", (object?)c.BillCity ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillState", (object?)c.BillState ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillZipCod", (object?)c.BillZipCod ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@UpdtUser", c.UpdtUser);

        await con.OpenAsync();
        var newId = await cmd.ExecuteScalarAsync();
        c.CustId = Convert.ToInt32(newId);

        return c;
    }

    
    public async Task<Customer?> UpdateAsync(Customer c)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_UpdateCustomer", con);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CustId", c.CustId);
        cmd.Parameters.AddWithValue("@Account", (object?)c.Account ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Dept", (object?)c.Dept ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Name", (object?)c.Name ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Route", c.Route);
        cmd.Parameters.AddWithValue("@Addr1", (object?)c.Addr1 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Addr2", (object?)c.Addr2 ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@City", (object?)c.City ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@State", (object?)c.State ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Zip", (object?)c.Zip ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Phone", (object?)c.Phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Fax", (object?)c.Fax ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@Freq", (object?)c.Freq ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@OSSFlag", c.OSSFlag);
        cmd.Parameters.AddWithValue("@PONumber", (object?)c.PONumber ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillName", (object?)c.BillName ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillAddr", (object?)c.BillAddr ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillCity", (object?)c.BillCity ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillState", (object?)c.BillState ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@BillZipCod", (object?)c.BillZipCod ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@UpdtUser", c.UpdtUser);

        await con.OpenAsync();
        var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return rows > 0 ? c : null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        using var con = new SqlConnection(_connectionString);
        using var cmd = new SqlCommand("sp_DeleteCustomer", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@CustId", id);

        await con.OpenAsync();
        var rows = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        return rows > 0;
    }
    private static Customer MapReaderToCustomer(SqlDataReader r) => new()
    {
        CustId = r.GetInt32(r.GetOrdinal("CustId")),
        MarketCenter = r.GetInt16(r.GetOrdinal("MarketCenter")),
        CustNbr = r.GetInt32(r.GetOrdinal("CustNbr")),
        Account = r.IsDBNull(r.GetOrdinal("Account")) ? null : r.GetInt32(r.GetOrdinal("Account")),
        Dept = r.IsDBNull(r.GetOrdinal("Dept")) ? null : r.GetInt32(r.GetOrdinal("Dept")),
        Name = r.IsDBNull(r.GetOrdinal("Name")) ? null : r.GetString(r.GetOrdinal("Name")),
        Route = r.GetInt32(r.GetOrdinal("Route")),
        GID = r.GetString(r.GetOrdinal("GID")),
        StopDt = r.IsDBNull(r.GetOrdinal("StopDt")) ? null : r.GetDateTime(r.GetOrdinal("StopDt")),
        Addr1 = r.IsDBNull(r.GetOrdinal("Addr1")) ? null : r.GetString(r.GetOrdinal("Addr1")),
        Addr2 = r.IsDBNull(r.GetOrdinal("Addr2")) ? null : r.GetString(r.GetOrdinal("Addr2")),
        City = r.IsDBNull(r.GetOrdinal("City")) ? null : r.GetString(r.GetOrdinal("City")),
        State = r.IsDBNull(r.GetOrdinal("State")) ? null : r.GetString(r.GetOrdinal("State")),
        Zip = r.IsDBNull(r.GetOrdinal("Zip")) ? null : r.GetString(r.GetOrdinal("Zip")),
        Phone = r.IsDBNull(r.GetOrdinal("Phone")) ? null : r.GetString(r.GetOrdinal("Phone")),
        Fax = r.IsDBNull(r.GetOrdinal("Fax")) ? null : r.GetString(r.GetOrdinal("Fax")),
        Freq = r.IsDBNull(r.GetOrdinal("Freq")) ? null : r.GetString(r.GetOrdinal("Freq")),
        OSSFlag = r.GetBoolean(r.GetOrdinal("OSSFlag")),
        CreateDt = r.IsDBNull(r.GetOrdinal("CreateDt")) ? null : r.GetDateTime(r.GetOrdinal("CreateDt")),
        PONumber = r.IsDBNull(r.GetOrdinal("PONumber")) ? null : r.GetString(r.GetOrdinal("PONumber")),
        BillName = r.IsDBNull(r.GetOrdinal("BillName")) ? null : r.GetString(r.GetOrdinal("BillName")),
        BillCity = r.IsDBNull(r.GetOrdinal("BillCity")) ? null : r.GetString(r.GetOrdinal("BillCity")),
        BillState = r.IsDBNull(r.GetOrdinal("BillState")) ? null : r.GetString(r.GetOrdinal("BillState")),
        BillZipCod = r.IsDBNull(r.GetOrdinal("BillZipCod")) ? null : r.GetString(r.GetOrdinal("BillZipCod")),
        UpdtUser = r.GetInt32(r.GetOrdinal("UpdtUser")),
        UpdtTime = r.GetDateTime(r.GetOrdinal("UpdtTime"))
    };
}
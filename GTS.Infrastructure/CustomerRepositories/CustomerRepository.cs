using Microsoft.Data.SqlClient;
using System.Data;
using GTS.Application.Interfaces.CustomerRepositories;
using GTS.Domain.Entities;
using Microsoft.Extensions.Configuration;

namespace GTS.Infrastructure.CustomerRepositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private Customer MapReaderToCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustId = reader.GetInt32(reader.GetOrdinal("CustId")),
                MarketCenter = reader.GetInt16(reader.GetOrdinal("MarketCenter")),
                CustNbr = reader.GetInt32(reader.GetOrdinal("CustNbr")),
                Account = reader.IsDBNull(reader.GetOrdinal("Account")) ? null : reader.GetInt32(reader.GetOrdinal("Account")),
                Dept = reader.IsDBNull(reader.GetOrdinal("Dept")) ? null : reader.GetInt32(reader.GetOrdinal("Dept")),
                Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                Route = reader.GetInt32(reader.GetOrdinal("Route")),
                GID = reader.GetString(reader.GetOrdinal("GID")),
                StopDt = reader.IsDBNull(reader.GetOrdinal("StopDt")) ? null : reader.GetDateTime(reader.GetOrdinal("StopDt")),
                Addr1 = reader.IsDBNull(reader.GetOrdinal("Addr1")) ? null : reader.GetString(reader.GetOrdinal("Addr1")),
                Addr2 = reader.IsDBNull(reader.GetOrdinal("Addr2")) ? null : reader.GetString(reader.GetOrdinal("Addr2")),
                City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                State = reader.IsDBNull(reader.GetOrdinal("State")) ? null : reader.GetString(reader.GetOrdinal("State")),
                Zip = reader.IsDBNull(reader.GetOrdinal("Zip")) ? null : reader.GetString(reader.GetOrdinal("Zip")),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax")),
                Freq = reader.IsDBNull(reader.GetOrdinal("Freq")) ? null : reader.GetString(reader.GetOrdinal("Freq")),
                Contact = reader.IsDBNull(reader.GetOrdinal("Contact")) ? null : reader.GetString(reader.GetOrdinal("Contact")),
                CreateDt = reader.IsDBNull(reader.GetOrdinal("CreateDt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreateDt")),
                PONumber = reader.IsDBNull(reader.GetOrdinal("PONumber")) ? null : reader.GetString(reader.GetOrdinal("PONumber")),
                BillName = reader.IsDBNull(reader.GetOrdinal("BillName")) ? null : reader.GetString(reader.GetOrdinal("BillName")),
                BillAddr = reader.IsDBNull(reader.GetOrdinal("BillAddr")) ? null : reader.GetString(reader.GetOrdinal("BillAddr")),
                BillCity = reader.IsDBNull(reader.GetOrdinal("BillCity")) ? null : reader.GetString(reader.GetOrdinal("BillCity")),
                BillState = reader.IsDBNull(reader.GetOrdinal("BillState")) ? null : reader.GetString(reader.GetOrdinal("BillState")),
                BillZipCod = reader.IsDBNull(reader.GetOrdinal("BillZipCod")) ? null : reader.GetString(reader.GetOrdinal("BillZipCod")),
                BillPhone = reader.IsDBNull(reader.GetOrdinal("BillPhone")) ? null : reader.GetString(reader.GetOrdinal("BillPhone")),
                UpdtTime = reader.GetDateTime(reader.GetOrdinal("UpdtTime"))
            };
        }

        private void AddCustomerParameters(SqlCommand cmd, Customer customer)
        {
            cmd.Parameters.AddWithValue("@MarketCenter", customer.MarketCenter);
            cmd.Parameters.AddWithValue("@CustNbr", customer.CustNbr);
            cmd.Parameters.AddWithValue("@Account", (object?)customer.Account ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Dept", (object?)customer.Dept ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Name", (object?)customer.Name ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Route", customer.Route);
            cmd.Parameters.AddWithValue("@GarmInServ", (object?)customer.GarmInServ ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@GID", customer.GID);
            cmd.Parameters.AddWithValue("@StopDt", (object?)customer.StopDt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MonSeq", customer.MonSeq);
            cmd.Parameters.AddWithValue("@TueSeq", customer.TueSeq);
            cmd.Parameters.AddWithValue("@WedSeq", customer.WedSeq);
            cmd.Parameters.AddWithValue("@ThuSeq", customer.ThuSeq);
            cmd.Parameters.AddWithValue("@FriSeq", customer.FriSeq);
            cmd.Parameters.AddWithValue("@SatSeq", customer.SatSeq);
            cmd.Parameters.AddWithValue("@SunSeq", customer.SunSeq);
            cmd.Parameters.AddWithValue("@Addr1", (object?)customer.Addr1 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Addr2", (object?)customer.Addr2 ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@City", (object?)customer.City ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@State", (object?)customer.State ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Zip", (object?)customer.Zip ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Phone", (object?)customer.Phone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Fax", (object?)customer.Fax ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Freq", (object?)customer.Freq ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@OSSFlag", customer.OSSFlag);
            cmd.Parameters.AddWithValue("@SoilFlag", (object?)customer.SoilFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@LRFlag", (object?)customer.LRFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@STFlag", (object?)customer.STFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InvSeq", (object?)customer.InvSeq ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@MastAcctNbr", (object?)customer.MastAcctNbr ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NatAcctNbr", (object?)customer.NatAcctNbr ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PrepFlag", (object?)customer.PrepFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@NameFlag", (object?)customer.NameFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ProdFlag", (object?)customer.ProdFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@EmbrFlag", (object?)customer.EmbrFlag ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CreateDt", (object?)customer.CreateDt ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PONumber", (object?)customer.PONumber ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@SterileCode", (object?)customer.SterileCode ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CtmndFlg", (object?)customer.CtmndFlg ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@DelTicket", (object?)customer.DelTicket ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@PropertyMark", (object?)customer.PropertyMark ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@ShipVia", (object?)customer.ShipVia ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Package", (object?)customer.Package ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Contact", (object?)customer.Contact ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillName", (object?)customer.BillName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillAddr", (object?)customer.BillAddr ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillExAddr", (object?)customer.BillExAddr ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillCity", (object?)customer.BillCity ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillState", (object?)customer.BillState ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillZipCod", (object?)customer.BillZipCod ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@BillPhone", (object?)customer.BillPhone ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CntnrsIn", (object?)customer.CntnrsIn ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@CntnrsOut", (object?)customer.CntnrsOut ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@UpdtUser", customer.UpdtUser);
            cmd.Parameters.AddWithValue("@UpdtTime", customer.UpdtTime);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            var customers = new List<Customer>();
            using var connection = CreateConnection();
            using var command = new SqlCommand("sp_GetAllCustomers", connection);
            command.CommandType = CommandType.StoredProcedure;
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                customers.Add(MapReaderToCustomer(reader));
            return customers;
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("sp_GetCustomerById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustId", id);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                return MapReaderToCustomer(reader);
            return null;
        }

        public async Task<Customer?> AddAsync(Customer customer)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("sp_CreateCustomer", connection);
            command.CommandType = CommandType.StoredProcedure;
            AddCustomerParameters(command, customer);
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                customer.CustId = reader.GetInt32(reader.GetOrdinal("CustId"));
            return customer;
        }

        public async Task UpdateAsync(Customer customer)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("sp_UpdateCustomer", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustId", customer.CustId);
            AddCustomerParameters(command, customer);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("sp_DeleteCustomer", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@CustId", id);
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
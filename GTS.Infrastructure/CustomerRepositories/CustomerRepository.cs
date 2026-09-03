using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerRepositories;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.CustomerRepositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _connectionString;

        public CustomerRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        // 1. Grid Selection & Search SP: dbo.GTS_GetFilteredCustomers
        public async Task<IEnumerable<CustomersSelListDto>> GetFilteredCustomersAsync(CustomerFilterRequestDto filter, CancellationToken ct = default)
        {
            var list = new List<CustomersSelListDto>();

            using var connection = CreateConnection();
            using var command = new SqlCommand("dbo.GTS_GetFilteredCustomers", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.AddWithValue("@NumRecsToFetch", filter.NumRecsToFetch);
            command.Parameters.AddWithValue("@MC", filter.MarketCenter);
            command.Parameters.AddWithValue("@Route", filter.Route);
            command.Parameters.AddWithValue("@wDay", filter.WDay);
            command.Parameters.AddWithValue("@GID", string.IsNullOrWhiteSpace(filter.GID) ? string.Empty : filter.GID);
            command.Parameters.AddWithValue("@CID", filter.CustNbr);
            command.Parameters.AddWithValue("@CustName", string.IsNullOrWhiteSpace(filter.CustName) ? string.Empty : filter.CustName);
            command.Parameters.AddWithValue("@WearerNbr", filter.WearerNbr);

            await connection.OpenAsync(ct);
            using var reader = await command.ExecuteReaderAsync(ct);
            while (await reader.ReadAsync(ct))
            {
                list.Add(new CustomersSelListDto
                {
                    CustId = reader.GetInt32(reader.GetOrdinal("CustId")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? null : reader.GetString(reader.GetOrdinal("Name")),
                    MarketCenter = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("MarketCenter"))),
                    CustNbr = reader.GetInt32(reader.GetOrdinal("CustNbr")),
                    Route = reader.GetInt32(reader.GetOrdinal("Route")),
                    GID = reader.IsDBNull(reader.GetOrdinal("GID")) ? null : reader.GetString(reader.GetOrdinal("GID")),
                    WDay = reader.IsDBNull(reader.GetOrdinal("wDay")) ? null : reader.GetString(reader.GetOrdinal("wDay")),
                    Addr1 = reader.IsDBNull(reader.GetOrdinal("Addr1")) ? null : reader.GetString(reader.GetOrdinal("Addr1")),
                    Addr2 = reader.IsDBNull(reader.GetOrdinal("Addr2")) ? null : reader.GetString(reader.GetOrdinal("Addr2")),
                    City = reader.IsDBNull(reader.GetOrdinal("City")) ? null : reader.GetString(reader.GetOrdinal("City")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader.GetString(reader.GetOrdinal("Phone")),
                    Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? null : reader.GetString(reader.GetOrdinal("Fax")),

                    // SAFE CAST: Handles both SQL bit (Boolean) or char/varchar
                    CtmndFlg = reader.IsDBNull(reader.GetOrdinal("CtmndFlg")) ? null : reader.GetValue(reader.GetOrdinal("CtmndFlg")).ToString(),
                    SterileCode = reader.IsDBNull(reader.GetOrdinal("SterileCode")) ? null : reader.GetValue(reader.GetOrdinal("SterileCode")).ToString(),
                    StopDt = reader.IsDBNull(reader.GetOrdinal("StopDt")) ? null : reader.GetValue(reader.GetOrdinal("StopDt")).ToString()
                });
            }

            return list;
        }

        // 2. Submodule: Customer Flags - Read
        public async Task<CustomerFlagsDto?> GetCustomerFlagsAsync(int custId, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("dbo.GetCustInfo", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CustId", custId);

            await connection.OpenAsync(ct);
            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                return new CustomerFlagsDto
                {
                    CustId = reader.GetInt32(reader.GetOrdinal("CustId")),
                    OSSFlag = !reader.IsDBNull(reader.GetOrdinal("OSSFlag")) && reader.GetBoolean(reader.GetOrdinal("OSSFlag")),
                    STFFlag = !reader.IsDBNull(reader.GetOrdinal("STFlag")) && reader.GetBoolean(reader.GetOrdinal("STFlag"))
                };
            }

            return null;
        }

        // 3. Submodule: Customer Flags - Save
        public async Task<int> SaveCustomerFlagsAsync(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("dbo.SaveCustomerFlags", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CustId", custId);
            command.Parameters.AddWithValue("@OSSFlag", ossFlag);
            command.Parameters.AddWithValue("@STFlag", stfFlag);

            await connection.OpenAsync(ct);
            return await command.ExecuteNonQueryAsync(ct);
        }

        // 4. Submodule: Customer Profile - Read
        public async Task<CustomerProfileDto?> GetCustomerProfileAsync(int custId, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("dbo.ReadCustProfileData", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CustId", custId);

            await connection.OpenAsync(ct);
            using var reader = await command.ExecuteReaderAsync(ct);
            if (await reader.ReadAsync(ct))
            {
                return new CustomerProfileDto
                {
                    CustId = reader.GetInt32(reader.GetOrdinal("CustId")),
                    CustNbr = reader.GetInt32(reader.GetOrdinal("CustNbr")),
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                    Route = reader.GetInt32(reader.GetOrdinal("Route")),
                    GID = reader.IsDBNull(reader.GetOrdinal("GID")) ? string.Empty : reader.GetString(reader.GetOrdinal("GID")),
                    Addr1 = reader.IsDBNull(reader.GetOrdinal("Addr1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Addr1")),
                    City = reader.IsDBNull(reader.GetOrdinal("City")) ? string.Empty : reader.GetString(reader.GetOrdinal("City")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone"))
                };
            }

            return null;
        }

        // 5. Submodule: Customer Profile - Update
        public async Task<bool> UpdateCustomerProfileAsync(UpdateCustomerProfileDto dto, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            using var command = new SqlCommand("dbo.UpdateCustomerProfile", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@CustId", dto.CustId);
            command.Parameters.AddWithValue("@Name", dto.Name ?? string.Empty);
            command.Parameters.AddWithValue("@Route", dto.Route);

            await connection.OpenAsync(ct);
            var rows = await command.ExecuteNonQueryAsync(ct);
            return rows > 0;
        }
    }
}
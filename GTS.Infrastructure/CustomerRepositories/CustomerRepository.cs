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

            command.Parameters.AddWithValue("@NumRecsToFetch", filter.NumRecsToFetch > 0 ? filter.NumRecsToFetch : 500);
            short marketCenter = filter.MarketCenter > 0 ? (short)filter.MarketCenter : (short)561;
            command.Parameters.AddWithValue("@MC", marketCenter);
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
                    Name = reader.IsDBNull(reader.GetOrdinal("Name")) ? string.Empty : reader.GetString(reader.GetOrdinal("Name")),
                    MarketCenter = Convert.ToInt16(reader.GetValue(reader.GetOrdinal("MarketCenter"))),
                    CustNbr = reader.GetInt32(reader.GetOrdinal("CustNbr")),
                    Route = reader.GetInt32(reader.GetOrdinal("Route")),
                    GID = reader.IsDBNull(reader.GetOrdinal("GID")) ? string.Empty : reader.GetString(reader.GetOrdinal("GID")),
                    WDay = reader.IsDBNull(reader.GetOrdinal("wDay")) ? string.Empty : reader.GetValue(reader.GetOrdinal("wDay")).ToString()!,
                    Addr1 = reader.IsDBNull(reader.GetOrdinal("Addr1")) ? string.Empty : reader.GetString(reader.GetOrdinal("Addr1")),
                    Addr2 = reader.IsDBNull(reader.GetOrdinal("Addr2")) ? string.Empty : reader.GetString(reader.GetOrdinal("Addr2")),
                    City = reader.IsDBNull(reader.GetOrdinal("City")) ? string.Empty : reader.GetString(reader.GetOrdinal("City")),
                    Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone")),
                    Fax = reader.IsDBNull(reader.GetOrdinal("Fax")) ? string.Empty : reader.GetString(reader.GetOrdinal("Fax")),
                    CtmndFlg = reader.IsDBNull(reader.GetOrdinal("CtmndFlg")) ? null : reader.GetValue(reader.GetOrdinal("CtmndFlg")).ToString(),
                    SterileCode = reader.IsDBNull(reader.GetOrdinal("SterileCode")) ? null : reader.GetValue(reader.GetOrdinal("SterileCode")).ToString(),
                    StopDt = reader.IsDBNull(reader.GetOrdinal("StopDt")) ? null : reader.GetValue(reader.GetOrdinal("StopDt")).ToString()
                });
            }

            return list;
        }

        // 2. Submodule: Customer Flags - Read via dbo.AdmCsWr_GetCustomer
        // 2. Submodule: Customer Flags - Read via dbo.sp_GetCustomerById
        public async Task<CustomerFlagsDto?> GetCustomerFlagsAsync(int custId, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(ct);

            using var command = new SqlCommand("dbo.sp_GetCustomerById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@CustId", SqlDbType.Int) { Value = custId });

            using var reader = await command.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            bool SafeBit(string col)
            {
                try
                {
                    int ord = reader.GetOrdinal(col);
                    return !reader.IsDBNull(ord) && Convert.ToBoolean(reader.GetValue(ord));
                }
                catch { return false; }
            }

            return new CustomerFlagsDto
            {
                CustId = custId,
                OSSFlag = SafeBit("OSSFlag"),
                STFFlag = SafeBit("STFlag")
            };
        }

        // 3. Submodule: Customer Flags - Save via dbo.sp_UpdateCustomer
        // 3. Submodule: Customer Flags - Save via dbo.sp_UpdateCustomer
        public async Task<int> SaveCustomerFlagsAsync(int custId, bool ossFlag, bool stfFlag, CancellationToken ct = default)
        {
            CustomerProfileDto? existing = await GetCustomerProfileAsync(custId, ct);
            if (existing == null) return 0;

            using var connection = CreateConnection();
            await connection.OpenAsync(ct);

            using var command = new SqlCommand("dbo.sp_UpdateCustomer", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@CustId", SqlDbType.Int) { Value = custId });
            command.Parameters.Add(new SqlParameter("@MarketCenter", SqlDbType.SmallInt) { Value = existing.MarketCenter });
            command.Parameters.Add(new SqlParameter("@CustNbr", SqlDbType.Int) { Value = existing.CustNbr });
            command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50) { Value = (object?)existing.Name ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@Route", SqlDbType.Int) { Value = existing.Route });
            command.Parameters.Add(new SqlParameter("@GID", SqlDbType.VarChar, 9) { Value = (object?)existing.GID ?? DBNull.Value });

            command.Parameters.Add(new SqlParameter("@OSSFlag", SqlDbType.Bit) { Value = ossFlag });
            command.Parameters.Add(new SqlParameter("@STFlag", SqlDbType.Bit) { Value = stfFlag });

            command.Parameters.Add(new SqlParameter("@UpdtUser", SqlDbType.Int) { Value = 1 });
            command.Parameters.Add(new SqlParameter("@UpdtTime", SqlDbType.DateTime) { Value = DateTime.Now });

            return await command.ExecuteNonQueryAsync(ct);
        }

        // 4. Submodule: Customer Profile - Read via dbo.sp_GetCustomerById
        public async Task<CustomerProfileDto?> GetCustomerProfileAsync(int custId, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(ct);

            using var command = new SqlCommand("dbo.sp_GetCustomerById", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.Add(new SqlParameter("@CustId", SqlDbType.Int) { Value = custId });

            using var reader = await command.ExecuteReaderAsync(ct);
            if (!await reader.ReadAsync(ct)) return null;

            string SafeStr(string col)
            {
                try
                {
                    int ord = reader.GetOrdinal(col);
                    return reader.IsDBNull(ord) ? string.Empty : reader.GetValue(ord).ToString()!.Trim();
                }
                catch { return string.Empty; }
            }

            int SafeInt(string col)
            {
                try
                {
                    int ord = reader.GetOrdinal(col);
                    return reader.IsDBNull(ord) ? 0 : Convert.ToInt32(reader.GetValue(ord));
                }
                catch { return 0; }
            }

            bool SafeBit(string col)
            {
                try
                {
                    int ord = reader.GetOrdinal(col);
                    return !reader.IsDBNull(ord) && Convert.ToBoolean(reader.GetValue(ord));
                }
                catch { return false; }
            }

            return new CustomerProfileDto
            {
                CustId = SafeInt("CustId") > 0 ? SafeInt("CustId") : custId,
                CustNbr = SafeInt("CustNbr"),
                Name = SafeStr("Name"),
                Route = SafeInt("Route"),
                GID = SafeStr("GID"),
                MarketCenter = (short)SafeInt("MarketCenter"),
                BillingCom = SafeStr("BillingCom"),
                PackoutCom = SafeStr("PackoutCom"),
                WashCom = SafeStr("WashCom"),
                SoilCom = SafeStr("SoilCom"),
                DryerCom = SafeStr("DryerCom"),
                ReceivingCom = SafeStr("ReceivingCom"),
                ShippingCom = SafeStr("ShippingCom"),
                DriverCom = SafeStr("DriverCom"),
                MendCom = SafeStr("MendCom"),
                QACom = SafeStr("QACom"),
                CustSrvCom = SafeStr("CustSrvCom"),
                OfficeCom = SafeStr("OfficeCom"),
                GenOfficeCom = SafeStr("GenOfficeCom"),
                MerControlCom = SafeStr("MerControlCom"),
                MainCleanRoomCom = SafeStr("MainCleanRoomCom"),
                QAInspCom = SafeStr("QAInspCom"),
                ProdCom = SafeStr("ProdCom"),
                OSSFlag = SafeBit("OSSFlag")
            };
        }

        // 5. Submodule: Customer Profile - Update via dbo.sp_UpdateCustomer
        public async Task<bool> UpdateCustomerProfileAsync(UpdateCustomerProfileDto dto, CancellationToken ct = default)
        {
            using var connection = CreateConnection();
            await connection.OpenAsync(ct);

            CustomerProfileDto? existing = await GetCustomerProfileAsync(dto.CustId, ct);
            if (existing == null) return false;

            using var command = new SqlCommand("dbo.sp_UpdateCustomer", connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            command.Parameters.Add(new SqlParameter("@CustId", SqlDbType.Int) { Value = dto.CustId });
            command.Parameters.Add(new SqlParameter("@MarketCenter", SqlDbType.SmallInt) { Value = existing.MarketCenter });
            command.Parameters.Add(new SqlParameter("@CustNbr", SqlDbType.Int) { Value = existing.CustNbr });
            command.Parameters.Add(new SqlParameter("@Name", SqlDbType.VarChar, 50) { Value = (object?)existing.Name ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@Route", SqlDbType.Int) { Value = existing.Route });
            command.Parameters.Add(new SqlParameter("@GID", SqlDbType.VarChar, 9) { Value = (object?)existing.GID ?? DBNull.Value });

            command.Parameters.Add(new SqlParameter("@ProdCom", SqlDbType.VarChar, 5100) { Value = (object?)dto.ProdCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@OfficeCom", SqlDbType.VarChar, 5100) { Value = (object?)dto.OfficeCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@GenOfficeCom", SqlDbType.VarChar, 750) { Value = (object?)dto.GenOfficeCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@MerControlCom", SqlDbType.VarChar, 750) { Value = (object?)dto.MerControlCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@ReceivingCom", SqlDbType.VarChar, 750) { Value = (object?)dto.ReceivingCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@SoilCom", SqlDbType.VarChar, 750) { Value = (object?)dto.SoilCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@WashCom", SqlDbType.VarChar, 750) { Value = (object?)dto.WashCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@DryerCom", SqlDbType.VarChar, 750) { Value = (object?)dto.DryerCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@MainCleanRoomCom", SqlDbType.VarChar, 750) { Value = (object?)dto.MainCleanRoomCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@PackoutCom", SqlDbType.VarChar, 750) { Value = (object?)dto.PackoutCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@ShippingCom", SqlDbType.VarChar, 750) { Value = (object?)dto.ShippingCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@DriverCom", SqlDbType.VarChar, 750) { Value = (object?)dto.DriverCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@MendCom", SqlDbType.VarChar, 750) { Value = (object?)dto.MendCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@QACom", SqlDbType.VarChar, 750) { Value = (object?)dto.QACom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@CustSrvCom", SqlDbType.VarChar, 750) { Value = (object?)dto.CustSrvCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@BillingCom", SqlDbType.VarChar, 750) { Value = (object?)dto.BillingCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@QAInspCom", SqlDbType.VarChar, 250) { Value = (object?)dto.QAInspCom ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@Formula", SqlDbType.VarChar, 250) { Value = (object?)dto.Formula ?? DBNull.Value });
            command.Parameters.Add(new SqlParameter("@UpdtUser", SqlDbType.Int) { Value = 1 });
            command.Parameters.Add(new SqlParameter("@UpdtTime", SqlDbType.DateTime) { Value = DateTime.Now });

            int rows = await command.ExecuteNonQueryAsync(ct);
            return rows > 0;
        }
    }
}
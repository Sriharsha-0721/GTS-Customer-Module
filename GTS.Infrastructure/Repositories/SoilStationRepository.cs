using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace GTS.Infrastructure.Repositories
{
    public class SoilStationRepository(IConfiguration configuration) : ISoilStationRepository
    {
        private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string is not configured.");

        public async Task<ReceiverDto?> GetReceiverDataAsync(long receiverId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Rcv_GetReceiverData", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@RCV", receiverId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new ReceiverDto
                {
                    IsSuccess = true,
                    ReceiverId = receiverId,
                    CustId = reader["CustId"] != DBNull.Value ? Convert.ToInt32(reader["CustId"]) : 0,
                    MarketCenter = reader["MarketCenter"] != DBNull.Value ? Convert.ToInt32(reader["MarketCenter"]) : 0,
                    CustNbr = reader["CustNbr"] != DBNull.Value ? Convert.ToInt32(reader["CustNbr"]) : 0,
                    CustomerName = reader["ShipName"]?.ToString(),
                    UserName = reader["UserName"]?.ToString(),
                    Status = reader["Status"]?.ToString(),
                    StopDt = reader["StopDt"]?.ToString(),
                    WashComments = reader["WashCom"]?.ToString(),
                    SoilComments = reader["SoilCom"]?.ToString()
                };
            }
            return null;
        }

        public async Task<bool> CheckContainerAsync(string containerId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Contnr_CheckContnr", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@CntrID", containerId);
            var returnParam = command.Parameters.Add("@Result", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)returnParam.Value == 0;
        }

        public async Task<GarmentInfoDto?> GetGarmentInfoAsync(string garmentBarcode)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Scan_GetGarmentInfo", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@BarCode", garmentBarcode);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new GarmentInfoDto
                {
                    Garment = reader["Garment"].ToString() ?? string.Empty,
                    CustId = reader["CustId"] != DBNull.Value ? Convert.ToInt32(reader["CustId"]) : 0,
                    WearerId = reader["WearerId"] != DBNull.Value ? Convert.ToInt32(reader["WearerId"]) : 0,
                    WearItemId = reader["WearItemId"] != DBNull.Value ? Convert.ToInt32(reader["WearItemId"]) : 0,
                    ItemCode = reader["ItemCode"].ToString() ?? string.Empty,
                    Size = reader["Size"]?.ToString(),
                    ProcStn = reader["ProcStn"] != DBNull.Value ? Convert.ToInt32(reader["ProcStn"]) : 0,
                    ScanCode = reader["ScanCode"]?.ToString(),
                    ScanDt = reader["ScanDt"] != DBNull.Value ? Convert.ToDateTime(reader["ScanDt"]) : null,
                    ServCode = reader["ServCode"]?.ToString(),
                    StopDt = reader["StopDt"]?.ToString()
                };
            }
            return null;
        }

        public async Task<bool> CheckPickUpDayAsync(int custId, int pickupDay)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Scan_CheckPickUpDay", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@CustID", custId);
            command.Parameters.AddWithValue("@PuDay", pickupDay);
            var returnParam = command.Parameters.Add("@Result", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)returnParam.Value == 0;
        }

        public async Task<int> CheckExceedsMaxWashAsync(int custId, string itemCode, string garment)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Rcv_ExceedsMaxWash", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@CustId", custId);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            command.Parameters.AddWithValue("@Garment", garment);

            var outRes = command.Parameters.Add("@Res", SqlDbType.Int);
            outRes.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return outRes.Value != DBNull.Value ? Convert.ToInt32(outRes.Value) : 0;
        }

        public async Task<bool> UpdateGarminServAsync(string garment, string pScanCode, DateTime? pScanDt, string scanCode, DateTime scanDt, int procStn, int updtUser, DateTime updtTime)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Update_GarminServ", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@Garment", garment);
            command.Parameters.AddWithValue("@pScanCode", (object?)pScanCode ?? DBNull.Value);
            command.Parameters.AddWithValue("@pScandt", (object?)pScanDt ?? DBNull.Value);
            command.Parameters.AddWithValue("@ScanCode", scanCode);
            command.Parameters.AddWithValue("@ScanDt", scanDt);
            command.Parameters.AddWithValue("@ProcStn", procStn);
            command.Parameters.AddWithValue("@UpdtUser", updtUser);
            command.Parameters.AddWithValue("@UpdtTime", updtTime);

            var returnParam = command.Parameters.Add("@Result", SqlDbType.Int);
            returnParam.Direction = ParameterDirection.ReturnValue;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return (int)returnParam.Value == 1;
        }

        public async Task<int> LogScanAsync(string barcode, string operType, string scanCode, int custId, int wearerId, int wearItemId, string itemCode, string size, string servCode, string statFlag, DateTime lotDate, int procStn, int userId, long receiverId, string containerId, int overlayNbr)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.cScan_LogScan2_CR", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@BarCode", barcode);
            command.Parameters.AddWithValue("@OperType", operType);
            command.Parameters.AddWithValue("@ScanCode", scanCode);
            command.Parameters.AddWithValue("@CustID", custId);
            command.Parameters.AddWithValue("@WearerID", wearerId);
            command.Parameters.AddWithValue("@WearItemId", wearItemId);
            command.Parameters.AddWithValue("@ItemCode", itemCode);
            command.Parameters.AddWithValue("@ItemSize", size);
            command.Parameters.AddWithValue("@ServCode", servCode);
            command.Parameters.AddWithValue("@StatFlag", statFlag);
            command.Parameters.AddWithValue("@LotDate", lotDate);
            command.Parameters.AddWithValue("@ProcStn", procStn);
            command.Parameters.AddWithValue("@UserID", userId);
            command.Parameters.AddWithValue("@Rcvr", receiverId);
            command.Parameters.AddWithValue("@Cntr", containerId);
            command.Parameters.AddWithValue("@OvrlNbr", overlayNbr);

            var scanLogIdParam = command.Parameters.Add("@ScanLogID", SqlDbType.Int);
            scanLogIdParam.Direction = ParameterDirection.Output;

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();

            return scanLogIdParam.Value != DBNull.Value ? Convert.ToInt32(scanLogIdParam.Value) : 0;
        }

        public async Task<SpecialInstructionsDto?> GetSpecialInstructionsAsync(int receiverId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("dbo.Rcv_GetReceiverData", connection) { CommandType = CommandType.StoredProcedure };
            command.Parameters.AddWithValue("@RCV", receiverId);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new SpecialInstructionsDto
                {
                    ReceiverId = receiverId,
                    CustomerName = reader["ShipName"]?.ToString(),
                    CustomerNumber = reader["CustNbr"] != DBNull.Value ? Convert.ToInt32(reader["CustNbr"]) : 0,
                    WashComments = reader["WashCom"]?.ToString(),
                    SoilComments = reader["SoilCom"]?.ToString(),
                    ReceivingComments = reader["ReceivingCom"]?.ToString(),
                    ProductionComments = reader["ProdCom"]?.ToString(),
                    OfficeComments = reader["OfficeCom"]?.ToString(),
                    GenOfficeComments = reader["GenOfficeCom"]?.ToString(),
                    MerControlComments = reader["MerControlCom"]?.ToString(),
                    DryerComments = reader["DryerCom"]?.ToString(),
                    MainCleanRoomComments = reader["MainCleanRoomCom"]?.ToString(),
                    PackoutComments = reader["PackoutCom"]?.ToString(),
                    ShippingComments = reader["ShippingCom"]?.ToString(),
                    DriverComments = reader["DriverCom"]?.ToString(),
                    MendComments = reader["MendCom"]?.ToString(),
                    QAComments = reader["QACom"]?.ToString(),
                    CustServiceComments = reader["CustSrvCom"]?.ToString(),
                    BillingComments = reader["BillingCom"]?.ToString(),
                    QAInspComments = reader["QAInspCom"]?.ToString()
                };
            }
            return null;
        }
    }
}
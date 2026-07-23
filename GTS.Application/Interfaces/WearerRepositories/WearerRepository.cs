using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using GTS.Domain.Entities;
using GTS.Application.DTOs;
using GTS.Application.Interfaces.WearerRepositories;

namespace GTS.Infrastructure.WearerRepositories
{
    public class WearerRepository : IWearerRepository
    {
        private readonly string _connectionString;

        public WearerRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("DefaultConnection")!;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        private Wearer MapReaderToWearer(SqlDataReader reader)
        {
            return new Wearer
            {
                WearerId =
                    reader.GetInt32(reader.GetOrdinal("WearerId")),

                WearNbr =
                    reader.GetString(reader.GetOrdinal("WearNbr")),

                FirstName =
                    reader.GetString(reader.GetOrdinal("FirstName")),

                LastName =
                    reader.GetString(reader.GetOrdinal("LastName")),

                Locker =
                    reader.GetString(reader.GetOrdinal("Locker")),

                LockRm =
                    reader.GetString(reader.GetOrdinal("LockRm")),

                Sex =
                    reader.GetInt32(reader.GetOrdinal("Sex")) == 1
            };
        }

        public async Task<Wearer?> GetWearerAsync(
    int custId,
    string wearNbr)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand("AdmCsWr_GetWearer", connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustID", custId);

            command.Parameters.AddWithValue("@WearNbr", wearNbr);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var wearer = MapReaderToWearer(reader);

                wearer.CustId = custId;

                return wearer;
            }

            return null;
        }
        public async Task<Wearer?> GetNextWearerAsync(
    int custId,
    int? wearerId)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand("AdmCsWr_GetNextWearer", connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustID", custId);

            if (wearerId.HasValue)
                command.Parameters.AddWithValue("@LastWrID", wearerId.Value);
            else
                command.Parameters.AddWithValue("@LastWrID", DBNull.Value);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var wearer = MapReaderToWearer(reader);

                wearer.CustId = custId;

                return wearer;
            }

            return null;
        }
        public async Task<Wearer?> GetPreviousWearerAsync(
    int custId,
    int? wearerId)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand("AdmCsWr_GetPrevWearer", connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@CustID", custId);

            if (wearerId.HasValue)
                command.Parameters.AddWithValue("@FirstWrID", wearerId.Value);
            else
                command.Parameters.AddWithValue("@FirstWrID", DBNull.Value);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                var wearer = MapReaderToWearer(reader);

                wearer.CustId = custId;

                return wearer;
            }

            return null;
        }
        public async Task<int> SaveWearerAsync(UpdateWearerDTO dto)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand("AdmCsWr_SaveWearer", connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@WearerID",
                dto.WearerId);

            command.Parameters.AddWithValue(
                "@Locker",
                dto.Locker ?? string.Empty);

            command.Parameters.AddWithValue(
                "@LockRm",
                dto.LockRm ?? string.Empty);

            command.Parameters.AddWithValue(
                "@Sex",
                dto.Sex ? "M" : "F");

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }
    }
}
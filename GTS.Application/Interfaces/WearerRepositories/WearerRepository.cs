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

        private Wearer MapReaderToWearer(
            SqlDataReader reader)
        {
            var sexOrdinal =
                reader.GetOrdinal("Sex");

            return new Wearer
            {
                WearerId =
                    reader.GetInt32(
                        reader.GetOrdinal("WearerId")),

                CustId =
                    reader.GetInt32(
                        reader.GetOrdinal("CustId")),

                WearNbr =
                    reader.IsDBNull(
                        reader.GetOrdinal("WearNbr"))
                        ? ""
                        : reader.GetValue(
                            reader.GetOrdinal("WearNbr"))
                            .ToString() ?? "",

                FirstName =
                    reader.IsDBNull(
                        reader.GetOrdinal("FirstName"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("FirstName")),

                LastName =
                    reader.IsDBNull(
                        reader.GetOrdinal("LastName"))
                        ? ""
                        : reader.GetString(
                            reader.GetOrdinal("LastName")),

                Locker =
                    reader.IsDBNull(
                        reader.GetOrdinal("Locker"))
                        ? ""
                        : reader.GetValue(
                            reader.GetOrdinal("Locker"))
                            .ToString() ?? "",

                LockRm =
                    reader.IsDBNull(
                        reader.GetOrdinal("LockRm"))
                        ? ""
                        : reader.GetValue(
                            reader.GetOrdinal("LockRm"))
                            .ToString() ?? "",

                Sex =
                    reader.IsDBNull(sexOrdinal)
                        ? true
                        : Convert.ToBoolean(
                            reader.GetValue(sexOrdinal))
            };
        }

        public async Task<Wearer?> GetWearerAsync(
            int custId,
            string wearNbr)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand(
                    "dbo.AdmCsWr_GetWearer",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustID",
                custId);

            command.Parameters.AddWithValue(
                "@WearNbr",
                wearNbr);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapReaderToWearer(reader);
        }
        public async Task<Wearer?> GetNextWearerAsync(
            int custId,
            int? wearerId)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand(
                    "dbo.AdmCsWr_GetNextWearer",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustID",
                custId);

            command.Parameters.AddWithValue(
                "@LastWrID",
                wearerId.HasValue
                    ? wearerId.Value
                    : DBNull.Value);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapReaderToWearer(reader);
        }
        public async Task<Wearer?> GetPreviousWearerAsync(
            int custId,
            int? wearerId)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand(
                    "dbo.AdmCsWr_GetPrevWearer",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustID",
                custId);

            command.Parameters.AddWithValue(
                "@FirstWrID",
                wearerId.HasValue
                    ? wearerId.Value
                    : DBNull.Value);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapReaderToWearer(reader);
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

            await command.ExecuteNonQueryAsync();

            return 1;
        }
        public async Task<List<Wearer>> GetAllWearersAsync(int custId)
        {
            using var connection = CreateConnection();

            using var command =
                new SqlCommand(
                    "dbo.AdmCsWr_GetAllWearers",
                    connection);

            command.CommandType =
                CommandType.StoredProcedure;

            command.Parameters.AddWithValue(
                "@CustID",
                custId);

            await connection.OpenAsync();

            using var reader =
                await command.ExecuteReaderAsync();

            var wearers = new List<Wearer>();

            while (await reader.ReadAsync())
            {
                var wearer = MapReaderToWearer(reader);

                wearer.CustId = custId;

                wearers.Add(wearer);
            }

            return wearers;
        }
    }
}
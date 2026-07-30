using Dapper;
using GTS.Application.DTOs;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class SpecialLinesRepository : ISpecialLinesRepository
    {
        private readonly IConfiguration _config;

        public SpecialLinesRepository(IConfiguration config)
        {
            _config = config;
        }

        public async Task<SpecialLinePage> GetSpecialLines(
            int custId,
            int page,
            int pageSize)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            using var multi =
                await con.QueryMultipleAsync(
                    "SpcLine_GetItemCodeC",
                    new
                    {
                        CustId = custId,
                        Page = page,
                        PageSize = pageSize
                    },
                    commandType: CommandType.StoredProcedure);

            var items =
                await multi.ReadAsync<SpecialLine>();

            var totalRecords =
                await multi.ReadFirstAsync<int>();

            return new SpecialLinePage
            {
                Items = items.ToList(),
                TotalRecords = totalRecords,
                CurrentPage = page,
                PageSize = pageSize
            };
        }

        public async Task<int> AddSpecialLine(
            int custId,
            int line)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.ExecuteAsync(
                "SpcLine_AddCustLine",
                new
                {
                    CustId = custId,
                    Line = line
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> DeleteSpecialLine(
            int line)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.ExecuteAsync(
                "SpcLine_DeleteCustLine",
                new
                {
                    Line = line
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task SaveSpecialLines(
            int custId,
            List<int> lines)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            await con.ExecuteAsync(
                "SpcLine_SaveCustomerLines",
                new
                {
                    CustId = custId,
                    Lines = string.Join(",", lines)
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
using Dapper;

using GTS.Domain.Entities;
using GTS.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace GTS.Infrastructure.Repositories
{
    public class MaxWashRepository
        : IMaxWashRepository
    {
        private readonly
            IConfiguration _config;

        public MaxWashRepository(
            IConfiguration config)
        {
            _config = config;
        }

        public async
            Task<IEnumerable<MaxWashDetails>>
            GetMaxWash(
                int custId)
        {
            using SqlConnection con =
                new(
                    _config.GetConnectionString(
                        "DefaultConnection"));

            return await
                con.QueryAsync
                <MaxWashDetails>(
                    "MaxWash_GetItemCode",
                    new
                    {
                        CustId = custId
                    },
                    commandType:
                    CommandType.StoredProcedure);
        }

        public async Task<int> CreateMaxWash(CreateMaxWash dto)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.ExecuteAsync(
                "MaxWash_CreateNewItemCode",
                new
                {
                    CustId = dto.CustId,
                    ItemCode = dto.ItemCode,
                    MaxWash = dto.MaxWash,
                    MaxWeeks = dto.MaxWeeks,
                    MaxCycles = dto.MaxCycles
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int> UpdateMaxWash(CreateMaxWash dto)
        {
            using SqlConnection con =
                new(_config.GetConnectionString("DefaultConnection"));

            return await con.ExecuteAsync(
                "MaxWash_UpdateItemCode",
                new
                {
                    CustId = dto.CustId,
                    OldItemCode = dto.OldItemCode,
                    NewItemCode = dto.NewItemCode,
                    MaxWash = dto.MaxWash,
                    MaxWeeks = dto.MaxWeeks,
                    MaxCycles = dto.MaxCycles
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<int>
            DeleteMaxWash(
                int custId,
                string itemCode)
        {
            using SqlConnection con =
                new(
                    _config.GetConnectionString(
                        "DefaultConnection"));

            return await
                con.ExecuteAsync(
                    "MaxWash_DeleteItemCode",
                    new
                    {
                        CustId = custId,
                        ItemCode = itemCode
                    },
                    commandType:
                    CommandType.StoredProcedure);
        }

    }
}
using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class MaxWashService
        : IMaxWashService
    {
        private readonly
            IMaxWashRepository _repo;

        public MaxWashService(
            IMaxWashRepository repo)
        {
            _repo = repo;
        }

        public async Task<
            IEnumerable<MaxWashDTO>>
            GetMaxWash(
                int custId)
        {
            var data =
                await _repo
                    .GetMaxWash(custId);

            return data.Select(x =>
                new MaxWashDTO
                {
                    CustId =
                        x.CustId,

                    ItemCode =
                        x.ItemCode,

                    MaxWash =
                        x.MaxWash,

                    MaxWeeks =
                        x.MaxWeeks,

                    MaxCycles =
                        x.MaxCycles
                });
        }

        public async Task<int>
            CreateMaxWash(
                MaxWashDTO dto)
        {
            CreateMaxWash entity =
                new();

            entity.CustId =
                dto.CustId;

            entity.ItemCode =
                dto.ItemCode;

            entity.MaxWash =
                dto.MaxWash;

            entity.MaxWeeks =
                dto.MaxWeeks;

            entity.MaxCycles =
                dto.MaxCycles;

            return await
                _repo
                    .CreateMaxWash(
                        entity);
        }

        public async Task<int> UpdateMaxWash(MaxWashDTO dto)
        {
            CreateMaxWash entity = new();

            entity.CustId = dto.CustId;

            entity.OldItemCode = dto.OldItemCode;

            entity.NewItemCode = dto.NewItemCode;

            entity.MaxWash = dto.MaxWash;

            entity.MaxWeeks = dto.MaxWeeks;

            entity.MaxCycles = dto.MaxCycles;

            return await _repo.UpdateMaxWash(entity);
        }

        public async Task<int>
            DeleteMaxWash(
                int custId,
                string itemCode)
        {
            return await
                _repo
                    .DeleteMaxWash(
                        custId,
                        itemCode);
        }
    }
}
using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class WashService : IWashService
    {
        private readonly IWashRepository _repository;

        public WashService(IWashRepository repository)
        {
            _repository = repository;
        }

        public async Task<WashDTO?> GetWashDetails(int custId)
        {
            var result =
                await _repository.GetWashDetails(custId);

            if (result == null)
                return null;

            return new WashDTO
            {
                CustId = result.CustId,
                WashCom = result.WashCom ?? "",
                Formula = result.Formula ?? ""
            };
        }

        public async Task<IEnumerable<string>> GetGarmentTypes()
        {
            var items =
                await _repository.GetGarmentTypes();

            return new[] { "All" }
                .Concat(
                    items.Where(x =>
                        !string.Equals(
                            x,
                            "All",
                            StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        public async Task<int> SaveWash(WashDTO dto)
        {
            var entity = new WashDetails
            {
                CustId = dto.CustId,
                WashCom = dto.WashCom ?? "",
                Formula = dto.Formula ?? ""
            };

            return await _repository.SaveWash(entity);
        }
    }
}
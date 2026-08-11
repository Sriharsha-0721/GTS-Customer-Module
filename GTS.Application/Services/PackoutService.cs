using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class PackoutService : IPackoutService
    {
        private readonly IPackoutRepository _repository;

        public PackoutService(IPackoutRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PackoutDTO>> GetPackoutDetails(int custId)
        {
            var result = await _repository.GetPackoutDetails(custId);

            return result.Select(x => new PackoutDTO
            {
                PkoutRestrictId = x.PkoutRestrictId,
                PkoutRestrict = x.PkoutRestrict,
                Item = x.Item,
                Color = x.Color,
                Size = x.Size
            });
        }

        public async Task<IEnumerable<string>> GetPackoutItems(int custId)
        {
            return await _repository.GetPackoutItems(custId);
        }

        public async Task<int> SavePackout(PackoutDTO dto)
        {
            return await _repository.SavePackout(new CreatePackout
            {
                CustId = dto.CustId,
                PkoutRestrict = dto.PkoutRestrict,
                Item = dto.Item,
                Color = dto.Color,
                Size = dto.Size,
                UpdtUser = dto.UpdtUser
            });
        }

        public async Task<int> DeletePackout(int pkoutRestrictId)
        {
            return await _repository.DeletePackout(pkoutRestrictId);
        }
    }
}
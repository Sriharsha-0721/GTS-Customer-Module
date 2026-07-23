using GTS.Application.DTOs;
using GTS.Application.Interfaces.WearerRepositories;
using GTS.Application.Interfaces.WearerServices;

namespace GTS.Application.Services
{
    public class WearerService : IWearerService
    {
        private readonly IWearerRepository _repository;

        public WearerService(IWearerRepository repository)
        {
            _repository = repository;
        }

        public async Task<WearerDTO?> GetWearerAsync(
            int custId,
            string wearNbr)
        {
            var wearer =
                await _repository.GetWearerAsync(custId, wearNbr);

            if (wearer == null)
                return null;

            return new WearerDTO
            {
                WearerId = wearer.WearerId,
                CustId = wearer.CustId,
                WearNbr = wearer.WearNbr,
                FirstName = wearer.FirstName,
                LastName = wearer.LastName,
                Locker = wearer.Locker,
                LockRm = wearer.LockRm,
                Sex = wearer.Sex
            };
        }

        public async Task<WearerDTO?> GetNextWearerAsync(
            int custId,
            int? wearerId)
        {
            var wearer =
                await _repository.GetNextWearerAsync(custId, wearerId);

            if (wearer == null)
                return null;

            return new WearerDTO
            {
                WearerId = wearer.WearerId,
                CustId = wearer.CustId,
                WearNbr = wearer.WearNbr,
                FirstName = wearer.FirstName,
                LastName = wearer.LastName,
                Locker = wearer.Locker,
                LockRm = wearer.LockRm,
                Sex = wearer.Sex
            };
        }

        public async Task<WearerDTO?> GetPreviousWearerAsync(
            int custId,
            int? wearerId)
        {
            var wearer =
                await _repository.GetPreviousWearerAsync(custId, wearerId);

            if (wearer == null)
                return null;

            return new WearerDTO
            {
                WearerId = wearer.WearerId,
                CustId = wearer.CustId,
                WearNbr = wearer.WearNbr,
                FirstName = wearer.FirstName,
                LastName = wearer.LastName,
                Locker = wearer.Locker,
                LockRm = wearer.LockRm,
                Sex = wearer.Sex
            };
        }

        public async Task<int> SaveWearerAsync(
            UpdateWearerDTO dto)
        {
            return await _repository.SaveWearerAsync(dto);
        }
    }
}
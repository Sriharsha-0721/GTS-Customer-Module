using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class BillingService : IBillingService
    {
        private readonly IBillingRepository _repo;

        public BillingService(IBillingRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<BillingChargeDTO>> GetBillingCharges(int custId)
        {
            var data = await _repo.GetBillingCharges(custId);

            return data.Select(x => new BillingChargeDTO
            {
                BillingChargesId = x.BillingChargesId,
                ChargeType = x.ChargeType,
                Charges = x.Charges
            });
        }

        public async Task<IEnumerable<BillingDataDTO>> GetBillingData()
        {
            var data = await _repo.GetBillingData();

            return data.Select(x => new BillingDataDTO
            {
                BillingDataId = x.BillingDataId,
                ChargeType = x.ChargeType
            });
        }

        public async Task<int> SaveBillingCharge(BillingChargeDTO dto)
        {
            CreateBillingCharge entity = new();

            entity.BillingChargesId = dto.BillingChargesId;
            entity.CustId = dto.CustId;
            entity.ChargeTypeId = dto.ChargeTypeId;
            entity.Charges = dto.Charges;
            entity.UpdtUser = dto.UpdtUser;

            return await _repo.SaveBillingCharge(entity);
        }

        public async Task<int> DeleteBillingCharge(int billingChargesId)
        {
            return await _repo.DeleteBillingCharge(billingChargesId);
        }
    }
}
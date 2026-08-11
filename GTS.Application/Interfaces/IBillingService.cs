using GTS.Application.DTOs;
using GTS.Domain.Entities;

namespace GTS.Application.Interfaces
{
    public interface IBillingService
    {
        Task<IEnumerable<BillingChargeDTO>>
            GetBillingCharges(int custId);

        Task<IEnumerable<BillingDataDTO>>
            GetBillingData();

        Task<int>
            SaveBillingCharge(BillingChargeDTO dto);

        Task<int>
            DeleteBillingCharge(int billingChargesId);
    }
}
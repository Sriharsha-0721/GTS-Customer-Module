using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface IBillingRepository
    {
        Task<IEnumerable<BillingChargeDetails>>
            GetBillingCharges(int custId);

        Task<IEnumerable<BillingData>>
            GetBillingData();

        Task<int>
            SaveBillingCharge(CreateBillingCharge dto);

        Task<int>
            DeleteBillingCharge(int billingChargesId);
    }
}
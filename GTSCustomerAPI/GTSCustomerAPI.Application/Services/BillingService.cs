using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class BillingService : IBillingService
{
    private readonly IBillingRepository _repo;

    public BillingService(IBillingRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ChargeDetailsDto>>
        GetBillingbyCustIdAsync(
        int custId, CancellationToken ct)
    {
        var list = await _repo
            .GetBillingbyCustIdAsync(custId);
        return list.Select(x => new ChargeDetailsDto
        {
            BillingChargesId = x.BillingChargesId,
            ChargeType = x.ChargeType,
            Charges = x.Charges
        });
    }

    public async Task<IEnumerable<BillingDataDto>>
        GetBillingDataAsync(CancellationToken ct)
    {
        var list = await _repo.GetBillingDataAsync();
        return list.Select(x => new BillingDataDto
        {
            BillingDataId = x.BillingDataId,
            ChargeType = x.ChargeType
        });
    }

    public async Task<int> SaveBillingAsync(
        BillingChargeDto dto, CancellationToken ct)
    {
        // ✅ dto.ChargeTypeId is the BillingDataId
        // SP @BillingChargesId = ChargeTypeId
        var entity = new BillingCharge
        {
            CustId = dto.CustId,
            ChargeTypeId = dto.ChargeTypeId,
            Charges = dto.Charges,
            UpdatedUser = dto.UpdtUser
        };
        return await _repo.SaveBillingAsync(entity);
    }

    public async Task<int> DeleteBillingAsync(
        int billingChargeId, CancellationToken ct)
    {
        return await _repo
            .DeleteBillingAsync(billingChargeId);
    }
}

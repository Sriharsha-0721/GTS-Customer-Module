using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class CustomerProfileService
    : ICustomerProfileService
{
    private readonly ICustomerProfileRepository _repo;

    public CustomerProfileService(
        ICustomerProfileRepository repo)
    {
        _repo = repo;
    }

    public async Task<CustomerProfileDto?>
        GetByCustIdAsync(int custId)
    {
        var p = await _repo.GetByCustIdAsync(custId);
        if (p == null) return null;
        return ToDto(p);
    }

    public async Task<bool> SaveAsync(
        UpdateCustomerProfileDto dto)
    {
        var entity = new CustomerProfile
        {
            CustId = dto.CustID,
            SterileCode = dto.SterileCode,
            CtmndFlg = dto.CtmndFlg ? 1 : 0,
            DelTicket = dto.DelTicket ? 1 : 0,
            PropertyMark = dto.PropertyMark ?? "",
            ShipVia = dto.ShipVia ?? "",
            Package = dto.Package ?? "",
            ProdCom = dto.ProdCom ?? "",
            OfficeCom = dto.OfficeCom ?? "",
            GenOfficeCom = dto.GenOfficeCom ?? "",
            MerControlCom = dto.MerControlCom ?? "",
            ReceivingCom = dto.ReceivingCom ?? "",
            SoilCom = dto.SoilCom ?? "",
            WashCom = dto.WashCom ?? "",
            DryerCom = dto.DryerCom ?? "",
            MainCleanRoomCom = dto.MainCleanRoomCom ?? "",
            PackoutCom = dto.PackoutCom ?? "",
            ShippingCom = dto.ShippingCom ?? "",
            DriverCom = dto.DriverCom ?? "",
            MendCom = dto.MendCom ?? "",
            QACom = dto.QACom ?? "",
            CustSrvCom = dto.CustSrvCom ?? "",
            BillingCom = dto.BillingCom ?? "",
            OSSFlag = dto.OSSFlag,
            QAInspCom = dto.QAInspCom ?? ""
        };
        return await _repo.SaveAsync(entity, dto.UserID);
    }

    private static CustomerProfileDto ToDto(
        CustomerProfile p) => new()
        {
            CustId = p.CustId,
            MarketCenter = p.MarketCenter.ToString(),
            CustNbr = p.CustNbr.ToString(),
            SterileCode = p.SterileCode,
            CtmndFlg = p.CtmndFlg,
            DelTicket = p.DelTicket,
            PropertyMark = p.PropertyMark,
            ShipVia = p.ShipVia,
            Package = p.Package,
            ProdCom = p.ProdCom,
            OfficeCom = p.OfficeCom,
            GenOfficeCom = p.GenOfficeCom,
            MerControlCom = p.MerControlCom,
            ReceivingCom = p.ReceivingCom,
            SoilCom = p.SoilCom,
            WashCom = p.WashCom,
            DryerCom = p.DryerCom,
            MainCleanRoomCom = p.MainCleanRoomCom,
            PackoutCom = p.PackoutCom,
            ShippingCom = p.ShippingCom,
            DriverCom = p.DriverCom,
            MendCom = p.MendCom,
            QACom = p.QACom,
            CustSrvCom = p.CustSrvCom,
            BillingCom = p.BillingCom,
            OSSFlag = p.OSSFlag,
            QAInspCom = p.QAInspCom
        };
}
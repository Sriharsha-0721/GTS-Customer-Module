namespace GTSCustomerAPI.Application.DTOs;

public class CustomerProfileDto
{
    public int CustId { get; set; }
    public string MarketCenter { get; set; }
                                     = string.Empty;
    public string CustNbr { get; set; }
                                     = string.Empty;
    public int SterileCode { get; set; }
    public int CtmndFlg { get; set; }
    public int DelTicket { get; set; }
    public string PropertyMark { get; set; }
                                     = string.Empty;
    public string ShipVia { get; set; }
                                     = string.Empty;
    public string Package { get; set; }
                                     = string.Empty;
    public string ProdCom { get; set; }
                                     = string.Empty;
    public string OfficeCom { get; set; }
                                     = string.Empty;
    public string GenOfficeCom { get; set; }
                                     = string.Empty;
    public string MerControlCom { get; set; }
                                     = string.Empty;
    public string ReceivingCom { get; set; }
                                     = string.Empty;
    public string SoilCom { get; set; }
                                     = string.Empty;
    public string WashCom { get; set; }
                                     = string.Empty;
    public string DryerCom { get; set; }
                                     = string.Empty;
    public string MainCleanRoomCom { get; set; }
                                     = string.Empty;
    public string PackoutCom { get; set; }
                                     = string.Empty;
    public string ShippingCom { get; set; }
                                     = string.Empty;
    public string DriverCom { get; set; }
                                     = string.Empty;
    public string MendCom { get; set; }
                                     = string.Empty;
    public string QACom { get; set; }
                                     = string.Empty;
    public string CustSrvCom { get; set; }
                                     = string.Empty;
    public string BillingCom { get; set; }
                                     = string.Empty;
    public bool OSSFlag { get; set; }
    public string QAInspCom { get; set; }
                                     = string.Empty;
}

public class UpdateCustomerProfileDto
{
    public int CustID { get; set; }
    public int SterileCode { get; set; }
    public bool CtmndFlg { get; set; }
    public bool DelTicket { get; set; }
    public string? PropertyMark { get; set; }
    public string? ShipVia { get; set; }
    public string? Package { get; set; }
    public string? ProdCom { get; set; }
    public string? OfficeCom { get; set; }
    public string? GenOfficeCom { get; set; }
    public string? MerControlCom { get; set; }
    public string? ReceivingCom { get; set; }
    public string? SoilCom { get; set; }
    public string? WashCom { get; set; }
    public string? DryerCom { get; set; }
    public string? MainCleanRoomCom { get; set; }
    public string? PackoutCom { get; set; }
    public string? ShippingCom { get; set; }
    public string? DriverCom { get; set; }
    public string? MendCom { get; set; }
    public string? QACom { get; set; }
    public string? CustSrvCom { get; set; }
    public string? BillingCom { get; set; }
    public int UserID { get; set; }
    public bool OSSFlag { get; set; } = false;
    public string? QAInspCom { get; set; }
}
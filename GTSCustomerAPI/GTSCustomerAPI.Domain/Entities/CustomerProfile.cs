namespace GTSCustomerAPI.Domain.Entities;

public class CustomerProfile
{
    public int CustId { get; set; }
    public int MarketCenter { get; set; }
    public int CustNbr { get; set; }
    public int SterileCode { get; set; }
    public int CtmndFlg { get; set; }
    public int DelTicket { get; set; }
    public string PropertyMark { get; set; } = "";
    public string ShipVia { get; set; } = "";
    public string Package { get; set; } = "";
    public bool OSSFlag { get; set; }
    public string WashCom { get; set; } = "";
    public string DryerCom { get; set; } = "";
    public string MainCleanRoomCom { get; set; } = "";
    public string PackoutCom { get; set; } = "";
    public string ShippingCom { get; set; } = "";
    public string DriverCom { get; set; } = "";
    public string MendCom { get; set; } = "";
    public string QACom { get; set; } = "";
    public string CustSrvCom { get; set; } = "";
    public string BillingCom { get; set; } = "";
    public string QAInspCom { get; set; } = "";
    public string OfficeCom { get; set; } = "";
    public string ProdCom { get; set; } = "";
    public string GenOfficeCom { get; set; } = "";
    public string MerControlCom { get; set; } = "";
    public string ReceivingCom { get; set; } = "";
    public string SoilCom { get; set; } = "";
}
namespace GTS.Application.DTOs
{
    public class CustomerFlagsDto
    {
        public int CustId { get; set; }
        public bool OSSFlag { get; set; }
        public bool STFFlag { get; set; }
    }

    public class CustomerProfileDto
    {
        public int CustId { get; set; }
        public int CustNbr { get; set; }
        public string? Name { get; set; }
        public int Route { get; set; }
        public string? GID { get; set; }
        public short MarketCenter { get; set; }
        public string? Addr1 { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }

        // Comment fields from ClnRm_ReadCustProfile
        public string BillingCom { get; set; } = string.Empty;
        public string PackoutCom { get; set; } = string.Empty;
        public string WashCom { get; set; } = string.Empty;
        public string SoilCom { get; set; } = string.Empty;
        public string DryerCom { get; set; } = string.Empty;
        public string ReceivingCom { get; set; } = string.Empty;
        public string ShippingCom { get; set; } = string.Empty;
        public string DriverCom { get; set; } = string.Empty;
        public string MendCom { get; set; } = string.Empty;
        public string QACom { get; set; } = string.Empty;
        public string CustSrvCom { get; set; } = string.Empty;
        public string OfficeCom { get; set; } = string.Empty;
        public string GenOfficeCom { get; set; } = string.Empty;
        public string MerControlCom { get; set; } = string.Empty;
        public string MainCleanRoomCom { get; set; } = string.Empty;
        public string QAInspCom { get; set; } = string.Empty;
        public string ProdCom { get; set; } = string.Empty;

        // Clean room attributes
        public string SterileCode { get; set; } = string.Empty;
        public bool CtmndFlg { get; set; }
        public bool DelTicket { get; set; }
        public bool PropertyMark { get; set; }
        public string ShipVia { get; set; } = string.Empty;
        public string Package { get; set; } = string.Empty;
        public bool OSSFlag { get; set; }
    }

    public class UpdateCustomerProfileDto
    {
        public int CustId { get; set; }
        public string? Name { get; set; }
        public int Route { get; set; }
        public string BillingCom { get; set; } = string.Empty;
        public string PackoutCom { get; set; } = string.Empty;
        public string WashCom { get; set; } = string.Empty;
        public string Formula { get; set; } = string.Empty;
        public string SoilCom { get; set; } = string.Empty;
        public string DryerCom { get; set; } = string.Empty;
        public string ReceivingCom { get; set; } = string.Empty;
        public string ShippingCom { get; set; } = string.Empty;
        public string DriverCom { get; set; } = string.Empty;
        public string MendCom { get; set; } = string.Empty;
        public string QACom { get; set; } = string.Empty;
        public string CustSrvCom { get; set; } = string.Empty;
        public string OfficeCom { get; set; } = string.Empty;
        public string GenOfficeCom { get; set; } = string.Empty;
        public string MerControlCom { get; set; } = string.Empty;
        public string MainCleanRoomCom { get; set; } = string.Empty;
        public string QAInspCom { get; set; } = string.Empty;
        public string ProdCom { get; set; } = string.Empty;
    }
}
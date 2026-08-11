namespace GTS.MVC.Models.CustomerProfile
{
    public class SaveCustomerProfileViewModel
    {
        public int CustId { get; set; }

        public string? BillingCom { get; set; }
        public string? PackoutCom { get; set; }
        public string? WashCom { get; set; }
        public string? Formula { get; set; }
        public string? SoilCom { get; set; }
        public string? DryerCom { get; set; }
        public string? ReceivingCom { get; set; }
        public string? ShippingCom { get; set; }
        public string? DriverCom { get; set; }
        public string? MendCom { get; set; }
        public string? QACom { get; set; }
        public string? CustSrvCom { get; set; }
        public string? OfficeCom { get; set; }
        public string? GenOfficeCom { get; set; }
        public string? MerControlCom { get; set; }
        public string? MainCleanRoomCom { get; set; }
        public string? QAInspCom { get; set; }
        public string? ProdCom { get; set; }
    }
}
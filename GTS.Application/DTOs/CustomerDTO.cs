namespace GTS.Application.DTOs
{
    public class CustomerDTO
    {
        public int CustId { get; set; }
        public short MarketCenter { get; set; }
        public int CustNbr { get; set; }
        public int? Account { get; set; }
        public int? Dept { get; set; }
        public string? Name { get; set; }
        public int Route { get; set; }
        public string GID { get; set; } = null!;
        public DateTime? StopDt { get; set; }
        public string? Addr1 { get; set; }
        public string? Addr2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? Freq { get; set; }
        public string? Contact { get; set; }
        public DateTime? CreateDt { get; set; }
        public string? PONumber { get; set; }
        public string? BillName { get; set; }
        public string? BillAddr { get; set; }
        public string? BillCity { get; set; }
        public string? BillState { get; set; }
        public string? BillZipCod { get; set; }
        public string? BillPhone { get; set; }
        public DateTime UpdtTime { get; set; }

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

        public string? QAInspCom { get; set; }

        public string Formula { get; set; } = "";
    }
}
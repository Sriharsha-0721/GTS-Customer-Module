using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Domain.Entities;

public class Customer
{
    public int CustId { get; set; }
    public short MarketCenter { get; set; }
    public int CustNbr { get; set; }

    public int? Account { get; set; }
    public int? Dept { get; set; }
    public string? Name { get; set; }

    public int Route { get; set; }
    public int? GarmInServ { get; set; }
    public string GID { get; set; } = string.Empty;

    public DateTime? StopDt { get; set; }

    public int MonSeq { get; set; }
    public int TueSeq { get; set; }
    public int WedSeq { get; set; }
    public int ThuSeq { get; set; }
    public int FriSeq { get; set; }
    public int SatSeq { get; set; }
    public int SunSeq { get; set; }

    public string? Addr1 { get; set; }
    public string? Addr2 { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Zip { get; set; }
    public string? Phone { get; set; }
    public string? Fax { get; set; }
    public string? Freq { get; set; }

    public bool OSSFlag { get; set; }
    public bool? SoilFlag { get; set; }
    public bool? LRFlag { get; set; }
    public bool? STFlag { get; set; }

    public string? InvSeq { get; set; }
    public int? MastAcctNbr { get; set; }
    public string? NatAcctNbr { get; set; }

    public bool? PrepFlag { get; set; }
    public bool? NameFlag { get; set; }
    public bool? ProdFlag { get; set; }
    public bool? EmbrFlag { get; set; }

    public DateTime? CreateDt { get; set; }
    public string? PONumber { get; set; }
    public byte? SterileCode { get; set; }

    public bool? CtmndFlg { get; set; }
    public bool? DelTicket { get; set; }

    public string? PropertyMark { get; set; }
    public string? ShipVia { get; set; }
    public string? Package { get; set; }
    public string? Contact { get; set; }

    public string? BillName { get; set; }
    public string? BillAddr { get; set; }
    public string? BillExAddr { get; set; }
    public string? BillCity { get; set; }
    public string? BillState { get; set; }
    public string? BillZipCod { get; set; }
    public string? BillPhone { get; set; }

    public short? CntnrsIn { get; set; }
    public short? CntnrsOut { get; set; }

    public int UpdtUser { get; set; }
    public DateTime UpdtTime { get; set; }

    public string? ProdCom { get; set; }
    public string? OfficeCom { get; set; }
    public string? GenOfficeCom { get; set; }

    public Guid rowguid { get; set; }

    public bool? DMWashFlag { get; set; }
    public bool? NMWashFlag { get; set; }
    public bool? PSWashFlag { get; set; }

    public string? Formula { get; set; }
}

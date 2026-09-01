using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Domain.Entities;

public class GarmentType
{
    public string ItemCode { get; set; }
        = "";

    public string ItemDesc { get; set; }
        = "";

    public string ShortDesc { get; set; }
        = "";
}


public class WashFormula
{
    public int CustId { get; set; }

    public int MarketCenter { get; set; }

    public string Formula { get; set; }
        = "";
}
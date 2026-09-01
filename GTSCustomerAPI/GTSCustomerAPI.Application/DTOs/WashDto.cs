using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Application.DTOs;

public class WashDto
{
    public int CustId { get; set; }

    public string WashCom { get; set; }
        = "";

    public string Formula { get; set; }
        = "";
}


public class GarmentTypeDto
{
    public string ItemCode { get; set; }
        = "";

    public string ItemDesc { get; set; }
        = "";

    public string ShortDesc { get; set; }
        = "";
}


public class SaveFormulaDto
{
    public int CustId { get; set; }

    public int MarketCenter { get; set; }

    public string FormulaString { get; set; }
        = "";
}
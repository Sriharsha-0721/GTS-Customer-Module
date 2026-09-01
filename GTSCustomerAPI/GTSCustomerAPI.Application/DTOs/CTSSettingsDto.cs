using System;
using System.Collections.Generic;
using System.Text;
namespace GTSCustomerAPI.Application.DTOs;

public class CTSSettingsDto
{
    public int CustId { get; set; }

    public int CustNbr { get; set; }

    public short MarketCenter { get; set; }

    public string? Name { get; set; }

    public int Route { get; set; }

    public string? GID { get; set; }

    public short? PrintIssueStatusFlag { get; set; }

    public short? PrintBornonDateFlag { get; set; }

    public bool? NOGFlag { get; set; }

    public string? LabelHeader { get; set; }
}

public class SaveCTSSettingsDto
{
    public int CustId { get; set; }
    public short? PrintIssueStatusFlag { get; set; }
    public short? PrintBornonDateFlag { get; set; }
    public bool? NOGFlag { get; set; }
    public string? LabelHeader { get; set; }
}
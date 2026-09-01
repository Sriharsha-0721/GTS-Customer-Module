using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Application.DTOs;

public class SaveCustomerProfileDto
{
    public int CustNbr { get; set; }
    public bool OSSFlag { get; set; }
    public bool? STFlag { get; set; }
}
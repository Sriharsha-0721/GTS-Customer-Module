using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Domain.Entities;

public class CustomerAdmin
{
    public int CustId { get; set; }
    public bool OSSFlag { get; set; }
    public bool? STFlag { get; set; }
}
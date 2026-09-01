using System;
using System.Collections.Generic;
using System.Text;

namespace GTSCustomerAPI.Application.DTOs;

public class CustomerAdminDto
{
    public int CustId { get; set; }
    public bool OSSFlag { get; set; }
    public bool? STFlag { get; set; }
}

public class SaveCustomerAdminDto
{
    public int CustId { get; set; }
    public bool OSSFlag { get; set; }
    public bool STFlag { get; set; }
}

using System;
using System.Collections.Generic;
using System.Text;
namespace GTSCustomerAPI.Domain.Entities;

public class CustomerDosage
{
    public int CustId { get; set; }
    public bool? Dosage { get; set; }
}
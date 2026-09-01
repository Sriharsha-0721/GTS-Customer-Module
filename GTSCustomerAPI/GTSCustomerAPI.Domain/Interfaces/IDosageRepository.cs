using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface IDosageRepository
{
    Task<CustomerDosage?> GetDosageAsync(int custId);
    Task UpdateDosageAsync(int custId, bool? dosage);
}

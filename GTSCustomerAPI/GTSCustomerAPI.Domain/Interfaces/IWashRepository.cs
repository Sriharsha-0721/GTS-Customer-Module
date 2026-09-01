using System;
using System.Collections.Generic;
using System.Text;

using GTSCustomerAPI.Domain.Entities;

namespace GTSCustomerAPI.Domain.Interfaces;

public interface IWashRepository
{
    Task<IEnumerable<GarmentType>>
        GetGarmentTypesAsync();

    Task<string>
        GetFormulaAsync(
            int marketCenter,
            int custId);

    Task<int>
        SaveFormulaAsync(
            int marketCenter,
            int custId,
            string formulaString);
}

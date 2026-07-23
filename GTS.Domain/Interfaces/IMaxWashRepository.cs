using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface IMaxWashRepository
    {
        Task<IEnumerable<MaxWashDetails>>
            GetMaxWash(int custId);

        Task<int>
            CreateMaxWash(
                CreateMaxWash dto);

        Task<int>
            UpdateMaxWash(
                CreateMaxWash dto);

        Task<int>
            DeleteMaxWash(
                int custId,
                string itemCode);
    }
}

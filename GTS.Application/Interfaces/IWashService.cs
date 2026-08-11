using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GTS.Application.DTOs;

namespace GTS.Application.Interfaces
{
    public interface IWashService
    {
        Task<WashDTO?> GetWashDetails(int custId);

        Task<IEnumerable<string>> GetGarmentTypes();

        Task<int> SaveWash(WashDTO dto);
    }
}

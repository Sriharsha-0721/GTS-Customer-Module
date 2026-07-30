using GTS.Application.DTOs;
using GTS.Domain.Entities;

namespace GTS.Application.Interfaces
{
    public interface ISpecialLinesService
    {
        Task<SpecialLinePage> GetSpecialLines(
            int custId,
            int page,
            int pageSize);

        Task<int> AddSpecialLine(int custId, int line);

        Task<int> DeleteSpecialLine(int line);
        Task SaveSpecialLines(SaveSpecialLinesRequest request);
    }
}
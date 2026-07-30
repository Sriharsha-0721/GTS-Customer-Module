using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface ISpecialLinesRepository
    {
        Task<SpecialLinePage> GetSpecialLines(
            int custId,
            int page,
            int pageSize);

        Task<int> AddSpecialLine(
            int custId,
            int line);

        Task<int> DeleteSpecialLine(
            int line);

        Task SaveSpecialLines(
            int custId,
            List<int> lines);
    }
}
using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class SpecialLinesService : ISpecialLinesService
    {
        private readonly ISpecialLinesRepository _repo;

        public SpecialLinesService(ISpecialLinesRepository repo)
        {
            _repo = repo;
        }

        public async Task<SpecialLinePage> GetSpecialLines(
            int custId,
            int page,
            int pageSize)
        {
            return await _repo.GetSpecialLines(
                custId,
                page,
                pageSize);
        }

        public async Task<int> AddSpecialLine(int custId, int line)
        {
            return await _repo.AddSpecialLine(custId, line);
        }

        public async Task<int> DeleteSpecialLine(int line)
        {
            return await _repo.DeleteSpecialLine(line);
        }
        public async Task SaveSpecialLines(
    SaveSpecialLinesRequest request)
        {
            await _repo.SaveSpecialLines(
    request.CustId,
    request.Lines);
        }
    }
}
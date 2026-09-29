using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class CTSSettingService : ICTSSettingService
    {
        private readonly ICTSSettingRepository _repo;

        public CTSSettingService(ICTSSettingRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CTSSettingDetailsDTO>> GetCTSSettingDetails(int custNbr)
        {
            var data = await _repo.GetCTSSettings(custNbr);

            return data.Select(x => new CTSSettingDetailsDTO
            {
                MarketCenter = x.MC,
                CustNbr = x.CustNbr,
                PrintIssueStatusFlag = x.PrintIssueStatusFlag,
                PrintBornonDateFlag = x.PrintBornonDateFlag,
                NOGFlag = x.NOGFlag,
                LabelHeader = x.LabelHeader
            });
        }

        public async Task<int> SaveCTSSettings(CreateCTSSettingDTO dto)
        {
            var entity = new CreateCTSSetting
            {
                MC = dto.MarketCenter ?? 0,
                CustNbr = dto.CustNbr,
                PrintIssueStatusFlag = dto.PrintIssueStatusFlag == 1,
                PrintBornonDateFlag = dto.PrintBornonDateFlag == 1,
                NOGFlag = dto.NOGFlag == 1,
                LabelHeader = dto.LabelHeader
            };

            return await _repo.SaveCTSSetting(entity);
        }
    }
}   
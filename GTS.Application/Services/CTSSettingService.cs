using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class CTSSettingService
        : ICTSSettingService
    {
        private readonly
            ICTSSettingRepository _repo;

        public CTSSettingService(
            ICTSSettingRepository repo)
        {
            _repo = repo;
        }

        public async
            Task<IEnumerable<CTSSettingDetailsDTO>>
            GetCTSSettingDetails(
                int custNbr)
        {
            var data =
                await _repo
                    .GetCTSSettings(
                        custNbr);

            return data.Select(x =>
                new CTSSettingDetailsDTO
                {
                    MarketCenter =
                        x.MarketCenter,

                    CustNbr =
                        x.CustNbr,

                    ItemCode =
                        x.ItemCode,

                    PrintIssueStatusFlag =
                        x.PrintIssueStatusFlag,

                    PrintBornonDateFlag =
                        x.PrintBornonDateFlag,

                    NOGFlag =
                        x.NOGFlag,

                    LabelHeader =
                        x.LabelHeader
                });
        }

        public async Task<int>
            SaveCTSSettings(
                CreateCTSSettingDTO dto)
        {
            CreateCTSSetting entity =
                new();

            entity.MarketCenter =
                dto.MarketCenter;

            entity.CustNbr =
                dto.CustNbr;

            entity.PrintIssueStatusFlag =
                dto.PrintIssueStatusFlag;

            entity.PrintBornonDateFlag =
                dto.PrintBornonDateFlag;

            entity.NOGFlag =
                dto.NOGFlag;

            entity.LabelHeader =
                dto.LabelHeader;

            return await    
                _repo
                    .SaveCTSSetting(
                        entity);
        }
    }
}
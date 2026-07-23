using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface ICTSSettingRepository
    {
        Task<IEnumerable<CTSSettingDetails>>
            GetCTSSettings(int custNbr);

        Task<int>
            SaveCTSSetting(CreateCTSSetting dto);
    }
}
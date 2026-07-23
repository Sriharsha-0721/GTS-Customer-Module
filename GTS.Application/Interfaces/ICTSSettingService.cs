using GTS.Application.DTOs;

public interface ICTSSettingService
{
    Task<IEnumerable<CTSSettingDetailsDTO>>
        GetCTSSettingDetails(int custNbr);

    Task<int>
        SaveCTSSettings(CreateCTSSettingDTO dto);
}
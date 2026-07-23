using GTS.Application.DTOs;

namespace GTS.Application.Interfaces
{
    public interface ICustomerLineCommentsService
    {
        Task<IEnumerable<CustWearerItemDTO>>
            GetCustWearItems(int custId);

        Task<int>
            InsertCustLineCmts(
                CustLineCmtsDTO dto);
    }
}
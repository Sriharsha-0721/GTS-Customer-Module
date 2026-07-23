using GTS.Domain.Entities;

namespace GTS.Domain.Interfaces
{
    public interface ICustomerLineCommentsRepository
    {
        Task<IEnumerable<CustWearerItem>>
            GetCustWearItems(int custId);

        Task<int>
            InsertCustLineCmts(
                CustLineCmts custLineCmts);
    }
}
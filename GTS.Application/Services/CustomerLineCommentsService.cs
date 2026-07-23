using GTS.Application.DTOs;
using GTS.Application.Interfaces;
using GTS.Domain.Entities;
using GTS.Domain.Interfaces;

namespace GTS.Application.Services
{
    public class CustomerLineCommentsService
        : ICustomerLineCommentsService
    {
        private readonly
            ICustomerLineCommentsRepository _repo;

        public CustomerLineCommentsService(
            ICustomerLineCommentsRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CustWearerItemDTO>>
            GetCustWearItems(int custId)
        {
            var data =
                await _repo.GetCustWearItems(custId);

            return data.Select(x =>
                new CustWearerItemDTO
                {
                    CustId = x.CustId,
                    WearItemId = x.WearItemId,
                    LineNbr = x.LineNbr,
                    Item = x.Item,
                    Size = x.Size,
                    WearId = x.WearId,
                    WearNbr = x.WearNbr,
                    Locker = x.Locker,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Name = x.Name,
                    EmblCode = x.EmblCode,
                    Descr = x.Descr
                });
        }

        public async Task<int>
            InsertCustLineCmts(
                CustLineCmtsDTO dto)
        {
            CustLineCmts entity =
                new();

            entity.CustId =
                dto.CustId;

            entity.WearItemId =
                dto.WearItemId;

            entity.Descr =
                dto.Descr;

            return await
                _repo.InsertCustLineCmts(entity);
        }
    }
}
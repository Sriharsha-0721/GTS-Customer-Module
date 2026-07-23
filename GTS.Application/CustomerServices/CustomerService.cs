using GTS.Application.DTOs;
using GTS.Application.Interfaces.CustomerRepositories;
using GTS.Application.Interfaces.CustomerServices;
using GTS.Domain.Entities;

namespace GTS.Application.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        private CustomerDTO MapToDTO(Customer customer)
        {
            return new CustomerDTO
            {
                CustId = customer.CustId,
                MarketCenter = customer.MarketCenter,
                CustNbr = customer.CustNbr,
                Account = customer.Account,
                Dept = customer.Dept,
                Name = customer.Name,
                Route = customer.Route,
                GID = customer.GID,
                StopDt = customer.StopDt,
                Addr1 = customer.Addr1,
                Addr2 = customer.Addr2,
                City = customer.City,
                State = customer.State,
                Zip = customer.Zip,
                Phone = customer.Phone,
                Fax = customer.Fax,
                Freq = customer.Freq,
                Contact = customer.Contact,
                CreateDt = customer.CreateDt,
                PONumber = customer.PONumber,
                BillName = customer.BillName,
                BillAddr = customer.BillAddr,
                BillCity = customer.BillCity,
                BillState = customer.BillState,
                BillZipCod = customer.BillZipCod,
                BillPhone = customer.BillPhone,
                UpdtTime = customer.UpdtTime
            };
        }

        public async Task<IEnumerable<CustomerDTO>> GetAllCustomersAsync()
        {
            var customers = await _repository.GetAllAsync();
            var dtos = new List<CustomerDTO>();
            foreach (var customer in customers)
                dtos.Add(MapToDTO(customer));
            return dtos;
        }

        public async Task<CustomerDTO?> GetCustomerByIdAsync(int id)
        {
            var customer = await _repository.GetByIdAsync(id);
            if (customer == null) return null;
            return MapToDTO(customer);
        }

        public async Task<CustomerDTO?> AddCustomerAsync(CreateCustomerDTO dto)
        {
            var customer = new Customer
            {
                MarketCenter = dto.MarketCenter,
                CustNbr = dto.CustNbr,
                Account = dto.Account,
                Dept = dto.Dept,
                Name = dto.Name,
                Route = dto.Route,
                GID = dto.GID,
                StopDt = dto.StopDt,
                MonSeq = dto.MonSeq,
                TueSeq = dto.TueSeq,
                WedSeq = dto.WedSeq,
                ThuSeq = dto.ThuSeq,
                FriSeq = dto.FriSeq,
                SatSeq = dto.SatSeq,
                SunSeq = dto.SunSeq,
                Addr1 = dto.Addr1,
                Addr2 = dto.Addr2,
                City = dto.City,
                State = dto.State,
                Zip = dto.Zip,
                Phone = dto.Phone,
                Fax = dto.Fax,
                Freq = dto.Freq,
                OSSFlag = dto.OSSFlag,
                SoilFlag = dto.SoilFlag,
                LRFlag = dto.LRFlag,
                STFlag = dto.STFlag,
                InvSeq = dto.InvSeq,
                MastAcctNbr = dto.MastAcctNbr,
                NatAcctNbr = dto.NatAcctNbr,
                PrepFlag = dto.PrepFlag,
                NameFlag = dto.NameFlag,
                ProdFlag = dto.ProdFlag,
                EmbrFlag = dto.EmbrFlag,
                CreateDt = dto.CreateDt,
                PONumber = dto.PONumber,
                SterileCode = dto.SterileCode,
                CtmndFlg = dto.CtmndFlg,
                DelTicket = dto.DelTicket,
                PropertyMark = dto.PropertyMark,
                ShipVia = dto.ShipVia,
                Package = dto.Package,
                Contact = dto.Contact,
                BillName = dto.BillName,
                BillAddr = dto.BillAddr,
                BillExAddr = dto.BillExAddr,
                BillCity = dto.BillCity,
                BillState = dto.BillState,
                BillZipCod = dto.BillZipCod,
                BillPhone = dto.BillPhone,
                CntnrsIn = dto.CntnrsIn,
                CntnrsOut = dto.CntnrsOut,
                UpdtUser = dto.UpdtUser,
                UpdtTime = dto.UpdtTime,
                rowguid = Guid.NewGuid()
            };

            var newCustomer = await _repository.AddAsync(customer);
            return newCustomer != null ? MapToDTO(newCustomer) : null;
        }

        public async Task UpdateCustomerAsync(UpdateCustomerDTO dto)
        {
            var customer = await _repository.GetByIdAsync(dto.CustId);
            if (customer == null) throw new Exception("Customer not found.");

            customer.MarketCenter = dto.MarketCenter;
            customer.CustNbr = dto.CustNbr;
            customer.Account = dto.Account;
            customer.Dept = dto.Dept;
            customer.Name = dto.Name;
            customer.Route = dto.Route;
            customer.GID = dto.GID;
            customer.StopDt = dto.StopDt;
            customer.MonSeq = dto.MonSeq;
            customer.TueSeq = dto.TueSeq;
            customer.WedSeq = dto.WedSeq;
            customer.ThuSeq = dto.ThuSeq;
            customer.FriSeq = dto.FriSeq;
            customer.SatSeq = dto.SatSeq;
            customer.SunSeq = dto.SunSeq;
            customer.Addr1 = dto.Addr1;
            customer.Addr2 = dto.Addr2;
            customer.City = dto.City;
            customer.State = dto.State;
            customer.Zip = dto.Zip;
            customer.Phone = dto.Phone;
            customer.Fax = dto.Fax;
            customer.Freq = dto.Freq;
            customer.OSSFlag = dto.OSSFlag;
            customer.SoilFlag = dto.SoilFlag;
            customer.LRFlag = dto.LRFlag;
            customer.STFlag = dto.STFlag;
            customer.InvSeq = dto.InvSeq;
            customer.MastAcctNbr = dto.MastAcctNbr;
            customer.NatAcctNbr = dto.NatAcctNbr;
            customer.PrepFlag = dto.PrepFlag;
            customer.NameFlag = dto.NameFlag;
            customer.ProdFlag = dto.ProdFlag;
            customer.EmbrFlag = dto.EmbrFlag;
            customer.CreateDt = dto.CreateDt;
            customer.PONumber = dto.PONumber;
            customer.SterileCode = dto.SterileCode;
            customer.CtmndFlg = dto.CtmndFlg;
            customer.DelTicket = dto.DelTicket;
            customer.PropertyMark = dto.PropertyMark;
            customer.ShipVia = dto.ShipVia;
            customer.Package = dto.Package;
            customer.Contact = dto.Contact;
            customer.BillName = dto.BillName;
            customer.BillAddr = dto.BillAddr;
            customer.BillExAddr = dto.BillExAddr;
            customer.BillCity = dto.BillCity;
            customer.BillState = dto.BillState;
            customer.BillZipCod = dto.BillZipCod;
            customer.BillPhone = dto.BillPhone;
            customer.CntnrsIn = dto.CntnrsIn;
            customer.CntnrsOut = dto.CntnrsOut;
            customer.UpdtUser = dto.UpdtUser;
            customer.UpdtTime = dto.UpdtTime;

            await _repository.UpdateAsync(customer);
        }

        public async Task DeleteCustomerAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
using GTSCustomerAPI.Application.DTOs;
using GTSCustomerAPI.Application.Interfaces;
using GTSCustomerAPI.Domain.Entities;
using GTSCustomerAPI.Domain.Interfaces;

namespace GTSCustomerAPI.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto?> GetCustomerByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer == null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            MarketCenter = dto.MarketCenter,
            CustNbr = dto.CustNbr,
            Account = dto.Account,
            Dept = dto.Dept,
            Name = dto.Name,
            Route = dto.Route,
            GarmInServ = dto.GarmInServ,
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
            PONumber = dto.PONumber,
            BillName = dto.BillName,
            BillAddr = dto.BillAddr,
            BillCity = dto.BillCity,
            BillState = dto.BillState,
            BillZipCod = dto.BillZipCod,
            UpdtUser = dto.UpdtUser,
            UpdtTime = DateTime.Now,
            rowguid = Guid.NewGuid()
        };

        var created = await _repository.CreateAsync(customer);
        return MapToDto(created);
    }

    public async Task<CustomerDto?> UpdateCustomerAsync(int id, UpdateCustomerDto dto)
    {
        // Step 1: check customer exists
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null) return null;

        // Step 2: update fields from dto
        existing.Account = dto.Account;
        existing.Dept = dto.Dept;
        existing.Name = dto.Name;
        existing.Route = dto.Route;
        existing.Addr1 = dto.Addr1;
        existing.Addr2 = dto.Addr2;
        existing.City = dto.City;
        existing.State = dto.State;
        existing.Zip = dto.Zip;
        existing.Phone = dto.Phone;
        existing.Fax = dto.Fax;
        existing.Freq = dto.Freq;
        existing.OSSFlag = dto.OSSFlag;
        existing.PONumber = dto.PONumber;
        existing.BillName = dto.BillName;
        existing.BillAddr = dto.BillAddr;
        existing.BillCity = dto.BillCity;
        existing.BillState = dto.BillState;
        existing.BillZipCod = dto.BillZipCod;
        existing.UpdtUser = dto.UpdtUser;
        existing.UpdtTime = DateTime.Now;

        // Step 3: ✅ pass only customer object — no id parameter
        var updated = await _repository.UpdateAsync(existing);
        return updated == null ? null : MapToDto(updated);
    }

    public async Task<bool> DeleteCustomerAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static CustomerDto MapToDto(Customer c) => new()
    {
        CustId = c.CustId,
        MarketCenter = c.MarketCenter,
        CustNbr = c.CustNbr,
        Account = c.Account,
        Dept = c.Dept,
        Name = c.Name,
        Route = c.Route,
        GID = c.GID,
        StopDt = c.StopDt,
        Addr1 = c.Addr1,
        Addr2 = c.Addr2,
        City = c.City,
        State = c.State,
        Zip = c.Zip,
        Phone = c.Phone,
        Fax = c.Fax,
        Freq = c.Freq,
        OSSFlag = c.OSSFlag,
        CreateDt = c.CreateDt,
        PONumber = c.PONumber,
        BillName = c.BillName,
        BillCity = c.BillCity,
        BillState = c.BillState,
        BillZipCod = c.BillZipCod,
        UpdtUser = c.UpdtUser,
        UpdtTime = c.UpdtTime
    };
}
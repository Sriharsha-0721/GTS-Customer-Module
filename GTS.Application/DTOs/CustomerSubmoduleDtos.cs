namespace GTS.Application.DTOs
{
    public class CustomerFlagsDto
    {
        public int CustId { get; set; }
        public bool OSSFlag { get; set; }
        public bool STFFlag { get; set; }
    }

    public class CustomerProfileDto
    {
        public int CustId { get; set; }
        public int CustNbr { get; set; }
        public string? Name { get; set; }
        public int Route { get; set; }
        public string? GID { get; set; }
        public string? Addr1 { get; set; }
        public string? City { get; set; }
        public string? Phone { get; set; }
    }

    public class UpdateCustomerProfileDto
    {
        public int CustId { get; set; }
        public string? Name { get; set; }
        public int Route { get; set; }
    }
}
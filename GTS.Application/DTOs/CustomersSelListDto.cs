namespace GTS.Application.DTOs
{
    public class CustomersSelListDto
    {
        public int CustId { get; set; }
        public string? Name { get; set; }
        public short MarketCenter { get; set; }
        public int CustNbr { get; set; }
        public int Route { get; set; }
        public string? GID { get; set; }
        public string? WDay { get; set; }
        public string? Addr1 { get; set; }
        public string? Addr2 { get; set; }
        public string? City { get; set; } // Formatted: City,State,Zip
        public string? Phone { get; set; }
        public string? Fax { get; set; }
        public string? CtmndFlg { get; set; }
        public string? SterileCode { get; set; }
        public string? StopDt { get; set; }
    }
}
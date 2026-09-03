namespace GTS.Application.DTOs
{
    public class CustomerFilterRequestDto
    {
        public int NumRecsToFetch { get; set; } = 0;
        public int MarketCenter { get; set; } = 569;
        public int Route { get; set; } = 0;
        public int WDay { get; set; } = 0;
        public string? GID { get; set; } = string.Empty;
        public int CustNbr { get; set; } = 0; // Maps to @CID
        public string? CustName { get; set; } = string.Empty;
        public int WearerNbr { get; set; } = 0;
    }
}
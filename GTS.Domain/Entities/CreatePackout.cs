namespace GTS.Domain.Entities
{
    public class CreatePackout
    {
        public int CustId { get; set; }

        public string PkoutRestrict { get; set; } = string.Empty;

        public string Item { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int UpdtUser { get; set; }
    }
}
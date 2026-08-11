namespace GTS.Domain.Entities
{
    public class PackoutDetails
    {
        public int PkoutRestrictId { get; set; }

        public string PkoutRestrict { get; set; } = string.Empty;

        public string Item { get; set; } = string.Empty;

        public string Color { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;
    }
}
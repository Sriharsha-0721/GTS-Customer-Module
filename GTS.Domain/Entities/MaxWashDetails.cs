namespace GTS.Domain.Entities
{
    public class MaxWashDetails
    {
        public int CustId { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public int MaxWash { get; set; }

        public int MaxWeeks { get; set; }

        public int MaxCycles { get; set; }
    }
}
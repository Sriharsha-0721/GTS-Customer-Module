//using GTS.Domain.Entities;

namespace GTS.Domain.Entities
{
    public class SpecialLinePage
    {
        public IEnumerable<SpecialLine> Items { get; set; } = new List<SpecialLine>();

        public int TotalRecords { get; set; }

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }
    }
}
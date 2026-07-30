namespace GTS.MVC.Models.SpecialLines
{
    public class SpecialLinePageViewModel
    {
        public List<SpecialLineViewModel> Items { get; set; } = new();

        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalRecords / PageSize);

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
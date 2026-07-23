namespace GTS.MVC.Models
{
    public class WearerViewModel
    {
        public int WearerId { get; set; }

        public int CustId { get; set; }

        public string WearNbr { get; set; } = "";

        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";

        public string Locker { get; set; } = "";

        public string LockRm { get; set; } = "";

        public bool Sex { get; set; }
    }
}
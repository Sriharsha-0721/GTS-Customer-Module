namespace GTSCustomerAPI.Web.Models.CustomerDeletion
{
    public class CustomerDeletionCustomerViewModel
    {
        public int CustId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CID { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; }

        public DateTime? LastUpdated { get; set; }

        public string LastUpdatedBy { get; set; } = string.Empty;

        public string Stage { get; set; } = string.Empty;

        public bool SystemRestrictions { get; set; }
    }
}
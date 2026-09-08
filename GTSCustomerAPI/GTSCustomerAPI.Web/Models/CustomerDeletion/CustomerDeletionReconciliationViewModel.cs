namespace GTSCustomerAPI.Web.Models.CustomerDeletion
{
    public class CustomerDeletionReconciliationViewModel
    {
        public int LineNumber { get; set; }

        public string WearerNumber { get; set; } = string.Empty;

        public string WearerName { get; set; } = string.Empty;

        public string Item { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int RasQty { get; set; }

        public int GtsQty { get; set; }

        public int PendingRts { get; set; }
    }
}
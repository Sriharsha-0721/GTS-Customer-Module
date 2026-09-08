namespace GTSCustomerAPI.Web.Models.CustomerDeletion
{
    public class CustomerDeletionViewModel
    {
        public int MarketCenter { get; set; }

        public int? SelectedCustId { get; set; }

        public string SelectedCustomerName { get; set; } = string.Empty;

        public List<CustomerDeletionCustomerViewModel> Customers { get; set; }
            = new List<CustomerDeletionCustomerViewModel>();
    }
}
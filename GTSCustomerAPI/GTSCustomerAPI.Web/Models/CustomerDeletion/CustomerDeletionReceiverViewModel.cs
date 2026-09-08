namespace GTSCustomerAPI.Web.Models.CustomerDeletion
{
    public class CustomerDeletionReceiverViewModel
    {
        public string Receiver { get; set; } = string.Empty;

        public string CID { get; set; } = string.Empty;

        public string ShipName { get; set; } = string.Empty;

        public DateTime? RcvCreateDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}
namespace GTSCustomerAPI.Web.Models.CustomerDeletion
{
    public class CustomerDeletionLostGarmentViewModel
    {
        public int Line { get; set; }

        public string ItemCode { get; set; } = string.Empty;

        public string Size { get; set; } = string.Empty;

        public int InService { get; set; }

        public int Returned { get; set; }

        public int Lost { get; set; }
    }
}

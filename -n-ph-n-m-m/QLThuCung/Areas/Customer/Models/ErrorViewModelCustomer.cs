namespace QLThuCung.Customer.Models
{
    public class ErrorViewModelCustomer
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

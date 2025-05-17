namespace QLThuCung.Admin.Models
{
    public class ErrorViewModelAdmin
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

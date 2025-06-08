namespace QLThuCung.Technician.Models
{
    public class ErrorViewModelTechnician
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}

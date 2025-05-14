using QLThuCung.Areas.Admin.ViewModels;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IThongKeService
    {
        Task<IEnumerable<DoanhThuVM>> DoanhThu();
    }
}

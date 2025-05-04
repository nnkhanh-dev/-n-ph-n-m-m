using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IDichVuKHService
    {
        Task<IEnumerable<DichVu>> List();
        Task<DichVu> Details(int id);
    }
}

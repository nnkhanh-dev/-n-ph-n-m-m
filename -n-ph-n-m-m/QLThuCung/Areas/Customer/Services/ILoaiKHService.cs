using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface ILoaiKHService
    {
        Task<IEnumerable<Loai>> List();
    }
}

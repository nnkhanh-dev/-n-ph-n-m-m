using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface ISanPhamKHService
    {
        Task<IEnumerable<SanPham>> List();
        Task<SanPham> Details(int id);
    }
}

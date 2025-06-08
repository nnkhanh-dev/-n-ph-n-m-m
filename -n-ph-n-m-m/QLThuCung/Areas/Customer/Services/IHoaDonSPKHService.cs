using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IHoaDonSPKHService
    {
        Task<bool> Create(HoaDonSanPham model);
        Task<IEnumerable<HoaDonSanPham>> ListByCustomer(string id);
        Task<bool> Cancel(int id);
        Task<HoaDonSanPham> Details(int id);
    }
}

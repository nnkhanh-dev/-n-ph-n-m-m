using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IHoaDonSPKHService
    {
        Task<bool> Create(HoaDonSanPham model);
    }
}

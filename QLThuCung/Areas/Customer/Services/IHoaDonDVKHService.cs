using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IHoaDonDVKHService
    {
        Task<bool> Create(HoaDonDichVu model);
        Task<IEnumerable<HoaDonDichVu>> ListByDate(DateTime NgayChamSoc);
        Task<IEnumerable<HoaDonDichVu>> ListByCustomer(string id);
        Task<decimal> TotalPrice(HoaDonDichVu model);
        Task<bool> Cancel(int id);
        Task<HoaDonDichVu> Details(int id);
    }
}

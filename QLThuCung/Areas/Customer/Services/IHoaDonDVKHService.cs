using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IHoaDonDVKHService
    {
        Task<bool> Create(HoaDonDichVu model);
        Task<IEnumerable<HoaDonDichVu>> ListByDate(DateTime NgayChamSoc);
        Task<IEnumerable<HoaDonDichVu>> ListByCustomer(string id);
    }
}

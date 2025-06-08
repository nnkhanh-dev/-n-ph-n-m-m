using QLThuCung.Models;

namespace QLThuCung.Areas.Technician.Services
{
    public interface IHoaDonDichVuTechnicianService
    {
        Task<IEnumerable<HoaDonDichVu>> List();
        Task<HoaDonDichVu> Details(int id);
        Task<bool> Create(HoaDonDichVu model);
        Task<bool> Edit(int id, HoaDonDichVu hoaDon, List<ChiTietHoaDonDichVu> chiTietHoaDonDichVu, string batDau, string ketThuc);
        Task<bool> Delete(int id);
        Task<decimal> TotalPrice(HoaDonDichVu model);
        Task<IEnumerable<HoaDonDichVu>> ListByDate(DateTime NgayChamSoc);

    }
}
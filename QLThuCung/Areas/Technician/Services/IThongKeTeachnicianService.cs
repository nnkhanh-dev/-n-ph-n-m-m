using QLThuCung.Areas.Technician.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Technician.Services
{
    public interface IThongKeTechnicianService
    {
        Task<IEnumerable<DoanhThuVM>> DoanhThu();
        Task<IEnumerable<SanPham>> TopSanPham();
        Task<IEnumerable<DichVu>> TopDichVu();
        Task<IEnumerable<DoanhThuSPVM>> DoanhThuSanPham();
        Task<IEnumerable<DoanhThuDVVM>> DoanhThuDichVu();
        Task<byte[]> XuatExcel();
    }
}

using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IThongKeService
    {
        Task<IEnumerable<DoanhThuVM>> DoanhThu();
        Task<IEnumerable<SanPham>> TopSanPham();
        Task<IEnumerable<DichVu>> TopDichVu();
        Task<IEnumerable<DoanhThuSPVM>> DoanhThuSanPham();
        Task<IEnumerable<DoanhThuDVVM>> DoanhThuDichVu();
        Task<byte[]> XuatExcel();
    }
}

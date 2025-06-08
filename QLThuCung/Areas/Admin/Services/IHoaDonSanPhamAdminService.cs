using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;
using System.Threading.Tasks;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IHoaDonSanPhamAdminService
    {
        Task<bool> Create(HoaDonSanPham model);
        Task<bool> UpdateStatus(string id);
        Task<bool> UpdateByStatus(string id, string status);
        Task<bool> UpdateSuccessStatus(string id);
        Task<IEnumerable<HoaDonSanPham>> ListByCustomer(string id);
        Task<IEnumerable<HoaDonSanPhamDTO>> List();
        Task<bool> Cancel(int id);
        Task<HoaDonSanPham> Details(int id);
    }
}

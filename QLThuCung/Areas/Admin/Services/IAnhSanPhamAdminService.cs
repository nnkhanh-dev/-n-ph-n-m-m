using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IAnhSanPhamAdminService
    {
        Task<IEnumerable<AnhSanPhamDTO>> List();
        Task<AnhSanPhamDTO> Details(string id);
        Task<bool> Create(AnhSanPhamDTO model);
        Task<bool> Edit(string id, AnhSanPhamDTO model);
        Task<bool> Delete(string id);
        Task<bool> DeleteImage(AnhSanPham img);
    }
}

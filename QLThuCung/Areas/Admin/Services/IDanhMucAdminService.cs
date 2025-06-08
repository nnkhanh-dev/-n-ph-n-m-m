using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IDanhMucAdminService
    {
        Task<IEnumerable<DanhMucDTO>> List();
        Task<NguoiDung> Details(string id);
        Task<bool> Create(DanhMucDTO model);
        Task<bool> Edit(string id, DanhMucDTO model);
        Task<bool> Delete(string id);
    }
}

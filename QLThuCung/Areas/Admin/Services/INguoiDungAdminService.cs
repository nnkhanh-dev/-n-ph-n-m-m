using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface INguoiDungAdminService
    {
        Task<IEnumerable<NguoiDung>> List();
        Task<NguoiDung> Details(string id);
        Task<bool> Create(UserCTO model);
        Task<bool> Edit(string id, NguoiDung model);
        Task<bool> Delete(string id);
    }
}

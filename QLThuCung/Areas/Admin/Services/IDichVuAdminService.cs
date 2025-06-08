using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IDichVuAdminService
    {
        Task<IEnumerable<DichVu>> List();
        Task<DichVu> Details(int id);
        Task<bool> Create(DichVu model);
        Task<bool> Edit(int id, DichVu model, int currentTime);
        Task<bool> Delete(int id);
    }
}

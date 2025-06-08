using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface ILoaiAdminService
    {
            Task<IEnumerable<Loai>> List();
            Task<Loai> Details(int id);
            Task<bool> Create(Loai model);
            Task<bool> Edit(int id, Loai model);
            Task<bool> Delete(int id);
        }
    }

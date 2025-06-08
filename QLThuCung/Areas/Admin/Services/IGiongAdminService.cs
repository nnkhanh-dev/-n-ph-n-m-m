using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IGiongAdminService
    {
            Task<IEnumerable<Giong>> List();
            Task<Giong> Details(int id);
            Task<bool> Create(Giong model);
            Task<bool> Edit(int id, Giong model);
            Task<bool> Delete(int id);
        }
    }

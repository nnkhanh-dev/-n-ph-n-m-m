using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public interface IDanhmucAdminService
    {
            Task<IEnumerable<DanhMuc>> List();
            Task<DanhMuc> Details(int id);
            Task<bool> Create(DanhMuc model);
            Task<bool> Edit(int id, DanhMuc model);
            Task<bool> Delete(int id);
        }
    }

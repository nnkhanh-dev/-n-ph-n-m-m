using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IDanhGiaSPKHService
    {
        Task<DanhGiaSP> Detail(int id);
        Task<bool> Create(DanhGiaSP model);
    }
}

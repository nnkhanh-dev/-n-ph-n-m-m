using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IDanhGiaDVKHService
    {
        Task<DanhGiaDV> Detail(int id);
        Task<bool> Create(DanhGiaDV model);
        Task<bool> Delete(int id);
    }
}

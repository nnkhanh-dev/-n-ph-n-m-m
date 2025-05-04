using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IThuCungKHService
    {
        Task<bool> Create(ThuCung model);
        Task<bool> Edit(int id, ThuCung model);
        Task<bool> Delete(int id);
        Task<IEnumerable<ThuCung>> List(string id);
        Task<ThuCung> Details(int id);
    }
}

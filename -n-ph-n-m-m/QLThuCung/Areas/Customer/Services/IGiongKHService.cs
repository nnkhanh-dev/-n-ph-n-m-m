using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IGiongKHService
    {
        Task<IEnumerable<Giong>> List();
    }
}

using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IDanhMucService
    {
        Task<IEnumerable<DanhMuc>> List();
    }
}

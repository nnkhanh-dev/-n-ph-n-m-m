using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public interface IGioHangKHService
    {
        Task<bool> CreateItem(ChiTietGioHang model);
        Task<bool> EditItem(ChiTietGioHang model);
        Task<bool> DeleteItem(int idSP, string idGH);
        Task<GioHang> Find(string id);
        Task<bool> Create(string id);
        Task<IEnumerable<ChiTietGioHang>> List(string id);
    }
}

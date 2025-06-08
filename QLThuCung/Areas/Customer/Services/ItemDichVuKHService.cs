using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{

    public class ItemDichVuKHService : IDichVuKHService
    {
        private readonly AppDbContext _context;

        public ItemDichVuKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DichVu> Details(int id)
        {
            if(id == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ!");
                return null;
            }
            var item = await _context.DichVu.Include(x => x.AnhDichVu)
                                            .Include(x => x.BangGiaDV)
                                                .ThenInclude(x => x.ChiTietBangGiaDV)
                                            .FirstOrDefaultAsync(x => x.Id == id);                                     
            if(item == null)
            {
                Console.WriteLine("Không tìm thấy dịch vụ!");
                return null;
            }
            return item;
        }

        public async Task<IEnumerable<DichVu>> List()
        {
            var list = await _context.DichVu.Include(x => x.AnhDichVu)
                                            .Include(x => x.BangGiaDV)
                                                .ThenInclude(x => x.ChiTietBangGiaDV)
                                            .ToListAsync();
            return list;
        }
    }
}

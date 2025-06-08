using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemDanhGiaSPKHService : IDanhGiaSPKHService
    {
        private readonly AppDbContext _context;

        public ItemDanhGiaSPKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(DanhGiaSP model)
        {
            if(model == null)
            {
                return false;
            }
            try
            {
                _context.DanhGiaSP.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false ;
            }
        }

        public async Task<DanhGiaSP> Detail(int id)
        {
            var details = await _context.DanhGiaSP.Include(x => x.TepDinhKem).FirstOrDefaultAsync(x => x.Id == id);
            return details;
        }
    }
}

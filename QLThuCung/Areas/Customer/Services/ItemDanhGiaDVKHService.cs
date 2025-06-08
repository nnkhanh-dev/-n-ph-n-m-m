using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemDanhGiaDVKHService : IDanhGiaDVKHService
    {
        private readonly AppDbContext _context;

        public ItemDanhGiaDVKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(DanhGiaDV model)
        {
            if(model == null)
            {
                return false;
            }
            try
            {
                _context.DanhGiaDV.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false ;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _context.DanhGiaDV.FindAsync(id);
                if (item == null)
                {
                    return false;
                }
                _context.DanhGiaDV.Remove(item);
                return await _context.SaveChangesAsync() > 0;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<DanhGiaDV> Detail(int id)
        {
            var details = await _context.DanhGiaDV.Include(x => x.TepDinhKem).FirstOrDefaultAsync(x => x.Id == id);
            return details;
        }
    }
}

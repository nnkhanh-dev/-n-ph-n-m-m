using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemGiongAdminService : IGiongAdminService
    {
        private readonly AppDbContext _context;
        public ItemGiongAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(Giong model)
        {
            if (model == null)
            {
                return false;
            }
            try
            {
                _context.Giong.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            if (id == null)
            {
                return false;
            }
            try
            {
                var item = await _context.Giong.FindAsync(id);
                if (item == null)
                {
                    return false;
                }
                _context.Giong.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Giong> Details(int id)
        {
            if (id == null)
            {
                return null;
            }
            var item = await _context.Giong.Include(x => x.Loai).FirstOrDefaultAsync(x => x.Id == id);
            return item;
        }

        public async Task<bool> Edit(int id, Giong model)
        {
            if (model == null || id == null)
            {
                return false;
            }
            try
            {
                var item = await _context.Giong.FindAsync(id);
                if(item == null)
                {
                    return false;
                }
                item.Ten = model.Ten;
                item.IdLoai = model.IdLoai;
                _context.Giong.Update(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Giong>> List()
        {
            var list = await _context.Giong.Include(x => x.Loai).ToListAsync();
            return list;
        }
    }
}

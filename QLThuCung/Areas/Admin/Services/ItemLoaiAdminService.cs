using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemLoaiAdminService : ILoaiAdminService
    {
        private readonly AppDbContext _context;
        public ItemLoaiAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(Loai model)
        {
            if (model == null)
            {
                return false;
            }
            try
            {
                _context.Loai.Add(model);
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
                var item = await _context.Loai.FindAsync(id);
                if (item == null)
                {
                    return false;
                }
                _context.Loai.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<Loai> Details(int id)
        {
            if (id == null)
            {
                return null;
            }
            var item = await _context.Loai.FindAsync(id);
            return item;
        }

        public async Task<bool> Edit(int id, Loai model)
        {
            if (model == null || id == null)
            {
                return false;
            }
            try
            {
                var item = await _context.Loai.FindAsync(id);
                if(item == null)
                {
                    return false;
                }
                item.Ten = model.Ten;
                _context.Loai.Update(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<Loai>> List()
        {
            var list = await _context.Loai.ToListAsync();
            return list;
        }
    }
}

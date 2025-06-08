using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemDanhmucAdminService : IDanhmucAdminService
    {
        private readonly AppDbContext _context;
        public ItemDanhmucAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(DanhMuc model)
        {
            if (model == null)
            {
                return false;
            }
            try
            {
                _context.DanhMuc.Add(model);
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
                var item = await _context.DanhMuc.FindAsync(id);
                if (item == null)
                {
                    return false;
                }
                _context.DanhMuc.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<DanhMuc> Details(int id)
        {
            if (id == null)
            {
                return null;
            }
            var item = await _context.DanhMuc.FindAsync(id);
            return item;
        }

        public async Task<bool> Edit(int id, DanhMuc model)
        {
            if (model == null || id == null)
            {
                return false;
            }
            try
            {
                var item = await _context.DanhMuc.FindAsync(id);
                if(item == null)
                {
                    return false;
                }
                item.AnhMinhHoa = model.AnhMinhHoa;
                item.MoTa = model.MoTa;
                item.Ten = model.Ten;
                _context.DanhMuc.Update(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<DanhMuc>> List()
        {
            var list = await _context.DanhMuc.ToListAsync();
            return list;
        }
    }
}

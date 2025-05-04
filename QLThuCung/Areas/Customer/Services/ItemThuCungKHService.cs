using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemThuCungKHService : IThuCungKHService
    {
        private readonly AppDbContext _context;

        public ItemThuCungKHService(AppDbContext context)
        {
            _context = context;
        }   

        public async Task<bool> Create(ThuCung model)
        {
            if (model == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ!");
                return false;
            }
            try
            {
                _context.ThuCung.Add(model);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            if (id == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ!");
                return false;
            }
            var item = await _context.ThuCung.FindAsync(id);
            if (item == null)
            {
                Console.WriteLine("Không tìm thấy thú cưng!");
                return false;
            }
            try
            {
                _context.ThuCung.Remove(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public async Task<ThuCung> Details(int id)
        {
            if(id == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ!");
                return null;
            }
            var item = await _context.ThuCung.Include(x => x.Giong)
                                             .ThenInclude(x => x.Loai)
                                             .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                Console.WriteLine("Không tìm thấy thú cưng!");
                return null;
            }
            return item;
        }

        public async Task<bool> Edit(int id, ThuCung model)
        {
            if (id == null || model == null)
            {
                Console.WriteLine("Dữ liệu không hợp lệ!");
                return false;
            }
            var item = await _context.ThuCung.Include(x => x.Giong)
                                             .ThenInclude(x => x.Loai)
                                             .FirstOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                Console.WriteLine("Không tìm thấy thú cưng!");
                return false;
            }

            item.Ten = model.Ten;
            item.CanNang = model.CanNang;
            item.Tuoi = model.Tuoi;
            item.GioiTinh = model.GioiTinh;
            item.DacDiem = model.DacDiem;
            item.AnhDaiDien = model.AnhDaiDien;
            item.GhiChu = model.GhiChu;
            item.NguoiCapNhat = model.NguoiCapNhat;
            item.NgayCapNhat = model.NgayCapNhat;

            try
            {
                _context.ThuCung.Update(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi: " + ex.Message);
                return false;
            }
        }

        public async Task<IEnumerable<ThuCung>> List(string id)
        {
            var list = await _context.ThuCung.Where(x => x.IdKhachHang == id).ToListAsync();
            return list;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemGioHangKHService : IGioHangKHService
    {
        private readonly AppDbContext _context;

        public ItemGioHangKHService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(string id)
        {
            if (id == null)
            {
                return false;
            }
            try
            {
                var gioHang = new GioHang();
                gioHang.IdKhachHang = id;
                _context.GioHang.Add(gioHang);
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateItem(ChiTietGioHang model)
        {
            if (model == null)
            {
                return false;
            }

            try
            {
                // Tìm item đã tồn tại trong giỏ hàng (dựa vào IdGioHang và IdSanPham)
                var existingItem = await _context.ChiTietGioHang
                    .FirstOrDefaultAsync(x => x.IdGioHang == model.IdGioHang && x.IdSanPham == model.IdSanPham);

                if (existingItem != null)
                {
                    // Nếu đã tồn tại, cập nhật số lượng
                    existingItem.SoLuong = model.SoLuong;

                    _context.ChiTietGioHang.Update(existingItem);
                }
                else
                {
                    // Nếu chưa có, thêm mới
                    _context.ChiTietGioHang.Add(model);
                }

                // Lưu thay đổi
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }


        public async Task<bool> DeleteItem(int idSanPham, string idGioHang)
        {
            if(idSanPham == null || idGioHang == null)
            {
                return false;
            }
            try
            {
                var item = await _context.ChiTietGioHang.FirstOrDefaultAsync(x => x.IdSanPham == idSanPham && x.IdGioHang == idGioHang);
                if (item == null)
                {
                    return false;
                }
                _context.ChiTietGioHang.Remove(item);
                var result = await _context.SaveChangesAsync() > 0;
                if (result)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<bool> EditItem(ChiTietGioHang model)
        {
            if( model == null)
            {
                return false;
            }
            try
            {
                var sanpham = await _context.SanPham.FindAsync(model.IdSanPham);
                if (sanpham.SoLuong < model.SoLuong)
                {
                    return false;
                }
                var item = await _context.ChiTietGioHang.FirstOrDefaultAsync(x => x.IdSanPham == model.IdSanPham && x.IdGioHang == model.IdGioHang);
                if (item == null)
                {
                    return false;
                }
                item.SoLuong = model.SoLuong;
                _context.ChiTietGioHang.Update(item);
                return await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<GioHang> Find(string id)
        {
            if (id == null)
            {
                return null;
            }
            var item = await _context.GioHang.FindAsync(id);
            if (item == null)
            {
                return null;
            }
            return item;
        }

        public async Task<IEnumerable<ChiTietGioHang>> List(string id)
        {
            var list = await _context.ChiTietGioHang.Where(x => x.IdGioHang == id)
                                                    .Include(x => x.SanPham)
                                                        .ThenInclude(x => x.AnhSanPham)
                                                    .ToListAsync();
            return list;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{

    public class ItemAnhSanPhamAdminService : IAnhSanPhamAdminService
    {
        private readonly AppDbContext _context;
        public ItemAnhSanPhamAdminService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Create(AnhSanPhamDTO model)
        {
            try
            {
                var anh = new AnhSanPham
                {
                    IdSanPham = model.IdSanPham,
                    DuongDan = model.DuongDan,
                };


            await _context.AnhSanPham.AddAsync(anh);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;
            }

        }

        public async Task<bool> Delete(string id)
        {

            try
            {
                var anh = await _context.AnhSanPham.FindAsync(id);
                if (anh != null)
                {

                    _context.AnhSanPham.Remove(anh);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteImage(AnhSanPham img)
        {

            try
            {
                _context.AnhSanPham.Remove(img);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Task<AnhSanPhamDTO> Details(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(string id, AnhSanPhamDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AnhSanPhamDTO>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AnhSanPhamDTO>> ListBySanPham(string id)
        {
            return await _context.AnhSanPham.Where(x=>x.IdSanPham == Int32.Parse(id)).Select(x=>new AnhSanPhamDTO
            {
                Id = x.Id,
                IdSanPham = x.IdSanPham,
                DuongDan = x.DuongDan,
                MoTa = x.MoTa ?? ""
            }).ToListAsync();
        }

    }
}

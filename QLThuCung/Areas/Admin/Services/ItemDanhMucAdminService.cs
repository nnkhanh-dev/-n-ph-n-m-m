using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemDanhMucAdminService : IDanhMucAdminService
    {
        private readonly AppDbContext _context;
        public ItemDanhMucAdminService(AppDbContext context)
        {
            _context = context;
        }
        public Task<bool> Create(DanhMucDTO model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<NguoiDung> Details(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Edit(string id, DanhMucDTO model)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<DanhMucDTO>> List()
        {
            return await _context.DanhMuc.Select(
                x=>new DanhMucDTO
                {
                    Id = x.Id,
                    Ten = x.Ten,
                    MoTa = x.MoTa,
                    AnhMinhHoa = x.AnhMinhHoa,
                }).ToListAsync();
        }
    }
}

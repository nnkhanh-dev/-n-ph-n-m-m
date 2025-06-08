using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.ViewModels;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Admin.Services
{
    public class ItemKhachHangAdminService : IKhachHangAdminService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<NguoiDung> _userManager;

        public ItemKhachHangAdminService(AppDbContext context, UserManager<NguoiDung> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<bool> Create(UserCTO model)
        {
            try
            {

                var user = new NguoiDung
                {
                    HoTen = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    UserName = model.PhoneNumber,
                    AnhDaiDien = model.AnhDaiDien
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    return false;
                }
                await _userManager.AddToRoleAsync(user, "KhachHang");

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: ex.Message
                return false;
            }
        }

        public async Task<bool> Delete(string id)
        {
            try
            {
                var user = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                {
                    return false; // Không tìm thấy người dùng
                }

                // Xóa user khỏi bảng NguoiDung (AspNetUsers)
                var deleteResult = await _userManager.DeleteAsync(user);
                return deleteResult.Succeeded;
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: ex.Message
                return false;
            }
        }

        public async Task<NguoiDung> Details(string id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> Edit(string id, NguoiDung model)
        {
            try
            {
                var existingUser = await _context.NguoiDung.FirstOrDefaultAsync(u => u.Id == id);
                if (existingUser == null)
                {
                    return false; // Không tìm thấy người dùng
                }

                // Cập nhật thông tin
                existingUser.HoTen = model.HoTen;
                existingUser.Email = model.Email;
                existingUser.PhoneNumber = model.PhoneNumber;
                existingUser.UserName = model.PhoneNumber;
                existingUser.DiaChi = model.DiaChi;
                if(model.AnhDaiDien != null)
                {
                    existingUser.AnhDaiDien = model.AnhDaiDien;
                }
                _context.NguoiDung.Update(existingUser);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Ghi log nếu cần: ex.Message
                return false;
            }
        }

        public async Task<IEnumerable<NguoiDung>> List()
        {
            // Lấy ID của role "NhanVien"
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "KhachHang");
            if (role == null) return new List<NguoiDung>();

            // Lấy danh sách userId thuộc role này
            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            // Lấy danh sách người dùng ứng với userIds
            var nhanViens = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToListAsync();

            return nhanViens;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using QLThuCung.Models;
using System;
using System.Threading.Tasks;

namespace QLThuCung.Data
{
    public static class DbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<NguoiDung>>();

            string[] roleNames = { "Admin", "NhanVien", "KyThuatVien", "KhachHang" };

            foreach (var roleName in roleNames)
            {
                bool roleExists = await roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var adminPhoneNumber = "0898866467";
            var adminUser = await userManager.FindByNameAsync(adminPhoneNumber);

            await SeedAdminAsync(serviceProvider);
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<NguoiDung>>();

            var adminPhoneNumber = "0898866467";
            var adminUser = await userManager.FindByNameAsync(adminPhoneNumber);

            if (adminUser == null)
            {
                var newAdmin = new NguoiDung
                {
                    UserName = adminPhoneNumber,
                    Email = "nguyennhatkhanh151203@gmail.com",
                    PhoneNumber = "0898866467",
                    HoTen = "Nguyễn Nhật Khánh"
                };

                var result = await userManager.CreateAsync(newAdmin, "Admin@12345");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }

    }
}

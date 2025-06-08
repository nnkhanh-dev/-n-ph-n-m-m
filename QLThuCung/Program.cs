using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.Areas.Customer.Services;
using QLThuCung.Areas.Technician.Services;
using QLThuCung.Data;
using QLThuCung.Models;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});
builder.Services.AddScoped<IDichVuKHService, ItemDichVuKHService>();
builder.Services.AddScoped<IThuCungKHService, ItemThuCungKHService>();
builder.Services.AddScoped<IHoaDonDVKHService, ItemHoaDonDVKHService>();
builder.Services.AddScoped<INhanVienAdminService, ItemNhanVienAdminService>();
builder.Services.AddScoped<IKyThuatVienAdminService, ItemKyThuatVienAdminService>();
builder.Services.AddScoped<IKhachHangAdminService, ItemKhachHangAdminService>();
builder.Services.AddScoped<IVNPayService, VNPayService>();
builder.Services.AddScoped<IDanhMucService, ItemDanhMucService>();
builder.Services.AddScoped<ISanPhamKHService, ItemSanPhamKHService>();
builder.Services.AddScoped<IGioHangKHService, ItemGioHangKHService>();
builder.Services.AddScoped<IHoaDonSPKHService, ItemHoaDonSPKHService>();
builder.Services.AddScoped<IThongKeService, ItemThongKeService>();
builder.Services.AddScoped<IGiongKHService, ItemGiongKHService>();
builder.Services.AddScoped<ILoaiKHService, ItemLoaiKHService>();
builder.Services.AddScoped<IDanhGiaDVKHService, ItemDanhGiaDVKHService>();
builder.Services.AddScoped<IDanhGiaSPKHService, ItemDanhGiaSPKHService>();
builder.Services.AddScoped<IDichVuAdminService, ItemDichVuAdminService>();
builder.Services.AddScoped<IHoaDonDichVuADService, ItemHoaDonDichVuADService>();
builder.Services.AddScoped<IThongKeTechnicianService, ItemThongKeTechnicianService>();
builder.Services.AddScoped<IHoaDonDichVuTechnicianService, ItemHoaDonDichVuTechnicianService>();

var connectionString = builder.Configuration.GetConnectionString("Default");
// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<NguoiDung, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredUniqueChars = 0;
}
).AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders()
.AddDefaultUI();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});




var app = builder.Build();

app.UseSession();


// Seed roles here
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbInitializer.SeedRolesAsync(services); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

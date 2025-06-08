using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Models;

namespace QLThuCung.Data
{
    public class AppDbContext : IdentityDbContext<NguoiDung>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<NguoiDung> NguoiDung { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<DichVu> DichVu { get; set; }
        public DbSet<ThuCung> ThuCung { get; set; }
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<Giong> Giong { get; set; }
        public DbSet<Loai> Loai { get; set; }
        public DbSet<GioHang> GioHang { get; set; }
        public DbSet<AnhSanPham> AnhSanPham { get; set; }
        public DbSet<AnhDichVu> AnhDichVu { get; set; }
        public DbSet<BangGiaDV> BangGiaDV { get; set; }
        public DbSet<ChiTietBangGiaDV> ChiTietBangGiaDV { get; set; }
        public DbSet<ThongBao> ThongBao { get; set; }
        public DbSet<ChiTietGioHang> ChiTietGioHang { get; set; }
        public DbSet<CuocHoiThoai> CuocHoiThoai { get; set; }
        public DbSet<TinNhan> TinNhan { get; set; }
        public DbSet<TepDinhKemTinNhan> TepDinhKemTinNhan { get; set; }
        public DbSet<PhieuGiamGia> PhieuGiamGia { get; set; }
        public DbSet<HoaDonSanPham> HoaDonSanPham { get; set; }
        public DbSet<HoaDonDichVu> HoaDonDichVu { get; set; }
        public DbSet<DipDacBiet> DipDacBiet { get; set; }
        public DbSet<DanhGiaSP> DanhGiaSP { get; set; }
        public DbSet<TepDinhKemDanhGiaSP> TepDinhKemDanhGiaSP { get; set; }
        public DbSet<DanhGiaDV> DanhGiaDV { get; set; }
        public DbSet<TepDinhKemDanhGiaDV> TepDinhKemDanhGiaDV { get; set; }
        public DbSet<ChiTietHoaDonDichVu> ChiTietHoaDonDichVu { get; set; }
        public DbSet<ChiTietHoaDonSanPham> ChiTietHoaDonSanPham { get; set; }
        public DbSet<Giuong> Giuong { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NguoiDung>()
                .HasOne(nd => nd.GioHang)
                .WithOne(gh => gh.KhachHang)
                .HasForeignKey<GioHang>(gh => gh.IdKhachHang);

            modelBuilder.Entity<ChiTietGioHang>()
                .HasKey(x => new { x.IdSanPham, x.IdGioHang });

            modelBuilder.Entity<ChiTietGioHang>()
                .HasOne(x => x.SanPham)
                .WithMany(x => x.ChiTietGioHang)
                .HasForeignKey(x => x.IdSanPham);

            modelBuilder.Entity<ChiTietGioHang>()
                .HasOne(x => x.GioHang)
                .WithMany(x => x.ChiTietGioHang)
                .HasForeignKey(x => x.IdGioHang);

            modelBuilder.Entity<ChiTietHoaDonSanPham>()
                .HasKey(x => new { x.IdSanPham, x.IdHoaDon });

            modelBuilder.Entity<ChiTietHoaDonSanPham>()
                .HasOne(x => x.SanPham)
                .WithMany(x => x.ChiTietHoaDonSanPham)
                .HasForeignKey(x => x.IdSanPham);

            modelBuilder.Entity<ChiTietHoaDonSanPham>()
                .HasOne(x => x.HoaDon)
                .WithMany(x => x.ChiTietHoaDonSanPham)
                .HasForeignKey(x => x.IdHoaDon);

            modelBuilder.Entity<ChiTietHoaDonDichVu>()
                .HasKey(x => new { x.IdDichVu, x.IdHoaDon });

            modelBuilder.Entity<ChiTietHoaDonDichVu>()
                .HasOne(x => x.DichVu)
                .WithMany(x => x.ChiTietHoaDonDichVu)
                .HasForeignKey(x => x.IdDichVu);

            modelBuilder.Entity<ChiTietHoaDonDichVu>()
                .HasOne(x => x.HoaDon)
                .WithMany(x => x.ChiTietHoaDonDichVu)
                .HasForeignKey(x => x.IdHoaDon);

            modelBuilder.Entity<CuocHoiThoai>()
                .HasOne(c => c.KhachHang)
                .WithMany()
                .HasForeignKey(c => c.IdKhachHang)
                .OnDelete(DeleteBehavior.Cascade); // giữ lại Cascade

            modelBuilder.Entity<CuocHoiThoai>()
                .HasOne(c => c.NhanVien)
                .WithMany()
                .HasForeignKey(c => c.IdNhanVien)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(modelBuilder);

        }

    }
}

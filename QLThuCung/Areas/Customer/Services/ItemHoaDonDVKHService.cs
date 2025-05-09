
using HotelApp.Areas.Client.Services;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Data;
using QLThuCung.Models;

namespace QLThuCung.Areas.Customer.Services
{
    public class ItemHoaDonDVKHService : IHoaDonDVKHService
    {
        private readonly AppDbContext _context;
        private readonly IVNPayService _vnpayService;

        public ItemHoaDonDVKHService(AppDbContext context, IVNPayService vnpayService)
        {
            _context = context;
            _vnpayService = vnpayService;
        }

        public async Task<bool> Create(HoaDonDichVu model)
        {
            if (model == null)
            {
                return false;
            }

            // Bắt đầu Transaction
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var hoaDon = new HoaDonDichVu
                {
                    IdThuCung = model.IdThuCung,
                    TrangThai = model.PhuongThucThanhToan == 1 ? -100 : 0,
                    PhuongThucThanhToan = model.PhuongThucThanhToan,
                    NgayChamSoc = model.NgayChamSoc,
                    ThoiGianChamSoc = model.ThoiGianChamSoc,
                    NgayTao = DateTime.Now,
                    NguoiTao = model.NguoiTao
                };
                _context.HoaDonDichVu.Add(hoaDon);
                var hoaDonResult = await _context.SaveChangesAsync() > 0;
                if (!hoaDonResult)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                foreach (var item in model.ChiTietHoaDonDichVu)
                {
                    var chiTiet = new ChiTietHoaDonDichVu
                    {
                        IdHoaDon = hoaDon.Id,
                        IdDichVu = item.IdDichVu,
                        DonGia = item.DonGia
                    };
                    _context.ChiTietHoaDonDichVu.Add(chiTiet);
                }

                var chiTietResult = await _context.SaveChangesAsync() > 0;
                if (!chiTietResult)
                {
                    await transaction.RollbackAsync();
                    return false;
                }

                // Commit transaction nếu tất cả lưu thành công
                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public Task<IEnumerable<HoaDonDichVu>> ListByCustomer(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HoaDonDichVu>> ListByDate(DateTime NgayChamSoc)
        {
            var list = await _context.HoaDonDichVu.Include(x => x.ChiTietHoaDonDichVu)
                                                  .ThenInclude(x => x.DichVu)
                                                  .Where(x => x.NgayChamSoc == NgayChamSoc)
                                                  .ToListAsync();
            return list;
        }

        public async Task<decimal> TotalPrice(HoaDonDichVu model)
        {
            decimal totalPrice = 0;
            if (model == null)
            {
                return totalPrice;
            }
            foreach(var item in model.ChiTietHoaDonDichVu)
            {
                totalPrice += item.DonGia;
            }
            return totalPrice;
        }
    }
}

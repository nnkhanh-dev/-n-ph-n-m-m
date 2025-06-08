using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLThuCung.Areas.Admin.Services;
using QLThuCung.ViewModels;

namespace QLThuCung.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,NhanVien")]
    public class CheckoutController : Controller
    {
        private readonly IHoaDonSanPhamAdminService _hoadon;
        private readonly IVNPayService _vnpayService;
        public CheckoutController(IHoaDonSanPhamAdminService hoadon, IVNPayService payService)
        {
            _hoadon = hoadon;
            _vnpayService = payService;
        }
        [Route("/Checkout/Payment")]
        public async Task<IActionResult> Payment()
        {
            var response = _vnpayService.PaymentExecute(Request.Query);
            var result = await _hoadon.UpdateStatus(response.OrderDescription);
            TempData["Success"] = "Đặt mua thành công!";
            return RedirectToAction("Index", "HoaDonSP", new {Areas = "Admin"});
        }
    }
}

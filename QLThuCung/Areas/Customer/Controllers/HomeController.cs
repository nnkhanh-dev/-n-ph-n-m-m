using System.Diagnostics;
using HotelApp.Areas.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QLThuCung.Customer.Models;

namespace QLThuCung.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "KhachHang")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IVNPayService _vnpayService;

        public HomeController(ILogger<HomeController> logger, IVNPayService vnpayService)
        {
            _logger = logger;
            _vnpayService = vnpayService;
        }
        [Route("/khach-hang/")]
        public IActionResult Index()
        {
            if (Request.Query.Keys.Any(k => k.StartsWith("vnp_")))
            {
                var response = _vnpayService.PaymentExecute(Request.Query);

                if (response == null || response.VnPayResponseCode != "00")
                {
                    TempData["Error"] = "Thanh toán thất bại";
                    return View();
                }

                TempData["Success"] = "Thanh toán thành công";
            }

            // Trường hợp không phải callback từ VNPay
            return View();
        }

        [Route("/khachhang/chungtoi")]
        public IActionResult AboutUs()
        {
            return View();
        }

        [Route("/khachhang/lienhe")]
        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

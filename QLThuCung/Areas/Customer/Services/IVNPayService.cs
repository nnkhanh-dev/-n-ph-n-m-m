using QLThuCung.Models;
using QLThuCung.ViewModels;

namespace HotelApp.Areas.Client.Services
{
    public interface IVNPayService
    {
        // Tạo URL thanh toán VNPay
        string CreatePaymentUrl(HttpContext context, decimal price, string note);

        VnPaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}

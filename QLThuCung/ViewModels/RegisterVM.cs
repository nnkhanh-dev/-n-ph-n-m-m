using System.ComponentModel.DataAnnotations;

namespace QLThuCung.ViewModels
{
    public class RegisterVM
    {
        [Required (ErrorMessage ="Họ và tên là bắt buộc")]
        public string FullName { get; set; }
        [Required (ErrorMessage = "Số điện thoại là bắt buộc")]
        [Phone (ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }
        [Required (ErrorMessage = "Email là bắt buộc")]
        [EmailAddress (ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        public string ConfirmPassword { get; set; }
    }
}

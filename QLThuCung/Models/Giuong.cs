namespace QLThuCung.Models
{
    public class Giuong
    {
        public int Id { get; set; }

        public string? MaGiuong { get; set; }            // Ví dụ: GB001
        public string? KhuVuc { get; set; }              // Ví dụ: Khu A, Khu B
        public string? LoaiGiuong { get; set; }          // Ví dụ: Giường thường, cách ly
        public bool? DangSuDung { get; set; }            // Đã có thú cưng đang nằm hay chưa
        public string? GhiChu { get; set; }              // Ghi chú thêm, nếu có

        // Khóa ngoại nếu liên kết với thú cưng đang nằm (tùy chọn)
        public int? ThuCungId { get; set; }
        public ThuCung? ThuCung { get; set; }
    }
}

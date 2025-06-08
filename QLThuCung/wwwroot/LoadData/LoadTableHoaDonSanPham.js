$(document).ready(function () {
    LoadTableHoaDonSanPham();
});

function LoadTableHoaDonSanPham() {
    const table = $('#HoaDonSanPhamTable').DataTable({
        order: [[1, 'desc']],
        ajax: {
            url: '/admin/hoadonsanpham/list',
            dataSrc: 'data'
        },
        columns: [
            {
                data: 'hoTenKhach',
                width: '20%',
                className: 'text-start'
            },
            {
                data: 'ngayTao',
                width: '20%',
                className: 'text-start',
                render: function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('vi-VN'); // định dạng theo ngày Việt Nam
                }
            },
            {
                data: 'chiTietHoaDonSanPham',
                width: '20%',
                className: 'text-start',
                render: function (data) {
                    // Tính tổng tiền từ mảng chi tiết dịch vụ
                    const total = data.reduce((sum, sp) => sum + (sp.donGia * sp.soLuong), 0);
                    return total.toLocaleString('vi-VN') + ' đ';
                }
            },
            {
                data: 'trangThai',
                width: '20%',
                className: 'text-start',
                render: function (data) {
                    // Hiển thị trạng thái đơn theo mã số
                    switch (data) {
                        case -1: return '<span class="badge bg-danger">Đã hủy</span>';
                        case 0: return '<span class="badge bg-secondary">Chờ xử lý</span>';
                        case 1: return '<span class="badge bg-info text-dark">Đã xử lý</span>';
                        case 2: return '<span class="badge bg-warning text-dark">Chờ Thanh Toán</span>';
                        case 3: return '<span class="badge bg-success">Hoàn thành</span>';
                        case 4: return '<span class="badge bg-dark">Không nhận hàng</span>';
                        default: return '<span class="badge bg-light">Không xác định</span>';
                    }
                }
            },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `<a class="btn btn-sm btn-success" href="/admin/hoadonsanpham/chitiet/${data}">Xem</a> `;
                },
                width: '20%',
                className: 'text-start'
            }
        ]
    }); 

}


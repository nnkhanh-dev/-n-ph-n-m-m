$(document).ready(function () {
    const id = $('#IdNguoiDung').val();
    LoadTableHoaDonSanPham(id);
});

function LoadTableHoaDonSanPham(id) {
    const table = $('#HoaDonSanPhamTable').DataTable({
        ajax: {
            url: '/khachhang/hoadonsanpham/listbycustomer/' + id,
            dataSrc: 'data'
        },
        columns: [
            {
                data: 'ngayTao',
                width: '25%',
                className: 'text-start',
                render: function (data) {
                    const date = new Date(data);
                    return date.toLocaleDateString('vi-VN'); // định dạng theo ngày Việt Nam
                }
            },
            {
                data: 'chiTietHoaDonSanPham',
                width: '25%',
                className: 'text-start',
                render: function (data) {
                    // Tính tổng tiền từ mảng chi tiết dịch vụ
                    const total = data.reduce((sum, sp) => sum + (sp.donGia * sp.soLuong), 0);
                    return total.toLocaleString('vi-VN') + ' đ';
                }
            },
            {
                data: 'trangThai',
                width: '25%',
                className: 'text-start',
                render: function (data) {
                    // Hiển thị trạng thái đơn theo mã số
                    switch (data) {
                        case -1: return '<span class="badge bg-danger">Hủy</span>';
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
                    let buttons = `<a class="btn btn-sm btn-success" href="/khachhang/hoadonsanpham/chitiet/${data}">Xem</a> `;

                    if (row.trangThai === 0) {
                        buttons += `<button class="btn btn-sm btn-danger btn-cancel" data-id="${data}">Hủy</button> `;
                    }

                    if (row.trangThai === 3 && (!row.danhGia || row.danhGia.length === 0)) {
                        buttons += `<a class="btn btn-sm btn-primary" href="/khachhang/hoadonsanpham/danhgia/${data}">Đánh giá</a> `;
                    }

                    return buttons;
                },
                width: '25%'
            }
        ]
    });

    $('#HoaDonSanPhamTable tbody').on('click', '.btn-cancel', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn hủy đơn này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/khachhang/hoadonsanpham/huy/${id}`,
                    type: 'DELETE',
                    success: function () {
                        toastr.success('Hủy thành công!');
                        table.ajax.reload(null, false);
                    },
                    error: function () {
                        toastr.error('Hủy thất bại!');
                    }
                });
            }
        });
    });
}


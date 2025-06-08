$(document).ready(function () {
    const table = $('#sanPhamTable').DataTable({ // ← GÁN vào biến table
        ajax: {
            url: 'https://localhost:44345/admin/sanpham/list',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ten' },
            { data: 'tenDanhMuc' },
            { data: 'gia' },
            { data: 'soLuong' },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `
                        <a class="btn btn-sm btn-success" href="/admin/sanpham/chitiet/${data}">Xem</a>
                        <a class="btn btn-sm btn-primary" href="/admin/sanpham/chinhsua/${data}">Sửa</a>
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                    `;
                }
            }
        ]
    });

    $('#sanPhamTable tbody').on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn xóa san pham này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/admin/sanpham/xoa/${id}`,
                    type: 'DELETE',
                    success: function () {
                        toastr.success('Xóa thành công!');
                        table.ajax.reload(null, false); // ← Reload lại bảng!
                    },
                    error: function () {
                        toastr.error('Xóa thất bại!');
                    }
                });
            }
        });
    });
});

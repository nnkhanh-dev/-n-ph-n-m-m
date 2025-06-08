$(document).ready(function () {
    const table = $('#dichVuTable').DataTable({ // ← GÁN vào biến table
        ajax: {
            url: '/admin/dichvu/list',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ten' },
            { data: 'thoiGian' },
            {
                data: 'trangThai',
                render: function (data, type, row) {
                    if (data === 0) return 'Đang mở';
                    if (data === -1) return 'Đã đóng';
                    return 'Không xác định';
                }
            },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `
                        <a class="btn btn-sm btn-success" href="/admin/dichvu/chitiet/${data}">Xem</a>
                        <a class="btn btn-sm btn-primary" href="/admin/dichvu/chinhsua/${data}">Sửa</a>
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                    `;
                }
            }
        ]
    });

    $('#dichVuTable tbody').on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn xóa dịch vụ này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/admin/dichvu/xoa/${id}`,
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

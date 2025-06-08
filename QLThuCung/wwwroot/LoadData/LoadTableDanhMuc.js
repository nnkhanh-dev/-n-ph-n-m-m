$(document).ready(function () {
    const table = $('#DanhMucTable').DataTable({ // ← GÁN vào biến table
        ajax: {
            url: '/admin/danhmuc/list',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ten' },
            { data: 'moTa' },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `
                        <a class="btn btn-sm btn-success" href="/admin/danhmuc/chitiet/${data}">Xem</a>
                        <a class="btn btn-sm btn-primary" href="/admin/danhmuc/chinhsua/${data}">Sửa</a>
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                    `;
                }
            }
        ]
    });

    $('#DanhMucTable tbody').on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn xóa danh mục này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/admin/danhmuc/xoa/${id}`,
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

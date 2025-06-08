$(document).ready(function () {
    const table = $('#LoaiTable').DataTable({ // ← GÁN vào biến table
        ajax: {
            url: '/admin/loai/list',
            dataSrc: 'data'
        },
        columns: [
            { data: 'ten' },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `
                        <a class="btn btn-sm btn-success" href="/admin/loai/chitiet/${data}">Xem</a>
                        <a class="btn btn-sm btn-primary" href="/admin/loai/chinhsua/${data}">Sửa</a>
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                    `;
                }
            }
        ]
    });

    $('#LoaiTable tbody').on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn xóa loài này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/admin/loai/xoa/${id}`,
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

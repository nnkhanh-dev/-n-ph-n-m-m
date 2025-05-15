$(document).ready(function () {
    const id = $('#IdNguoiDung').val();
    LoadTableThuCungKH(id);
});

function LoadTableThuCungKH(id) {
    const table = $('#TableThuCung').DataTable({
        ajax: {
            url: '/khachhang/thucung/list/' + id,
            dataSrc: 'data'
        },
        columns: [
            { data: 'ten' },
            { data: 'giong.ten' },
            { data: 'giong.loai.ten' },
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    return `
                        <a class="btn btn-sm btn-success" href="/khachhang/thucung/chitiet/${data}">Xem</a>
                        <a class="btn btn-sm btn-primary" href="/khachhang/thucung/chinhsua/${data}">Sửa</a>
                        <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                    `;
                }
            }
        ]
    });

    $('#TableThuCung tbody').on('click', '.btn-delete', function () {
        const id = $(this).data('id');

        Swal.fire({
            title: 'Xác nhận xóa?',
            text: "Bạn có chắc chắn muốn xóa thú cưng này?",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#d33',
            cancelButtonColor: '#6c757d',
            confirmButtonText: 'Xóa',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: `/khachhang/thucung/xoa/${id}`,
                    type: 'DELETE',
                    success: function () {
                        toastr.success('Xóa thành công!');
                        table.ajax.reload(null, false);
                    },
                    error: function () {
                        toastr.error('Xóa thất bại!');
                    }
                });
            }
        });
    });
}


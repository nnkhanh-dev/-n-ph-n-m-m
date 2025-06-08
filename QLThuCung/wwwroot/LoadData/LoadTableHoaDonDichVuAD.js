$(document).ready(function () {
    const table = $('#hoaDonDichVuTable').DataTable({
        ajax: {
            url: '/admin/hoadondichvuad/list',
            dataSrc: 'data'
        },
        columns: [
            {
                data: null,
                render: function (data, type, row, meta) {
                    return meta.row + 1; // STT (index + 1)
                }
            },
            { data: 'tenKhachHang' },
            { data: 'tenThuCung' },
            {
                data: 'ngayChamSoc',
                render: function (data) {
                    return new Date(data).toLocaleDateString('vi-VN'); // Chỉ ngày
                }
            },
            {
                data: 'tongTien',
                render: function (data) {
                    return data.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
                }
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                    <a class="btn btn-sm btn-success" href="/admin/hoadondichvuad/details/${data}">Xem</a>
                    <a class="btn btn-sm btn-primary" href="/admin/hoadondichvuad/edit/${data}">Sửa</a>
                    <button class="btn btn-sm btn-danger btn-delete" data-id="${data}">Xóa</button>
                `;
                }
            }
        ]
    });


    $('#hoaDonDichVuTable tbody').on('click', '.btn-delete', function () {
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
                    url: `/admin/hoadondichvuad/delete/${id}`,
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

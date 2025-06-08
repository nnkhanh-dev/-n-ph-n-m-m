$(document).ready(function () {
    const table = $('#hoaDonDichVuTable').DataTable({
        ajax: {
            url: '/technician/hoadondichvutechnician/list',
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
                data: 'trangThai', // Thêm cột trạng thái
                render: function (data) {
                    // Ánh xạ trạng thái dựa trên giá trị số
                    let trangThaiText = '';
                    let badgeClass = 'badge bg-secondary';
                    switch (data) {
                        case 0:
                            trangThaiText = 'Chờ xử lý';
                            badgeClass = 'badge bg-secondary';
                            break;
                        case 1:
                            trangThaiText = 'Đã xử lý';
                            badgeClass = 'badge bg-info text-dark';
                            break;
                        case 2:
                            trangThaiText = 'Chờ thanh toán';
                            badgeClass = 'badge bg-warning text-dark';
                            break;
                        case 3:
                            trangThaiText = 'Hoàn thành';
                            badgeClass = 'badge bg-success';
                            break;
                        case -1:
                            trangThaiText = 'Hủy';
                            badgeClass = 'badge bg-danger';
                            break;
                        default:
                            trangThaiText = 'Không xác định';
                            badgeClass = 'badge bg-dark';
                    }
                    return `<span class="${badgeClass}">${trangThaiText}</span>`;
                }
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                    <a class="btn btn-sm btn-success" href="/technician/hoadondichvutechnician/details/${data}">Xem</a>
                `;
                }
            }
        ]
    });


    $('#loadAllBills').on('click', function () {
        $('#loadAllBills').addClass('btn-primary').removeClass('btn-secondary');
        //$('#loadPendingToday').addClass('btn-secondary').removeClass('btn-success');
        table.ajax.url('/technician/hoadondichvutechnician/list').load();
    });

    
    $('#loadPendingToday').on('click', function () {
        $('#loadPendingToday').addClass('btn-success').removeClass('btn-secondary');
        //$('#loadAllBills').addClass('btn-secondary').removeClass('btn-primary');
        table.ajax.url('/technician/hoadondichvutechnician/listpendingtoday').load();
    });

    // Nút Lọc theo ngày
    $('#filterByDateBtn').on('click', function () {
        $('#filterByDateBtn').addClass('d-none');
        $('#filterDateBlock').removeClass('d-none');
    });

    $('#applyDateFilter').on('click', function () {
        var filterDate = $('#filterDate').val();
        if (filterDate) {
            var url = `/technician/hoadondichvutechnician/filterbydate?filterDate=${filterDate}`;
            table.ajax.url(url).load();
        }
        $('#filterDateBlock').addClass('d-none');
        $('#filterByDateBtn').removeClass('d-none');
    });

    $('#cancelDateFilter').on('click', function () {
        $('#filterDateBlock').addClass('d-none');
        $('#filterDate').val('');
        $('#filterByDateBtn').removeClass('d-none');
    });

    // Nút Lọc theo trạng thái
    $('#filterByStatusBtn').on('click', function () {
        $('#filterByStatusBtn').addClass('d-none');
        $('#filterStatusBlock').removeClass('d-none');
    });

    $('#applyStatusFilter').on('click', function () {
        var filterStatus = $('#filterStatus').val();
        if (filterStatus) {
            var url = `/technician/hoadondichvutechnician/filterbystatus?trangThai=${filterStatus}`;
            table.ajax.url(url).load();
        }
        $('#filterStatusBlock').addClass('d-none');
        $('#filterByStatusBtn').removeClass('d-none');
    });

    $('#cancelStatusFilter').on('click', function () {
        $('#filterStatusBlock').addClass('d-none');
        $('#filterStatus').val('');
        $('#filterByStatusBtn').removeClass('d-none');
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
                    url: `/technician/hoadondichvutechnician/delete/${id}`,
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

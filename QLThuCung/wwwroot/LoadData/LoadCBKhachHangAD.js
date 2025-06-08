$(document).ready(function () {
    // Load danh sách khách hàng
    $('#khachHangSearch').on('input', function () {
        const term = $(this).val().trim();
        const khachHangSelect = $('#khachHangSelect');
        if (term.length > 0) {
            $.ajax({
                url: '/admin/hoadondichvuad/searchkhachhang',
                type: 'GET',
                data: { term: term },
                success: function (response) {
                    console.log('Dữ liệu trả về: ', response);
                    khachHangSelect.empty();
                    khachHangSelect.append('<option value="">Chọn khách hàng</option>');

                    if (response.length > 0) {
                        response.forEach(khachHang => {
                            khachHangSelect.append(`<option id="UserId" value="${khachHang.id}">${khachHang.phoneNumber} - ${khachHang.hoTen}</option>`);
                        });
                        khachHangSelect.prop('disabled', false);
                    } else {
                        khachHangSelect.append('<option value="">Không tìm thấy khách hàng</option>');
                        khachHangSelect.prop('disabled', true);
                    }
                },
                error: function (xhr, status, error) {
                    console.log('Lỗi AJAX: ', error);
                    toastr.error('Không thể tìm kiếm khách hàng!');
                    khachHangSelect.prop('disabled', true);
                }
            });
        } else {
            khachHangSelect.empty();
            khachHangSelect.append('<option value="">Chọn khách hàng</option>');
            khachHangSelect.prop('disabled', true);
        }
    });



    // Thừa bên dịch vụ
    $('form').validate({
        rules: {
            "ChiTietHoaDonDichVu[0].IdDichVu": {
                required: true
            }
        },
        messages: {
            "ChiTietHoaDonDichVu[0].IdDichVu": "Vui lòng chọn ít nhất một dịch vụ!"
        },
        errorPlacement: function (error, element) {
            if (element.attr("name").startsWith("ChiTietHoaDonDichVu")) {
                error.appendTo('#dichVuValidation');
            } else {
                error.insertAfter(element);
            }
        }
    });
});
$(document).ready(function () {
    $('#khachHangSelect').on('change', function () {
        var userId = $(this).val();
        $('#khachHangSearch').val("");
        LoadCBThuCung(userId);
    });
});

function LoadCBThuCung(id) {
    $.ajax({
        url: '/admin/thucung/list/' + id,
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const comboBox = $('#ComboBoxThuCung');

            // Xóa các option cũ và thêm option mặc định
            comboBox.empty().append('<option value="">Chọn thú cưng</option>');

            // Thêm các option mới từ dữ liệu
            response.data.forEach(function (item) {
                comboBox.append(`<option value="${item.id}" data-weight="${item.canNang}">${item.ten}</option>`);
            });
        },
        error: function (xhr, status, error) {
            console.error('Lỗi khi lấy dữ liệu thú cưng:', error);
        }
    });
}

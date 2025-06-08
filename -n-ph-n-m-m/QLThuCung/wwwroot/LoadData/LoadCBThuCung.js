$(document).ready(function () {
    var userId = $('#UserId').val();
    if (userId) {
        LoadCBThuCung(userId); 
    } else {
        console.error('Không tìm thấy UserId. Vui lòng đăng nhập!');
    }
});

function LoadCBThuCung(id) {
    $.ajax({
        url: '/khachhang/thucung/list/' + id,
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

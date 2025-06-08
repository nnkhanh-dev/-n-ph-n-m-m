$(document).ready(function () {
    const selectedId = $('#IdLoaiChon');
    LoadCBLoai(selectedId); // ← Gọi hàm để tải danh sách loài khi trang được load xong
});
function LoadCBLoai(selectedId ) {
    $.ajax({
        url: '/admin/loai/list',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const comboBox = $('#ComboBoxLoai');

            // Xóa các option cũ và thêm option mặc định
            comboBox.empty().append('<option value="">Chọn loài</option>');

            // Thêm các option mới từ dữ liệu
             response.data.forEach(function (item) {
                const isSelected = selectedId && selectedId == item.id ? 'selected' : '';
                comboBox.append(`<option value="${item.id}" ${isSelected}>${item.ten}</option>`);
            });
        },
        error: function () {
            console.error('Lỗi khi tải danh sách loài!');
            $('#ComboBoxLoai').html('<option value="">Tải dữ liệu thất bại</option>');
        }
    });
}

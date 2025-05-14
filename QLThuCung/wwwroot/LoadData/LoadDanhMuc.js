$(document).ready(function () {
    LoadDanhMuc();
});

function LoadDanhMuc() {
    $.ajax({
        url: '/khachhang/danhmuc/list',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('#ListDanhMuc');
            list.empty();
            list.append(`<li data-danhmuc="0">Tất cả</li>`)
            response.data.forEach(function (item) {
                list.append(`<li data-danhmuc="${item.id}">${item.ten}</li>`);
            });
        },
         error: function (xhr, status, error) {
            console.error('Lỗi khi lấy dữ liệu danh mục:', error);
        }

    });
}
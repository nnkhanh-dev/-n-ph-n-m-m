$(document).ready(function () {
    const loaiSelect = $('#cbLoaiThuCung');
    const giongSelect = $('#cbGiongThuCung');
    const IdGiong = $('#IdGiongCB').val();
    const IdLoai = $('#IdLoaiCB').val();

    // Gọi API để lấy danh sách loại thú cưng và giống
    $.ajax({
        url: '/khachhang/loai/list',
        type: 'GET',
        success: function (response) {
            if (response.data && Array.isArray(response.data)) {
                loaiSelect.empty();
                // Đổ dữ liệu vào combobox loại thú cưng
                loaiSelect.empty().append(`<option value="">Chọn loài</option>`);
                giongSelect.empty().append(`<option value="">Chọn giống</option>`);
                response.data.forEach(loai => {
                    loaiSelect.append(`<option value="${loai.id}" ${loai.id == IdLoai ? 'selected' : ''}>${loai.ten}</option>`);
                });

                // Khi chọn loại thú cưng → đổ giống tương ứng
                loaiSelect.on('change', function () {
                    const selectedLoaiId = parseInt($(this).val());
                    giongSelect.empty().append(`<option value="">Chọn giống</option>`);

                    const selectedLoai = response.data.find(x => x.id === selectedLoaiId);
                    if (selectedLoai && selectedLoai.giong) {
                        selectedLoai.giong.forEach(g => {
                            giongSelect.append(`<option value="${g.id}" ${g.id == IdGiong ? 'selected' : ''}>${g.ten}</option>`);
                        });
                    }
                });
                loaiSelect.trigger('change');
            } else {
                toastr.warning('Không có dữ liệu loại thú cưng!');
            }
        },
        error: function () {
            toastr.error('Lỗi khi tải dữ liệu loại thú cưng!');
        }
    });
});
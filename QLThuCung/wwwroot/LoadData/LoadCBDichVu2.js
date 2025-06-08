let totalTimeGlobal = 0;

$(document).ready(function () {
    LoadCBDichVu2();


    // Lắng nghe sự thay đổi combobox thú cưng
    $('#ComboBoxThuCung').change(function () {
        updateEstimateTime();
        updateTotalPrice();
        updateHiddenInput();
    });
});

// Cập nhật LoadCBDichVu
function LoadCBDichVu2() {
    $.ajax({
        url: '/admin/dichvu/list/',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('#ListCheckBoxDichVu');
            list.empty();
            response.data.forEach(function (item) {
                list.append(`
                    <div>
                        <input type="checkbox" class="dichvu-checkbox" id="dichvu_${item.id}" 
                            data-time="${item.thoiGian}" 
                            data-prices='${JSON.stringify(item.bangGiaDV)}'
                            name="dichvu1" value="${item.id}">
                        <label for="dichvu_${item.id}" class="me-2">${item.ten} </label>
                    </div>
                    
                `);
            });
            updateEstimateTime();
            updateTotalPrice();
            updateHiddenInput();
            // Gắn sự kiện thay đổi
            $('.dichvu-checkbox').change(function () {
                updateEstimateTime();
                updateTotalPrice();
                updateHiddenInput();
            });

            
        },
        error: function (xhr, status, error) {
            console.error('Lỗi khi lấy dữ liệu thú cưng:', error);
        }
    });
}

function updateEstimateTime() {
    let totalTime = 0;

    // Tính tổng thời gian dịch vụ đã chọn
    $('.dichvu-checkbox:checked').each(function () {
        const time = parseInt($(this).attr('data-time')) || 0;
        totalTime += time;
    });

    totalTimeGlobal = totalTime;

    // Hiển thị thời gian ước tính
    $('#EstimateTime').text('Thời gian ước tính: ' + totalTime + ' phút');

    disableBusyTimes(hoaDonsData);
}


function updateTotalPrice() {
    let totalPrice = 0;

    const selectedPet = $('#ComboBoxThuCung').find(':selected');
    const petWeight = parseFloat(selectedPet.data('weight')) || 0;

    // Kiểm tra nếu chưa chọn thú cưng
    if (selectedPet.val() === '') {
        $('#TotalPrice').text('Vui lòng chọn thú cưng');
        return;
    }

    // Phần tính tiền như cũ...
    $('.dichvu-checkbox:checked').each(function () {
        const prices = JSON.parse($(this).attr('data-prices'));
        if (prices.length > 0) {
            const priceInfo = prices[0];
            if (priceInfo.loai === 0) {
                if (priceInfo.chiTietBangGiaDV.length > 0) {
                    totalPrice += priceInfo.chiTietBangGiaDV[0].chiPhi;
                }
            } else {
                let matchedPrice = 0;
                priceInfo.chiTietBangGiaDV.forEach(function (detail) {
                    if (petWeight <= detail.canNang && matchedPrice === 0) {
                        matchedPrice = detail.chiPhi;
                    }
                });
                if (matchedPrice === 0 && priceInfo.chiTietBangGiaDV.length > 0) {
                    matchedPrice = priceInfo.chiTietBangGiaDV[priceInfo.chiTietBangGiaDV.length - 1].chiPhi;
                }
                totalPrice += matchedPrice;
            }
        }
    });

    $('#TotalPrice').text(totalPrice.toLocaleString() + ' VNĐ');
} 

function updateHiddenInput() {
    const listHiddenInput = $('#listDichVuInput');
    listHiddenInput.empty(); // Xóa input cũ mỗi lần gọi

    const selectedPet = $('#ComboBoxThuCung').find(':selected');

    // Nếu chưa chọn thú cưng thì không thực hiện gì cả
    if (selectedPet.val() === '' || selectedPet.val() == null) {
        return;
    }

    const petWeight = parseFloat(selectedPet.data('weight')) || 0;
    let index = 0;

    $('.dichvu-checkbox:checked').each(function () {
        const prices = JSON.parse($(this).attr('data-prices'));
        const idDichVu = $(this).val();

        if (prices.length > 0) {
            const priceInfo = prices[0];
            let donGia = 0;

            if (priceInfo.loai === 0) {
                if (priceInfo.chiTietBangGiaDV.length > 0) {
                    donGia = priceInfo.chiTietBangGiaDV[0].chiPhi;
                }
            } else {
                priceInfo.chiTietBangGiaDV.forEach(function (detail) {
                    if (petWeight <= detail.canNang && donGia === 0) {
                        donGia = detail.chiPhi;
                    }
                });
                if (donGia === 0 && priceInfo.chiTietBangGiaDV.length > 0) {
                    donGia = priceInfo.chiTietBangGiaDV[priceInfo.chiTietBangGiaDV.length - 1].chiPhi;
                }
            }

            // Thêm input ẩn
            listHiddenInput.append(`<input type="hidden" name="ChiTietHoaDonDichVu[${index}].IdDichVu" value="${idDichVu}" />`);
            listHiddenInput.append(`<input type="hidden" name="ChiTietHoaDonDichVu[${index}].DonGia" value="${donGia}" />`);
            listHiddenInput.append(`<input type="hidden" name="ChiTietHoaDonDichVu[${index}].IdHoaDon" value="0" />`);

            index++;
        }
    });
}

let totalTimeGlobal = 0;

$(document).ready(function () {
    LoadCBDichVu();

    // Lắng nghe sự thay đổi combobox thú cưng
    $('#ComboBoxThuCung').change(function () {
        updateEstimateTime();
        updateTotalPrice();
        updateHiddenInput();
    });
});

// Cập nhật LoadCBDichVu
function LoadCBDichVu() {
    $.ajax({
        url: '/khachhang/dichvu/list/',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('#ListCheckBoxDichVu');
            list.empty();
            response.data.forEach(function (item) {
                list.append(`
                    <input type="checkbox" class="dichvu-checkbox" id="dichvu_${item.id}" 
                        data-time="${item.thoiGian}" 
                        data-prices='${JSON.stringify(item.bangGiaDV)}'
                        name="dichvu1" value="${item.id}">
                    <label for="dichvu_${item.id}" class="me-2">${item.ten}</label>
                `);
            });

            // Gắn sự kiện thay đổi
            $('.dichvu-checkbox').change(function () {
                updateEstimateTime();
                updateTotalPrice();
                updateHiddenInput();
            });

            updateEstimateTime();
            updateTotalPrice();
            updateHiddenInput();
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

    // Gọi hàm disableBusyTimes trước khi lấy danh sách các option
    // Truyền vào dữ liệu hoaDons nếu cần
    if (typeof hoaDonsData !== 'undefined') {
        disableBusyTimes(hoaDonsData); // Disable các option bị bận
    } else {
        console.error('Dữ liệu hoaDons chưa được tải.');
    }

    // Lấy danh sách tất cả các option có thể chọn
    const availableOptions = $('#ThoiGianChamSoc option').not(':disabled');

    const notAvailableOptions = $('#ThoiGianChamSoc option:disabled');


    // Sau khi disable các option, tiếp tục xử lý các option có thể chọn
    availableOptions.each(function () {
        const optionValue = parseInt($(this).val());  // Mốc thời gian của option

        let nextBusyOption = null;
        // Chỉ tìm nextBusyOption nếu có notAvailableOptions
        if (notAvailableOptions.length > 0) {
            notAvailableOptions.each(function () {
                const val = parseInt($(this).val());
                if (val > optionValue && (nextBusyOption === null || val < nextBusyOption)) {
                    nextBusyOption = val;
                }
            });
        }

        // Chỉ thực hiện khi nextBusyOption có giá trị
        if (nextBusyOption !== null && nextBusyOption - optionValue < totalTimeGlobal) {
            $(this).prop('disabled', true);
        }

        if (optionValue + totalTime > 1020) {
            $(this).prop('disabled', true);
        }

        if (optionValue + totalTime > 660 && optionValue < 660) {
            $(this).prop('disabled', true);
        }
    });
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

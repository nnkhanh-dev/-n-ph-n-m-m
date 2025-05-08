let hoaDonsData = [];
$(document).ready(function () {
    // Khi người dùng thay đổi ngày chăm sóc
    $("#NgayChamSoc").change(function () {
        let selectedDate = $(this).val();
        console.log(selectedDate);
        if (selectedDate) {
            // Gọi API lấy hóa đơn theo ngày
            $.ajax({
                url: `/khachhang/datlich/listbydate/${selectedDate}`,
                method: 'GET',
                success: function (response) {
                    if (response && response.data) {
                        hoaDonsData = response.data;
                        disableBusyTimes(hoaDonsData);
                    } else {
                        console.error('Dữ liệu trả về không hợp lệ.');
                    }
                },
                error: function () {
                    console.error('Lỗi khi lấy danh sách hóa đơn.');
                }
            });
        } else {
            // Nếu ngày bị xóa thì enable lại tất cả option
            $("#ThoiGianChamSoc option").prop('disabled', false);
            hoaDonsData = []; // Đặt lại hoaDonsData để tránh sử dụng dữ liệu cũ
            updateEstimateTime(); // Cập nhật lại thời gian ước tính
        }
    });
});

function disableBusyTimes(hoaDons) {
    // Enable lại tất cả option trước
    $("#ThoiGianChamSoc option").prop('disabled', false);

    // Lưu tất cả khoảng thời gian bận
    let busyTimes = [];

    hoaDons.forEach(function (hoaDon) {
        let startMinutes = hoaDon.thoiGianChamSoc;

        let totalServiceMinutes = 0;
        if (hoaDon.chiTietHoaDonDichVu && hoaDon.chiTietHoaDonDichVu.length > 0) {
            hoaDon.chiTietHoaDonDichVu.forEach(function (chiTiet) {
                if (chiTiet.dichVu && chiTiet.dichVu.thoiGian) {
                    totalServiceMinutes += chiTiet.dichVu.thoiGian;
                }
            });
        }

        let endMinutes = startMinutes + totalServiceMinutes;

        busyTimes.push({ start: startMinutes, end: endMinutes });
    });

    // Duyệt từng option để disable nếu bị bận
    $("#ThoiGianChamSoc option").each(function () {
        let option = $(this);
        let value = parseInt(option.val());

        if (isNaN(value)) return; // Bỏ qua option đầu tiên "Chọn giờ đến"

        for (let i = 0; i < busyTimes.length; i++) {
            if (value >= busyTimes[i].start && value < busyTimes[i].end) {
                option.prop('disabled', true);
                break;
            }
        }
    });

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

        if (optionValue + totalTimeGlobal > 1020 ) {
            $(this).prop('disabled', true);
        }

        if (optionValue + totalTimeGlobal > 660 && optionValue < 660) {
            $(this).prop('disabled', true);
        }

    });

}
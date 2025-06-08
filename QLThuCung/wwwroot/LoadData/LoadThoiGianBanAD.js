let hoaDonsData = [];

$(document).ready(function () {
    // Khi người dùng thay đổi ngày chăm sóc
    $("#NgayChamSoc").change(function () {
        let selectedDate = $(this).val();
        if (selectedDate) {
            // Gọi API lấy hóa đơn theo ngày
            $.ajax({
                url: `/admin/datlich/listbydate/${selectedDate}`,
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
    $.ajax({
        url: '/admin/giuong/list',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const soGiuong = response.data.length;

            $("#ThoiGianChamSoc option").prop('disabled', false);

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

            $("#ThoiGianChamSoc option").each(function () {
                let option = $(this);
                let value = parseInt(option.val());

                if (isNaN(value)) return;

                let endEstimate = value + totalTimeGlobal;

                // Đếm số ca đang bận trong khoảng này
                let countBusy = 0;
                for (let i = 0; i < busyTimes.length; i++) {
                    let bt = busyTimes[i];
                    if ((value < bt.end && endEstimate > bt.start)) {
                        countBusy++;
                    }
                }

                if (countBusy >= soGiuong -1 || (value < 660 && endEstimate > 660) || endEstimate > 1020) {
                    option.prop('disabled', true);
                }
            });
        },
        error: function (xhr, status, error) {
            console.error('Lỗi khi lấy dữ liệu giường:', error);
        }
    });
}
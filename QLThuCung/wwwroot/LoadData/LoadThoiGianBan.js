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
                        disableBusyTimes(response.data);    
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
}
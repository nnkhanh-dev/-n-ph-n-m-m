$(document).ready(function () {
    var chiTietList = []; // Global list to store selected ChiTietHoaDonSanPham

    LoadCartItem();
    UpdateQuantity();
    handleCheckboxChange(); // Initialize checkbox event listener
    handleMuaNgayClick(); // Initialize Mua Ngay button click handler
    bindQuantityButtonEvents(); // Initialize quantity button event listeners
    bindDeleteButtonEvents(); // Initialize delete button event listeners

    // Function to bind events to quantity buttons
    function bindQuantityButtonEvents() {
        $(document).on('click', '.quantity-selector .btn-decrease', function () {
            var $input = $(this).siblings('.quantityCart');
            var current = parseInt($input.val());
            if (current > 1) {
                $input.val(current - 1).trigger('change'); // Trigger change event
            }
        });

        $(document).on('click', '.quantity-selector .btn-increase', function () {
            var $input = $(this).siblings('.quantityCart');
            var current = parseInt($input.val());
            var max = parseInt($input.attr('max'));
            if (current < max) {
                $input.val(current + 1).trigger('change'); // Trigger change event
            }
        });
    }

    // Function to bind events to delete buttons
    function bindDeleteButtonEvents() {
        $(document).on('click', '.cart-btn .btn-delete', function () {
            var idSP = $(this).data('idsp'); // Get idSanPham from data attribute
            Swal.fire({
                title: 'Bạn có chắc chắn?',
                text: 'Sản phẩm này sẽ bị xóa khỏi giỏ hàng!',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Xóa',
                cancelButtonText: 'Hủy'
            }).then((result) => {
                if (result.isConfirmed) {
                    $.ajax({
                        url: `/khachhang/giohang/xoa/${idSP}`,
                        type: 'DELETE',
                        dataType: 'json',
                        success: function (response) {
                            if (response.success) {
                                toastr.success('Xóa sản phẩm khỏi giỏ hàng thành công!');
                                LoadCartItem(); // Refresh cart
                            } else {
                                toastr.error('Xóa sản phẩm thất bại. Vui lòng thử lại!');
                            }
                        },
                        error: function (xhr) {
                            toastr.error('Đã có lỗi xảy ra. Vui lòng thử lại!');
                            console.log(xhr.responseText);
                        }
                    });
                }
            });
        });
    }

    // Function to update the summary (number of selected products and total price)
    function updateSummary() {
        var soLuongSanPhamChon = chiTietList.length;
        var tongTien = chiTietList.reduce((total, item) => total + (item.SoLuong * item.DonGia), 0);

        $('#soLuongSanPhamChon').text(soLuongSanPhamChon);
        $('#tongTienSanPhamChon').text(tongTien.toLocaleString('vi-VN') + ' VNĐ');
    }

    // Function to handle checkbox change events
    function handleCheckboxChange() {
        $(document).on('change', '.cart-checkbox input[type="checkbox"]', function () {
            var $checkbox = $(this);
            var idSanPham = $checkbox.attr('data-idSanPhamChon');
            var soLuong = parseInt($checkbox.attr('data-soLuongSanPhamChon'));
            var donGia = parseFloat($checkbox.attr('data-donGiaSanPhamChon')) / soLuong; // Calculate unit price

            if ($checkbox.is(':checked')) {
                // Add to chiTietList if checked
                var chiTiet = {
                    IdSanPham: parseInt(idSanPham),
                    SoLuong: soLuong,
                    DonGia: donGia
                };
                chiTietList.push(chiTiet);
            } else {
                // Remove from chiTietList if unchecked
                chiTietList = chiTietList.filter(item => item.IdSanPham !== parseInt(idSanPham));
            }

            // Update summary display
            updateSummary();

            console.log('Current chiTietList:', chiTietList); // Debugging
        });
    }

    // Function to handle Mua Ngay button click
    function handleMuaNgayClick() {
        $('#muaNgayGioHang').click(function () {
            if (chiTietList.length === 0) {
                toastr.error('Vui lòng chọn ít nhất một sản phẩm để mua!');
                return;
            }

            $.ajax({
                url: '/khachhang/hoadonsanpham/muangay',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(chiTietList),
                success: function (response) {
                    if (response.success) {
                        toastr.success(response.message);
                        // Redirect to create invoice page
                        window.location.href = '/khachhang/hoadonsanpham/taomoi';
                    } else {
                        toastr.error(response.message);
                    }
                },
                error: function () {
                    toastr.error('Đã có lỗi xảy ra, vui lòng thử lại!');
                }
            });
        });
    }

    function LoadCartItem() {
        $.ajax({
            url: '/khachhang/giohang/list',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                const list = $('#list-cart');
                list.empty();
                chiTietList = []; // Clear chiTietList when reloading cart
                response.data.forEach(function (item) {
                    list.append(`
                        <div class="card cart-item mt-3">
                            <div class="d-flex align-content-center justify-content-around">
                                <div class="cart-checkbox">
                                    <input data-idSanPhamChon="${item.idSanPham}" data-soLuongSanPhamChon="${item.soLuong}" data-donGiaSanPhamChon="${(item.sanPham.giamGia != null && item.sanPham.giamGia > 0
                            ? (item.sanPham.gia * (1 - item.sanPham.giamGia / 100) * item.soLuong)
                            : item.sanPham.gia * item.soLuong
                        )}" type="checkbox"></input>
                                </div>
                                <div class="d-flex cart-content m-3">
                                    <div class="cart-itemimg shadow">
                                        <img src="${item.sanPham.anhSanPham[0].duongDan}" class="w-100"></img>
                                    </div>
                                    <div class="cart-itemcontent ps-3">
                                        <div class="d-flex justify-content-between align-items-center">
                                            <p class="card-title"><strong>${item.sanPham.ten}</strong></p>
                                        </div>
                                        <p class="price m-0 p-0">${(item.sanPham.giamGia != null && item.sanPham.giamGia > 0
                            ? (item.sanPham.gia * (1 - item.sanPham.giamGia / 100) * item.soLuong)
                            : item.sanPham.gia * item.soLuong
                        ).toLocaleString('vi-VN')} VNĐ
                                        </p>
                                        <div><strong>Số lượng hiện còn: </strong>${item.sanPham.soLuong}</div>
                                    </div>
                                    <div class="quantity-selector">
                                        <button type="button" class="btn btn-outline-secondary btn-decrease">-</button>
                                        <input data-idSP="${item.idSanPham}" type="number" class="quantityCart" value="${item.soLuong}" min="1" max="2370965" style="width: 60px; padding:5px; text-align: center;" />
                                        <button type="button" class="btn btn-outline-secondary btn-increase">+</button>
                                    </div>
                                </div>
                                <div class="cart-btn">
                                    <button class="btn btn-danger btn-delete" data-idSP="${item.idSanPham}">
                                        <svg xmlns="http://www.w3.org/2000/svg" fill="white" width="20px" viewBox="0 0 384 512">
                                            <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                                        </svg>
                                    </button>
                                </div>
                            </div>
                        </div>
                    `);
                });
                updateSummary(); // Initialize summary after loading cart
            }
        });
    }

    function UpdateQuantity() {
        $(document).on('change', '.quantityCart', function () {
            var $input = $(this);
            var soLuongMoi = parseInt($input.val());
            var idSanPham = $input.attr('data-idSP');

            // Validate idSanPham
            if (!idSanPham) {
                toastr.error('Không tìm thấy ID sản phẩm!');
                $input.val($input.data('previousValue') || 1);
                return;
            }

            // Validate quantity
            if (isNaN(soLuongMoi) || soLuongMoi < 1) {
                toastr.error('Số lượng không hợp lệ!');
                $input.val(1); // Reset to minimum
                return;
            }

            // Prepare data for request
            var model = {
                IdSanPham: idSanPham,
                SoLuong: soLuongMoi
            };

            // Send AJAX request to update quantity
            $.ajax({
                url: '/khachhang/giohang/chinhsua/',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(model),
                dataType: 'json',
                success: function (response) {
                    if (response.success) {
                        toastr.success('Cập nhật số lượng thành công!');
                        LoadCartItem(); // Refresh cart
                    } else {
                        toastr.error('Cập nhật số lượng thất bại. Vui lòng thử lại!');
                        LoadCartItem();
                    }
                },
                error: function (xhr) {
                    toastr.error('Đã xảy ra lỗi. Vui lòng thử lại!');
                    LoadCartItem();
                    console.log(xhr.responseText);
                }
            });

            // Update chiTietList if the product is selected
            var checkbox = $input.closest('.cart-item').find('.cart-checkbox input');
            if (checkbox.is(':checked')) {
                var index = chiTietList.findIndex(item => item.IdSanPham === parseInt(idSanPham));
                if (index !== -1) {
                    chiTietList[index].SoLuong = soLuongMoi;
                    // Update DonGia based on the new quantity
                    chiTietList[index].DonGia = parseFloat(checkbox.attr('data-donGiaSanPhamChon')) / chiTietList[index].SoLuong;
                }
            }

            // Update summary display
            updateSummary();
        });
    }
});
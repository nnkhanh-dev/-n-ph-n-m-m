$(document).ready(function () {
    LoadCartItem();
    UpdateQuantity()
});

function LoadCartItem() {
    $.ajax({
        url: '/khachhang/giohang/list',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            const list = $('#list-cart');
            list.empty();
            response.data.forEach(function (item) {
                list.append(`
                    <div class="card cart-item mt-3">
                        <div class="d-flex align-content-center justify-content-around">
                            <div class="cart-checkbox">
                                <input type="checkbox"></input>
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
                                    <button type="button" class="btn btn-outline-secondary" onclick="decreaseQuantityCart(this)">-</button>
                                    <input data-idSP="${item.idSanPham}" type="number" class="quantityCart"  value="${item.soLuong}" min="1" max="2370965" style="width: 60px; padding:5px; text-align: center;" />
                                    <button type="button" class="btn btn-outline-secondary" onclick="increaseQuantityCart(this)">+</button>
                                </div>
                            </div>
                            <div class="cart-btn" >
                                <button class="btn btn-danger" onClick="Delete(${item.idSanPham})">
                                    <svg xmlns="http://www.w3.org/2000/svg" fill="white" width="20px" viewBox="0 0 384 512">
                                        <path d="M342.6 150.6c12.5-12.5 12.5-32.8 0-45.3s-32.8-12.5-45.3 0L192 210.7 86.6 105.4c-12.5-12.5-32.8-12.5-45.3 0s-12.5 32.8 0 45.3L146.7 256 41.4 361.4c-12.5 12.5-12.5 32.8 0 45.3s32.8 12.5 45.3 0L192 301.3 297.4 406.6c12.5 12.5 32.8 12.5 45.3 0s12.5-32.8 0-45.3L237.3 256 342.6 150.6z" />
                                    </svg>
                                </button>
                            </div>
                        </div>
                    </div>
                `);
            });
        }
    });
}

function Delete(idSP) {
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
                        LoadCartItem(); // Làm mới danh sách giỏ hàng
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
}

function decreaseQuantityCart(button) {
    var $input = $(button).siblings('.quantityCart');
    var current = parseInt($input.val());
    if (current > 1) {
        $input.val(current - 1).trigger('change'); // Kích hoạt sự kiện change
    }
}

function increaseQuantityCart(button) {
    var $input = $(button).siblings('.quantityCart');
    var current = parseInt($input.val());
    var max = parseInt($input.attr('max'));
    if (current < max) {
        $input.val(current + 1).trigger('change'); // Kích hoạt sự kiện change
    }
}

function UpdateQuantity() {
    $(document).on('change', '.quantityCart', function () {
        var $input = $(this);
        var soLuongMoi = parseInt($input.val());
        var idSanPham = $input.attr('data-idSP');
        console.log($input.attr('data-idSP'));

        // Kiểm tra idSanPham
        if (!idSanPham) {
            toastr.error('Không tìm thấy ID sản phẩm!');
            $input.val($input.data('previousValue') || 1);
            return;
        }

        // Kiểm tra số lượng hợp lệ
        if (isNaN(soLuongMoi) || soLuongMoi < 1) {
            toastr.error('Số lượng không hợp lệ!');
            $input.val(1); // Đặt lại về giá trị tối thiểu
            return;
        }

        // Chuẩn bị dữ liệu cho yêu cầu
        var model = {
            IdSanPham: idSanPham,
            SoLuong: soLuongMoi
        };

        // Gửi yêu cầu AJAX để cập nhật số lượng
        $.ajax({
            url: '/khachhang/giohang/chinhsua/',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(model),
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    toastr.success('Cập nhật số lượng thành công!');
                    LoadCartItem(); // Làm mới danh sách giỏ hàng
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

      
    });

    
}
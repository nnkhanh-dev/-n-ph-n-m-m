$(document).ready(function () {
    LoadTheSanPham(); // Load all products when the page is loaded

    function LoadTheSanPham() {
        $.ajax({
            url: '/khachhang/sanpham/list',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                const list = $('.my_productlist');
                list.empty();

                // Iterate through the products and append product cards
                response.data.forEach(function (item) {
                    // Calculate total sold
                    const totalSold = item.chiTietHoaDonSanPham && item.chiTietHoaDonSanPham.length > 0
                        ? item.chiTietHoaDonSanPham.reduce((sum, detail) => sum + (detail.soLuong || 0), 0)
                        : 0;

                    // Calculate average rating
                    let averageRating = 0;
                    if (item.chiTietHoaDonSanPham && item.chiTietHoaDonSanPham.length > 0) {
                        const ratings = item.chiTietHoaDonSanPham
                            .filter(detail => detail.hoaDon && detail.hoaDon.danhGia && detail.hoaDon.danhGia.length > 0)
                            .flatMap(detail => detail.hoaDon.danhGia.map(danhGia => danhGia.sao));
                        if (ratings.length > 0) {
                            averageRating = Number((ratings.reduce((sum, rating) => sum + rating, 0) / ratings.length).toFixed(1));
                        }
                    }

                    // Append product card to the list
                    list.append(`
                        <div class="my_productcard shadow">
                            <div class="my_productspecial">${item.giamGia != null && item.giamGia > 0 ? '-' + item.giamGia + '%' : 'Mới'}</div>
                            <div class="my_productimg">
                                <img src="${item.anhSanPham && item.anhSanPham.length > 0 ? item.anhSanPham[0].duongDan : '/assets/images/placeholder.jpg'}">
                            </div>
                            <div class="my_productcontent">
                                <p class="my_producttitle">${item.ten}</p>
                                <p class="my_productprice">
                                    ${(item.giamGia != null && item.giamGia > 0
                            ? (item.gia * (1 - item.giamGia / 100))
                            : item.gia
                        ).toLocaleString('vi-VN')} VNĐ
                                </p>
                                <div class="d-flex align-items-center my-3">
                                    <div class="d-flex align-items-center">
                                        ${averageRating}
                                        <svg xmlns="http://www.w3.org/2000/svg" class="star" viewBox="0 0 576 512">
                                            <path d="M316.9 18C311.6 7 300.4 0 288.1 0s-23.4 7-28.8 18L195 150.3 51.4 171.5c-12 1.8-22 10.2-25.7 21.7s-.7 24.2 7.9 32.7L137.8 329 113.2 474.7c-2 12 3 24.2 12.9 31.3s23 8 33.8 2.3l128.3-68.5 128.3 68.5c10.8 5.7 23.9 4.9 33.8-2.3s14.9-19.3 12.9-31.3L438.5 329 542.7 225.9c8.6-8.5 11.7-21.2 7.9-32.7s-13.7-19.9-25.7-21.7L381.2 150.3 316.9 18z" />
                                        </svg>
                                    </div>
                                    <span class="mx-1">|</span>
                                    <div class="d-flex align-items-center">${totalSold} lượt bán</div>
                                </div>
                                <div class="my_productbutton">
                                    <a href="/khachhang/sanpham/chitiet/${item.id}" class="btn btn-primary w-100">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                                            <path d="M288 32c-80.8 0-145.5 36.8-192.6 80.6C48.6 156 17.3 208 2.5 243.7c-3.3 7.9-3.3 16.7 0 24.6C17.3 304 48.6 356 95.4 399.4C142.5 443.2 207.2 480 288 480s145.5-36.8 192.6-80.6c46.8-43.5 78.1-95.4 93-131.1c3.3-7.9 3.3-16.7 0-24.6c-14.9-35.7-46.2-87.7-93-131.1C433.5 68.8 368.8 32 288 32zM144 256a144 144 0 1 1 288 0 144 144 0 1 1 -288 0zm144-64c0 35.3-28.7 64-64 64c-7.1 0-13.9-1.2-20.3-3.3c-5.5-1.8-11.9 1.6-11.7 7.4c.3 6.9 1.3 13.8 3.2 20.7c13.7 51.2 66.4 81.6 117.6 67.9s81.6-66.4 67.9-117.6c-11.1-41.5-47.8-69.4-88.6-71.1c-5.8-.2-9.2 6.1-7.4 11.7c2.1 6.4 3.3 13.2 3.3 20.3z"/>
                                        </svg>
                                        Chi tiết
                                    </a>
                                    <button class="btn btn-warning text-light w-100 mt-2" onClick="AddToCart(${item.id})">
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                                            <path d="M0 24C0 10.7 10.7 0 24 0L69.5 0c22 0 41.5 12.8 50.6 32l411 0c26.3 0 45.5 25 38.6 50.4l-41 152.3c-8.5 31.4-37 53.3-69.5 53.3l-288.5 0 5.4 28.5c2.2 11.3 12.1 19.5 23.6 19.5L488 336c13.3 0 24 10.7 24 24s-10.7 24-24 24l-288.3 0c-34.6 0-64.3-24.6-70.7-58.5L77.4 54.5c-.7-3.8-4-6.5-7.9-6.5L24 48C10.7 48 0 37.3 0 24zM128 464a48 48 0 1 1 96 0 48 48 0 1 1 -96 0zm336-48a48 48 0 1 1 0 96 48 48 0 1 1 0-96z" />
                                        </svg>
                                        Thêm vào giỏ hàng
                                    </button>
                                </div>
                            </div>
                        </div>
                    `);

                    // Khởi tạo hoặc làm mới Owl Carousel
                    if (list.hasClass('owl-carousel')) {
                        list.trigger('destroy.owl.carousel'); // Hủy carousel hiện tại
                    }

                    // Kiểm tra số lượng sản phẩm để cấu hình loop
                    const loop = response.data.length > 1; // Chỉ bật loop nếu có nhiều hơn 1 sản phẩm

                    list.owlCarousel({
                        loop: loop,
                        nav: false,
                        dots: false,
                        autoplay: true,
                        autoplayTimeout: 3000,
                        responsive: {
                            0: { items: 2 },
                            600: { items: 3 },
                            750: { items: 3 },
                            1000: { items: 3 },
                            1300: { items: 4 }
                        }
                    });
                });
            },
            error: function (xhr, status, error) {
                console.error('Lỗi khi lấy dữ liệu sản phẩm:', error);
                $('.my_productlist').empty().append('<p class="text-center">Đã xảy ra lỗi khi tải sản phẩm. Vui lòng thử lại.</p>');
            }
        });
       
    }

    function AddToCart(id) {
        var chiTietGioHang = {
            IdSanPham: id,
            SoLuong: 1,
            IdGioHang: 'temp'
        };

        $.ajax({
            url: '/khachhang/giohang/taomoi',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(chiTietGioHang),
            success: function (response) {
                if (response.success) {
                    toastr.success(response.message);
                } else {
                    toastr.error(response.message);
                }
            },
            error: function () {
                toastr.error('Đã có lỗi xảy ra, vui lòng thử lại !');
            }
        });
    }
});
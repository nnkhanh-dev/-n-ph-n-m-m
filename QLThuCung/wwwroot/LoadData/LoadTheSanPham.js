$(document).ready(function () {
    LoadDanhMuc(); // Giả định hàm này đã có để tải danh mục
    LoadTheSanPham(); // Tải toàn bộ sản phẩm khi trang được tải

    // Biến để lưu trạng thái hiện tại
    let currentDanhMuc = 0;
    let currentMaxPrice = null;
    let priceSortOrder = null; // null: không sắp xếp giá, "asc": tăng dần, "desc": giảm dần
    let salesFilter = "tatCaLuotBan"; // Mặc định là "Tất cả"
    let salesSortOrder = null; // null: không sắp xếp lượt bán, "asc": tăng dần, "desc": giảm dần
    let ratingFilter = "tatCaSao"; // Mặc định là "Tất cả"
    let ratingSortOrder = null; // null: không sắp xếp số sao, "asc": tăng dần, "desc": giảm dần
    let searchQuery = ""; // Mặc định không có từ khóa tìm kiếm
    let currentPage = 1; // Trang hiện tại
    const itemsPerPage = 6; // Số sản phẩm mỗi trang

    // Lắng nghe sự kiện click vào danh mục
    $('#ListDanhMuc').on('click', 'li', function () {
        currentDanhMuc = $(this).data('danhmuc');
        priceSortOrder = null;
        salesSortOrder = null;
        salesFilter = "tatCaLuotBan";
        ratingSortOrder = null;
        ratingFilter = "tatCaSao";
        searchQuery = "";
        currentPage = 1; // Reset về trang 1
        $('#tatCaLuotBan').prop('checked', true);
        $('#tatCaSao').prop('checked', true);
        $('#TimKiemSanPham').val("Tìm kiếm");
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $('#ListDanhMuc li').removeClass('active');
        $(this).addClass('active');
    });

    // Lắng nghe sự kiện thay đổi của thanh trượt giá
    $('#myRange').on('input', function () {
        currentMaxPrice = $(this).val();
        $('#rangePrice').text(currentMaxPrice);
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
    });

    // Lắng nghe sự kiện click vào nút sắp xếp giá tăng dần
    $('#GiaTangDan').on('click', function () {
        priceSortOrder = 'asc';
        salesSortOrder = null;
        ratingSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#GiaGiamDan').removeClass('active-sort');
        $('#LuotBanTangDan').removeClass('active-sort');
        $('#LuotBanGiamDan').removeClass('active-sort');
        $('#DanhGiaTangDan').removeClass('active-sort');
        $('#DanhGiaGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện click vào nút sắp xếp giá giảm dần
    $('#GiaGiamDan').on('click', function () {
        priceSortOrder = 'desc';
        salesSortOrder = null;
        ratingSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#GiaTangDan').removeClass('active-sort');
        $('#LuotBanTangDan').removeClass('active-sort');
        $('#LuotBanGiamDan').removeClass('active-sort');
        $('#DanhGiaTangDan').removeClass('active-sort');
        $('#DanhGiaGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện click vào nút sắp xếp lượt bán tăng dần
    $('#LuotBanTangDan').on('click', function () {
        salesSortOrder = 'asc';
        priceSortOrder = null;
        ratingSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#LuotBanGiamDan').removeClass('active-sort');
        $('#GiaTangDan').removeClass('active-sort');
        $('#GiaGiamDan').removeClass('active-sort');
        $('#DanhGiaTangDan').removeClass('active-sort');
        $('#DanhGiaGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện click vào nút sắp xếp lượt bán giảm dần
    $('#LuotBanGiamDan').on('click', function () {
        salesSortOrder = 'desc';
        priceSortOrder = null;
        ratingSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#LuotBanTangDan').removeClass('active-sort');
        $('#GiaTangDan').removeClass('active-sort');
        $('#GiaGiamDan').removeClass('active-sort');
        $('#DanhGiaTangDan').removeClass('active-sort');
        $('#DanhGiaGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện click vào nút sắp xếp số sao tăng dần
    $('#DanhGiaTangDan').on('click', function () {
        ratingSortOrder = 'asc';
        priceSortOrder = null;
        salesSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#DanhGiaGiamDan').removeClass('active-sort');
        $('#GiaTangDan').removeClass('active-sort');
        $('#GiaGiamDan').removeClass('active-sort');
        $('#LuotBanTangDan').removeClass('active-sort');
        $('#LuotBanGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện click vào nút sắp xếp số sao giảm dần
    $('#DanhGiaGiamDan').on('click', function () {
        ratingSortOrder = 'desc';
        priceSortOrder = null;
        salesSortOrder = null;
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        $(this).addClass('active-sort');
        $('#DanhGiaTangDan').removeClass('active-sort');
        $('#GiaTangDan').removeClass('active-sort');
        $('#GiaGiamDan').removeClass('active-sort');
        $('#LuotBanTangDan').removeClass('active-sort');
        $('#LuotBanGiamDan').removeClass('active-sort');
    });

    // Lắng nghe sự kiện thay đổi của radio buttons lọc lượt bán
    $(document).on('change', 'input[name="luotBan"]', function () {
        salesFilter = $(this).attr('id');
        console.log('Sales radio changed:', salesFilter);
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
    });

    // Lắng nghe sự kiện thay đổi của radio buttons lọc số sao
    $(document).on('change', 'input[name="danhGia"]', function () {
        ratingFilter = $(this).attr('id');
        console.log('Rating radio changed:', ratingFilter);
        currentPage = 1; // Reset về trang 1
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
    });

    // Lắng nghe sự kiện trên ô tìm kiếm
    let debounceTimeout;
    $('#TimKiemSanPham').on('input', function () {
        let query = $(this).val().trim();
        if (query === "Tìm kiếm") query = "";
        searchQuery = query;
        clearTimeout(debounceTimeout);
        debounceTimeout = setTimeout(() => {
            console.log('Search query:', searchQuery);
            currentPage = 1; // Reset về trang 1
            LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        }, 300);
    }).on('focus', function () {
        if ($(this).val() === "Tìm kiếm") {
            $(this).val("");
        }
    }).on('blur', function () {
        if ($(this).val().trim() === "") {
            $(this).val("Tìm kiếm");
            searchQuery = "";
            currentPage = 1; // Reset về trang 1
            LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
        }
    });

    // Lắng nghe sự kiện click vào nút phân trang
    $(document).on('click', '.pagination-btn', function () {
        const page = $(this).data('page');
        if (page === 'prev' && currentPage > 1) {
            currentPage--;
        } else if (page === 'next') {
            const totalPages = Math.ceil(filteredProductsCount / itemsPerPage);
            if (currentPage < totalPages) currentPage++;
        } else if (typeof page === 'number') {
            currentPage = page;
        }
        LoadTheSanPham(currentDanhMuc, currentMaxPrice, priceSortOrder, salesFilter, salesSortOrder, ratingFilter, ratingSortOrder, searchQuery, currentPage);
    });

    // Biến lưu số lượng sản phẩm sau khi lọc để sử dụng trong phân trang
    let filteredProductsCount = 0;

    function LoadTheSanPham(dataDanhMuc = 0, maxPrice = null, priceSortOrder = null, salesFilter = "tatCaLuotBan", salesSortOrder = null, ratingFilter = "tatCaSao", ratingSortOrder = null, searchQuery = "", page = 1) {
        $.ajax({
            url: '/khachhang/sanpham/list',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                console.log('Filters:', { salesFilter, ratingFilter, searchQuery, page });
                const list = $('#product-list');
                list.empty();

                LoadPriceRange(response.data, maxPrice);

                let filteredProducts = response.data;

                // Lọc theo danh mục
                if (dataDanhMuc != 0) {
                    filteredProducts = filteredProducts.filter(item => item.idDanhMuc == dataDanhMuc);
                }

                // Lọc theo giá
                if (maxPrice !== null) {
                    filteredProducts = filteredProducts.filter(item => item.gia <= maxPrice);
                }

                // Lọc theo từ khóa tìm kiếm
                if (searchQuery) {
                    const normalizedQuery = removeVietnameseTones(searchQuery.toLowerCase());
                    filteredProducts = filteredProducts.filter(item =>
                        removeVietnameseTones(item.ten.toLowerCase()).includes(normalizedQuery)
                    );
                }

                // Tính tổng số lượng bán và số sao trung bình
                filteredProducts = filteredProducts.map(item => {
                    const totalSold = item.chiTietHoaDonSanPham && item.chiTietHoaDonSanPham.length > 0
                        ? item.chiTietHoaDonSanPham.reduce((sum, detail) => sum + (detail.soLuong || 0), 0)
                        : 0;

                    let averageRating = 0;
                    if (item.chiTietHoaDonSanPham && item.chiTietHoaDonSanPham.length > 0) {
                        const ratings = item.chiTietHoaDonSanPham
                            .filter(detail => detail.hoaDon && detail.hoaDon.danhGia && detail.hoaDon.danhGia.length > 0)
                            .flatMap(detail => detail.hoaDon.danhGia.map(danhGia => danhGia.sao));
                        if (ratings.length > 0) {
                            averageRating = Number((ratings.reduce((sum, rating) => sum + rating, 0) / ratings.length).toFixed(1));
                        }
                    }

                    return { ...item, totalSold, averageRating };
                });

                // Lọc theo lượt bán
                if (salesFilter === "duoi100") {
                    filteredProducts = filteredProducts.filter(item => item.totalSold < 100);
                } else if (salesFilter === "tren100duoi1000") {
                    filteredProducts = filteredProducts.filter(item => item.totalSold >= 100 && item.totalSold <= 1000);
                } else if (salesFilter === "tren1000") {
                    filteredProducts = filteredProducts.filter(item => item.totalSold > 1000);
                }

                // Lọc theo số sao
                if (ratingFilter === "motSao") {
                    filteredProducts = filteredProducts.filter(item => item.averageRating >= 1.0 && item.averageRating < 2.0);
                } else if (ratingFilter === "haiSao") {
                    filteredProducts = filteredProducts.filter(item => item.averageRating >= 2.0 && item.averageRating < 3.0);
                } else if (ratingFilter === "baSao") {
                    filteredProducts = filteredProducts.filter(item => item.averageRating >= 3.0 && item.averageRating < 4.0);
                } else if (ratingFilter === "bonSao") {
                    filteredProducts = filteredProducts.filter(item => item.averageRating >= 4.0 && item.averageRating < 5.0);
                } else if (ratingFilter === "namSao") {
                    filteredProducts = filteredProducts.filter(item => item.averageRating === 5.0);
                }

                // Sắp xếp sản phẩm
                if (priceSortOrder === 'asc') {
                    filteredProducts.sort((a, b) => a.gia - b.gia);
                } else if (priceSortOrder === 'desc') {
                    filteredProducts.sort((a, b) => b.gia - a.gia);
                } else if (salesSortOrder === 'asc') {
                    filteredProducts.sort((a, b) => a.totalSold - b.totalSold);
                } else if (salesSortOrder === 'desc') {
                    filteredProducts.sort((a, b) => b.totalSold - a.totalSold);
                } else if (ratingSortOrder === 'asc') {
                    filteredProducts.sort((a, b) => a.averageRating - b.averageRating);
                } else if (ratingSortOrder === 'desc') {
                    filteredProducts.sort((a, b) => b.averageRating - a.averageRating);
                }

                // Lưu số lượng sản phẩm sau khi lọc
                filteredProductsCount = filteredProducts.length;

                // Tính toán phân trang
                const totalPages = Math.ceil(filteredProducts.length / itemsPerPage);
                const startIndex = (page - 1) * itemsPerPage;
                const endIndex = startIndex + itemsPerPage;
                const paginatedProducts = filteredProducts.slice(startIndex, endIndex);

                // Kiểm tra nếu không có sản phẩm
                if (paginatedProducts.length === 0) {
                    list.append('<p class="text-center">Không có sản phẩm phù hợp với bộ lọc hoặc từ khóa tìm kiếm.</p>');
                    $('#pagination').empty();
                    return;
                }

                // Hiển thị danh sách sản phẩm
                paginatedProducts.forEach(function (item) {
                    const totalSold = item.totalSold;
                    const averageRating = item.averageRating;

                    list.append(`
                        <div class="my_productcard shadow mx-3">
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
                                    <button class="btn btn-warning text-light w-100 mt-2" onClick="AddToCart(${item.id})" >
                                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
                                            <path d="M0 24C0 10.7 10.7 0 24 0L69.5 0c22 0 41.5 12.8 50.6 32l411 0c26.3 0 45.5 25 38.6 50.4l-41 152.3c-8.5 31.4-37 53.3-69.5 53.3l-288.5 0 5.4 28.5c2.2 11.3 12.1 19.5 23.6 19.5L488 336c13.3 0 24 10.7 24 24s-10.7 24-24 24l-288.3 0c-34.6 0-64.3-24.6-70.7-58.5L77.4 54.5c-.7-3.8-4-6.5-7.9-6.5L24 48C10.7 48 0 37.3 0 24zM128 464a48 48 0 1 1 96 0 48 48 0 1 1 -96 0zm336-48a48 48 0 1 1 0 96 48 48 0 1 1 0-96z" />
                                        </svg>
                                        Thêm vào giỏ hàng
                                    </button>
                                </div>
                            </div>
                        </div>
                    `);
                });

                // Hiển thị phân trang
                renderPagination(totalPages, page);

                // Debug: Log danh sách sản phẩm sau khi lọc
                console.log('Filtered products:', filteredProducts.map(item => ({ ten: item.ten, totalSold: item.totalSold, averageRating: item.averageRating })));
            },
            error: function (xhr, status, error) {
                console.error('Lỗi khi lấy dữ liệu sản phẩm:', error);
                $('#product-list').empty().append('<p class="text-center">Đã xảy ra lỗi khi tải sản phẩm. Vui lòng thử lại.</p>');
                $('#pagination').empty();
            }
        });
    }

    function renderPagination(totalPages, currentPage) {
        const pagination = $('#pagination');
        pagination.empty();

        if (totalPages <= 1) return;

        const ul = $('<ul>').addClass('pagination justify-content-center');

        // Nút Previous
        ul.append(`
            <li class="page-item ${currentPage === 1 ? 'disabled' : ''}">
                <a class="page-link pagination-btn" href="#" data-page="prev">Previous</a>
            </li>
        `);

        // Tính toán các trang hiển thị
        const maxPagesToShow = 5;
        let startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
        let endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);

        if (endPage - startPage + 1 < maxPagesToShow) {
            startPage = Math.max(1, endPage - maxPagesToShow + 1);
        }

        // Hiển thị dấu ba chấm đầu
        if (startPage > 1) {
            ul.append(`
                <li class="page-item">
                    <a class="page-link pagination-btn" href="#" data-page="1">1</a>
                </li>
            `);
            if (startPage > 2) {
                ul.append('<li class="page-item disabled"><span class="page-link">...</span></li>');
            }
        }

        // Hiển thị các trang
        for (let i = startPage; i <= endPage; i++) {
            ul.append(`
                <li class="page-item ${i === currentPage ? 'active' : ''}">
                    <a class="page-link pagination-btn" href="#" data-page="${i}">${i}</a>
                </li>
            `);
        }

        // Hiển thị dấu ba chấm cuối
        if (endPage < totalPages) {
            if (endPage < totalPages - 1) {
                ul.append('<li class="page-item disabled"><span class="page-link">...</span></li>');
            }
            ul.append(`
                <li class="page-item">
                    <a class="page-link pagination-btn" href="#" data-page="${totalPages}">${totalPages}</a>
                </li>
            `);
        }

        // Nút Next
        ul.append(`
            <li class="page-item ${currentPage === totalPages ? 'disabled' : ''}">
                <a class="page-link pagination-btn" href="#" data-page="next">Next</a>
            </li>
        `);

        pagination.append(ul);
    }

    function LoadPriceRange(data, selectedPrice = null) {
        if (!data || data.length === 0) {
            $('#myRange').attr('min', 0).attr('max', 100).val(0);
            $('#rangePrice').text('0');
            return;
        }

        const prices = data.map(item => item.gia);
        const minPrice = Math.min(...prices);
        const maxPrice = Math.max(...prices);
        const defaultValue = selectedPrice !== null ? selectedPrice : maxPrice;

        $('#myRange')
            .attr('min', minPrice)
            .attr('max', maxPrice)
            .attr('step', 1000)
            .val(defaultValue);

        $('#rangePrice').text(defaultValue);
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

    // Hàm chuẩn hóa chuỗi, loại bỏ dấu tiếng Việt
    function removeVietnameseTones(str) {
        return str
            .normalize('NFD') // Tách dấu khỏi ký tự
            .replace(/[\u0300-\u036f]/g, '') // Xóa các ký tự dấu
            .replace(/đ/g, 'd') // Thay 'đ' bằng 'd'
            .replace(/Đ/g, 'D'); // Thay 'Đ' bằng 'D'
    }
});
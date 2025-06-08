$(document).ready(function () {
    LoadTheDichVu();
});

function LoadTheDichVu(){
    const list = $('#list_service_card');
    const tomtat = $('#list-dichvu-tomtat');
    list.empty();
    tomtat.empty();


    let allChiPhis = [];
    $.ajax({
        url: '/khachhang/dichvu/list',
        type: 'GET',
        dataType: 'json',
        success: function (response) {
            response.data.forEach(function (item) {
                list.append(`
                    <div class="card shadow service_card">
                        <div class="card-header">
                            ${item.ten.toUpperCase()}
                        </div>
                        <div class="card-body">
                            <div>
                                <img src="${item.anhDichVu?.[0]?.duongDan}" alt="Ảnh dịch vụ"></img>
                                <div class="price">
                                    ${
                                        (() => {
                                            const chiPhis = item.bangGiaDV?.flatMap(x => x.chiTietBangGiaDV?.map(ct => ct.chiPhi) || []) || [];
                                            const giaMin = Math.min(...chiPhis);
                                            const giaMax = Math.max(...chiPhis);
                                            return giaMin === giaMax
                                                ? giaMin.toLocaleString()
                                                : `${giaMin.toLocaleString()} - ${giaMax.toLocaleString()}`;
                                        })()
                                    }
                                </div>
                            </div>
                        </div>
                        <div class="card-footer d-flex justify-content-center align-items-center">
                            <button class="btn btn-primary w-100" onclick="Details(${item.id})">Xem chi tiết</button>
                        </div>
                    </div>
                `);

                tomtat.append(`<li>${item.ten}</li>`);
 
            });
        },
        error: function (xhr, status, error) {
            console.log('Lỗi: ' + error);
        }
    });
}

function Details(serviceId) {
            $.ajax({
                url: '/khachhang/dichvu/' + serviceId,  // Gọi Action Detail
                type: 'GET',
                success: function (data) {
                    // Hiển thị PartialView trả về vào vùng div #serviceDetail
                    $('#serviceDetail').html(data);
                    // Mở modal sau khi cập nhật nội dung
                    $('#serviceDetailModal').modal('show');
                },
                error: function () {
                    alert("Không thể tải chi tiết dịch vụ.");
                }
            });
        }
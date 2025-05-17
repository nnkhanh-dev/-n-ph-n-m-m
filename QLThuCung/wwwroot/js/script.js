document.addEventListener('DOMContentLoaded', function () {
    const dropdownToggles = document.querySelectorAll('.my_dropdowntoggle');
  
    dropdownToggles.forEach(toggle => {
      const parent = toggle.closest('.my_dropdown');
      const dropdownMenu = toggle.nextElementSibling;
  
      toggle.addEventListener('click', function (e) {
        const isOnly = parent.classList.contains('dropdown-only');
        const isLargeScreen = window.innerWidth >= 990.98;
  
        // Nếu là dropdown-only và chưa đủ lớn thì không mở
        if (isOnly && !isLargeScreen) return;
  
        e.stopPropagation();
  
        const isOpen = dropdownMenu.style.display === 'block';
  
        // Đóng tất cả dropdown khác
        document.querySelectorAll('.my_dropdownlist').forEach(menu => {
          menu.style.display = 'none';
        });
  
        // Toggle menu hiện tại
        if (!isOpen) {
          dropdownMenu.style.display = 'block';
        }
      });
    });
  
    // Đóng dropdown khi click ra ngoài
    document.addEventListener('click', function () {
      document.querySelectorAll('.my_dropdownlist').forEach(menu => {
        menu.style.display = 'none';
      });
    });
  });

document.querySelectorAll('.toggle-submenu').forEach(function (toggle) {
    toggle.addEventListener('click', function (e) {
        e.preventDefault();

        const submenu = this.nextElementSibling;
        submenu.classList.toggle('show');
        this.classList.toggle('active');
    });
});

function decreaseQuantity() {
    var qtyInput = document.getElementById("quantity");
    var current = parseInt(qtyInput.value);
    if (current > 1) qtyInput.value = current - 1;
}

function increaseQuantity() {
    var qtyInput = document.getElementById("quantity");
    var current = parseInt(qtyInput.value);
    var max = parseInt(qtyInput.max);
    if (current < max) qtyInput.value = current + 1;
}

$(document).ready(function () {
    var slider = document.getElementById("myRange");
    var output = document.getElementById("rangePrice");
    output.innerHTML = slider.value;

    slider.oninput = function () {
        output.innerHTML = this.value;
    }
});
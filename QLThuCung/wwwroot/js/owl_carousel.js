$(document).ready(function () {
    $(".my_carousel1").owlCarousel({
        loop: true,
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

    $(".my_carousel2").owlCarousel({
        loop: true,
        nav: false,
        dots: false,
        autoplay: true,
        autoplayTimeout: 3000,
        responsive: {
            0: { items: 2 },
            600: { items: 3 },
            750: { items: 4 },
            1000: { items: 4 },
            1300: { items: 5 }
        }
    });

    $(".my_carousel3").owlCarousel({
        loop: true,
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
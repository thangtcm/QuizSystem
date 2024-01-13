(function ($) {
    "use strict";
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }
    // Install Animation 
    AOS.init();
    $(document).ready(function () {
        $("#owl-list-subject").each(function () {
            // Cấu hình Owl Carousel
            $(this).owlCarousel({
                startPosition: 0,
                autoWidth: true,
                responsive: {
                    0: {
                        items: 1
                    },
                    550: {
                        items: 3
                    },
                    950: {
                        items: 5
                    },
                    1200: {
                        items: 6
                    }
                },
                navText: ["<i class=\"fas fa-chevron-left\"></i>", "<i class=\"fas fa-chevron-right\"></i>"],
            });
        });
    });
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }
    const examtotop = select('.sidebar-exam')
    if (examtotop) {
        const toggleExamtotop = () => {
            if (window.scrollY > 100) {
                examtotop.classList.add('top');
            } else {
                examtotop.classList.remove('top')
            }
        }
        window.addEventListener('load', toggleExamtotop)
        onscroll(document, toggleExamtotop)
    }
    const mobileexamtotop = select('.mobileexam-to-top')
    if (mobileexamtotop) {
        const toggleMobileExamtotop = () => {
            if (window.scrollY > 100) {
                mobileexamtotop.classList.add('active')
            } else {
                mobileexamtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleMobileExamtotop)
        onscroll(document, toggleMobileExamtotop)
    }

    // Select 2

    if ($('.selectsearch').length > 0) {
        $('.selectsearch').select2({
            minimumResultsForSearch: 1,
            width: '100%'
        });
    }

    $('.select').each(function () {
        var $select = $(this);
        var modalId = $select.data('modal-id');

        if (modalId) {
            $select.select2({
                minimumResultsForSearch: -1,
                width: '100%',
                dropdownParent: $('#' + modalId)
            });
        } else {
            $select.select2({
                minimumResultsForSearch: -1,
                width: '100%'
            });
        }
    });
})(jQuery);
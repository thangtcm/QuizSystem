$(document).ready(function () {
    toastr.options = {
        "closeButton": true,
        "debug": false,
        "newestOnTop": false,
        "progressBar": true,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
    var mess = $('#message').text();
    if (mess !== '') {
        var status = $('#status').text();
        switch (status) {
            case "success":
                toastr.success(mess, 'Thành công');
                break;
            case "error":
                toastr.error(mess, 'Lỗi');
                break;
            case "info":
                toastr.info(mess, 'Thông báo');
                break;
            case "warning":
                toastr.warning(mess, 'Cảnh báo');
                break;
        }
    }
});
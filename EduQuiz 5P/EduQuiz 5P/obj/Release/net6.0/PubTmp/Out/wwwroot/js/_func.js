function addNewObject(apiEndpoint, containerSelector) {
    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        success: function (result) {
            var newIndex = $(".create-dynamic").length;
            result = result.replace(/\[0\]/g, "[" + newIndex + "]");
            $(containerSelector).append(result);
        },
        error: function (xhr, status, error) {
            console.error('Error updating objects:', error);
        }
    });
}
function addNewObject(apiEndpoint, containerSelector) {
    $('#spinner').removeClass('d-none');
    $('#spinner').addClass('show');
    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        success: function (result) {
            var newIndex = $(".create-dynamic").length;
            result = result.replace(/\[0\]/g, "[" + newIndex + "]");
            result = result.replace(/SelectSubject0/g, "SelectSubject" + newIndex);
            result = result.replace(/SelectChapter/g, "SelectChapter" + newIndex);
            $(containerSelector).append(result);
            LoadEditor();
            LoadSelectSearch();
            LoadSelect();
        },
        error: function (xhr, status, error) {
            console.error('Error updating objects:', error);
        },
        complete: function () {
            // Tắt spinner sau khi request hoàn thành
            $('#spinner').removeClass('show');
            $('#spinner').addClass('d-none');
        }
    });
}

function getChapter(ClassElement, SubjectElement, ChaptetElement) {
    var classId = $(ClassElement).val();
    var subjectId = $(SubjectElement).val();
    $('#spinner').removeClass('d-none');
    $('#spinner').addClass('show');
    $.ajax({
        url: "/Admin/Chapter/LoadChapter",
        type: "GET",
        data: {
            classId: classId,
            subjectId: subjectId
        },
        success: function (data) {
            $(ChaptetElement).empty();
            $(ChaptetElement).append("<option value=''>Chọn chương</option>");
            $.each(data, function (index, item) {
                $(ChaptetElement).append("<option value='" + item.value + "'>" + item.text + "</option>");
            });
        },
        complete: function () {
            // Tắt spinner sau khi request hoàn thành
            $('#spinner').removeClass('show');
            $('#spinner').addClass('d-none');
        }
    });
}
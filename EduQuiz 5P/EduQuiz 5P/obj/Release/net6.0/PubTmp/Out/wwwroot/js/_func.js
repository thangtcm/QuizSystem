function addNewObject(apiEndpoint, containerSelector) {
    $('#spinner').removeClass('d-none');
    $('#spinner').addClass('show');
    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        success: function (result) {
            var newIndex = $(".create-dynamic").length;
            result = result.replace(/\[0\]/g, "[" + newIndex + "]");
            result = result.replace(/SelectClass0/g, "SelectClass" + newIndex);
            result = result.replace(/SelectSubject0/g, "SelectSubject" + newIndex);
            result = result.replace(/SelectChapter0/g, "SelectChapter" + newIndex);
            result = result.replace(/UploadImage0/g, "UploadImage" + newIndex);
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

function getExamMatrix(SubjectElement, ExamMatrixElement) {
    var subjectId = $(SubjectElement).val();
    $('#spinner').removeClass('d-none');
    $('#spinner').addClass('show');
    $.ajax({
        url: "/Admin/ExamMatrix/LoadExamMatrix",
        type: "GET",
        data: {
            subjectId: subjectId
        },
        success: function (data) {
            $(ExamMatrixElement).empty();
            $(ExamMatrixElement).append("<option value=''>Chọn ma trận đề</option>");
            $.each(data, function (index, item) {
                $(ExamMatrixElement).append("<option value='" + item.value + "'>" + item.text + "</option>");
            });
        },
        complete: function () {
            // Tắt spinner sau khi request hoàn thành
            $('#spinner').removeClass('show');
            $('#spinner').addClass('d-none');
        }
    });
}

function previewImage(input, previewId) {
    var preview = document.getElementById(previewId);
    var file = input.files[0];

    if (preview && file) {
        var reader = new FileReader();

        reader.onload = function (e) {
            preview.src = e.target.result;
        };

        reader.readAsDataURL(file);
    }
}

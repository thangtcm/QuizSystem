function addNewObject(apiEndpoint, containerSelector) {
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
        }
    });
}

function getSubjectByClasses(classesId, subjectSelectId) {
    if (classesId) {
        $.ajax({
            url: "/Admin/Subject/LoadSubjects",
            type: "GET",
            data: { classId: classesId },
            success: function (data) {
                $(subjectSelectId).empty();
                $(subjectSelectId).append("<option value=''>Chọn môn học</option>");
                $.each(data, function (index, item) {
                    $(subjectSelectId).append("<option value='" + item.value + "'>" + item.text + "</option>");
                });
            }
        });
    }
}

function getChapterbySubject(subjectId, ChapterSelectId) {
    if (subjectId) {
        $.ajax({
            url: "/Admin/Chapter/LoadChapter",
            type: "GET",
            data: { subjectId: subjectId },
            success: function (data) {
                $(ChapterSelectId).empty();
                $(ChapterSelectId).append("<option value=''>Chọn chương</option>");
                $.each(data, function (index, item) {
                    $(ChapterSelectId).append("<option value='" + item.value + "'>" + item.text + "</option>");
                });
            }
        });
    }
}
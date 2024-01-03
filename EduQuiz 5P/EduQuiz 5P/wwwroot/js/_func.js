function addNewObject(apiEndpoint, containerSelector) {
    $.ajax({
        url: apiEndpoint,
        method: 'GET',
        success: function (result) {
            var newIndex = $(".create-dynamic").length;
            result = result.replace(/\[0\]/g, "[" + newIndex + "]");
            console.log(result);
            result = result.replace(/SelectSubject0/g, "SelectSubject" + newIndex);
            console.log(result);
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
    console.log('RUNNN');
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

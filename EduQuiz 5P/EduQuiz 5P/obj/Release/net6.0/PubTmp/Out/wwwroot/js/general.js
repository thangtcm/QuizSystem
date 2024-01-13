document.addEventListener("DOMContentLoaded", function () {
    renderMathInElement(document.body, {
        delimiters: [
            { left: '$$', right: '$$', display: true },
            { left: '$', right: '$', display: false },
            { left: "\\(", right: "\\)", display: false },
            { left: "\\begin{equation}", right: "\\end{equation}", display: true },
            { left: "\\begin{align}", right: "\\end{align}", display: true },
            { left: "\\begin{alignat}", right: "\\end{alignat}", display: true },
            { left: "\\begin{gather}", right: "\\end{gather}", display: true },
            { left: "\\begin{CD}", right: "\\end{CD}", display: true },
            { left: "\\begin{tikzpicture}", right: "\\end{tikzpicture}", display: true },
            { left: "\\[", right: "\\]", display: true }
        ],
        // • rendering keys, e.g.:
        ignoredClasses: ["disable-katex-render"],
        throwOnError: false
    });
});

function updateQuestionComplete(userExamId) {
    let elementList = document.querySelectorAll('#question-complete');
    $.ajax({
        url: '/UserExam/GetQuestionComplete',
        type: 'GET',
        data: {
            UserExamId: userExamId,
        },
        success: function (result) {
            elementList.forEach(item => {
                item.innerText = result;
            });
            console.log(result);
            console.log('Run Set Câu');
        }
    });
}

function countdown(remainingTime, elementIdSetTime, userExamId) {
    setInterval(function () {
        var minutes = Math.floor(remainingTime / 60);
        var seconds = remainingTime % 60;

        minutes = String(minutes).padStart(2, '0');
        seconds = String(seconds).padStart(2, '0');

        let listElementTimes = document.querySelectorAll(elementIdSetTime);
        listElementTimes.forEach(listElementTime => {
            listElementTime.textContent = minutes + ':' + seconds;
        });

        if (--remainingTime <= 0) {
            $.ajax({
                url: "/UserExam/Result",
                type: "POST",
                data: {
                    Id: userExamId,
                },
                success: function () {
                    console.log("Nộp bài thành công");
                    window.location.href = "/UserExam/QuizResult?Id=" + userExamId;
                },
                error: function (_xhr, _status, error) {
                    console.error(error);
                }
            });
        }
    }, 1000);
}
$(document).ready(function () {
    var QuizList = [];
    var StudentChoice = [];

    var QuestionPointer = { index: "" };
    // var remainingHour = parseInt($('#hour').text());
    // var remainingMinute = parseInt($('#minute').text());
    // var remainingSecond = parseInt($('#second').text());
    // var remainingTimeInSecond = takeDurationInSecond(remainingHour, remainingMinute, remainingSecond);

    $('.quiz-info').each(function (index) {
        QuizList.push({
            'id': $(this).children('#ques-id').text(),
            'content': $(this).children('#ques-content').text(),
            'opta': $(this).children('#ques-opta').text(),
            'optb': $(this).children('#ques-optb').text(),
            'optc': $(this).children('#ques-optc').text(),
            'optd': $(this).children('#ques-optd').text(),
            'quiztime': $(this).children('#quiz-time').text(),
        })
    })

    var time = QuizList[0].quiztime;
    time = 1;
  
    var hourTime = parseInt(time/60);
    console.log('gioi lam bai: ' + hourTime);
    $('#hour').text(hourTime)
    var minuteTime = ((time/60) - parseInt(time/60)) * 60;
    console.log('phut lam bai: ' + minuteTime);
    $('#minute').text(minuteTime);

    var remainingHour = parseInt($('#hour').text());
    var remainingMinute = parseInt($('#minute').text());
    var remainingSecond = parseInt($('#second').text());
    var remainingTimeInSecond = takeDurationInSecond(remainingHour, remainingMinute, remainingSecond);

    console.log(QuizList[0].content);

    countdownTime(remainingTimeInSecond, QuizList.length, StudentChoice);

    loadSideBar(...QuizList);

    QuestionPointer.index = $('.quiz-num > :first').attr('id');
    var DefaultQuestionID = QuestionPointer.index;
    LoadQuestionView(DefaultQuestionID, '', ...QuizList);
    // console.log("QuestionPointer: " + QuestionPointer);

    // sự kiện khi bấm vào một câu hỏi
    $('.num').click(function () {
        QuestionPointer.index = $(this).attr('id');
        var quesID = QuestionPointer.index;
        var choosenOpt = getOptFromChoosenQuestion(quesID, ...StudentChoice);
        console.log('click opt: ' + choosenOpt);
        unhightlightOption();
        LoadQuestionView(quesID, choosenOpt, ...QuizList);
    });

    // sự kiện khi bấm vào 1 trong 4 câu trả lời
    $('.column').click(function () {
        var currentID = QuestionPointer.index;
        console.log('ID of question: ' + currentID);
        var opt = $(this).attr('id');
        unhightlightOption();
        chooseAnswer(currentID, opt, StudentChoice);
        hightlightChoosenOption(opt);
        hightlightChoosenQuestion(currentID);
    });

    // sự kiện khi click nút Nộp bài
    $('#btn_submit').click(function () {
        hideInfo();
        setTimeout(function(){
            if(StudentChoice.length == 0){
                alert('Cần chọn ít nhất một câu trước khi nộp bài!')
            }else{

                submitQuiz(QuizList.length, StudentChoice);
            }
        }, 10);
        setTimeout(function(){
            unhideInfo();
        }, 20)
    });

    // xử lý sự kiên khi bấm nút Hủy
    $('#btn_cancel').click(function(){
        hideInfo();
        setTimeout(function(){
            // if(StudentChoice.length == 0){
            //     alert('Quá trình làm của bạn sẽ mất!\nBạn chắc chứ?');
            // }else{
            //     cancelQuiz();
            // }
            cancelQuiz();
        }, 10);
        setTimeout(function(){
            unhideInfo();
        }, 20)
    })

});


// XỬ LÝ NỘP BÀI VÀ HỦY

function hideInfo(){
    $('.quiz-content').css('display', 'none');
}

function unhideInfo(){
    $('.quiz-content').css('display', '');
}

function submitQuiz(quizTotal, answerList) {
    answerList.forEach(element => {
        element.opt = classifyAnswer(element.opt);
        console.log(element.id + "\t" + element.opt)
    });

    checkResult(quizTotal, answerList);
    storeResultInLocal(answerList);
}

function storeResultInLocal(answerList){
    localStorage.setItem('answerList', JSON.stringify(answerList));
}

function cancelQuiz(){
    if(confirm('Quá trình làm bài của bạn sẽ không được lưu lại nếu thoát!\n Bạn chắc chứ?') == true){
        location.href = "Index";
    }
}

function classifyAnswer(input_opt) {
    var out_opt
    switch (input_opt) {
        case 'opta':
            out_opt = 'a';
            break;
        case 'optb':
            out_opt = 'b';
            break;
        case 'optc':
            out_opt = 'c';
            break;
        case 'optd':
            out_opt = 'd';
            break;
        default:
            out_opt = 'err'
    }
    return out_opt;
}

function checkResult(quizTotal, submittedAnswerList){

    // Lấy id của bài trắc nghiệm từ URL 
    const queryString = window.location.search;
    const urlParams = new URLSearchParams(queryString);
    const quizID = urlParams.get('id');
    const userID = localStorage.getItem('uid');

    if(submittedAnswerList.length == 0){
        alert('Bạn chưa chọn đáp nào! Hãy bắt đầu lại!');
        location.href = "home";
    }else{
        $.ajax({
            type: 'POST',
            url: '/Home/SubmitQuiz',
            dataType: 'json',
            data: {'quizID': quizID, 'answerList': JSON.stringify(submittedAnswerList), 'quesNum': quizTotal, 'userID': userID},
            success: function(data){
                data = Math.round(data * 100) / 100
                console.log("Kết quả của bạn là: \n" + data);
                alert('Kết quả làm bài của bạn là: ' + data + ' điểm');
            }
        })
        location.href = "ReviewQuiz?id=" + quizID;
    }
}

// CHỌN ĐÁP ÁN

function chooseAnswer(quesID, answer, answerList) {
    // Trả về 0 nếu cập nhật đáp án, 1 nếu là đáp án của câu chưa từng chọn
    for (var i = 0; i < answerList.length; i++) {
        if (answerList[i].id == quesID) {
            answerList[i].opt = answer;
            return 0;
        }
    }
    answerList.push({
        'id': quesID,
        'opt': answer
    });
    return 1;
}

//  OBJECT ĐỂ LƯU TRỮ THÔNG TIN BÀI TRẮC NGHIỆM LẤY VỀ TỪ SERVER

function Question(id, content, opta, optb, optc, optd) {
    this.id = id;
    this.content = content;
    this.opta = opta;
    this.optb = optb;
    this.optc = optc;
    this.optd = optd;
}

function Choice(id, opt) {
    this.id = id;
    this.opt = opt;
}

// XỬ LÝ THỜI GIAN NỘP BÀI

function countdownTime(remainingQuizTimeInSecond, quizTotal, answerList) {
    console.log('countdownTime...');
    var countdownLoop = setInterval(() => {
        remainingQuizTimeInSecond--;
        setQuizTime(takeHour(remainingQuizTimeInSecond), takeMinute(remainingQuizTimeInSecond), takeSecond(remainingQuizTimeInSecond));
        if(remainingQuizTimeInSecond <= 5 && remainingQuizTimeInSecond >0){
            console.log('Sắp hết thời gian...');
            alertTime();
        }
        if(remainingQuizTimeInSecond == 0){
            clearInterval(countdownLoop);
            submitQuiz(quizTotal, answerList);
        }
    }, 1000);
    return countdownLoop;
}

function alertTime(){
    $('#hour').css('color', 'red');
    $('#minute').css('color', 'red');
    $('#second').css('color', 'red');
}

function takeDurationInSecond(hour, minute, second) {
    return (hour * 60 * 60) + (minute * 60) + second;
}

function takeHour(duration) {
    return parseInt(duration / 3600);
}

function takeMinute(duration) {
    return parseInt(
        parseInt((duration - takeHour(duration) * 3600) / 60)
    );
}

function takeSecond(duration) {
    return duration - takeHour(duration) * 3600 - takeMinute(duration) * 60;
}

function setQuizTime(hour, minute, second) {
    if (hour >= 10) {
        $('#hour').text(hour);
    } else {
        $('#hour').text('0' + hour);
    }
    if (minute >= 10) {
        $('#minute').text(minute);
    } else {
        $('#minute').text('0' + minute);
    }
    if (second >= 10) {
        $('#second').text(second);
    } else {
        $('#second').text('0' + second);
    }
}

// LOAD CÂU HỎI, LỰA CHỌN LÊN PAGE

function getOptFromChoosenQuestion(quesID, ...ChoosenAnswerList) {
    var choosenOpt = '';
    for (var i = 0; i < ChoosenAnswerList.length; i++) {
        if (quesID == ChoosenAnswerList[i].id) {
            choosenOpt = ChoosenAnswerList[i].opt;
            break;
        }
    }
    return choosenOpt;
}

function loadSideBar(...questionList) {
    console.log('loadSideBar...');
    for (var i = 0; i < questionList.length; i++) {
        $('.quiz-num').append(
            `
                <div class="num" id="` + questionList[i].id + `">
                    <p>` + (i + 1) + `</p>
                </div>
            `
        );
    };
}

function LoadQuestionView(clickedItem, choosenOpt, ...questionList) {
    console.log('You clicked ' + clickedItem);

    var question = '';
    var opta = '';
    var optb = '';
    var optc = '';
    var optd = '';

    for (var i = 0; i < questionList.length; i++) {
        if (clickedItem == questionList[i].id) {
            question = questionList[i].content;
            opta = questionList[i].opta;
            optb = questionList[i].optb;
            optc = questionList[i].optc;
            optd = questionList[i].optd;
            break;
        }
    }

    loadQuestion(question);

    loadOption(opta, optb, optc, optd, choosenOpt);
}

function loadQuestion(questionContent) {
    $('.quiz-question').html(
        `   
            <h1>` + katex.renderToString(questionContent) + `</h1>
        `
    );
}

function loadOption(opta, optb, optc, optd, choosenOpt) {

    var a = katex.renderToString(opta);
    var b = katex.renderToString(optb);
    var c = katex.renderToString(optc);
    var d = katex.renderToString(optd);

    // $('#opta h2').text('A) ' + a);
    // $('#optb h2').text('B) ' + b);
    // $('#optc h2').text('C) ' + c);
    // $('#optd h2').text('D) ' + d);

    $('#opta h2').html(a);
    $('#optb h2').html(b);
    $('#optc h2').html(c);
    $('#optd h2').html(d);

    hightlightChoosenOption(choosenOpt);
}

function hightlightChoosenQuestion(quesID) {
    console.log('hightlightChoosenQuestion: ' + quesID);
    if (quesID != '') {
        $("#" + quesID).css('background-color', 'pink');
    }
}

function unhightlightOption() {
    $(".column").css('background-color', 'white');
}

function hightlightChoosenOption(opt) {
    console.log('hightlightChoosenOption: ' + opt);
    if (opt != '') {
        $("#" + opt).css('background-color', 'tomato');
    }
}


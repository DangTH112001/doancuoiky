$(document).ready(function () {
    var limit = 6;
    var start = 0;
    var action = 'inactive';

    lazzyLoader();

    if (action == 'inactive') {
        action = 'active';
        load_data(limit, start);
    }

    $(window).scroll(function () {
        // var a = $(window).scrollTop() + $(window).height() + 500;
        // var b = $(".grid-subjects").height();
        console.log('you scroll...');
        // mặc định ban đầu là (-500)
        if ($(window).scrollTop() + $(window).height() - 100 > $(".data-item-wrapper").height() && action == 'inactive') {
            lazzyLoader();
            console.log("executing pagination process...");
            action = 'active';
            start = start + limit;
            setTimeout(function () {
                load_data(limit, start);
            }, 600);
        }
    });

    // SUPPORT FUNCTIONS 
    function lazzyLoader() {
        var output = '';
        output += "<p>loading...</p>";
        $(".loading-items-space").html(output);
    }

    function load_data(limit, start) {
        console.log("loading more data...");
        $.ajax({
            url: "/Home/ScrollPage",
            method: "POST",
            dataType: 'json',
            data: { limit: limit, start: start },
            success: function (data) {
                console.log(data);
                DataLength = data.length;

                if (DataLength == 0) {
                    $('.loading-items-space').html('<h4 style="font-size: 14px; text-align: center;font-weight: 800;">Xin lỗi nhưng không còn bài trắc nghiệm nào khác!</h4>');
                    action = 'active';
                } else {
                    data.forEach(function (obj) {
                        prepareItemData(obj.id, obj.description, obj.title, obj.total, obj.time, obj.participant);
                        action = 'inactive';
                    });
                }
            },
            error: function(xhr){
                console.log(xhr);
            }
        })
    };

    function prepareItemData(questionID, topic, title, questionNum, duration, participant) {
        var topScore = 9.2;
        var item = `
                                <div class="grid-item">
                                <div class="quiz-name">
                                <h5>` + topic + `</h5>
                                <h3>` + title + `</h3>
                                </div>
                                <div class="quiz-content">
                                <div class="quiz-channel">
                                    <div class="quiz-channel-icon">
                                    <img src="https://icon-library.com/images/anonymous-user-icon/anonymous-user-icon-2.jpg" alt="" />
                                    </div>
                                    <p>Số câu hỏi: <b>` + questionNum + ` câu</b></p>
                                </div>
                                <div class="quiz-action">
                                    <button id="` + questionID + `" class="btn_bookmark" onclick="bookmark(this.id)">Đánh dấu</button>
                                    <button id="` + questionID + `" class="btn_join" onclick="startQuiz(this.id)">
                                    Thi ngay
                                    </button>
                                </div>
                                <div class="quiz-descr">
                                    <p>
                                    <span>Thời lượng</span><br />
                                    <span>` + duration + ` phút</span>
                                    </p>
                                    <p style="display: none">
                                    <span>Cao nhất</span><br />
                                    <span>` + topScore + `</span>
                                    </p>
                                    <p>
                                    <span>Tham gia</span><br />
                                    <span>` + participant + `</span>
                                    </p>
                                </div>
                                </div>
                            </div>
        `;
        $('#list-all-quiz').append(item);
    }

});







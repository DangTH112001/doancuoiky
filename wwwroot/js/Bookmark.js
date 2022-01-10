$(document).ready(function () {
    loadBookmarkedQuiz();
})

function loadBookmarkedQuiz() {
    var userID = localStorage.getItem('currentUID');
    console.log(userID);

    $.ajax({
        url: "getbookmark",
        method: "POST",
        dataType: 'json',
        data: { userid: userID },
        success: function (data) {
            console.log(data);
            var DataLength = Object.keys(data).length;
            if (DataLength == 0) {
                // $('.loading-items-space').html('<h4 style="font-size: 14px; text-align: center;font-weight: 800;">Xin lỗi nhưng không còn bài trắc nghiệm nào khác!</h4>');
                //     action = 'active';
            } else {
                data.forEach(function (obj) {
                    prepareQuizItem(obj.id, obj.description, obj.title, obj.total, obj.time, obj.participant);
                    // action = 'inactive';
                });
            }
        },

    })
}


function prepareQuizItem(quizID, topic, title, quesNum, duration, joiner) {
    var data = `
                            <li>
                            <div class="bookmark-item-name">
                            <div class="quiz-channel-icon">
                                <img src="../../Assets/avatar-icon.jpg" alt="" />
                            </div>
                            <div class="quiz-name">
                                <p>`+ topic + `</p>
                                <h3>`+ title + `</h3>
                            </div>
                            </div>
                            <div class="bookmark-item-info">
                            <div class="info-col">
                                <div class="col-name">
                                <p>SỐ CÂU</p>
                                </div>
                                <div class="col-val">
                                <h2>`+ quesNum + ` câu</h2>
                                </div>
                            </div>
                            <div class="info-col">
                                <div class="col-name">
                                <p>THỜI LƯỢNG</p>
                                </div>
                                <div class="col-val">
                                <h2>`+ duration + ` phút</h2>
                                </div>
                            </div>
                            <div class="info-col" style="display: none">
                                <div class="col-name">
                                <p>ĐIỂM CAO NHẤT</p>
                                </div>
                                <div class="col-val">
                                <h2>9.8</h2>
                                </div>
                            </div>
                            <div class="info-col">
                                <div class="col-name">
                                <p>THAM GIA</p>
                                </div>
                                <div class="col-val">
                                <h2>`+ joiner +` bạn</h2>
                                </div>
                            </div>
                            <div class="bookmark-item-action">
                                <button id="`+ quizID + `" class="btn_join" onclick="startQuiz(this.id)">Thi ngay</button>
                                <button id="`+ quizID + `" class="btn_remove" onclick="removeBookmark(this.id)">Xóa bỏ</button>
                            </div>
                            </div>
                        </li>
    `;

    $('.bookmark-list').append(data);
}

function removeBookmark(clickID) {
    console.log('remove bookmark...');
    var userid = localStorage.getItem('currentUID');
    console.log('uid: ' + userid + " quizid: " + clickID);
    $.ajax({
        url: "removebookmark",
        method: "POST",
        dataType: 'json',
        data: { userid: userid, quizid: clickID },
        success: function (data) {
            console.log('Xóa khỏi bookmark thành công');
            $('.bookmark-list').empty();
            loadBookmarkedQuiz();
        }
    })
}

function startQuiz(clickedID) {
    console.log('startQuiz...');
    window.location = 'startquiz?id=' + clickedID;
}

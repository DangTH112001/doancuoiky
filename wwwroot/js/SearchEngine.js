$(document).ready(function () {

    hideSearchView();

    $('#btn_search').click(function () {
        showQuizSearchView();
    })

    $('#btn_clearsearch').click(function () {
        console.log('Xóa nội dung trên thanh tìm kiếm');
        $('#tf_search').val('');
    })
})

function showQuizSearchView() {
    var selectedFilter = $('#selected_filter_item').find(":selected").val();
    var searchText = normalizeSearchText($('#tf_search').val());

    if (selectedFilter == 'all' && searchText == '') {
        console.log('Tìm tất cả');
        hideSearchView();
        showDefaultView();
    } else {
        console.log('Show kết quả tìm kiếm');
        findQuiz(selectedFilter, searchText);
    }
}

function normalizeSearchText(text){
    var result = text.replace(/^\s+/g, "");
    result = result.replace(/\s+$/g, "");
    result = result.replace(/\s{2,}/g, ' ');
    result = result.toLowerCase();
    return result;
}

function findQuiz(filter, text) {
    console.log('Bạn đang tìm kiếm với filter value: ' + filter + ' - text value: ' + text);

    var constainData = 0;

    $.ajax({
        url: "searchquiz",
        method: "GET",
        dataType: 'json',
        data: { filter: filter, text: text },
        success: function (data) {
            console.log(data);
            var DataLength = Object.keys(data).length;
            if (DataLength == 0) {
                hideSearchView();
                hideDefaultView();
                $('.grid-subject-title').text('KHÔNG TÌM THẤY KẾT QUẢ');
            } else {
                $('#list-search-quiz').empty();
                hideDefaultView();
                showSearchView();
                data.forEach(function (obj) {
                    prepareResultItem(obj.id, obj.title, obj.description, obj.total, obj.time);
                });
            }
        }
    })

    return constainData;
}

function hideDefaultView() {
    $('.subject-container').css('height', '0px');
    $('.grid-subject-title').text('KẾT QUẢ TÌM KIẾM');
    $('#all-quiz').css('height', '0px');
}

function showDefaultView() {
    $('.subject-container').css('height', '500px');
    $('.grid-subject-title').text('CÁC NỘI DUNG ĐANG CÓ');
    $('#all-quiz').css('height', 'fit-content');
}

function hideSearchView() {
    $('#result-data').css('height', '0px');
}

function showSearchView() {
    $('#result-data').css('height', 'fit-content');
}

function prepareResultItem(id, title, topic, quesNum, duration) {
    var data = `
                <div class="grid-item">
                <div class="quiz-name">
                <h5>`+ topic + `</h5>
                <h3>`+ title + `</h3>
                </div>
                <div class="quiz-content">
                <div class="quiz-channel">
                    <div class="quiz-channel-icon">
                    <img src="/app/Assets/avatar-icon.jpg" alt="" />
                    </div>
                    <p>Số câu hỏi: <b>` + quesNum + ` câu</b></p>
                </div>
                <div class="quiz-action">
                    <button id="`+ id + `" class="btn_bookmark" onclick="bookmarkQuiz(this.id)">Đánh dấu</button>
                    <button id="`+ id + `" class="btn_join" onclick="startQuiz(this.id)">
                    Thi ngay
                    </button>
                </div>
                <div class="quiz-descr">
                    <p>
                    <span>Thời lượng</span><br />
                    <span>`+ duration + ` phút</span>
                    </p>
                    <p>
                    <span>Cao nhất</span><br />
                    <span>12</span>
                    </p>
                    <p>
                    <span>Tham gia</span><br />
                    <span>12</span>
                    </p>
                </div>
                </div>
            </div>
    `;

    $('#list-search-quiz').append(data);
}
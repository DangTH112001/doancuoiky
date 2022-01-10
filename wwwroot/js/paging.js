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
        var a = $(window).scrollTop() + $(window).height() + 500;
        var b = $("#question").height();
        if ($(window).scrollTop() + $(window).height() - 80 > $(".content-question").height() && action == 'inactive') {
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
        output += `<p style="text-align: center;">LOADING...</p>`;
        $(".loading").html(output);
    }

    $('#btn_search').click(function () {
        let search = $('#tf_search').val();
        searchData(search);
    })

    function searchData(search) {
        $.ajax({
            url: "/search",
            method: "POST",
            dataType: 'json',
            data: { search: search },
            success: function (data) {
                data_length = Object.keys(data).length;
                console.log('Da lay ' + data_length);
                if (data_length == 0) {
                    $('.loading').html('<h4 style="font-size: 14px; text-align: center;font-weight: 800;">Xin lỗi nhưng không còn bài trắc nghiệm nào khác!</h4>');
                    action = 'active';
                } else {
                    data.forEach(function (obj) {
                        prepareQuestion1(obj.id, obj.question, obj.filter, obj.A, obj.B, obj.C, obj.D);
                        action = 'inactive';
                    });
                }
            }
        }

        )
    }


    function prepareQuestion(id, question, A, B, C, D) {
        var k_question = katex.renderToString(question);
        var k_A = katex.renderToString(A);
        var k_B = katex.renderToString(B);
        var k_C = katex.renderToString(C);
        var k_D = katex.renderToString(D);
        var item = `
                    <li class="q-item" style="padding-left: 10px;">           
                        <div id="filter">
                        <a id = `+ id + ` class="btn btn-success" style="padding-left: 300px;">Sửa</a>
                        <a id = `+ id + ` class="btn btn-danger float-right del">Xóa</a>
                        </div>
                        <div class="qtion">
                            <h5>Câu hỏi: </h5><p>` + k_question + `</p>                           
                        </div>
                        
                        <div id="option-a">
                            <p>A: `+ k_A + `</p>
                        </div>
                        <div id="option-b">
                            <p>B: `+ k_B + `</p>
                        </div>
                        <div id="option-c">
                            <p>C: `+ k_C + `</p>
                        </div>
                        <div id="option-d">
                            <p>D: `+ k_D + `</p>
                        </div>
                        <input id="`+ id + `" type="checkbox"> Chọn câu hỏi</input>
                      </li>

        `;
        $("#questions").append(item);
    }

    function load_data(limit, start) {
        var userID = localStorage.getItem('currentUID');
        var data_length;
        $.ajax({
            url: "/question",
            // url: "question",
            method: "POST",
            dataType: 'json',
            data: { limit: limit, start: start, userID: userID},
            success: function (data) {
                data_length = Object.keys(data).length;
                console.log('Da lay ' + data_length);
                if (data_length == 0) {
                    $('.loading').html('<h4 style="font-size: 14px; text-align: center;font-weight: 800;">Xin lỗi nhưng không còn bài trắc nghiệm nào khác!</h4>');
                    action = 'active';
                } else {
                    data.forEach(function (obj) {
                        prepareQuestion(obj.id, obj.question, obj.A, obj.B, obj.C, obj.D);
                        action = 'inactive';
                    });
                }
            }
        })
    };
    load_data();


    function prepareQuestion(id, question, A, B, C, D) {
        var k_question = katex.renderToString(question);
        var k_A = katex.renderToString(A);
        var k_B = katex.renderToString(B);
        var k_C = katex.renderToString(C);
        var k_D = katex.renderToString(D);
        var item = `
                    <li class="q-item" style="padding-left: 10px;">           
                       
                        <div class="qtion">

                            <h5 style="display: inline-block;">Câu hỏi: </h5>
                            <a id = `+ id + ` class="btn btn-success float-right" style="margin-left: 1014px;margin-top: 10px;" onclick="test(this.id)">Sửa</a>
                            <a id = `+ id + ` class="btn btn-danger float-right del"  style="margin-top: 10px;" onclick="test1(this.id)">Xóa</a> 
                            <p>` + k_question + `</p>   
                                                  
                        </div>
                        
                        <div id="option-a">
                            <p>A: `+ k_A + `</p>
                        </div>
                        <div id="option-b">
                            <p>B: `+ k_B + `</p>
                        </div>
                        <div id="option-c">
                            <p>C: `+ k_C + `</p>
                        </div>
                        <div id="option-d">
                            <p>D: `+ k_D + `</p>
                        </div>
                        <input id="`+ id + `" type="checkbox" onchange="test2(this.id)"> Chọn câu hỏi</input>
                      </li>

        `;
        $('#question').append(item);
    }

    function prepareTagName(tagname) {
        var item_t = `
        <option value="`+ tagname + `">` + tagname + `</option>
        `;
        $('#selected_filter_item').append(item_t);
    }



    /* function renderListPage() {
        let html = '';
        html += `<li class="page-item active"><a class="page-link" >${1}</a></li>`;
        for (let i = 2; i <= totalPages; i++) {
            html += `<li class="page-item"><a class="page-link" >${i}</a></li>`;
        }
        document.getElementById('number-page').innerHTML = html;
    }
    renderListPage();
    
    function changePage() {
        const currentPages = document.querySelectorAll('#number-page li');
        for (let i = 0; i < currentPages.length; i++) {
            currentPages[i].addEventListener('click', () => {
                const value = i + 1;
                currentPage = value;
                $('.number-page li').removeClass('active');
                if (currentPage === 1) {
                    $('.btn-prev').addClass('disabled');
                }
                if (currentPage === totalPages) {
                    $('.btn-next').addClass('disabled');
                }
                currentPages[i].classList.add('active');
                getCurrentPage(currentPage);
                pload_data();
            })
        }
    }
    changePage();
    
    btnNext.addEventListener('click', () => {
        currentPage++;
        if (currentPage > totalPages) {
            currentPage = totalPages;
        }
        if (currentPage === totalPages) {
            $('.btn-next').addClass('disabled');
        }
        $('.btn-prev').removeClass('disabled');
        $('.number-page li').removeClass('active');
        $(`.number-page li:eq(${currentPage - 1})`).addClass('active');
        getCurrentPage(currentPage);
        load_data();
    })
    
    btnPrev.addEventListener('click', () => {
        currentPage--;
        if (currentPage <= 1) {
            currentPage = 1;
        }
        if (currentPage === 1) {
            $('.btn-prev').addClass('disabled');
        }
        $('.btn-next').removeClass('disabled');
        $('.number-page li').removeClass('active');
        $(`.number-page li:eq(${currentPage - 1})`).addClass('active');
        getCurrentPage(currentPage);
        load_data();
    }) */
}); 
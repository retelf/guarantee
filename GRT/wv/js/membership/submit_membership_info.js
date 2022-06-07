var class_submit_membership_info = function () {

    this.exe = function () {

        // 일단 validation 중에서 급한 것만

        let rb_checked_yes = $('#rb_key_file_generate_yes').prop('checked');
        let rb_checked_no = $('#rb_key_file_generate_no').prop('checked');

        if (!rb_checked_yes && !rb_checked_no) {
            alert("키파일 생성 여부를 체크해 주십시오.");
            return;
        }

        let jsonObject = {
            key: "submit_membership_info",
            value:
            {
                email: $('#text_email').val(),
                password: $('#text_password').val(),
                name_english: $('#text_name_english').val(),
                name_home_language: $('#text_name_home_language').val(),
                country: $('#text_country').val(),
                phone_number: $('#text_phone_number').val(),
                identity_number: $('#text_identity_number').val(),
                whether_key_file_generate: rb_checked_yes
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_submit_membership_info = new class_submit_membership_info();

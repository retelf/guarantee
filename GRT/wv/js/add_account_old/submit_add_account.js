var class_submit_add_account = function () {

    this.exe = function () {

        // 일단 validation 중에서 급한 것만

        if ($('#text_password').prop('disabled')) {

            let tem_private_key = $('#text_private_key').val();
            tem_private_key = tem_private_key.replace(/^0[xX]/, '');

            let reg_hex_element = /[^0123456789abcdefABCDEF]/;

            if (reg_hex_element.test(tem_private_key) || tem_private_key.length != 64) {
                alert($('#text_private_key').val() + "은 유효한 비밀키가 아닙니다.");
                return;
            }
        }

        let rb_checked_yes = $('#rb_key_file_generate_yes').prop('checked');
        let rb_checked_no = $('#rb_key_file_generate_no').prop('checked');

        if (!rb_checked_yes && !rb_checked_no) {
            alert("키파일 생성 여부를 체크해 주십시오.");
            return;
        }

        let guarantee_checked = $('#rb_guarantee_add_account').prop('checked');
        let coin_name;
        let jsonObject;

        if (guarantee_checked) {
            coin_name = "guarantee";
        } else {
            coin_name = "ethereum";
        }

        jsonObject = {
            key: "submit_add_account",
            value:
            {
                public_key: $('#text_public_key').val(),
                password: $('#text_password').val(),
                private_key: $('#text_private_key').val(),
                coin_name: coin_name,
                node: $('#text_node').val(),
                whether_key_file_generate: rb_checked_yes
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_submit_add_account = new class_submit_add_account();

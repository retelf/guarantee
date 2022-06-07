var class_submit_login = function () {

    this.exe = function ($button) {

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

        let guarantee_checked = $('#rb_guarantee_login').prop('checked');
        let coin_name, command, ethereum_transaction_type;
        let jsonObject;

        if ($button.text() == "login") {
            command = "submit_login";
        } else {
            command = "submit_logout";
        }

        if (guarantee_checked) {
            coin_name = "guarantee";
        } else {
            coin_name = "ethereum";
        }

        $('.rb_ethereum_transaction_type').each(function () {
            if ($(this).prop('checked')) {
                ethereum_transaction_type = $(this).prop('id').replace(/^rb_/, '');
                return false;
            }            
        })

        jsonObject = {
            key: command,
            value:
            {
                public_key: $('#text_public_key').val(),
                password: $('#text_password').val(),
                private_key: $('#text_private_key').val(),
                coin_name: coin_name,
                ethereum_transaction_type: ethereum_transaction_type
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_submit_login = new class_submit_login();

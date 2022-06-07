var class_submit_mobile_authentication_number = function () {

    this.exe = function () {

        let jsonObject = {
            key: "submit_mobile_authentication_number",
            value:
            {
                public_key: $('#text_public_key').val(),
                private_key: $('#text_private_key').val(),
                node: $('#text_node').val(),
                mobile_authentication_number: $('#text_mobile_authentication_number').val(),
                hash: $('#div_hidden_hash').html()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_submit_mobile_authentication_number = new class_submit_mobile_authentication_number();

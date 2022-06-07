var class_submit_register_info = function () {

    this.exe = function () {

        // 일단 validation 중에서 급한 것만

        let jsonObject = {
            key: "submit_register_info",
            value:
            {
                eoa: $('#text_eoa').val(),
                na: $('#text_na').val(),
                domain: $('#text_domain').val(),
                ip: $('#text_ip').val(),
                port: $('#text_port').val(),
                type: $('#text_type').val()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_submit_register_info = new class_submit_register_info();

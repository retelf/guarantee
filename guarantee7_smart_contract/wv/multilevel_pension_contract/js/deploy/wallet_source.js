var class_wallet_source = function () {

    this.exe = function () {
        let jsonObject = {
            key: "submit_wallet_source",
            value:
            {
                code: $('#textarea_code').val()
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_wallet_source = new class_wallet_source();

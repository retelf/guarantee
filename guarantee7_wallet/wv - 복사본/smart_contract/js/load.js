var class_load = function () {

    this.exe = function () {

        let jsonObject = {
            key: "load_smart_contract_page",
            value:
            {
                smart_contract_name: $('#text_smart_contract_name').val()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_load = new class_load();

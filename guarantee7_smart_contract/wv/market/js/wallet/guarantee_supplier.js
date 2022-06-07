var class_guarantee_supplier = function () {

    this.exe = function () {

        // 먼저 개런티 유효 잔고를 찾는다.
        // 있으면 개런티 거래소로 간다.
        
        let jsonObject = {
            key: "submit_multilevel_pension_guarantee_supplier",
            value:
            {
                ethereum_eoa: $('#text_ethereum_eoa').val(),
                ethereum_balance: $('#div_ethereum_balance').val()
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }
}; var c_guarantee_supplier = new class_guarantee_supplier();
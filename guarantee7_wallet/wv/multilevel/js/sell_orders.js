var class_sell_order = function () {

    this.exe = function () {

        let exchange_rate = $('#text_exchange_rate').val() * 1;

        if (exchange_rate < 0.6) {
            alert("1개런티에 대한 교환비율은 0.6 이더리움 이상이어야 합니다.");
            return;
        }

        let jsonObject = {
            key: "submit_sell_order",
            value:
            {
                balance: $('#div_balance').html(),
                exchange_rate: $('#text_exchange_rate').val(),
                days_span: $('#text_days_span').val()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_sell_order = new class_sell_order();

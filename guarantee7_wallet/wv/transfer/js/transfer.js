var class_transfer = function () {

    this.exe = function () {

        let jsonObject = {
            key: "submit_transfer",
            value:
            {
                balance: $('#div_balance').html(),
                eoa_from: $('#div_from').html(),
                amount: $('#text_amount').val(),
                gasPrice: $('#text_gasPrice').val(),
                eoa_to: $('#text_to').val()
            }
        }

        window.chrome.webview.postMessage(jsonObject);
    }
}

var c_transfer = new class_transfer();

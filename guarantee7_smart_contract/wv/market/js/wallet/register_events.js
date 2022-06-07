var class_register_events = function () {

    this.exe = function () {

        $('#select_counterpart_coin_name').change(function () {

            let counter_part_coin_name = $("#select_counterpart_coin_name option:selected").text();
            let sell_amount = $('#text_sell_amount').val();

            document.getElementById('label_sell_amount_former').innerText =
                GR.login_state + ' 코인 ' + sell_amount + ' 개에 관하여 ' + '1 ' + GR.login_state + ' 를 ';

            document.getElementById('label_sell_amount_latter').innerText = ' ' + counter_part_coin_name + ' 의 비율로 교환합니다.';

        });

        $('#btn_contract').bind('click', function () {

            // 먼저 최소한의 검증

            if (GR.balance * 1 > $('#text_sell_amount').val().trim() * 1) {
                alert(GR.login_state + ' 잔고가 부족합니다.');
                return false;
            } else {
                let jsonObject = {
                    key: "submit_exchange",
                    value:
                    {
                        eoa: GR.eoa,
                        coin_name_from: GR.login_state,
                        amount: $('#text_sell_amount').val().trim(),
                        exchange_rate: $('#text_exchange_rate').val().trim(),
                        gasPrice: $('#text_gasPrice').val(),
                        coin_name_to: $("#select_counterpart_coin_name option:selected").text()
                    }
                }
                window.chrome.webview.postMessage(jsonObject);

            }
        });
    }
}; var c_register_events = new class_register_events();
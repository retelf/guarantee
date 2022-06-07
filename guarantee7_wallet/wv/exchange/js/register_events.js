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

            let amount = $('#text_sell_amount').val().trim() * 1;
            let exchange_rate = $('#text_exchange_rate').val().trim() * 1;
            let exchange_fee = c_float_fix.exe(amount, GR.exchange_fee_rate);
            let coin_name_from = GR.login_state;
            let coin_name_to = $("#select_counterpart_coin_name option:selected").text();
            let bool;

            let message = '매도물량 : ' + amount + ' ' + coin_name_from +
                '\n매수물량 : ' + c_float_fix.exe(amount, exchange_rate) + ' ' + coin_name_to +
                '\n수수료 : ' + exchange_fee + ' ' + coin_name_from + '(수수료 전액 매도자 부담)' +
                '\n수수료는 매도주문시 부과되며 취소시 환불됩니다.'

            if (coin_name_from != 'guarantee') {
                message += '\n취소시 가스비가 추가로 발생하게 됨은 불가피함을 유의하시기 바랍니다.';
            }

            bool = confirm(message)

            if (bool) {

                GR.set_interval_running = false;

                // 먼저 최소한의 검증

                if (GR.balance * 1 < $('#text_sell_amount').val().trim() * 1) {
                    alert(GR.login_state + ' 잔고가 부족합니다.');
                    return false;
                } else {
                    let jsonObject = {
                        key: "submit_load_order",
                        value:
                        {
                            eoa: GR.eoa,
                            coin_name_from: coin_name_from,
                            amount: amount,
                            exchange_rate: exchange_rate,
                            gasPrice: $('#text_gasPrice').val(),
                            gasLimit: $('#text_gasLimit').val(),
                            coin_name_to: coin_name_to
                        }
                    }
                    window.chrome.webview.postMessage(jsonObject);

                }

            } else {
                return false;
            }
        });
    }

}; var c_register_events = new class_register_events();
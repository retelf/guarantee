var class_submit_exchange = function () {

    this.exe = function ($button) {

        $button.prop('disabled', true);

        $tr_parent = $button.closest("tr");

        let key = 'submit_' + $button.prop('id').match(/exchange|cancel/);

        let jsonObject = {
            key: key,
            value:
            {
                block_number: $tr_parent.find('.div_board_block_number:first').text(),
                eoa: $tr_parent.find('.div_board_eoa:first').text(),
                na: $tr_parent.find('.div_board_na:first').text(),
                domain: $tr_parent.find('.div_board_domain:first').text(),
                ip: $tr_parent.find('.div_board_ip:first').text(),
                port: $tr_parent.find('.div_board_port:first').text(),
                coin_name_from: $tr_parent.find('.div_board_coin_name_from:first').text(),
                coin_name_to: $tr_parent.find('.div_board_coin_name_to:first').text(),
                amount: $tr_parent.find('.div_board_amount:first').text(),
                exchange_rate: $tr_parent.find('.div_board_exchange_rate:first').text(),
                exchange_fee_rate: $tr_parent.find('.div_board_exchange_fee_rate:first').text()
            }
        }
        window.chrome.webview.postMessage(jsonObject);
    }

}; var c_submit_exchange = new class_submit_exchange();
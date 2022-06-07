var class_submit_refund = function () {

    var Me; this.initialize = function () { Me = c_submit_refund; }

    this.button_selected;

    this.exe = function ($button) {

        let $tr_parent = $button.closest("tr");

        this.button_selected = $button;

        let jsonObject = {
            key: 'submit_refund',
            value:
            {
                password: $('#text_password').val(),
                whether_key_file_generate: rb_checked_yes,
                block_number: $tr_parent.find('.div_board_block_number:first').text(),
                eoa: $tr_parent.find('.div_board_eoa:first').text(),
                na: $tr_parent.find('.div_board_na:first').text(),
                domain: $tr_parent.find('.div_board_domain:first').text(),
                ip: $tr_parent.find('.div_board_ip:first').text(),
                port: $tr_parent.find('.div_board_port:first').text(),
                exchange_rate: $tr_parent.find('.div_board_exchange_rate:first').text(),
                days_span: $tr_parent.find('.div_board_days_span:first').text(),
                exchange_fee_rate: $tr_parent.find('.div_board_exchange_fee_rate:first').text()
            }
        }
        window.chrome.webview.postMessage(jsonObject);

    }

}; var c_submit_refund = new class_submit_refund();
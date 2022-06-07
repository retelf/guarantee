var class_GR = function () {

    var Me; this.initialize = function () { Me = GR; }

    this.login_state;
    this.eoa;
    this.ca;
    this.balance;

    this.smart_contract_common_initializing = function () {

        c_register_events.exe();

        $('#div_coin_name').html('coin_name : ' + GR.login_state);
        $('#div_eoa').html('eoa : ' + GR.eoa);
        $('#div_balance').html('balance : ' + GR.balance);

        if (GR.login_state == 'guarantee') {
            $('#div_gasPrice_outer').css('display', 'none');
        } else if (GR.login_state == 'guarantee') {
            $('#div_gasPrice_outer').css('display', 'block');
        }

        document.getElementById('text_sell_amount').innerText = GR.login_state;

        $('#select_counterpart_coin_name option[value=' + this.login_state + ']').attr('disabled', 'disabled').siblings().removeAttr('disabled');

    }
}

var GR = new class_GR(); GR.initialize();
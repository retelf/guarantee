var class_GR = function () {

    var Me; this.initialize = function () { Me = GR; }

    this.login_state;
    this.eoa;
    this.ca;
    this.balance;
    this.exchange_fee_rate = 0.002;
    this.id_set_interval;
    this.set_interval_running;
    this.set_interval_time = 5000;

    this.exchange_initializing = function () {

        $('#div_coin_name').html('coin_name : ' + GR.login_state);
        $('#div_eoa').html('eoa : ' + GR.eoa);
        $('#div_balance').html('balance : ' + GR.balance);

        document.getElementById('label_sell_amount').innerText = GR.login_state;

        if (GR.login_state == 'guarantee') {
            $('#div_gasPrice_outer').css('display', 'none');
        } else if (GR.login_state == 'ethereum') {
            $('#div_gasPrice_outer').css('display', 'block');
        }

        document.getElementById('text_sell_amount').innerText = GR.login_state;

        $('#select_counterpart_coin_name option[value=' + this.login_state + ']').attr('disabled', 'disabled').siblings().removeAttr('disabled');

    }
}

var GR = new class_GR(); GR.initialize();
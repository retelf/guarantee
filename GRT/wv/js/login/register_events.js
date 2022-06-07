var class_register_events = function () {

    this.exe = function () {

        $("#btn_login").bind("click", function () {
            c_submit_login.exe($(this));
        });

        $("#rb_guarantee_login").bind("click", function () {
            c_select_coin.exe('guarantee');
        });

        $("#rb_ethereum_login").bind("click", function () {
            c_select_coin.exe('ethereum');
        });

        $(".rb_ethereum_transaction_type").bind("click", function () {
            c_select_coin.ethereum_transaction_type($(this));
        });
    }
}

var c_register_events = new class_register_events();

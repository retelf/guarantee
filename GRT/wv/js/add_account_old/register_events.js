var class_register_events = function () {

    this.exe = function () {

        $("#btn_add_account").bind("click", function () {
            c_submit_add_account.exe();
        });

        $("#rb_guarantee_add_account").bind("click", function () {
            c_select_coin.exe('guarantee');
        });

        $("#rb_key_file_generate_yes").bind("click", function () {
            c_whether_key_file_generate.exe(true);
        });

        $("#rb_key_file_generate_no").bind("click", function () {
            c_whether_key_file_generate.exe(false);
        });

        $("#rb_ethereum_add_account").bind("click", function () {
            c_select_coin.exe('ethereum');
        });
    }
}

var c_register_events = new class_register_events();

var class_register_events = function () {

    this.exe = function () {

        $("#btn_add_account").bind("click", function () {
            c_submit_add_account.exe();
        });

        $("#rb_add_account_new").bind("click", function () {
            c_select_add_type.exe('new');
        });

        $("#rb_add_account_already").bind("click", function () {
            c_select_add_type.exe('already');
        });

        $("#rb_key_file_generate_yes").bind("click", function () {
            c_whether_key_file_generate.exe(true);
        });

        $("#rb_key_file_generate_no").bind("click", function () {
            c_whether_key_file_generate.exe(false);
        });
    }
}

var c_register_events = new class_register_events();

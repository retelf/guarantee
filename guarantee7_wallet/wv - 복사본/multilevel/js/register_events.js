var class_register_events = function () {

    this.exe = function () {

        $("#btn_sell_order").bind("click", function () {
            c_sell_order.exe();
        });

        $("#btn_execute_finally").bind("click", function () {
            c_submit_multilevel.button_selected.trigger("click");
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
